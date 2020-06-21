FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Acme.csproj", "./"]
RUN dotnet restore "./Acme.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Acme.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Acme.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:80
ENTRYPOINT ["dotnet", "Acme.dll"]
