# TransactionAuthorizer

Transaction Authorizer é um sistema de autorização de transações com cartão de crédito, projetado para aprovar ou rejeitar transações com base em regras predefinidas, utilizando Microservices e arquitetura hexagonal.

## Funcionalidades

- Autorização de transações com base em MCC (Merchant Category Codes).
- Gerenciamento de contas e comerciantes.
- Validação de payloads de entrada com FluentValidation.
- Registro de transações autorizadas e rejeitadas.

## Tecnologias Utilizadas

- C# 
- .NET 8
- Dapper
- FluentValidation
- xUnit
- SQL Server

## Migrations

O projeto utiliza migrações manuais para gerenciar o esquema do banco de dados. As migrações são implementadas na camada `Infrastructure`, garantindo controle de versão do banco de dados. Os scripts de migração permitem a criação e atualização de tabelas, mantendo a integridade e consistência dos dados ao longo do desenvolvimento.

## Estrutura do Projeto

TransactionAuthorizer │ ├── TransactionAuthorizer.API │ ├── TransactionAuthorizer.Application │ ├── TransactionAuthorizer.CrossCutting.IoC │ ├── TransactionAuthorizer.Domain│ ├── TransactionAuthorizer.Infrastructure │ └── TransactionAuthorizer.Tests

## Configuração

1. Clone o repositório:
   ```bash
   git clone https://github.com/Gabriellsg/TransactionAuthorizer.git

1. Configure a string de conexão no appsettings.json(Já configurada para acesso padrão ao MSSQLLocalDB):
   ```bash
   "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TransactionAuthorizerDB;Trusted_Connection=True;" }

As migrações são executadas na inicicialização do projeto para criar a base de dados e as tabelas necessárias no banco de dados.

Testes
Os testes podem ser executados utilizando o xUnit. Certifique-se de que o banco de dados esteja configurado corretamente antes de executar os testes.

Contribuição
Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.
