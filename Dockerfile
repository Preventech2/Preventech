FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .
RUN dotnet restore
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine-composite AS runtime
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 5001

ENTRYPOINT ["dotnet", "Preventech.dll"]