FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

EXPOSE 8080
ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install --global dotnet-ef
COPY . .

RUN dotnet ef migrations add docker --project Preventech.Core --startup-project Preventech.Server
RUN dotnet ef database update --project Preventech.Core --startup-project Preventech.Server
RUN dotnet restore
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app

COPY --from=build /app/out .

