$rgName="rg-todoapi-infrastate-dev"
$storageAccountName="sttodoapiinfrastate001"
$location="southeast asia"
$devContainer="dev"

Write-Host "Creating Resource Group"
$null = az group create --location westus --name $rgName

Write-Host "Creating Storage Account"
$null = az storage account create --name $storageAccountName --resource-group $rgName --location $location --sku Standard_LRS

Write-Host "Getting Storage Key"
$key = az storage account keys list -g $rgName -n $storageAccountName --query '[0].value' 

Write-Host "Creating Storage Container"
$null = az storage container create --name $devContainer --account-key $key --account-name $storageAccountName


