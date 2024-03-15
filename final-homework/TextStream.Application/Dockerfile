FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TextStream.Host/TextStream.Host.csproj", "src/TextStream.Host/"]
RUN dotnet restore "src/TextStream.Host/TextStream.Host.csproj"
COPY . .
WORKDIR "/src/src/TextStream.Host"
RUN dotnet build "TextStream.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TextStream.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TextStream.Host.dll"]
