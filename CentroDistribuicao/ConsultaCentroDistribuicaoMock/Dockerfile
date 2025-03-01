FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ConsultaCentroDistribuicaoMock.csproj", "."]
RUN dotnet restore "ConsultaCentroDistribuicaoMock.csproj"

COPY . .
WORKDIR "/src"

RUN dotnet build "ConsultaCentroDistribuicaoMock.csproj" -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
RUN ls -l /app

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "ConsultaCentroDistribuicaoMock.dll"]