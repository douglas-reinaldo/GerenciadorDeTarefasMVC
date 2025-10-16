# 📋 Gerenciador de Tarefas

Sistema web para gerenciamento de tarefas pessoais, desenvolvido com ASP.NET Core MVC e Entity Framework Core.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap&logoColor=white)

## 📸 Screenshots

### Lista de Tarefas
![Lista de Tarefas](docs/screenshots/lista-tarefas.png)

### Detalhes da Tarefa
![Detalhes](docs/screenshots/detalhes-tarefa.png)

## 🚀 Funcionalidades

- ✅ **Autenticação de Usuários**
  - Cadastro de novos usuários
  - Login seguro com hash de senhas (PasswordHasher)
  - Sistema de sessão

- ✅ **Gerenciamento de Tarefas**
  - Criar, editar e excluir tarefas
  - Definir status (Pendente, Em Andamento, Concluída)
  - Definir prioridade (Alta, Média, Baixa)
  - Visualização detalhada de cada tarefa

- ✅ **Segurança**
  - Proteção CSRF com Anti-Forgery Tokens
  - Validação de autorização por usuário
  - Hash de senhas com ASP.NET Core Identity PasswordHasher

- ✅ **Interface Responsiva**
  - Design adaptável para desktop e mobile
  - Cards coloridos por status
  - Badges de prioridade

## 🛠️ Tecnologias Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core 9.0** - ORM
- **C# 12** - Linguagem de programação
- **MySQL 8.0** - Banco de dados relacional

### Frontend
- **Razor Pages** - Template engine
- **Bootstrap 5.3** - Framework CSS
- **Bootstrap Icons** - Ícones

### Ferramentas
- **Visual Studio 2022** - IDE
- **Git/GitHub** - Controle de versão

## 📋 Pré-requisitos

Antes de começar, você precisa ter instalado:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server 8.0+](https://dev.mysql.com/downloads/mysql/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (opcional, mas recomendado)
- [Git](https://git-scm.com/)

## 🔧 Instalação e Configuração

### 1. Clone o repositório

```bash
git clone https://github.com/douglas-reinaldo/GerenciadorDeTarefasMVC.git
cd GerenciadorDeTarefas
```

### 2. Configure o Banco de Dados

#### Crie o banco no MySQL:

```sql
CREATE DATABASE gerenciadortarefasbd;
CREATE USER 'developer'@'localhost' IDENTIFIED BY 'SuaSenhaSegura';
GRANT ALL PRIVILEGES ON gerenciadortarefasbd.* TO 'developer'@'localhost';
FLUSH PRIVILEGES;
```

#### Configure a Connection String:

Edite o arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "GerenciadorDeTarefas": "server=localhost;userid=developer;password=SuaSenhaSegura;database=gerenciadortarefasbd"
  }
}
```

### 3. Execute as Migrations

```bash
cd GerenciadorDeTarefas
dotnet ef database update
```

### 4. Execute o Projeto

```bash
dotnet run
```

Acesse: `https://localhost:7019` ou `http://localhost:5028`

## 📁 Estrutura do Projeto

```
GerenciadorDeTarefas/
├── Controllers/           # Controladores MVC
│   ├── HomeController.cs
│   ├── TarefaController.cs
│   └── UsuarioController.cs
├── Models/               # Modelos de dados
│   ├── Tarefa.cs
│   ├── Usuario.cs
│   ├── Enums/
│   │   ├── Status.cs
│   │   └── Prioridade.cs
│   └── ViewModels/
│       └── TarefaFormViewModel.cs
├── Services/             # Lógica de negócio
│   ├── TarefaService.cs
│   └── UsuarioService.cs
├── Data/                 # Contexto do banco
│   └── GerenciadorTarefasDbContext.cs
├── Views/               # Views Razor
│   ├── Home/
│   ├── Tarefa/
│   └── Usuario/
├── Filters/             # Filtros personalizados
│   └── SessionAuthorizeAttribute.cs
├── DTO/                 # Data Transfer Objects
│   └── LoginRequestDTO.cs
├── Migrations/          # Migrations do EF Core
└── wwwroot/            # Arquivos estáticos

GerenciadorDeTarefas.Tests/
└── TarefaControllerTests.cs  # Testes unitários
```

## 🗄️ Modelo de Dados

### Tabela: Usuario

| Campo      | Tipo         | Descrição                    |
|------------|--------------|------------------------------|
| Id         | int (PK)     | Identificador único          |
| Nome       | varchar(20)  | Nome do usuário              |
| Email      | varchar      | Email (único)                |
| SenhaHash  | varchar      | Senha criptografada          |

### Tabela: Tarefa

| Campo       | Tipo          | Descrição                    |
|-------------|---------------|------------------------------|
| Id          | int (PK)      | Identificador único          |
| Titulo      | varchar(100)  | Título da tarefa             |
| Descricao   | varchar(500)  | Descrição detalhada          |
| DataCriacao | datetime      | Data de criação              |
| Status      | int (enum)    | 0=Pendente, 1=Andamento, 2=Concluída |
| Prioridade  | int (enum)    | 0=Alta, 1=Média, 2=Baixa     |
| UsuarioId   | int (FK)      | Referência ao usuário        |

## 🔐 Segurança

- **Autenticação:** Sistema de sessão com validação em cada requisição
- **Autorização:** Filtro `SessionAuthorizeAttribute` protege rotas privadas
- **Senhas:** Hash com `PasswordHasher<Usuario>` do ASP.NET Core Identity
- **CSRF:** Proteção com `[ValidateAntiForgeryToken]` em formulários
- **Validação:** Data Annotations nos models + validação server-side

## 📊 Logging

O sistema utiliza o `ILogger` do ASP.NET Core para registrar:

- Criação, atualização e exclusão de tarefas
- Tentativas de login (sucesso e falha)
- Erros de banco de dados
- Operações sensíveis

Logs são salvos no console em desenvolvimento.

## 🧪 Testes

```bash
cd GerenciadorDeTarefas.Tests
dotnet test
```

> 📝 **Nota:** Implementação de testes em andamento.

## 🤝 Contribuindo

Contribuições são bem-vindas! Siga os passos:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👤 Autor

**Douglas Reinaldo**

- GitHub: [@seu-usuario](https://github.com/douglas-reinaldo)
- LinkedIn: [Seu Nome](https://linkedin.com/in/douglas-reinaldo-534271233)
- Email: Douglas39510@gmail.com

## 🙏 Agradecimentos

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Bootstrap](https://getbootstrap.com)

---

⭐ Se este projeto te ajudou, considere dar uma estrela!
