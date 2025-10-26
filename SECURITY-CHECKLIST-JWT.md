# Checklist de Segurança para Deploy .NET + JWT

- [x] Endpoints sensíveis protegidos com `[Authorize]`
- [x] Endpoint de login com `[AllowAnonymous]`
- [x] JWT com chave secreta forte (mínimo 32 caracteres)
- [x] Chave secreta NUNCA commitada (usar User Secrets/env vars)
- [x] HTTPS obrigatório em produção
- [x] Senhas sempre com hash forte (BCrypt, Argon2)
- [x] Política de expiração de token (ex: 8h)
- [x] Claims mínimas no token (id, email, perfil)
- [x] Sem dados sensíveis no payload do JWT
- [x] CORS restrito para domínios confiáveis
- [x] Rate limiting em endpoints públicos
- [x] Logging estruturado sem expor dados sensíveis
- [x] Testes automatizados de login/autorização
- [x] Health check dos endpoints
- [x] Atualizações de dependências de segurança

**Dica:**
- Use Azure Key Vault, AWS Secrets Manager ou variáveis de ambiente para segredos.
- Sempre revise logs e alertas de segurança após o deploy.
