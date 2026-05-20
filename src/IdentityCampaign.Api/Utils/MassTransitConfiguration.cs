using IdentityCampaign.Application.Messaging.Events;
using MassTransit;

namespace IdentityCampaign.Api.Utils
{
    public class MassTransitConfiguration
    {
        public static void Configure(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            var host = Environment.GetEnvironmentVariable("RabbitMQ__Host") ?? "127.0.0.1";
            var user = Environment.GetEnvironmentVariable("RabbitMQ__Username") ?? "guest";
            var pass = Environment.GetEnvironmentVariable("RabbitMQ__Password") ?? "guest";

            cfg.Host(host, "/", h =>
            {
                h.Username(user);
                h.Password(pass);
            });

            cfg.UseMessageRetry(r =>
            {
                r.Interval(3, TimeSpan.FromSeconds(5));
            });

            cfg.Message<DonationReceivedEvent>(x =>
            {
                x.SetEntityName("donation-received");
            });

            cfg.ReceiveEndpoint("campaigns.donation-received", e =>
            {
                e.ConfigureConsumer<DonationReceivedConsumer>(context);
            });
        }
    }
}
