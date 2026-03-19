# Identity Campaign Service

Serviço responsável pelo gerenciamento de campanhas e usuários dentro do sistema de doações.

## 🧠 Visão Geral

Este serviço expõe uma API REST responsável por:

- criação de campanhas
- consulta de campanhas
- gerenciamento de dados relacionados a campanhas

Ele atua como o ponto de entrada do sistema.

---

## 🏗️ Arquitetura

O projeto segue uma abordagem baseada em **Clean Architecture**, dividido em:

- Domain
- Application
- Infrastructure
- API

### 📌 Domain
- Entidades do negócio (`Campaign`, `Donor`)
- Regras de negócio puras

### 📌 Application
- Casos de uso (Commands / Handlers)
- Abstrações (interfaces)
- Uso de MediatR (CQRS simplificado)

### 📌 Infrastructure
- Persistência com EF Core
- Implementação de repositórios

### 📌 API
- Controllers
- Entrada HTTP
- Swagger

---

## 🔄 Fluxo de Criação de Campanha

<div align="left">

HTTP Request  
↓  
Controller  
↓  
CreateCampaignCommand  
↓  
CreateCampaignHandler  
↓  
Repository  
↓  
Database  

</div>

---

## 🚀 Tecnologias

- .NET 8
- ASP.NET Core
- MediatR
- Entity Framework Core
- Swagger

---

## ▶️ Como executar

```bash
dotnet restore
dotnet build
dotnet run --project src/IdentityCampaign.Api