#!/bin/bash
# 等待 SQL Server 啟動並準備接受連接
echo "等待 SQL Server 啟動..."
# until /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -C -P $MSSQL_SA_PASSWORD -Q "SELECT 1" & > /dev/null; do
until /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -C -P $MSSQL_SA_PASSWORD -Q "SELECT 1" & > /dev/null; do
    echo "SQL Server 尚未啟動，等待中..."
    sleep 1
done

# 執行初始化腳本
echo "SQL Server 已啟動，開始執行初始化腳本..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -C -P $MSSQL_SA_PASSWORD -i /init/init.sql 
echo "初始化完成！"
