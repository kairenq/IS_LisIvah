@echo off
chcp 65001 >nul
title Приемная комиссия - Сборка проекта
color 0B

echo.
echo ╔═══════════════════════════════════════════════════╗
echo ║        СБОРКА ПРОЕКТА "ПРИЕМНАЯ КОМИССИЯ"         ║
echo ╚═══════════════════════════════════════════════════╝
echo.

cd /d "%~dp0"

echo Проверка наличия .NET SDK...
where dotnet >nul 2>&1
if %errorlevel% neq 0 (
    echo.
    echo ✗ ОШИБКА: .NET SDK не найден!
    echo.
    echo Пожалуйста, установите .NET 6.0 SDK или выше:
    echo https://dotnet.microsoft.com/download
    echo.
    echo После установки запустите этот файл снова.
    echo.
    pause
    exit /b 1
)

echo ✓ .NET SDK найден
echo.

echo Восстановление пакетов NuGet...
dotnet restore AdmissionSystem\AdmissionSystem.csproj
if %errorlevel% neq 0 (
    echo.
    echo ✗ Ошибка при восстановлении пакетов!
    pause
    exit /b 1
)

echo.
echo Сборка проекта в режиме Release...
dotnet build AdmissionSystem\AdmissionSystem.csproj -c Release
if %errorlevel% neq 0 (
    echo.
    echo ✗ Ошибка при сборке проекта!
    pause
    exit /b 1
)

echo.
echo ╔═══════════════════════════════════════════════════╗
echo ║            ✓ СБОРКА ЗАВЕРШЕНА УСПЕШНО!            ║
echo ╚═══════════════════════════════════════════════════╝
echo.
echo Теперь вы можете запустить программу через "Запуск.bat"
echo.
pause
