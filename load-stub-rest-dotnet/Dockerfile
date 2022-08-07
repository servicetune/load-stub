#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["load-stub-rest-dotnet/load-stub-rest-dotnet.csproj", "load-stub-rest-dotnet/"]
RUN dotnet restore "load-stub-rest-dotnet/load-stub-rest-dotnet.csproj"
COPY . .
WORKDIR "/src/load-stub-rest-dotnet"
RUN dotnet build "load-stub-rest-dotnet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "load-stub-rest-dotnet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "load-stub-rest-dotnet.dll"]