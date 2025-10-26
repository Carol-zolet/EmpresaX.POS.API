# Rollback automático pós-deploy (Render)

# 1. Monitore endpoint de health após deploy
# 2. Se falhar, acione rollback via API Render

# Exemplo de script bash para CI/CD (pode ser adaptado para step do GitHub Actions)

#!/bin/bash
set -e
RENDER_API_KEY="$RENDER_API_KEY"
RENDER_SERVICE_ID="$RENDER_SERVICE_ID"
RENDER_DEPLOY_ID="$RENDER_DEPLOY_ID"

# Aguarda até 2 minutos pelo health
for i in {1..12}; do
  STATUS=$(curl -s -o /dev/null -w "%{http_code}" https://SEU-APP-RENDER.onrender.com/health)
  if [ "$STATUS" == "200" ]; then
    echo "Healthcheck OK! Deploy bem-sucedido."
    exit 0
  fi
  echo "Aguardando healthcheck... ($i)"
  sleep 10
done

echo "Healthcheck falhou! Iniciando rollback."
curl -X POST \
  -H "Authorization: Bearer $RENDER_API_KEY" \
  -H "Content-Type: application/json" \
  -d '{"rollback": true}' \
  https://api.render.com/v1/services/$RENDER_SERVICE_ID/deploys/$RENDER_DEPLOY_ID/rollback
exit 1
