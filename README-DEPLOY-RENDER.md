# Deploy da API .NET 8 no Render (Docker)

## Pré-requisitos
- Conta no [Render](https://render.com)
- Projeto com Dockerfile e .render.yaml na raiz
- Repositório no GitHub

---

## Passo a Passo

### 1. Suba o código para o GitHub
- Inclua o `Dockerfile` e `.render.yaml` no commit.

### 2. Crie o serviço no Render
1. Acesse https://dashboard.render.com/
2. Clique em **New +** → **Web Service**
3. Conecte seu repositório GitHub
4. Render detecta o Dockerfile automaticamente
5. Configure:
   - **Branch:** main (ou a branch desejada)
   - **Root Directory:** (deixe vazio se o Dockerfile está na raiz)
   - **Port:** 8080
   - **Environment:**
     - `ASPNETCORE_ENVIRONMENT=Production`
     - `JWT_KEY` (adicione pelo painel, nunca no Dockerfile)
     - `ConnectionStrings__DefaultConnection` (adicione pelo painel)

### 3. Deploy e Teste
- Render irá buildar e publicar sua API.
- Acesse a URL gerada (ex: `https://empresa-x-pos-api.onrender.com`)
- Teste `/health` e `/swagger` se disponíveis.

### 4. Dicas
- Para banco de dados, use o serviço PostgreSQL do próprio Render ou configure a connection string para um banco externo.
- Configure variáveis sensíveis apenas pelo painel do Render.
- Se precisar de storage persistente, use Volumes do Render.

---

## Exemplo de .render.yaml
```yaml
services:
  - type: web
    name: empresax-pos-api
    env: docker
    plan: free
    autoDeploy: true
    envVars:
      - key: ASPNETCORE_ENVIRONMENT
        value: Production
      - key: JWT_KEY
        sync: false
      - key: ConnectionStrings__DefaultConnection
        sync: false
    healthCheckPath: /health
    disk:
      name: data
      mountPath: /data
```

---

## Troubleshooting
- Veja logs em **Logs** no painel do Render
- Se a porta não for 8080, ajuste o Dockerfile e as configurações
- Para dúvidas, consulte https://render.com/docs/deploy-docker
