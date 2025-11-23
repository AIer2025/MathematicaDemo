# C# .NET 8.0 调用 Mathematica 14.3 示例项目

## 📑 目录导航

- [项目说明](#项目说明)
- [环境要求](#环境要求)
- [安装步骤](#安装步骤)
- [编译和运行](#编译和运行)
- [项目结构](#项目结构)
- [示例说明](#示例说明) ⭐含最新示例 17、18
- [自定义 Mathematica 函数](#自定义-mathematica-函数)
- [使用技巧](#使用技巧)
- [常见问题](#常见问题)
- [性能优化建议](#性能优化建议)
- [扩展建议](#扩展建议)
- [版本历史](#版本历史) ⭐查看最新更新

---

## 项目说明

本项目演示如何使用 C# .NET 8.0 调用 Mathematica 14.3，包含 **18 个完整示例**，涵盖以下功能：

- ✅ 单个参数调用（整数、字符串）
- ✅ 多个参数调用（混合类型）
- ✅ 复杂参数调用（List 类型）
- ✅ 不同返回值类型（整数、浮点数、字符串、数组）
- ✅ 同步调用
- ✅ 异步调用
- ✅ 并发异步调用
- ✅ 3D 图像生成与保存（**新增**）
- ✅ 复杂嵌套函数调用（**新增**）

## 环境要求

1. **Mathematica 14.3** 或更高版本
2. **.NET 8.0 SDK** 或更高版本
3. **Windows 操作系统**（推荐，也可以在 Mac/Linux 上运行，需要调整路径）
4. **Visual Studio 2022** 或 **VS Code**（可选）

## 安装步骤

### 1. 安装 Mathematica 14.3

确保 Mathematica 已正确安装在系统中。默认安装路径：
```
C:\Program Files\Wolfram Research\Wolfram\14.3
```

### 2. 找到 Wolfram.NETLink.dll

Wolfram.NETLink.dll 通常位于：
```
C:\Program Files\Wolfram Research\Wolfram\14.3\SystemFiles\Links\NETLink\Wolfram.NETLink.dll
```

### 3. 修改项目文件

打开 `MathematicaDemo.csproj`，确认 `Wolfram.NETLink.dll` 的路径正确：

```xml
<Reference Include="Wolfram.NETLink">
  <HintPath>C:\Program Files\Wolfram Research\Wolfram\14.3\SystemFiles\Links\NETLink\Wolfram.NETLink.dll</HintPath>
</Reference>
```

### 4. 修改 MathKernel 路径

打开 `MathematicaHelper.cs`，确认 MathKernel.exe 的路径正确：

```csharp
mathematicaPath = @"C:\Program Files\Wolfram Research\Wolfram\14.3\MathKernel.exe";
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
├── Program.cs                   # 主程序，包含所有 18 个示例
├── MathematicaHelper.cs        # Mathematica 辅助类
├── MathematicaFunctions.m      # Mathematica 自定义函数（含嵌套函数）
├── appsettings.json            # 配置文件
├── run.bat / run.sh            # 运行脚本
├── README.md                    # 本文档
└── UPDATE_NOTES.md             # 更新说明（v1.1 新增内容）
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

### 示例 17：3D 图像生成 ⭐**新增**
- 生成 3D 图形（Plot3D）
- 导出为 JPG 图像文件
- 保存到桌面
- 演示图像字节流处理

### 示例 18：复杂嵌套函数调用 ⭐**新增**
- 主函数调用 5 个辅助函数
- 数据验证和清洗
- 基本和高级统计分析
- 数据分类和过滤
- 生成综合分析报告
- 演示模块化函数设计

## 自定义 Mathematica 函数

在 `MathematicaFunctions.m` 中定义了以下函数：

### 基础函数（示例 1-11）
1. **AddNumbers[a, b]** - 两数相加
2. **Multiply[a, b]** - 两数相乘
3. **SquareNumber[n]** - 计算平方
4. **ComputeFactorial[n]** - 计算阶乘
5. **GreetUser[name]** - 生成问候语
6. **ConcatStrings[str1, str2]** - 连接字符串
7. **ProcessMultipleParams[num, str, factor]** - 处理多参数
8. **SumList[list]** - 列表求和
9. **ProcessComplexList[list]** - 列表统计分析
10. **GenerateSequence[start, end]** - 生成序列
11. **AnalyzeList[list]** - 列表分析（JSON 格式）
12. **ComplexCalculation[x, y, z]** - 复杂计算

### 图像处理函数（示例 17）⭐**新增**
13. **GenerateSamplePlot[]** - 生成 3D 示例图形

### 复杂嵌套函数（示例 18）⭐**新增**
14. **ComplexFuncCall[dataList, threshold]** - 主函数，执行复杂数据分析
    - **ValidateData[dataList]** - 辅助函数：数据验证和清洗
    - **CalculateBasicStats[dataList]** - 辅助函数：基本统计量计算
    - **CalculateAdvancedStats[dataList]** - 辅助函数：高级统计量计算
    - **FilterByThreshold[dataList, threshold]** - 辅助函数：数据分类过滤
    - **GenerateReport[...]** - 辅助函数：生成综合报告

### ComplexFuncCall 函数调用链
```
ComplexFuncCall[dataList, threshold]
  ├─> ValidateData[dataList]          // 步骤1: 清洗数据
  ├─> CalculateBasicStats[validData]  // 步骤2: 基本统计
  ├─> CalculateAdvancedStats[...]     // 步骤3: 高级统计
  ├─> FilterByThreshold[...]          // 步骤4: 数据分类
  └─> GenerateReport[...]             // 步骤5: 生成报告
```

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

### 4. 图像生成与保存 ⭐**新增**

```csharp
// 生成 3D 图像
string plotCmd = "Plot3D[Sin[x*y], {x, -2, 2}, {y, -2, 2}]";
byte[] imageBytes = await math.ExecuteForImageBytesAsync(plotCmd, "JPG");

// 保存到文件
string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
string filePath = Path.Combine(desktopPath, "plot.jpg");
await File.WriteAllBytesAsync(filePath, imageBytes);
```

### 5. 复杂嵌套函数调用 ⭐**新增**

```csharp
// 准备数据和参数
double[] data = { 10.5, 25.3, -5.2, 42.8, 18.7, 0, 33.1, 51.4 };
string dataStr = "{" + string.Join(", ", data) + "}";
double threshold = 30.0;

// 调用复杂函数（内部会调用5个辅助函数）
string report = math.ExecuteForString(
    $"MathematicaFunctions`ComplexFuncCall[{dataStr}, {threshold}]");

// 输出分析报告
Console.WriteLine(report);
```

**输出示例：**
```
=== Data Analysis Report ===
Original data count: 8
Cleaned data count: 6

--- Basic Statistics ---
Sum: 186.9
Mean: 31.15
Median: 29.35
Max: 51.4
Min: 10.5

--- Advanced Statistics ---
Variance: 205.4
Std Dev: 14.33
Range: 40.9
Q1 (25%): 21.9
Q3 (75%): 40.25

--- Threshold Analysis ---
Values above threshold: 3
Values below threshold: 3
===========================
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

### Q6: 图像生成失败 ⭐**新增**
**A:** 
- 确保 Mathematica 图形系统正常工作
- 检查临时文件目录是否有写入权限
- 某些图形可能需要特定的图形库支持

### Q7: ComplexFuncCall 返回空结果 ⭐**新增**
**A:** 
- 检查输入数据是否包含有效的数值
- ValidateData 会过滤掉非数值和负数
- 确保至少有一些正数在数据列表中

### Q8: 图像文件保存位置 ⭐**新增**
**A:** 默认保存到桌面，可以通过修改 `Environment.GetFolderPath` 的参数来改变保存位置。

## 性能优化建议

1. **复用内核连接**：避免频繁创建和销毁 `MathematicaHelper` 实例
2. **异步调用**：对于耗时计算，使用异步方法避免阻塞
3. **批量处理**：尽量在一次调用中处理多个数据
4. **缓存结果**：对于重复计算，考虑缓存结果

## 扩展建议

### 基础扩展
1. 添加错误处理和重试机制
2. 实现连接池管理
3. 添加日志记录功能
4. 支持更多 Mathematica 数据类型（如符号表达式）
5. 创建更高级的类型转换器

### 图像处理扩展 ⭐**新增**
1. 支持更多图像格式（PNG、SVG、PDF）
2. 批量生成图像
3. 实时图像预览
4. 图像参数动态调整
5. 生成动画（GIF）

### 数据分析扩展 ⭐**新增**
1. 添加更多统计指标（偏度、峰度、相关性）
2. 实现数据可视化（图表生成）
3. 支持多维数据分析
4. 添加机器学习算法调用
5. 实现时间序列分析
6. 生成 Excel 报表
7. 导出 JSON/XML 格式结果

### 性能优化 ⭐**新增**
1. 实现数据分批处理
2. 添加结果缓存机制
3. 优化大数据集处理
4. 实现并行计算支持

## 许可证

本示例项目仅供学习参考使用。Mathematica 是 Wolfram Research 的商业软件，使用需遵守其许可协议。

## 参考资料

- [Wolfram .NET/Link 官方文档](https://reference.wolfram.com/language/NETLink/tutorial/Overview.html)
- [Mathematica 文档中心](https://www.wolfram.com/mathematica/)
- [.NET 8.0 文档](https://docs.microsoft.com/dotnet/)

## 版本历史

### v1.1 (2024-11-23) ⭐**最新版本**
**新增功能：**
- ✅ 示例 17：3D 图像生成与保存
  - 支持将 Mathematica 图形导出为 JPG
  - 实现图像字节流处理
  - 自动保存到桌面
  
- ✅ 示例 18：复杂嵌套函数调用
  - `ComplexFuncCall` 主函数
  - 5 个辅助函数（ValidateData、CalculateBasicStats 等）
  - 完整的数据分析工作流
  - 综合报告生成

**改进：**
- 📝 更新所有文档
- 📝 添加详细的代码注释
- 📝 新增 UPDATE_NOTES.md

### v1.0 (初始版本)
- 基础的 16 个示例
- 同步/异步调用
- 多种数据类型支持

## 联系方式

如有问题或建议，请提交 Issue 或 Pull Request。
