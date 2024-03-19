using MassTransit;
using PosTech.CadPac.Domain.Repositories;
using PosTech.CadPac.MicroService.Consumer;
using PosTech.CadPac.MicroService.Consumer.Eventos;
using PosTech.CadPac.Repository;
using PosTech.CadPac.Repository.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        var conexao = Environment.GetEnvironmentVariable("postechazappconfiguration") ?? string.Empty;
        config.AddAzureAppConfiguration(conexao);
    })
    .ConfigureServices((hostcontext, services) =>
    {
        var configuration = hostcontext.Configuration;

        var conexao = configuration["postechcadpac:masstransit:azurebus"] ?? string.Empty;
        var filaHistorico = configuration.GetSection("MedicalHistoryAzureBus")["QueueName"] ?? string.Empty;
        var filaPaciente = configuration.GetSection("PatientAzureBus")["QueueName"] ?? string.Empty;

        services.AddHostedService<Worker>();

        services.AddRepositories(configuration);

        services.AddScoped<IPacienteRepository, PacienteRepository>();

        services.AddMassTransit(x =>
        {
            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(conexao);

                cfg.ReceiveEndpoint(filaHistorico, e =>
                {
                    e.ConfigureConsumer<MedicalHistoryOperationConsumer>(context);
                });

                cfg.ReceiveEndpoint(filaPaciente, e =>
                {
                    e.ConfigureConsumer<PatienteOperationConsumer>(context);
                });
            });
            
            x.AddConsumer<MedicalHistoryOperationConsumer>();
            x.AddConsumer<PatienteOperationConsumer>();
        });
    })
    .Build();

host.Run();
