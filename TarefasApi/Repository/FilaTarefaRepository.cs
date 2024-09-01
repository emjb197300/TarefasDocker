using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TarefasApi.Repository.Interfaces;

namespace TarefasApi.Repository
{
    public class FilaTarefaRepository : IFilaTarefaRepository
    {
        public void SendMsgTarefa(string msgTarefa)
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "tarefas",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(msgTarefa);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "tarefas",
                                 basicProperties: null,
                                 body: body);
        }

        public async Task<List<string>> RecieveMsgTarefa()
        {
            var listaMsgs = new List<string>();
            var factory = new ConnectionFactory { HostName = "rabbitmq" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "tarefas",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                listaMsgs.Add(message);
            };

            channel.BasicConsume(queue: "tarefas",
                                 autoAck: true,
                                 consumer: consumer);

            return await Task.Run(() => listaMsgs);
        }
    }
}
