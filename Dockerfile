#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["twatter-userservice/twatter-userservice.csproj", "twatter-userservice/"]
RUN dotnet restore "twatter-userservice/twatter-userservice.csproj"
COPY . .
WORKDIR "/src/twatter-userservice"
RUN dotnet build "twatter-userservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "twatter-userservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "twatter-userservice.dll"]