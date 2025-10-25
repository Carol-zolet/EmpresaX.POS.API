
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore src/EmpresaX.POS.API/EmpresaX.POS.API.csproj
RUN dotnet build src/EmpresaX.POS.API/EmpresaX.POS.API.csproj -c Release -o /app/build
RUN dotnet publish src/EmpresaX.POS.API/EmpresaX.POS.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "EmpresaX.POS.API.dll"]
