FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Dotnet6.EFCore6.Record.ValueObject.WebAPI/Dotnet6.EFCore6.Record.ValueObject.WebAPI.csproj", "Dotnet6.EFCore6.Record.ValueObject.WebAPI/"]
RUN dotnet restore "Dotnet6.EFCore6.Record.ValueObject.WebAPI/Dotnet6.EFCore6.Record.ValueObject.WebAPI.csproj"
COPY . .
WORKDIR "/src/Dotnet6.EFCore6.Record.ValueObject.WebAPI"
RUN dotnet build "Dotnet6.EFCore6.Record.ValueObject.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dotnet6.EFCore6.Record.ValueObject.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dotnet6.EFCore6.Record.ValueObject.WebAPI.dll"]
