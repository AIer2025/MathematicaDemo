#!/bin/bash

echo "===================================="
echo "C# 调用 Mathematica 示例程序"
echo "===================================="
echo ""

echo "正在编译项目..."
dotnet build --configuration Release

if [ $? -ne 0 ]; then
    echo ""
    echo "编译失败！请检查错误信息。"
    exit 1
fi

echo ""
echo "编译成功！正在运行程序..."
echo ""

dotnet run --configuration Release

echo ""
echo "程序执行完毕。"
