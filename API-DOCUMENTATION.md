# Documentação da API - EmpresaX POS

## 📋 Visão Geral

Esta API fornece endpoints para gerenciamento completo de um sistema de Ponto de Venda (POS) com funcionalidades financeiras.

**Base URL:** `https://localhost:5245/api/v1`

**Swagger UI:** `https://localhost:5245/swagger`

## 🔐 Autenticação

A API utiliza autenticação JWT (JSON Web Token). Para acessar endpoints protegidos:

1. Faça login através do endpoint `/auth/login`
2. Copie o token JWT retornado
3. Inclua o token no header `Authorization` de cada requisição:
   ```
   Authorization: Bearer {seu-token-jwt}
   ```

### Endpoints de Autenticação

#### POST /api/v1/auth/login
Realiza login do usuário e retorna token JWT.

**Request Body:**
```json
{
  "email": "usuario@example.com",
  "senha": "senha123"
}
```

**Response (200 OK):**
```json
{
  "sucesso": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuario": {
    "id": 1,
    "nome": "João Silva",
    "email": "usuario@example.com"
  }
}
```

## 💰 Contas a Pagar

### GET /api/v1/contas
Retorna lista de todas as contas a pagar.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "descricao": "Conta de Luz",
    "valor": 250.00,
    "dataVencimento": "2025-10-30T00:00:00",
    "pago": false
  }
]
```

### GET /api/v1/contas/{id}
Busca uma conta específica por ID.

**Parameters:**
- `id` (path) - ID da conta

**Response (200 OK):**
```json
{
  "id": 1,
  "descricao": "Conta de Luz",
  "valor": 250.00,
  "dataVencimento": "2025-10-30T00:00:00",
  "pago": false
}
```

**Response (404 Not Found):**
```json
{
  "erro": "Conta não encontrada"
}
```

### POST /api/v1/contas
Cria uma nova conta a pagar.

**Request Body:**
```json
{
  "descricao": "Conta de Água",
  "valor": 120.50,
  "dataVencimento": "2025-11-05T00:00:00"
}
```

**Response (201 Created):**
```json
{
  "id": 2,
  "descricao": "Conta de Água",
  "valor": 120.50,
  "dataVencimento": "2025-11-05T00:00:00",
  "pago": false
}
```

**Response (400 Bad Request):**
```json
{
  "errors": {
    "Descricao": ["O campo Descricao é obrigatório"],
    "Valor": ["O valor deve ser maior que zero"]
  }
}
```

### PUT /api/v1/contas/{id}
Atualiza uma conta existente.

**Parameters:**
- `id` (path) - ID da conta

**Request Body:**
```json
{
  "id": 1,
  "descricao": "Conta de Luz - Atualizada",
  "valor": 275.00,
  "dataVencimento": "2025-10-30T00:00:00",
  "pago": true
}
```

**Response (204 No Content)**

**Response (404 Not Found):**
```json
{
  "erro": "Conta não encontrada"
}
```

### DELETE /api/v1/contas/{id}
Remove uma conta.

**Parameters:**
- `id` (path) - ID da conta

**Response (204 No Content)**

**Response (404 Not Found):**
```json
{
  "erro": "Conta não encontrada"
}
```

## 📦 Produtos

### GET /api/v1/produtos
Retorna lista de todos os produtos.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "nome": "Notebook Dell",
    "preco": 3500.00,
    "estoque": 10,
    "categoriaId": 1
  }
]
```

### GET /api/v1/produtos/{id}
Busca um produto específico por ID.

### POST /api/v1/produtos
Cria um novo produto.

**Request Body:**
```json
{
  "nome": "Mouse Logitech",
  "preco": 89.90,
  "estoque": 50,
  "categoriaId": 2
}
```

### PUT /api/v1/produtos/{id}
Atualiza um produto existente.

### DELETE /api/v1/produtos/{id}
Remove um produto.

## 🏷️ Categorias

