using GeradorCertificados.Dominio.Compartilhado.Auth;
using GeradorCertificados.Infraestrutura.Compartilhado.Auth;
using GeradorCertificados.Infraestrutura.Compartilhado.Orm;
using GeradorCertificados.Infraestrutura.Modulos.Cursos;
using GeradorCertificados.Infraestrutura.Modulos.GeracaoCertificados;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace GeradorCertificados.Infraestrutura;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IGerenciadorDeIdentidade, GerenciadorDeIdentidade>();
        services.AddScoped<IRepositorioCurso, RepositorioCursoEmOrm>();
        services.AddScoped<IRepositorioCertificados, RepositorioCertificadoEmOrm>();
        services.AddScoped<IRepositorioSolitacaoCertificados, RepositorioSolitacaoCertificadosEmOrm>();

        services.AddDataProtection();

        services.AddIdentityCore<IdentityUser<Guid>>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<GeradorCertificadosDbContext>();

        services.AddDbContext<GeradorCertificadosDbContext>(options =>
        {
            var connection = configuration.GetConnectionString("SqlServer");

            if (string.IsNullOrWhiteSpace(connection))
                throw new InvalidOperationException("Connection string \"SqlServer\" não configura.");
                
            options.UseSqlServer(connection);
        });

        //Uso de Postgres

        // services.AddDbContext<GeradorCertificadosDbContext>(options =>
        // {
        //     if (configuration["Infra:DatabaseProvider"] == "InMemory")
        //     {
        //         options.UseInMemoryDatabase("GeradorCertificados");
        //     }
        //     else
        //     {
        //         string? connectionString = configuration.GetConnectionString("PostgresEF");

        //         if (string.IsNullOrWhiteSpace(connectionString))
        //         {
        //             throw new InvalidOperationException(
        //                 $"A connection string \"PostgresEF\" não foi encontrada."
        //             );
        //         }

        //         options.UseNpgsql(connectionString, opt =>
        //         {
        //             opt.EnableRetryOnFailure(3);
        //         });
        //     }
        // });
    }
}