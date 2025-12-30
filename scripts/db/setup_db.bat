@echo off
set PGPASSWORD=postgres
echo Creating database 'travel_payment_db'...
psql -h localhost -U postgres -c "CREATE DATABASE travel_payment_db;"
if %ERRORLEVEL% equ 0 (
    echo Database created successfully.
) else (
    echo Failed to create database. Please check if PostgreSQL is running and credentials are correct.
)
pause
