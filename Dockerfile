FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/ ./
RUN dotnet publish Simplify.csproj -c Release -o /out -p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0
COPY --from=build /out /app
ENTRYPOINT ["dotnet", "/app/Simplify.dll"]
