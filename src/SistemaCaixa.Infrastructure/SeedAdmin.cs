using SistemaCaixa.Domain.Entities;

public static class SeedAdmin
{
    public static void SeedUsuarioAdmin(AppDbContext context)
    {
        if (!context.Usuarios.Any(u => u.Email == "admin@empresa.com"))
        {
            var admin = new Usuario
            {
                Email = "admin@empresa.com",
                Nome = "Administrador",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("SenhaForte123"),
                Perfil = PerfilUsuario.Admin
            };
            context.Usuarios.Add(admin);
            context.SaveChanges();
        }
    }
}
