# EmpresaX POS API

Sistema de Ponto de Venda (POS) com gestão financeira completa, desenvolvido com Clean Architecture e .NET 8.

## 🚀 Características

- **Clean Architecture** - Arquitetura limpa e escalável
- **API RESTful** - Endpoints documentados com Swagger/OpenAPI
- **Autenticação JWT** - Segurança com tokens Bearer
- **Testes Automatizados** - Cobertura completa com xUnit
- **CI/CD** - Pipeline automático com GitHub Actions e Azure DevOps
- **Monitoramento** - Health checks e logs estruturados com Serilog
- **Frontend React** - Interface moderna e responsiva

## 📋 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/) (para o frontend)
- [PostgreSQL 14+](https://www.postgresql.org/) ou SQLite (para desenvolvimento)
- [Docker](https://www.docker.com/) (opcional)

## 🏗️ Arquitetura

```
EmpresaX.POS.API/
├── src/
│   ├── Controllers/          # Endpoints da API
│   ├── Services/             # Lógica de negócio
│   ├── Models/               # Modelos e DTOs
│   ├── Data/                 # Contexto do banco de dados
│   └── Program.cs            # Configuração da aplicação
├── tests/
│   └── EmpresaX.POS.API.Tests/  # Testes unitários
├── empresax-frontend/        # Aplicação React
│   ├── src/
│   │   ├── pages/           # Páginas da aplicação
│   │   ├── components/      # Componentes reutilizáveis
│   │   └── config/          # Configurações
│   └── public/
└── README.md
```

## 🔧 Instalação e Configuração

### Backend (.NET)

1. **Clone o repositório**
```bash
git clone <repository-url>
cd EmpresaX.POS.API
```

2. **Configure a string de conexão**
Edite `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=empresax_pos;Username=postgres;Password=sua_senha"
  }
}
```

3. **Execute as migrações**
```bash
dotnet ef database update
```

4. **Rode a aplicação**
```bash
cd src
dotnet run
```

A API estará disponível em: `https://localhost:5245`

### Frontend (React)

1. **Instale as dependências**
```bash
cd empresax-frontend
npm install
```

2. **Configure a URL da API**
Edite `src/config/constants.ts`:
```typescript
export const API_CONFIG = {
  baseURL: 'https://localhost:5245/api/v1',
  // ...
};
```

3. **Rode a aplicação**
```bash
npm start
```

O frontend estará disponível em: `http://localhost:3000`

## 📚 Documentação da API

### Swagger/OpenAPI

Acesse a documentação interativa em: `https://localhost:5245/swagger`

### Endpoints Principais

#### Autenticação
- `POST /api/v1/auth/login` - Login de usuário
- `POST /api/v1/auth/register` - Registro de usuário

#### Contas a Pagar
- `GET /api/v1/contas` - Lista todas as contas
- `GET /api/v1/contas/{id}` - Busca conta por ID
- `POST /api/v1/contas` - Cria nova conta
- `PUT /api/v1/contas/{id}` - Atualiza conta
- `DELETE /api/v1/contas/{id}` - Remove conta

#### Produtos
- `GET /api/v1/produtos` - Lista todos os produtos
- `GET /api/v1/produtos/{id}` - Busca produto por ID
- `POST /api/v1/produtos` - Cria novo produto
- `PUT /api/v1/produtos/{id}` - Atualiza produto
- `DELETE /api/v1/produtos/{id}` - Remove produto

#### Categorias
- `GET /api/v1/categorias` - Lista todas as categorias
- `GET /api/v1/categorias/{id}` - Busca categoria por ID
- `POST /api/v1/categorias` - Cria nova categoria
- `PUT /api/v1/categorias/{id}` - Atualiza categoria
- `DELETE /api/v1/categorias/{id}` - Remove categoria

#### Caixa
- `POST /api/v1/caixa/{id}/fechar` - Fecha o caixa

#### Health Check
- `GET /health` - Status da aplicação e dependências

## 🧪 Testes

### Executar todos os testes

**PowerShell:**
```powershell
.\run-tests.ps1
```

**Bash/Linux:**
```bash
dotnet test
```

### Executar testes específicos

```bash
dotnet test --filter "FullyQualifiedName~ContasControllerTests"
```

### Cobertura de código

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 🔄 CI/CD

### GitHub Actions

O pipeline é executado automaticamente em:
- Push para branches `main` e `develop`
- Pull requests para `main` e `develop`

**Etapas:**
1. Build do backend
2. Testes unitários
3. Build do frontend
4. Análise de qualidade de código
5. Scan de segurança
6. Cobertura de código

### Azure DevOps

Configure o pipeline apontando para `azure-pipelines.yml` na raiz do projeto.

## 🐳 Docker

### Build da imagem

```bash
docker build -t empresax-pos-api .
```

### Executar container

```bash
docker run -p 5245:5245 empresax-pos-api
```

### Docker Compose

```bash
docker-compose up
```

## 🔐 Segurança

- **Autenticação JWT** - Tokens com expiração configurável
- **HTTPS** - Comunicação segura
- **CORS** - Configuração restrita de origens permitidas
- **Validação de entrada** - Data Annotations e validações customizadas
- **Scan de vulnerabilidades** - Trivy no pipeline CI/CD

## 📊 Monitoramento

### Health Checks

Endpoint: `GET /health`

Monitora:
- PostgreSQL connection
- Application status
- Dependencies

### Logs

Logs estruturados com Serilog em:
- Console (desenvolvimento)
- Arquivos (produção)
- Application Insights (Azure)

## 🛠️ Tecnologias

### Backend
- .NET 8
- ASP.NET Core
- Entity Framework Core
- PostgreSQL / SQLite
- Serilog
- Swagger/OpenAPI
- xUnit, FluentAssertions, Moq

### Frontend
- React 18
- TypeScript
- TailwindCSS
- Recharts
- Heroicons
- Axios

## 📝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## 👥 Autores

- **Carol** - Desenvolvimento inicial

## 🙏 Agradecimentos

- Jason Taylor - Clean Architecture template
- Comunidade .NET
- Comunidade React

## 📞 Suporte

Para reportar bugs ou solicitar features, abra uma issue no repositório.

---

**Desenvolvido com ❤️ usando Clean Architecture**
