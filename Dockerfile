FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ./src/EnterpriseInventoryApi/EnterpriseInventoryApi.csproj ./src/EnterpriseInventoryApi/
RUN dotnet restore ./src/EnterpriseInventoryApi/EnterpriseInventoryApi.csproj
COPY . .
WORKDIR /src/src/EnterpriseInventoryApi
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EnterpriseInventoryApi.dll"]
