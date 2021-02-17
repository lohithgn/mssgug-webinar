param([Parameter(Mandatory=$true)][string]$servicePrincipalObjId)

$rgName="rg-todoapi-infra"
$storageAccountName="sttodoapiinfra001"
$location="southeast asia"
$infraContainer="pulumi"
$keyVaultName="kv-todoapi-infra"
$keyName="pulumi"

Write-Host "Creating Infra State"

Write-Host "`n`t Creating Resource Group"
$null = az group create --location westus --name $rgName

Write-Host "`n`t Creating Storage Account"
$null = az storage account create --name $storageAccountName --resource-group $rgName --location $location --sku Standard_LRS

Write-Host "`n`t Getting Storage Key"
$key = az storage account keys list -g $rgName -n $storageAccountName --query '[0].value' 

Write-Host "`n`t Creating Storage Container"
$null = az storage container create --name $infraContainer --account-key $key --account-name $storageAccountName

Write-Host "`n`t Creating Key Vault"
$null = az keyvault create --name $keyVaultName --resource-group $rgName --location $location --enabled-for-template-deployment true --sku "standard"

Write-Host "`n`t Checking for Key"
$key = az keyvault key show --vault-name $keyVaultName --name $keyName
if ($null -ne $key) 
{
    Write-Host "`n`t Key exists"
}
else 
{
    Write-Host "`n`t Creating key"
    $key = az keyvault key create --name $keyName --vault-name $keyVaultName --kty "RSA" --protection "software" --size 2048
}

Write-Host "`n`t Add access policy to KV"
$null = az keyvault set-policy --name $keyVaultName --object-id $servicePrincipalObjId --key-permissions get list encrypt decrypt

Write-Host "Done"