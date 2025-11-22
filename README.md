# C# .NET 8.0 调用 Mathematica 14.3 示例项目

## 项目说明

本项目演示如何使用 C# .NET 8.0 调用 Mathematica 14.3，包含以下功能：

- ✅ 单个参数调用（整数、字符串）
- ✅ 多个参数调用（混合类型）
- ✅ 复杂参数调用（List 类型）
- ✅ 不同返回值类型（整数、浮点数、字符串、数组）
- ✅ 同步调用
- ✅ 异步调用
- ✅ 并发异步调用

## 环境要求

1. **Mathematica 14.3** 或更高版本
2. **.NET 8.0 SDK** 或更高版本
3. **Windows 操作系统**（推荐，也可以在 Mac/Linux 上运行，需要调整路径）
4. **Visual Studio 2022** 或 **VS Code**（可选）

## 安装步骤

### 1. 安装 Mathematica 14.3

确保 Mathematica 已正确安装在系统中。默认安装路径：
```
C:\Program Files\Wolfram Research\Mathematica\14.3\
```

### 2. 找到 Wolfram.NETLink.dll

Wolfram.NETLink.dll 通常位于：
```
C:\Program Files\Wolfram Research\Mathematica\14.3\SystemFiles\Links\NETLink\Wolfram.NETLink.dll
```

### 3. 修改项目文件

打开 `MathematicaDemo.csproj`，确认 `Wolfram.NETLink.dll` 的路径正确：

```xml
<Reference Include="Wolfram.NETLink">
  <HintPath>C:\Program Files\Wolfram Research\Mathematica\14.3\SystemFiles\Links\NETLink\Wolfram.NETLink.dll</HintPath>
</Reference>
```

### 4. 修改 MathKernel 路径

打开 `MathematicaHelper.cs`，确认 MathKernel.exe 的路径正确：

```csharp
mathematicaPath = @"C:\Program Files\Wolfram Research\Mathematica\14.3\MathKernel.exe";
```

**Mac 用户请使用：**
```csharp
mathematicaPath = "/Applications/Mathematica.app/Contents/MacOS/MathKernel";
```

**Linux 用户请使用：**
```csharp
mathematicaPath = "/usr/local/Wolfram/Mathematica/14.3/Executables/MathKernel";
```

## 编译和运行

### 使用命令行

```bash
cd MathematicaDemo
dotnet build
dotnet run
```

### 使用 Visual Studio

1. 打开 `MathematicaDemo.csproj`
2. 按 F5 运行

## 项目结构

```
MathematicaDemo/
├── MathematicaDemo.csproj      # 项目文件
├── Program.cs                   # 主程序，包含所有示例
├── MathematicaHelper.cs        # Mathematica 辅助类
├── MathematicaFunctions.m      # Mathematica 自定义函数
└── README.md                    # 本文档
```

## 示例说明

### 示例 1-3：单参数调用
- **示例 1**：单个整数参数，返回数值（平方）
- **示例 2**：单个整数参数，返回数值（阶乘）
- **示例 3**：单个字符串参数，返回字符串（问候语）

### 示例 4-6：多参数调用
- **示例 4**：两个整数参数，返回数值（加法、乘法）
- **示例 5**：混合参数（整数、字符串、浮点数），返回字符串
- **示例 6**：两个字符串参数，返回字符串（连接）

### 示例 7-11：复杂参数和返回值
- **示例 7**：List 参数，返回数值（求和）
- **示例 8**：List 参数，返回字符串（统计信息）
- **示例 9**：生成序列，返回数组
- **示例 10**：复杂列表分析，返回 JSON 格式字符串
- **示例 11**：多个数值参数，返回浮点数（复杂计算）

### 示例 12-15：异步调用
- **示例 12**：异步调用 - 单个整数参数
- **示例 13**：异步调用 - 字符串参数
- **示例 14**：异步调用 - List 参数
- **示例 15**：并发异步调用多个函数

