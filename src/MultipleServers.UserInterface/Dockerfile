FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MultipleServers.UserInterface/MultipleServers.UserInterface.csproj", "MultipleServers.UserInterface/"]
RUN dotnet restore "MultipleServers.UserInterface/MultipleServers.UserInterface.csproj"
COPY . .
WORKDIR "/src/MultipleServers.UserInterface"
RUN dotnet build "MultipleServers.UserInterface.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MultipleServers.UserInterface.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MultipleServers.UserInterface.dll"]
