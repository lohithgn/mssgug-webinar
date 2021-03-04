using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNextGen.DocumentDB.Latest;
using Pulumi.AzureNextGen.DocumentDB.Latest.Inputs;
using Pulumi.AzureNextGen.Resources.Latest;
using Pulumi.AzureNextGen.Storage.Latest;
using Pulumi.AzureNextGen.Storage.Latest.Inputs;
using Pulumi.AzureNextGen.Web.Latest;
using Pulumi.AzureNextGen.Web.Latest.Inputs;

class ToDoApiStack : Stack
{
    public ToDoApiStack()
    {
        var config = new Config();
        var environment = config.Require("env");
        string appName = "todoapi";
        string resourceGroupName = $"{ResourcePrefixes.ResourceGroup}{appName}-{environment}";
        string location = "southeast asia";
        string funcStorageName = $"{ResourcePrefixes.Storage}{appName}{environment}001";
        string funcAppServicePlanName = $"{ResourcePrefixes.AppServicePlan}{appName}-{environment}";
        string funcAppName = $"{ResourcePrefixes.FunctionApp}{appName}-{environment}";
        string cosmosDBAccountName = $"{ResourcePrefixes.Cosmos}{appName}-{environment}";

        //1. Create Resource Group
        var resourceGroup = CreateResourceGroup(resourceGroupName, location);

        //2. Create Storage Account for Func App
        var storageAccount = CreateStorageAccount(funcStorageName, resourceGroup);

        //3. Create Cosmos DB
        CreateCosmosDB(location, cosmosDBAccountName, resourceGroup);

        //4. Create Consumption Plan for Func App
        var appServicePlan = CreateAppServicePlan(funcAppServicePlanName, resourceGroup);

        //5. Create Function App
        var functionApp = CreateFunctionApp(funcAppName, resourceGroup, storageAccount, appServicePlan);
    }

    [Output]
    public Output<string> PrimaryStorageKey { get; set; }

    [Output]
    public Output<string> CosmosConnectionString { get; set; }

    private WebApp CreateFunctionApp(string funcAppName, ResourceGroup resourceGroup, StorageAccount storageAccount, AppServicePlan appServicePlan)
    {
        var functionApp = new WebApp(funcAppName, new WebAppArgs()
        {
            Name = funcAppName,
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            ServerFarmId = appServicePlan.Id,
            Kind = "functionapp",
            SiteConfig = new SiteConfigArgs()
            {
                AppSettings = new List<NameValuePairArgs>()
                {
                    new NameValuePairArgs
                    {
                        Name = "AzureWebJobsStorage",
                        Value = Output.Format($"DefaultEndpointsProtocol=https;AccountName={storageAccount.Name};AccountKey={this.PrimaryStorageKey};EndpointSuffix=core.windows.net;")
                    },
                    new NameValuePairArgs
                    {
                        Name = "AzureWebJobsDashboard",
                        Value = Output.Format($"DefaultEndpointsProtocol=https;AccountName={storageAccount.Name};AccountKey={this.PrimaryStorageKey};EndpointSuffix=core.windows.net;")
                    },
                    new NameValuePairArgs
                    {
                        Name = "ToDoDBConnection",
                        Value = Output.Format($"{this.CosmosConnectionString}")
                    },
                    new NameValuePairArgs{ Name = "Runtime", Value = "dotnet"},
                    new NameValuePairArgs{ Name = "FUNCTIONS_EXTENSION_VERSION", Value = "~3"}
                    new NameValuePairArgs{Name = "ToDoDBName",Value = "todos-db"},
                    new NameValuePairArgs{Name = "ToDoCollection",Value = "todos-sql-Container"},
                },
            },
        });

        return functionApp;
    }