### 示例 16：直接调用内置函数
- 计算 Pi（高精度）
- 矩阵行列式
- 求解方程

## 自定义 Mathematica 函数

在 `MathematicaFunctions.m` 中定义了以下函数：

1. **AddNumbers[a, b]** - 两数相加
2. **Multiply[a, b]** - 两数相乘
3. **SquareNumber[n]** - 计算平方
4. **Factorial[n]** - 计算阶乘
5. **GreetUser[name]** - 生成问候语
6. **ConcatStrings[str1, str2]** - 连接字符串
7. **ProcessMultipleParams[num, str, factor]** - 处理多参数
8. **SumList[list]** - 列表求和
9. **ProcessComplexList[list]** - 列表统计分析
10. **GenerateSequence[start, end]** - 生成序列
11. **AnalyzeList[list]** - 列表分析（JSON 格式）
12. **ComplexCalculation[x, y, z]** - 复杂计算

## 使用技巧

### 1. 参数传递

**整数：**
```csharp
math.ExecuteForInteger("MathematicaFunctions`SquareNumber[5]")
```

**字符串：**
```csharp
math.ExecuteForString("MathematicaFunctions`GreetUser[\"张三\"]")
```

**List：**
```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
string listStr = "{" + string.Join(", ", numbers) + "}";
math.ExecuteForInteger($"MathematicaFunctions`SumList[{listStr}]")
```

### 2. 返回值类型

- `ExecuteForInteger()` - 返回整数
- `ExecuteForDouble()` - 返回浮点数
- `ExecuteForString()` - 返回字符串
- `ExecuteForIntArray()` - 返回整数数组
- `ExecuteCommand()` - 返回字符串（通用）

### 3. 异步调用

```csharp
// 单个异步任务
int result = await math.ExecuteForIntegerAsync("...");

// 并发执行多个任务
var task1 = math.ExecuteForIntegerAsync("...");
var task2 = math.ExecuteForStringAsync("...");
await Task.WhenAll(task1, task2);
```

## 常见问题

### Q1: 找不到 Wolfram.NETLink.dll
**A:** 检查 Mathematica 安装路径，确保 `.csproj` 中的引用路径正确。

### Q2: 内核启动失败
**A:** 检查 `MathematicaHelper.cs` 中的 `mathematicaPath` 是否指向正确的 `MathKernel.exe`。

### Q3: 函数包加载失败
**A:** 确保 `MathematicaFunctions.m` 文件在输出目录中，检查项目文件中的 `CopyToOutputDirectory` 设置。

### Q4: 平台目标错误
**A:** Mathematica .NET/Link 需要 x64 平台。在项目属性中设置 `Platform Target` 为 `x64`。

### Q5: Mac/Linux 运行问题
**A:** 修改 `MathematicaHelper.cs` 中的路径为对应操作系统的路径。

## 性能优化建议

1. **复用内核连接**：避免频繁创建和销毁 `MathematicaHelper` 实例
2. **异步调用**：对于耗时计算，使用异步方法避免阻塞
3. **批量处理**：尽量在一次调用中处理多个数据
4. **缓存结果**：对于重复计算，考虑缓存结果

## 扩展建议

1. 添加错误处理和重试机制
2. 实现连接池管理
3. 添加日志记录功能
4. 支持更多 Mathematica 数据类型（如符号表达式）
5. 创建更高级的类型转换器

## 许可证

本示例项目仅供学习参考使用。Mathematica 是 Wolfram Research 的商业软件，使用需遵守其许可协议。

## 参考资料

- [Wolfram .NET/Link 官方文档](https://reference.wolfram.com/language/NETLink/tutorial/Overview.html)
- [Mathematica 文档中心](https://www.wolfram.com/mathematica/)
- [.NET 8.0 文档](https://docs.microsoft.com/dotnet/)

## 联系方式

如有问题或建议，请提交 Issue 或 Pull Request。
