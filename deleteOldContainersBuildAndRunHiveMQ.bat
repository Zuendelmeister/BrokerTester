docker-compose -f src/docker-compose-hivemq.yml down

docker rmi -f brokertester
dotnet restore src/
dotnet build src/
dotnet publish -c Release src/
cp .\configurations\configuration_Mqtt-Probability-Sinus.json .\src\bin\Release\netcoreapp3.1\publish\configuration.json
docker build -t brokertester -f src/Dockerfile .

docker-compose -f src/docker-compose-hivemq.yml up