    private AppServicePlan CreateAppServicePlan(string funcAppServicePlanName, ResourceGroup resourceGroup)
    {
        return new AppServicePlan(funcAppServicePlanName, new AppServicePlanArgs()
        {
            Name = funcAppServicePlanName,
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            Kind = "FunctionApp",
            Sku = new SkuDescriptionArgs
            {
                Name = "Y1",
                Tier = "Dynamic",
                Size = "Y1",
            },
        });
    }

    private void CreateCosmosDB(string location, string cosmosDBAccountName, ResourceGroup resourceGroup)
    {
        var dbAccount = new DatabaseAccount(cosmosDBAccountName, new DatabaseAccountArgs
        {
            AccountName = cosmosDBAccountName,
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            Locations = {
                new LocationArgs
                {
                    LocationName = location,
                    FailoverPriority = 0
                }
            },
            DatabaseAccountOfferType = DatabaseAccountOfferType.Standard,
            Capabilities = new[] {
                new Pulumi.AzureNextGen.DocumentDB.Latest.Inputs.CapabilityArgs
                {
                    Name = "EnableServerless"
                }
            }
        });

        // Export the primary key of the Storage Account
        this.CosmosConnectionString = Output.Tuple(resourceGroup.Name, dbAccount.Name).Apply(names =>
            Output.CreateSecret(GetCosmosConnectionString(names.Item1, names.Item2)));

        var dbName = "todos-db";
        var cosmosSqlDB = new SqlResourceSqlDatabase(dbName, new SqlResourceSqlDatabaseArgs
        {
            AccountName = dbAccount.Name,
            DatabaseName = dbName,
            Options = new CreateUpdateOptionsArgs(),
            Resource = new SqlDatabaseResourceArgs
            {
                Id = dbName
            },
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location
        });

        var containerName = "todos-sql-Container";
        var dbContainer = new SqlResourceSqlContainer(containerName, new SqlResourceSqlContainerArgs
        {
            AccountName = dbAccount.Name,
            ContainerName = containerName,
            DatabaseName = cosmosSqlDB.Name,
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            Options = new CreateUpdateOptionsArgs(),
            Resource = new SqlContainerResourceArgs
            {
                Id = containerName
            }
        });
    }

    private StorageAccount CreateStorageAccount(string funcStorageName, ResourceGroup resourceGroup)
    {
        var storageAccount = new StorageAccount(funcStorageName, new StorageAccountArgs
        {
            ResourceGroupName = resourceGroup.Name,
            AccountName = funcStorageName,
            Location = resourceGroup.Location,
            Sku = new SkuArgs
            {
                Name = SkuName.Standard_LRS
            },
            Kind = Kind.StorageV2
        });

        // Export the primary key of the Storage Account
        this.PrimaryStorageKey = Output.Tuple(resourceGroup.Name, storageAccount.Name).Apply(names =>
            Output.CreateSecret(GetStorageAccountPrimaryKey(names.Item1, names.Item2)));
        return storageAccount;
    }

    private ResourceGroup CreateResourceGroup(string resourceGroupName, string location)
    {
        return new ResourceGroup(resourceGroupName, new ResourceGroupArgs
        {
            ResourceGroupName = resourceGroupName,
            Location = location
        });
    }

    

    private static async Task<string> GetStorageAccountPrimaryKey(string resourceGroupName, string accountName)
    {
        var accountKeys = await ListStorageAccountKeys.InvokeAsync(new ListStorageAccountKeysArgs
        {
            ResourceGroupName = resourceGroupName,
            AccountName = accountName
        });
        return accountKeys.Keys[0].Value;
    }

    private static async Task<string> GetCosmosConnectionString(string resourceGroupName, string accountName)
    {
        var connectionStrings = await ListDatabaseAccountConnectionStrings.InvokeAsync(new ListDatabaseAccountConnectionStringsArgs
        {
            AccountName = accountName,
            ResourceGroupName = resourceGroupName
        });
        
        return connectionStrings.ConnectionStrings[0].ConnectionString;
    }
}