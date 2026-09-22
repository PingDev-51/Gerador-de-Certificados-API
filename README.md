# 🎓 Gerador de Certificados Online

> API desenvolvida em **C# e .NET 10** para automatizar a geração de certificados de cursos, utilizando processamento assíncrono com RabbitMQ.

<p align="center">
  <img src="https://img.shields.io/badge/.NET%2010-512BD4?style=for-the-badge&logo=dotnet&logoColor=white">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white">
  <img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white">
  <img src="https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white">
</p>

---

## 📖 Sobre o projeto

O sistema permite:

- 🔐 Cadastro e autenticação com JWT
- 📚 Cadastro e consulta de cursos
- 🎓 Solicitação de certificados para vários alunos
- 📄 Geração de certificados em PDF
- 📦 Geração de ZIP com os certificados
- 📊 Consulta do status da geração
- 🧪 Testes automatizados

---

## 🛠️ Tecnologias

- **C# / .NET 10**
- **ASP.NET Core**
- **Entity Framework Core**
- **PostgreSQL**
- **MediatR / CQRS**
- **MassTransit**
- **RabbitMQ**
- **QuestPDF**
- **FluentResults**
- **MSTest / Moq / FluentAssertions**

---

## 🏗️ Arquitetura

```text
src/
├── Api
├── Aplicacao
├── Dominio
└── Infraestrutura

tests/
└── GeradorDeCertificados.Testes.Unidade
```

A aplicação utiliza arquitetura em camadas, CQRS + MediatR, injeção de dependência e repositórios.

---

## ⚡ Processamento

A geração dos certificados acontece de forma assíncrona:

```text
API
 │
 ▼
MediatR
 │
 ▼
PostgreSQL
 │
 ▼
RabbitMQ
 │
 ▼
Consumer
 │
 ├── QuestPDF → PDFs
 │
 └── ZIP
```

**Status:**

`Pendente → GerandoCertificados → GerandoZip → Concluido`

Em caso de erro:

`GerandoCertificados → Falha`

---

## 📡 Principais endpoints

| Método | Rota | Função |
|--------|------|--------|
| POST | `/auth/cadastro` | Cadastro |
| POST | `/auth/login` | Login |
| POST | `/cursos` | Criar curso |
| GET | `/cursos/{cursoId}` | Consultar curso |
| POST | `/cursos/{cursoId}/certificados` | Solicitar certificados |
| GET | `/cursos/{cursoId}/status` | Consultar status |
| GET | `/cursos/{cursoId}/certificados` | Listar certificados |
| GET | `/cursos/{cursoId}/certificados/download` | Baixar ZIP |

---

## 🧪 Testes

Executar:

```bash
dotnet test
```

Os testes abrangem entidades de domínio, serviços e handlers da aplicação.

---

## ▶️ Executando o projeto

```bash
git clone github.com/PingDev-51/Gerador-de-Certificados-API.git
cd GeradorDeCertificados
dotnet restore
dotnet ef database update
dotnet run
```

É necessário configurar o PostgreSQL, RabbitMQ e as credenciais da aplicação através das configurações do ambiente.

---

## 👨‍💻 Desenvolvedores

- Kauan Silva
- Kauan Rafael Galvani

Projeto desenvolvido durante os estudos de Full Stack .NET na Academia do Programador.

<div align="center">

</div>