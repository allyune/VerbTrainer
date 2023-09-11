using System;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VerbTrainerEmail.Application.SendPasswordResetEmail.Handler;
using VerbTrainerEmail.Infrastructure.Messaging.Configuration;
using VerbTrainerMessaging;

public class ConsumerHostedService : BackgroundService
{
    private readonly IRabbitMqConnectionFactory _factoryBuilder;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private IConnection _connection;
    private IModel _channel;

    public ConsumerHostedService(
        ILogger<ConsumerHostedService> logger,
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        IRabbitMqConnectionFactory connectionFactory)
    {
        _logger = logger;
        _factoryBuilder = connectionFactory;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        InitRabbitMQ();
    }

    private void InitRabbitMQ()
    {

        var factory = _factoryBuilder.CreateConnectionFactory();

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        Console.WriteLine(_connection);
        Console.WriteLine(_channel);

        string exchangeName = CentralExchange.SetupExchange(_channel);

        foreach (var queueSettings in _configuration.GetSection("Queues").GetChildren())
        {
            var queueName = queueSettings["Name"];
            var routingKeys = queueSettings.GetSection("RoutingKeys").Get<string[]>();

            _channel.QueueDeclare(
                queue: queueName,
                durable: bool.Parse(queueSettings["Durable"]),
                exclusive: bool.Parse(queueSettings["Exclusive"]),
                autoDelete: bool.Parse(queueSettings["AutoDelete"])
            );

            foreach (var routingKey in routingKeys)
            {
                _channel.QueueBind(
                    queue: queueName,
                    exchange: exchangeName,
                    routingKey: routingKey
                );
            }
        }

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = System.Text.Encoding.UTF8.GetString(body);
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IPasswordResetEmailHandler>();
                await context.SendPasswordResetEmail(message);
            }

   
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        consumer.Shutdown += OnConsumerShutdown;
        consumer.Registered += OnConsumerRegistered;
        consumer.Unregistered += OnConsumerUnregistered;
        consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

        _channel.BasicConsume(queue: "password_reminder_queue", autoAck: false, consumer: consumer);
        return Task.CompletedTask;
    }

    private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
    private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
    private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
    private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}