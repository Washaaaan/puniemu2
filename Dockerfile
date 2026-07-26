FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Puniemu.csproj ./
RUN dotnet restore Puniemu.csproj
COPY . ./

# --- DIAGNÓSTICO TEMPORAL ---
RUN echo "=== BUILD STAGE: listing /src ===" && ls -la /src
RUN echo "=== BUILD STAGE: dataDownload check ===" && (ls -la /src/dataDownload 2>&1 | head -5 || echo "NO EXISTE en /src")
RUN echo "=== BUILD STAGE: dataDownload file count ===" && (find /src/dataDownload -type f 2>&1 | wc -l || echo "0")
# --- FIN DIAGNÓSTICO ---

RUN dotnet publish Puniemu.csproj -c Release -o /app/publish

# --- DIAGNÓSTICO EN LA ETAPA FINAL TAMBIÉN ---
RUN echo "=== PUBLISH: dataDownload in output ===" && (find /app/publish/dataDownload -type f 2>&1 | wc -l || echo "0 en /app/publish")

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# --- DIAGNÓSTICO EN LA IMAGEN FINAL ---
RUN echo "=== FINAL IMAGE: dataDownload check ===" && (find /app/dataDownload -type f 2>&1 | wc -l || echo "0 en /app")

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Puniemu.dll"]