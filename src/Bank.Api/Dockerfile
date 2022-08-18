FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Bank.Services.Api/Bank.Services.Api.csproj", "Bank.Services.Api/"]
RUN dotnet restore "Bank.Services.Api/Bank.Services.Api.csproj"
COPY . .
WORKDIR "/src/Bank.Services.Api"
RUN dotnet build "Bank.Services.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Bank.Services.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Bank.Services.Api.dll"]