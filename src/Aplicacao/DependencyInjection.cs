using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado.Mensageria;
using GeradorCertificados.Aplicacao.Modulos.GerarCertificadoZip;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeradorCertificados.Aplicacao;

public static class DependencyInjection
{
    public static void AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        var connectionStringRabbitMq = configuration.GetConnectionString("RabbitMq")
            ?? throw new InvalidOperationException("A ConectionString \"RabbitMq\" Não foi configurada!");

        services.AddMassTransit(config =>
        {
            //Configura a injeção dos consumers

            config.AddConsumer<GerarCertificadosConsumer>();

            config.UsingRabbitMq((context, rabbitMq) =>
            {
                rabbitMq.Host(new Uri(connectionStringRabbitMq));

                rabbitMq.ReceiveEndpoint("certificados-solicitados", endpoit =>
                {
                    endpoit.PrefetchCount = 4; //quantas mensagens o rabbitmq deve carregar adiantado
                    endpoit.ConcurrentMessageLimit = 2; //quantas mensagens ele pode processar em paralelo (ao mesmo tempo) qnds de instancia consumers

                    endpoit.ConfigureConsumer<GerarCertificadosConsumer>(context);

                });
            });
        });
        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
        });

        services.AddScoped<GeradorPdfCertificao>();
        services.AddScoped<GeradorDeZipCertificados>();
    }
}
