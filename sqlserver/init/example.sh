#!/bin/bash
# 等待 SQL Server 啟動並準備接受連接
echo "等待 SQL Server 啟動..."
echo "等待 SQL Server 啟動...2"
/opt/mssql-tools18/bin/sqlcmd -S "tcp:localhost,1433;TrustServerCertificate=True" -U sa -P $MSSQL_SA_PASSWORD -Q "SELECT 1"


#/opt/mssql-tools18/bin/sqlcmd -S localhost -U testuser -C -P Test@User123
#/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -C -P AdminPassword0000