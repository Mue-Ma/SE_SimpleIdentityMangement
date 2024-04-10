dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx"  -p password
dotnet dev-certs https --trust
docker-compose up --build -d