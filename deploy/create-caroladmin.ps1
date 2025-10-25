cd "C:\Users\Carol\OneDrive - PUCRS - BR\Área de Trabalho\Projetos\EmpresaX.POS.API"

# Script PowerShell para criar usuário no PostgreSQL Azure via psql
# Requer psql instalado e senha do admin

$env:PGPASSWORD = "SUA_SENHA_ADMIN"
$psqlPath = "psql" # ajuste se necessário
$pgHost = "empresax-pg-21160.postgres.database.azure.com"
$port = 5432
$db = "empresaxposdb"
$user = "postgres"

$sql = @"
CREATE USER caroladmin WITH PASSWORD 'Carol2025!Test';
ALTER USER caroladmin WITH SUPERUSER;
GRANT ALL PRIVILEGES ON DATABASE empresaxposdb TO caroladmin;
"@

& $psqlPath -h $pgHost -p $port -U $user -d $db -c $sql
