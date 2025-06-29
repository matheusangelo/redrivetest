# Teste de Redrive com AWS SQS e DLQ

Este projeto tem como objetivo testar o comportamento de redrive (reenvio de mensagens) do Amazon SQS utilizando Dead Letter Queue (DLQ). A aplicação é uma Console App desenvolvida em .NET que simula falhas forçadas para envio de mensagens para a DLQ após o número máximo de tentativas.

## Tecnologias Utilizadas

- .NET (Console Application)
- AWS SQS + DLQ
- Terraform (infraestrutura como código)
- LocalStack (emulador local da AWS)
- Docker

## Funcionamento

1. **Infraestrutura**:
   - Criação de duas filas SQS: uma principal e uma DLQ.
   - A fila principal possui configuração de redrive para a DLQ após X tentativas.
   - Tudo provisionado via Terraform.

2. **Aplicação**:
   - A aplicação consome mensagens da fila principal.
   - Força uma exceção durante o processamento para simular uma falha.
   - Após o número máximo de tentativas, a mensagem é automaticamente movida para a DLQ.

## Requisitos

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Terraform](https://developer.hashicorp.com/terraform/downloads)
- [Docker](https://www.docker.com/)
- [LocalStack CLI](https://docs.localstack.cloud/get-started/)

## Como Executar

### 1. Subir o LocalStack com Docker

```bash
docker run --rm -it -p 4566:4566 -p 4571:4571 localstack/localstack
```

### 2. Aplicar a infraestrutura com Terraform

```bash
cd terraform
terraform init
terraform apply
```

### 3. Executar a aplicação

```bash
cd src/RedriveTestApp
dotnet run
```

A aplicação começará a consumir as mensagens da fila principal, e, ao forçar a exceção, enviará as mensagens para a DLQ após o número de tentativas configurado.
```

## Configurações

A quantidade máxima de tentativas antes da mensagem ser redirecionada para a DLQ pode ser ajustada no Terraform:

```hcl
redrive_policy = jsonencode({
  deadLetterTargetArn = aws_sqs_queue.dlq.arn
  maxReceiveCount     = 3
})
```

## Observações

- Certifique-se de que a aplicação esteja apontando para o endpoint local do SQS em `http://localhost:4566`.
- A DLQ é útil para lidar com mensagens que não podem ser processadas após várias tentativas, permitindo análise e correção posterior.

---


## Comandos Úteis com AWS CLI (LocalStack)

Abaixo estão os principais comandos para interação manual com as filas SQS no LocalStack usando a AWS CLI.

### 📦 Criar Filas SQS via Script (Exemplo)

```bash
create-sqs

aws sqs list-queues --endpoint-url=http://localhost:4566

# Fila principal
aws sqs get-queue-url --queue-name='sqs-app' --endpoint-url=http://localhost:4566

# Dead Letter Queue (DLQ)
aws sqs get-queue-url --queue-name='sqs-dlq-app' --endpoint-url=http://localhost:4566

aws sqs send-message \
  --queue-url=http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/sqs-app \
  --message-body='teste mensagem dlq' \
  --endpoint-url=http://localhost:4566

# Da DLQ
aws sqs receive-message \
  --queue-url=http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/sqs-dlq-app \
  --endpoint-url=http://localhost:4566

# Da fila principal
aws sqs receive-message \
  --queue-url=http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/sqs-app \
  --endpoint-url=http://localhost:4566