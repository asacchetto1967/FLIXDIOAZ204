# APIM Configuration Commands for FlixDioAZ204

# Variables
$RESOURCE_GROUP="FLIXDIOAZ204"
$APIM_NAME="apim-flixdioaz204"
$LOCATION="eastus2"

# Note: The Function App must be deployed first to get its URL.
# Replace 'your-function-app-name' with the actual name after deployment.
$FUNCTION_APP_NAME="your-function-app-name"

# Create the API in APIM
# Using manual setup for each endpoint if OpenAPI is not installed
az apim api create --resource-group $RESOURCE_GROUP --service-name $APIM_NAME --api-id "flix-api" --path "/flix" --display-name "Flix Catalog API"

# Example: Add fnGetAllMovies operation
az apim api operation create --resource-group $RESOURCE_GROUP --service-name $APIM_NAME --api-id "flix-api" --operation-id "getAllMovies" --display-name "Get All Movies" --method "GET" --url-template "/movies"
