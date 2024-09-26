### Build the Docker Image

To build the Docker image, run the following command in the directory containing the Dockerfile:

```bash
docker build -t your-image-name .
```

### Run the Docker Container

To run the SQL Server container, execute the following command:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 --name sqlserver -d your-image-name
```

Replace `YourStrong@Passw0rd` with a secure password of your choice. Ensure that the password meets SQL Server's security requirements.

### Connect to SQL Server

You can connect to the SQL Server instance using SQL Server Management Studio (SSMS) or any SQL client of your choice.

**Connection String Example:**

```plaintext
Server=localhost,1433;Database=master;User Id=SA;Password=YourStrong@Passw0rd;Encrypt=True;TrustServerCertificate=True;
```

### Stopping and Removing the Container

To stop the SQL Server container, use:

```bash
docker stop sqlserver
```

To remove the container, use:

```bash
docker rm sqlserver
```

## Troubleshooting

If you encounter issues connecting to the SQL Server, ensure the following:

- The SQL Server container is running.
- The port `1433` is not blocked by a firewall.
- You are using the correct connection string and credentials.