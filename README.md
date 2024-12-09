# Linka

## Pré-requisitos

Certifique-se de que você possui os seguintes itens instalados em sua máquina:

- [.NET SDK](https://dotnet.microsoft.com/download) (versão mínima recomendada: 6.0 ou superior)
- [Git](https://git-scm.com/) (para clonar o repositório)
- Um editor de código como [Visual Studio Code](https://code.visualstudio.com/) ou [Visual Studio](https://visualstudio.microsoft.com/)
- Uma instância de SQL Server acessível

## Instalação e Execução

Siga as etapas abaixo para configurar e executar o projeto:

1. **Clone o repositório**
   ```bash
   git clone https://github.com/mauritsstrijker/linka-api.git
   cd linka-api

2. **Instalar dependências e executar aplicação**
   dotnet restore
   dotnet build
   dotnet run

**Certifique-se de atualizar a connectionString do banco de dados no arquivo appsettings.json**

