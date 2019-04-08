FROM microsoft/dotnet:2.2-sdk AS build-env

ARG framework=netcoreapp2.2
ARG buildConfiguration=Release

WORKDIR /code

COPY src src
COPY tests tests
COPY CustomerInformationSystem.sln .

RUN dotnet restore -s https://www.nuget.org/api/v2/ 

RUN dotnet test tests/SmokeTests/SmokeTests.csproj --configuration ${buildConfiguration} --framework ${framework}

RUN dotnet publish src/Api/Api.csproj -o /build -c ${buildConfiguration}

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /build /app
ENV TZ=Europe/London
ENTRYPOINT ["dotnet", "Api.dll"]