@echo off
setlocal enabledelayedexpansion
cd /d %~dp0

:: .envファイルの読み込み
if not exist "..\..\.env" (
    echo .env file not found. Please create one from .env.example.
    pause
    exit /b 1
)

for /f "usebackq eol=# tokens=1* delims==" %%i in ("..\..\.env") do (
    set %%i=%%j
)

:: 変数が設定されているか確認
if "%POSTGRES_USER%"=="" (
    echo POSTGRES_USER is not set in .env
    pause
    exit /b 1
)
if "%POSTGRES_PASSWORD%"=="" (
    echo POSTGRES_PASSWORD is not set in .env
    pause
    exit /b 1
)
if "%POSTGRES_DB%"=="" (
    echo POSTGRES_DB is not set in .env
    pause
    exit /b 1
)

:: psqlコマンドの解決
set "PSQL_CMD=psql"
where psql >nul 2>nul
if %ERRORLEVEL% neq 0 (
    if defined POSTGRES_BIN_PATH (
        set "PSQL_CMD=!POSTGRES_BIN_PATH!\psql.exe"
    ) else (
        echo 'psql' command not found in PATH.
        echo Please add PostgreSQL bin directory to PATH or set POSTGRES_BIN_PATH in .env file.
        echo Example: POSTGRES_BIN_PATH="C:\Program Files\PostgreSQL\18\bin"
        pause
        exit /b 1
    )
)

:: 1. ログインユーザー(ロール)の作成
echo 1. Creating database role '%POSTGRES_USER%'...
set PGPASSWORD=postgres
"!PSQL_CMD!" -h localhost -U postgres -v user=%POSTGRES_USER% -v password=%POSTGRES_PASSWORD% -f create_login_user.sql
if %ERRORLEVEL% neq 0 (
    echo Failed to create role.
    pause
    exit /b 1
)
echo 1. Database role created successfully.

:: 2. データベースの再作成 (既存のDBを削除して作り直す)
echo 2. Re-creating database '%POSTGRES_DB%'...

:: 既存のDBがあれば削除 (接続強制切断)
"!PSQL_CMD!" -h localhost -U postgres -c "DROP DATABASE IF EXISTS \"%POSTGRES_DB%\" WITH (FORCE);"
if %ERRORLEVEL% neq 0 (
    echo Failed to drop database.
    pause
    exit /b 1
)

:: DB作成
"!PSQL_CMD!" -h localhost -U postgres -c "CREATE DATABASE \"%POSTGRES_DB%\" OWNER \"%POSTGRES_USER%\";"
if %ERRORLEVEL% neq 0 (
    echo Failed to create database.
    pause
    exit /b 1
)
echo Database created successfully.

:: 3. スキーマの作成 (EF Core Migration)
echo 3. Applying EF Core migrations...
pushd ..\..\
:: .envの設定を使って接続文字列を上書きする
set "ConnectionStrings__DefaultConnection=Host=localhost;Database=%POSTGRES_DB%;Username=%POSTGRES_USER%;Password=%POSTGRES_PASSWORD%"
dotnet ef database update --project src/Infrastructure --startup-project src/API
if %ERRORLEVEL% neq 0 (
    echo Failed to apply migrations.
    popd
    pause
    exit /b 1
)
popd
echo 3. EF Core migrations applied successfully.

:: 4. 初期データの投入
echo 4. Inserting initial data...
set PGPASSWORD=%POSTGRES_PASSWORD%
"!PSQL_CMD!" -h localhost -U %POSTGRES_USER% -d %POSTGRES_DB% -f insert_initial_data.sql
if %ERRORLEVEL% neq 0 (
    echo Failed to insert initial data.
    pause
    exit /b 1
)
echo 4. Initial data inserted successfully.
echo Setup completed successfully.
pause
