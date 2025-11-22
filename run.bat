@echo off
chcp 65001 >nul
echo ====================================
echo C# 调用 Mathematica 示例程序
echo ====================================
echo.

echo 正在编译项目...
dotnet build --configuration Release

if %errorlevel% neq 0 (
    echo.
    echo 编译失败！请检查错误信息。
    pause
    exit /b 1
)

echo.
echo 编译成功！正在运行程序...
echo.

dotnet run --configuration Release

echo.
echo 程序执行完毕。
pause
