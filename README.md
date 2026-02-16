# Gerenciador de Catálogos Netflix - Azure Functions & CosmosDB

Este projeto é um Gerenciador de Catálogos da Netflix desenvolvido com **Azure Functions** (modelo .NET 8 Isolated) e **Azure CosmosDB NoSQL**. Ele permite o upload de vídeos e thumbnails para o Azure Storage e a persistência de metadados no CosmosDB.

## 🚀 Arquitetura

- **Azure Functions**: Endpoints HTTP para processamento de dados.
- **Azure CosmosDB (NoSQL)**: Armazenamento persistente de metadados dos filmes.
- **Azure Blob Storage**: Armazenamento de arquivos de vídeo e thumbnails.
- **Azure API Management (APIM)**: Camada de API para gerenciamento e exposição dos endpoints.

## 📁 Estrutura do Projeto

- `Functions/`: Contém as implementações das funções (PostVideo, PostThumbnail, PostDataBase, GetAllMovies, GetMovieDetails).
- `Models/`: Modelos de dados (Movie).
- `local.settings.json`: Configurações de conexão (CosmosDB e Storage).

## 🛠️ Como Executar Localmente

1. Certifique-se de ter o [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) e o [Azure Functions Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local) instalados.
2. Configure as strings de conexão no arquivo `local.settings.json`.
3. Execute o comando:
   ```bash
   func start
   ```

## 📦 Inicialização do Repositório GitHub

Para transformar esta pasta em um repositório e enviá-lo para o GitHub, siga estes passos:

```bash
# Inicializar o git
git init

# Adicionar todos os arquivos (o .gitignore evitará arquivos desnecessários)
git add .

# Criar o commit inicial
git commit -m "Initial commit: Netflix Catalog Manager with Azure Functions and CosmosDB"

# Adicionar o seu repositório remoto (substitua URL_DO_SEU_REPOSITORIO)
# git remote add origin URL_DO_SEU_REPOSITORIO

# Enviar para o GitHub
# git push -u origin main
```

## 🌐 Configuração do API Management (APIM)

Para configurar o APIM `apim-flixdioaz204` para apontar para as suas funções, primeiro você deve publicar as funções no Azure. Após a publicação, você pode usar os seguintes comandos da Azure CLI:

```bash
# 1. Definir variáveis (substitua pelos seus valores reais após o deploy)
RESOURCE_GROUP="FLIXDIOAZ204"
APIM_NAME="apim-flixdioaz204"
FUNCTION_APP_NAME="SUBSTITUA_PELO_NOME_DA_SUA_FUNCAO"

# 2. Importar a Function App para o APIM
# (Isso cria automaticamente as APIs baseadas nas suas funções)
az apim api create --resource-group $RESOURCE_GROUP \
    --service-name $APIM_NAME \
    --api-id "netflix-catalog-api" \
    --path "/catalog" \
    --display-name "Netflix Catalog API" \
    --specification-url "https://$FUNCTION_APP_NAME.azurewebsites.net/api/swagger.json" \
    --specification-format "OpenApi"
```

*Nota: Para que o comando acima funcione com `swagger.json`, você pode adicionar o pacote `Microsoft.Azure.WebJobs.Extensions.OpenApi` ao seu projeto.*
