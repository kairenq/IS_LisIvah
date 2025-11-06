@echo off
chcp 65001 >nul
title Приемная комиссия - Запуск
color 0A

echo.
echo ╔═══════════════════════════════════════════════════╗
echo ║   ИНФОРМАЦИОННАЯ СИСТЕМА "ПРИЕМНАЯ КОМИССИЯ"      ║
echo ╚═══════════════════════════════════════════════════╝
echo.
echo Запуск программы...
echo.

cd /d "%~dp0"

if exist "AdmissionSystem\bin\Release\net6.0-windows\AdmissionSystem.exe" (
    echo ✓ Найден скомпилированный файл
    start "" "AdmissionSystem\bin\Release\net6.0-windows\AdmissionSystem.exe"
    echo.
    echo Программа запущена!
    timeout /t 2 >nul
) else if exist "AdmissionSystem\bin\Debug\net6.0-windows\AdmissionSystem.exe" (
    echo ✓ Найден Debug файл
    start "" "AdmissionSystem\bin\Debug\net6.0-windows\AdmissionSystem.exe"
    echo.
    echo Программа запущена!
    timeout /t 2 >nul
) else (
    echo ✗ Ошибка: Исполняемый файл не найден!
    echo.
    echo Пожалуйста, сначала скомпилируйте проект:
    echo 1. Запустите "Сборка.bat"
    echo 2. Или откройте проект в Visual Studio и выполните Build
    echo.
    pause
)
