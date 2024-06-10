dotnet tool install --global dotnet-ef --version 6.0.7

dotnet ef migrations add init

dotnet ef migrations remove

dotnet ef database update