### GET /api/v1/categorias
Retorna lista de todas as categorias.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "nome": "Eletrônicos"
  },
  {
    "id": 2,
    "nome": "Acessórios"
  }
]
```

### GET /api/v1/categorias/{id}
Busca uma categoria específica por ID.

### POST /api/v1/categorias
Cria uma nova categoria.

**Request Body:**
```json
{
  "nome": "Periféricos"
}
```

### PUT /api/v1/categorias/{id}
Atualiza uma categoria existente.

### DELETE /api/v1/categorias/{id}
Remove uma categoria.

## 💵 Caixa

### POST /api/v1/caixa/{id}/fechar
Fecha o caixa e calcula diferenças.

**Parameters:**
- `id` (path) - ID do caixa

**Request Body:**
```json
{
  "valorInformado": 1500.00
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "valorCalculado": 1485.50,
  "valorInformado": 1500.00,
  "diferenca": 14.50,
  "dataFechamento": "2025-10-20T18:30:00"
}
```

## 🏥 Health Check

### GET /health
Verifica o status da aplicação e suas dependências.

**Response (200 Healthy):**
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0234567",
  "entries": {
    "PostgreSQL": {
      "status": "Healthy",
      "duration": "00:00:00.0123456"
    }
  }
}
```

## ⚠️ Códigos de Status HTTP

| Código | Descrição |
|--------|-----------|
| 200 OK | Requisição bem-sucedida |
| 201 Created | Recurso criado com sucesso |
| 204 No Content | Ação realizada sem conteúdo de retorno |
| 400 Bad Request | Dados inválidos na requisição |
| 401 Unauthorized | Token ausente ou inválido |
| 403 Forbidden | Sem permissão para acessar o recurso |
| 404 Not Found | Recurso não encontrado |
| 500 Internal Server Error | Erro interno do servidor |

## 📝 Validações

### Conta a Pagar
- `descricao`: Obrigatório, máximo 500 caracteres
- `valor`: Obrigatório, maior que 0.01
- `dataVencimento`: Obrigatório, formato ISO 8601

### Produto
- `nome`: Obrigatório, máximo 200 caracteres
- `preco`: Obrigatório, maior que 0.01
- `estoque`: Opcional, maior ou igual a 0

### Categoria
- `nome`: Obrigatório, máximo 100 caracteres

## 🔄 Paginação

Endpoints que retornam listas suportam paginação:

**Query Parameters:**
- `page` - Número da página (padrão: 1)
- `pageSize` - Itens por página (padrão: 10, máximo: 100)

**Exemplo:**
```
GET /api/v1/produtos?page=2&pageSize=20
```

**Response:**
```json
{
  "items": [...],
  "page": 2,
  "pageSize": 20,
  "totalItems": 150,
  "totalPages": 8
}
```

## 🌐 CORS

A API está configurada para aceitar requisições das seguintes origens:
- `http://localhost:3000` (desenvolvimento)
- `https://app.empresax.com` (produção)

## 🔍 Filtros e Ordenação

Alguns endpoints suportam filtros e ordenação via query parameters:

**Exemplo:**
```
GET /api/v1/contas?status=pendente&ordenarPor=dataVencimento&direcao=asc
```

## 📊 Rate Limiting

A API possui limitação de taxa para prevenir abuso:
- 100 requisições por minuto por IP
- 1000 requisições por hora por usuário autenticado

**Response (429 Too Many Requests):**
```json
{
  "erro": "Limite de requisições excedido. Tente novamente em 60 segundos."
}
```

## 🛠️ Testando a API

### Via Swagger UI
1. Acesse `https://localhost:5245/swagger`
2. Clique em "Authorize" e insira seu token
3. Teste os endpoints diretamente na interface

### Via cURL
```bash
# Login
curl -X POST "https://localhost:5245/api/v1/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"usuario@example.com","senha":"senha123"}'

# Listar contas (com token)
curl -X GET "https://localhost:5245/api/v1/contas" \
  -H "Authorization: Bearer {seu-token}"
```

### Via Postman
Importe a collection Swagger em `https://localhost:5245/swagger/v1/swagger.json`

## 📧 Suporte

Para dúvidas ou problemas com a API:
- Email: support@empresax.com
- Issues: GitHub Repository

---

**Última atualização:** Outubro 2025
