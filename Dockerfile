# Use the official SQL Server 2022 image from Microsoft
FROM mcr.microsoft.com/mssql/server:2022-latest

# Set environment variables for SQL Server
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=YourStrong@Passw0rd

# Expose port 1433 to allow connections to SQL Server
EXPOSE 1433

# Optionally, copy initialization scripts or files to the container
# COPY ./init.sql /usr/src/app/init.sql

# Run SQL Server process
CMD /opt/mssql/bin/sqlservr