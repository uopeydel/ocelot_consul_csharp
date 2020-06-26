
docker pull rabbitmq:3-management

docker run -d -p 15672:15672 -p 5672:5672 --name rabbit-test-for-medium rabbitmq:3-management


http://localhost:15672/#/


guest guest