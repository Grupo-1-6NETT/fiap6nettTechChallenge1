# API de Gerenciamento de Contatos

Esta é uma API desenvolvida em .NET Core 8 para gerenciar o cadastro de contatos. 
Este projeto sugere uma solução para o Tech Challenge da Fase 2 do curso de pós graduação 6NETT na FIAP.

## Índice
- [Pré-requisitos](#pré-requisitos)
- [Configuração do Projeto](#configuração-do-projeto)
- [Endpoints da API](#endpoints-da-api)
  - [Autenticação e Registro de Usuários](#autenticação-e-registro-de-usuários)
  - [Gerenciamento de Contatos](#gerenciamento-de-contatos)
- [Uso da API](#uso-da-api)
  - [Exemplos de Requisições](#exemplos-de-requisições)
- [Configuração do Monitoramento](#configuração-do-monitoramento-com-prometheus-e-grafana)
- [Testes](#testes)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)

## Pré-requisitos

- [Docker](https://www.docker.com/get-started/) e [Docker Compose](https://docs.docker.com/compose/install/) (necessário para executar o projeto)
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) (somente para executar local)
- [SQLite](https://www.sqlite.org/index.html) (somente para executar local)
- [Postman](https://www.postman.com/) (opcional, para testar a API)

## Configuração do Projeto

**1. Clone o repositório:**

   ```bash
   git clone https://github.com/Grupo-1-6NETT/fiap6nettTechChallenge1.git
   cd CadastroApi
   ```

**2. Adicione o Secret em appsettings.{env}.json** (Somente se executar local)

Crie uma chave para criptografia de senhas (secret) e adicione no appsettings do ambiente que estiver rodando.  
Por exempo, em ambiente de desenvolvimento, adicione a propriedade em `appsettings.Development.json`.  
O secret deve ter ao menos 256 bytes.


```json

"Secret":"ITISASECRETFOREVERYONE256B..."
```

**3. Inicie todos os serviços (API, Prometheus, Grafana, Node Exporter):**

```bash
docker-compose up --build
```

Após iniciar, os serviços estarão disponíveis em:

- API: http://localhost:5000 (porta padrão)
- Prometheus: http://localhost:9090
- Grafana: http://localhost:3000

---
## Endpoints da API
### Autenticação e Registro de Usuários
|Método|Endpoint|Descrição|
|---|---|---|
|GET|/Token|Gera um token de autenticação para o usuário e senha informados|
|POST|/Usuario|Adiciona um Usuário na base de dados|
|DELETE|/Usuario{id}|Remove o Usuário na base de dados com o ID informado|

### Gerenciamento de Contatos
|Método|Endpoint|Descrição|
|---|---|---|
|GET|/Contatos|Lista os Contatos cadastrados, ordenados por nome, que correspondem aos parâmetros informados|
|POST|/Contatos|Adiciona um Contato na base de dados|
|PATCH|/Contatos|Atualiza um Contato na base de dados|
|DELETE|/Contatos{id}|Remove o contato na base de dados com o ID informado|

## Uso da API
Para acessar os endpoints, você precisará autenticar o usuário e incluir o token JWT no cabeçalho das requisições aos endpoints protegidos.

### Exemplos de Requisições
**1. Registrar um Novo Usuário**
**Endpoint**: POST /Usuario

**Corpo da Requisição:**

```json
{
  "nome": "novo_usuario",  
  "senha": "SuaSenha123",
  "permissao": "admin"
}
```

**2. Gerar Token**
**Endpoint**: GET /token

**Corpo da Requisição:**

```json
{
  "usuario": "novo_usuario",
  "senha": "SuaSenha123"
}
```

**Resposta:**
```json
 "eyJhbGciOiJIUzI1NiIsInR5..."
```

**3. Criar um Novo Contato**
**Endpoint**: POST /contatos

**Cabeçalho**: `Authorization: Bearer {seu_token_jwt}`

**Corpo da Requisição:**

```json
{
  "nome": "Maria Souza",
  "telefone": "99999-9999",
  "ddd": "11",
  "email": "maria.souza@example.com"
}
```
---
## Configuração do Monitoramento com Prometheus e Grafana
A API já está configurada para expor métricas no endpoint `/metrics`

Após acessar o Grafana (http://localhost:3000), siga os passos abaixo:

1. Configure o Prometheus como fonte de dados.
2. Importe dashboards pré-configurados para métricas de API e sistema.

---
## Testes
Para executar os testes, use o comando:

```bash
dotnet test
```

Os testes de unidade foram implementados utilizando o Moq e FluentAssertions para validações.

---
## Tecnologias Utilizadas
- **ASP.NET Core 8** - Framework principal para desenvolvimento da API
- **Entity Framework Core** - ORM para manipulação do banco de dados
- **JWT** - JSON Web Token para autenticação
- **Swagger** - Documentação interativa da API


- **SQLite** - Banco de dados
- **Moq e FluentAssertions** - Testes unitários
- **Prometheus e Grafana** - Monitoramento e visualização de métricas.
- **Node Exporter** - Coleta de métricas de hardware e sistema.
