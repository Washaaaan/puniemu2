Despliegue rápido

Requisitos:
- Docker y Docker Compose (opcional si usas systemd)
- Base de datos Postgres existente con el esquema aplicado (Database/schema.sql)

Opciones:

1) Usando Docker (recomendado)

Editar `docker-compose.yml` y actualizar la variable `PostgresConnectionString` con la cadena de conexión a tu Postgres.

Construir y levantar:

```bash
docker compose build
docker compose up -d
```

2) Ejecutar localmente sin contenedores

Exporta la variable de entorno y ejecuta:

```bash
export PostgresConnectionString="Host=mi-host;Database=puniemu;Username=postgres;Password=mi-pass"
export PORT=8080
dotnet publish -c Release -o out
cd out
dotnet Puniemu.dll
```

Notas:
- La aplicación lee `PostgresConnectionString` desde `appsettings.json` si existe, o desde la variable de entorno `PostgresConnectionString`.
- Si usas systemd, crea un servicio que establezca las variables de entorno necesarias y ejecute `dotnet Puniemu.dll` desde la carpeta `out`.
- Si ves errores relacionados con "render", indícame el mensaje exacto o el log para investigarlo.
