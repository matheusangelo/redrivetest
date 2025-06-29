using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.Runtime;

var queueUrl = "http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/sqs-app";

var sqsClient = new AmazonSQSClient(
    new BasicAWSCredentials("teste", "teste"),
    new AmazonSQSConfig
    {
        ServiceURL = "http://localhost:4566",
        AuthenticationRegion = "sa-east-1"
    });

Console.WriteLine("🔁 Iniciando leitura da fila SQS...");

while (true)
{
    var response = await sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
    {
        QueueUrl = queueUrl,
        MaxNumberOfMessages = 1,
        WaitTimeSeconds = 5
    });

    if (response.Messages.Count == 0)
    {
        Console.WriteLine("⏳ Nenhuma mensagem recebida...");
        await Task.Delay(2000);
        continue;
    }

    foreach (var message in response.Messages)
    {
        Console.WriteLine($"📨 Mensagem recebida: {message.Body}");

        try
        {
            // Simula um erro no processamento
            throw new Exception("Erro proposital!");

            // Se tudo tivesse dado certo:
            // await sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao processar: {ex.Message}");
            // Não deletar => SQS irá reenviar após visibilidade expirar
        }
    }

    await Task.Delay(1000); // evitar flood
}
