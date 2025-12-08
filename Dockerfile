FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
RUN apk add --no-cache dotnet9-sdk
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
ENV PATH=$PATH:/root/.dotnet/tools

RUN dotnet workload install wasm-tools
COPY ["Preventech.Core/Preventech.Core.csproj", "Preventech.Core/"]
COPY ["Preventech.Server/Preventech.Server.csproj", "Preventech.Server/"]

RUN dotnet restore "Preventech.Server/Preventech.Server.csproj"

COPY Preventech.sln Preventech.sln
COPY Preventech.Core/ Preventech.Core/
COPY Preventech.Server/ Preventech.Server/
WORKDIR /src/Preventech.Server

RUN dotnet build "Preventech.Server.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "Preventech.Server.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app

ENV PATH="$PATH:/root/.dotnet/tools"

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Preventech.Server.dll"]
