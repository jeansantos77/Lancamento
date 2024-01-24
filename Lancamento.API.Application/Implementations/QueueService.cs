using Lancamento.API.Application.Interfaces;
using Lancamento.API.Domain.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace Lancamento.API.Application.Implementations
{
    public class QueueService : IQueueService
    {
        private readonly QueueConfig _queueConfig;

        public QueueService(IOptions<QueueConfig> queueConfig)
        {
            _queueConfig = queueConfig.Value;
        }

        public void PublishMessage(MessageQueue msg)
        {
            var factory = new ConnectionFactory
            {
                HostName = _queueConfig.HostName,
                Port = _queueConfig.Port,
                UserName = _queueConfig.UserName,
                Password = _queueConfig.Password
            };

            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "consolidadoQueue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string msgJson = JsonSerializer.Serialize(msg);
                    var body = Encoding.UTF8.GetBytes(msgJson);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "consolidadoQueue",
                                         basicProperties: null,
                                         body: body);
                }
            }
            catch 
            {
                throw new Exception("Erro ano conectar no RabbitMQ");
            }


        }

    }
}
