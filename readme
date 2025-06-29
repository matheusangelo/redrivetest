create-sqs
aws sqs ls --endpoint-url=http://localhost:4566


get sqs url
aws sqs get-queue-url --queue-name='sqs-app' --endpoint-url=http://localhost:4566

aws sqs get-queue-url --queue-name='sqs-dlq-app' --endpoint-url=http://localhost:4566



send message sqs

aws sqs send-message --queue-url=http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/sqs-app --message-body='teste mensagem dlq' --endpoint-url=http://localhost:4566

recieve message dlq

aws sqs receive-message --queue-url=http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/sqs-dlq-app --endpoint-url=http://localhost:4566

aws sqs receive-message --queue-url=http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/sqs-app --endpoint-url=http://localhost:4566

  
  