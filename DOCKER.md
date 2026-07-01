# Docker notes

This project is an ASP.NET MVC application targeting .NET Framework 4.8. The web
application therefore needs a Windows IIS container image. The SQL Server image in
`docker-compose.yml` is the official Linux SQL Server container image.

## Database container

With Docker Desktop in Linux container mode:

```powershell
docker compose up -d db
```

SQL Server is exposed on `localhost,1433`.

Credentials:

- User: `sa`
- Password: `Your_strong_password123`
- Database: `DoctorLab`

The app uses EF migrations at startup, so the schema is created when the web app
first connects.

## Web container

The included `Dockerfile` builds the ASP.NET Framework 4.8 app into IIS:

```powershell
docker build -t webapplicationdoclab .
```

This requires Docker Desktop to be switched to Windows containers. It cannot be
built while Docker is in Linux container mode.

## Full stack limitation

Docker Desktop cannot run the Windows IIS container for this .NET Framework app
and the official Linux SQL Server container in the same local Docker engine at the
same time. To run the whole stack in one compose project, the application would
need to be migrated to ASP.NET Core so it can run in a Linux container, or the
database would need a supported Windows SQL Server container image.
