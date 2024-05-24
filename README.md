### Wave_Commerce.API
Repositório com o projeto para a vaga de desenvolvedor backend dotnet Júnior.

### 📌 Sobre o Projeto
Esta API é responsável por realizar o CRUD de um Produto. e possui como funcionalidades pricipais:
* Fornece Endpoints com as operações de acordo a regra de negócio estabelecida.
* Armazenamento dos dados no PostgreSQL.
* Conteinerização da aplicação em Docker para utilizar em qualquer máquina.

### 📌 Especificações do Projeto
Abordagens que foram utilizadas durante a construção do Software.

# Arquitetura em Camadas
- **Wave.Commerce.API**: Contém os controladores e configurações de rotas da API.
- **Wave.Commerce.Application**: Implementa a lógica de negócios e os casos de uso.
- **Wave.Commerce.Domain**: Define as entidades e interfaces do domínio.
- **Wave.Commerce.Persistence**: Implementa a camada de acesso a dados utilizando Entity Framework.
- **Wave.Commerce.DependencyInjection**: Configura a injeção de dependências para todo o projeto.
- **Wave.Commerce.Tests**: Inclui testes unitários para validar a lógica de negócio.
- **Wave.Commerce.IntegrationTests**: Contém testes de integração para assegurar a interação correta entre diferentes partes do sistema.

# Code-First com EntityFramework
Utilizei essa abordagem, pois permite maior controle sobre o modelo de dados diretamente no código. Isso facilita o versionamento e a evolução do esquema do banco de dados conforme o projeto se desenvolve.

# Command Query Responsability Segregation
Durante conversa técnica foi discutido sobre o uso do pattern em meu dia a dia em outros projetos, por isso optei por implementar esse pattern para separar operações de leitura e escrita. Alguns benefícios do CQRS incluem:
    * Separação de preocupações: Facilita a manutenção, pois a lógica de leitura e escrita são independentes.
    * Escalabilidade: Permite escalar a leitura e escrita de forma independente, melhorando o desempenho.
    *Simplificação de modelos: Os modelos de leitura podem ser otimizados para consultas, enquanto os de escrita podem focar na consistência dos dados.

### 🛠️ Construído com

-   [C#](https://learn.microsoft.com/pt-br/dotnet/csharp/) - Linguagem de Programação
-   [.NET 6.0](https://learn.microsoft.com/pt-br/dotnet/fundamentals/) - Plataforma de desenvolvimento
-   [PostgreSQL](https://www.postgresql.org/docs/) - Sistema gerenciador de banco de dados relacional

### 🚀 Utilizando sistema localmente com Docker

Essas instruções permitirão que você obtenha acesso ao projeto em operação na sua máquina local para fins de teste.

### 📋 Pré-requisitos

De quais coisas você precisa para instalar o software e como instalá-lo?

```
WSL
DOCKER DESKTOP
DOCKER COMPOSE
```

### 🔧 Instalação

Uma série de exemplos passo-a-passo que informam o que você deve executar para ter um ambiente de teste em execução.

```
Instalar o WSL
https://docs.docker.com/desktop/wsl/
```

```
Instalar o Docker Desktop e Docker compose
https://docs.docker.com/desktop/install/windows-install/
```

```
Clone este repositório em sua máquina local utilizando o seguinte comando: 
git clone https://github.com/manzanofp/Wave_Commerce.API.git
```

```
Navegue no terminal do windows até o diretório do projeto:
cd Wave_Commerce.API
Verifique se o arquivo docker-compose está nessa pasta para prosseguir
```

```
execute a aplicação:
docker-compose up -build
comando necessário para buildar a aplicação no docker.
```

```
execute a aplicação:
docker-compose up -d
para subir a aplicação sem os logs você pode utilizar esse comando acima.
```

```
encerrando a aplicação:
docker-compose down
```

### ⚙️ Utilização do sistema localmente com docker

Para utilizar a API acesse em seu navegador a seguinte url da documentação com o Swagger:
http://localhost:8080/swagger/index.html

Para utilizar o Client do banco de dados acesse a seguinte url:
http://localhost:8086/?pgsql=db&username=postgres&db=WaveCommerce

a senha padrão do banco de dados é: 123456.
preencha os dados de acesso ao banco conforme sua string de conexão.

### 😁 Aplicação está de pé! teste como preferir.

### 🚀 Utilizando sistema localmente

Essas instruções permitirão que você obtenha acesso ao projeto em operação na sua máquina local para fins de desenvolvimento e teste.

### 📋 Pré-requisitos

De quais coisas você precisa para instalar o software e como instalá-lo?

```
C# e .NET 6
Visual Studio 2022
PostgreSQL
DBeaver ou o software de visualização de banco de dados da sua preferência.
```

### 🔧 Instalação

Uma série de exemplos passo-a-passo que informam o que você deve executar para ter um ambiente de desenvolvimento em execução.

```
Instalar o .NET 6
https://dotnet.microsoft.com/pt-br/download/dotnet/6.0
```

```
Instalar o Visual Studio 2022
https://visualstudio.microsoft.com/pt-br/downloads/
```

```
Instalar o PostgreSQL
https://www.postgresql.org/download/
```

```
Instalar o DBeaver
https://dbeaver.io/download/
```

```
Clone este repositório em sua máquina local utilizando o seguinte comando: 
git clone https://github.com/manzanofp/Wave_Commerce.API.git
```

```
Navegue até o diretório Wave.Commerce.API e abra com o Visual Studio 2022 o arquivo chamado Wave.Commerce.Sln:
```

```
Selecione para inicialização o seguinte projeto: Wave.Commerce.API
```

```
Restaure as dependências do projeto com o comando:
dotnet restore
```

```
Compile e execute a aplicação:
dotnet run
```

### ⚙️ Utilização do sistema localmente

Após o comando run será aberto em seu navegador a API na url da documentação com o Swagger:

Os dados serão armazenados em seu PostgreSQL local, é necessário ter ele configurado!

### 😁 Aplicação está localmente de pé! teste como preferir.