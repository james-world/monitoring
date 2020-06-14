FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Demo3/src/DotNetSample/DotNetSample.csproj", "Demo3/src/DotNetSample/"]
RUN dotnet restore "Demo3/src/DotNetSample/DotNetSample.csproj"
COPY . .
WORKDIR "/src/Demo3/src/DotNetSample"
RUN dotnet build "DotNetSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetSample.dll"]
