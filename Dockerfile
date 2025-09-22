FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
# instalar o .net -> rodar o EFCore -> banco de dados funciona :)
RUN apk add --no-cache dotnet9-sdk
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
ENV PATH=$PATH:/root/.dotnet/tools

RUN dotnet tool install --global dotnet-ef

COPY ["Preventech.Core/Preventech.Core.csproj", "Preventech.Core/"]
COPY ["Preventech.Server/Preventech.Server.csproj", "Preventech.Server/"]

RUN dotnet restore "Preventech.Server/Preventech.Server.csproj"

COPY . .
WORKDIR /src/Preventech.Server

RUN dotnet build "Preventech.Server.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Preventech.Server.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app

RUN dotnet tool install --global dotnet-ef

ENV PATH="$PATH:/root/.dotnet/tools"

COPY --from=publish /app/publish .

RUN echo "0   0   *   *   * "

ENTRYPOINT ["dotnet", "Preventech.Server.dll"]
