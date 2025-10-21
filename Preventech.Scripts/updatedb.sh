#sempre atualizar migration
dotnet ef migrations add M5 --project Preventech.Core --startup-project Preventech.Server
dotnet ef database update --project Preventech.Core --startup-project Preventech.Server --connection 'Host=localhost;Port=5432;Database=Preventech;Username=preventech;Password=1234'