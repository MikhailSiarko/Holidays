FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Holidays.API/Holidays.API.csproj", "src/Holidays.API/"]
RUN dotnet restore "src/Holidays.API/Holidays.API.csproj"
COPY . .
WORKDIR "/src/src/Holidays.API"
RUN dotnet build "Holidays.API.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Holidays.API.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Holidays.API.dll"]
