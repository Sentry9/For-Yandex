FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Loans.Host/Loans.Host.csproj", "src/Loans.Host/"]
RUN dotnet restore "src/Loans.Host/Loans.Host.csproj"
COPY . .
WORKDIR "/src/src/Loans.Host"
RUN dotnet build "Loans.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Loans.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Loans.Host.dll"]