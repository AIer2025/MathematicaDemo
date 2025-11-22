# Mathematica .NET/Link 快速参考

## 初始化

```csharp
var math = new MathematicaHelper();
math.Initialize();  // 或指定路径: math.Initialize("path/to/MathKernel.exe");
```

## 基本调用

### 整数返回值
```csharp
int result = math.ExecuteForInteger("2 + 3");  // 返回 5
```

### 浮点数返回值
```csharp
double result = math.ExecuteForDouble("N[Pi, 20]");  // 返回 Pi
```

### 字符串返回值
```csharp
string result = math.ExecuteForString("ToString[42]");  // 返回 "42"
```

### 数组返回值
```csharp
int[] result = math.ExecuteForIntArray("Range[1, 10]");  // 返回 {1,2,...,10}
```

### 通用返回值
```csharp
string result = math.ExecuteCommand("Solve[x^2 == 4, x]");
```

## 参数传递

### 单个整数
```csharp
int n = 5;
int result = math.ExecuteForInteger($"Factorial[{n}]");
```

### 单个字符串
```csharp
string name = "World";
string result = math.ExecuteForString($"\"Hello, \" <> \"{name}\"");
```

### 多个参数
```csharp
int a = 10, b = 20;
int result = math.ExecuteForInteger($"Max[{a}, {b}]");
```

### List 参数
```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
string listStr = "{" + string.Join(", ", numbers) + "}";
int sum = math.ExecuteForInteger($"Total[{listStr}]");
```

### 二维数组（矩阵）
```csharp
int[,] matrix = { {1, 2}, {3, 4} };
string matrixStr = "{{1, 2}, {3, 4}}";
int det = math.ExecuteForInteger($"Det[{matrixStr}]");
```

## 异步调用

### 基本异步
```csharp
int result = await math.ExecuteForIntegerAsync("Factorial[100]");
```

### 并发执行
```csharp
var task1 = math.ExecuteForIntegerAsync("Factorial[10]");
var task2 = math.ExecuteForDoubleAsync("N[Pi, 50]");
var task3 = math.ExecuteForStringAsync("ToString[E]");

await Task.WhenAll(task1, task2, task3);

Console.WriteLine($"结果1: {task1.Result}");
Console.WriteLine($"结果2: {task2.Result}");
Console.WriteLine($"结果3: {task3.Result}");
```

## 自定义函数调用

### 定义 Mathematica 函数
```mathematica
(* 在 .m 文件中 *)
MyFunction[x_] := x^2 + 2*x + 1
```

### C# 调用
```csharp
math.ExecuteCommand("Get[\"path/to/file.m\"]");
int result = math.ExecuteForInteger("MyFunction[5]");
```

## 常用 Mathematica 函数

| 功能 | Mathematica 代码 | C# 示例 |
|------|-----------------|---------|
| 求和 | `Total[{1,2,3}]` | `ExecuteForInteger("Total[{1,2,3}]")` |
| 平均值 | `Mean[{1,2,3}]` | `ExecuteForDouble("Mean[{1,2,3}]")` |
| 标准差 | `StandardDeviation[{1,2,3}]` | `ExecuteForDouble("StandardDeviation[{1,2,3}]")` |
| 最大值 | `Max[1,2,3]` | `ExecuteForInteger("Max[1,2,3]")` |
| 最小值 | `Min[1,2,3]` | `ExecuteForInteger("Min[1,2,3]")` |
| 排序 | `Sort[{3,1,2}]` | `ExecuteForIntArray("Sort[{3,1,2}]")` |
| 阶乘 | `Factorial[5]` | `ExecuteForInteger("Factorial[5]")` |
| 幂运算 | `Power[2, 10]` | `ExecuteForInteger("Power[2, 10]")` |
| 平方根 | `Sqrt[16]` | `ExecuteForDouble("Sqrt[16]")` |
| 绝对值 | `Abs[-5]` | `ExecuteForInteger("Abs[-5]")` |
| 求导 | `D[x^2, x]` | `ExecuteCommand("D[x^2, x]")` |
| 积分 | `Integrate[x^2, x]` | `ExecuteCommand("Integrate[x^2, x]")` |
| 求解 | `Solve[x^2==4, x]` | `ExecuteCommand("Solve[x^2==4, x]")` |
| 矩阵乘法 | `{{1,2}}.{{3},{4}}` | `ExecuteForIntArray("Flatten[{{1,2}}.{{3},{4}}]")` |

## 数据类型映射

| Mathematica | C# | 方法 |
|-------------|----|----|
| Integer | `int` | `ExecuteForInteger()` |
| Real | `double` | `ExecuteForDouble()` |
| String | `string` | `ExecuteForString()` |
| List (Int) | `int[]` | `ExecuteForIntArray()` |
| List (Real) | `double[]` | `ExecuteForDoubleArray()` |
| Symbol/Expr | `string` | `ExecuteCommand()` |

## 错误处理

```csharp
try
{
    int result = math.ExecuteForInteger("1/0");
}
catch (MathLinkException ex)
{
    Console.WriteLine($"MathLink 错误: {ex.Message}");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"操作错误: {ex.Message}");
}
```

## 资源清理

```csharp
// 方法1：using 语句（推荐）
using (var math = new MathematicaHelper())
{
    math.Initialize();
    // 使用 math...
}  // 自动释放

// 方法2：手动释放
var math = new MathematicaHelper();
try
{
    math.Initialize();
    // 使用 math...
}
finally
{
    math.Dispose();
}
```

## 性能提示

### ✅ 好的做法
```csharp
// 1. 复用连接
using var math = new MathematicaHelper();
math.Initialize();
for (int i = 0; i < 1000; i++)
{
    var result = math.ExecuteForInteger($"Factorial[{i}]");
}

// 2. 批量处理
int[] data = Enumerable.Range(1, 1000).ToArray();
string listStr = "{" + string.Join(", ", data) + "}";
int sum = math.ExecuteForInteger($"Total[{listStr}]");

// 3. 异步处理长时间操作
var result = await math.ExecuteForIntegerAsync("VeryLongComputation[]");
```

### ❌ 避免的做法
```csharp
// 1. 频繁创建销毁连接
for (int i = 0; i < 1000; i++)
{
    using var math = new MathematicaHelper();  // ❌ 慢
    math.Initialize();
    var result = math.ExecuteForInteger($"Factorial[{i}]");
}

// 2. 逐个处理大量数据
for (int i = 0; i < 1000; i++)  // ❌ 慢
{
    var result = math.ExecuteForInteger($"SomeFunction[{i}]");
}

// 3. 同步阻塞长时间操作
var result = math.ExecuteForInteger("VeryLongComputation[]");  // ❌ 阻塞UI
```

## 调试命令

```csharp
// 查看 Mathematica 版本
string version = math.ExecuteCommand("$Version");

// 查看内存使用
int memory = math.ExecuteForInteger("MemoryInUse[]");

// 查看已加载的包
string packages = math.ExecuteCommand("$Packages");

// 获取上一个输出
string lastOut = math.ExecuteCommand("Out[-1]");

// 清除所有变量
math.ExecuteCommand("ClearAll[\"Global`*\"]");
```

## 路径处理

```csharp
// Windows 路径转 Mathematica 格式
string winPath = @"C:\Users\Documents\file.txt";
string mathPath = winPath.Replace("\\", "/");  // C:/Users/Documents/file.txt

// 使用
math.ExecuteCommand($"data = Import[\"{mathPath}\"]");
```

## 常见错误

| 错误 | 原因 | 解决方法 |
|------|------|---------|
| `MathLinkException` | 连接错误 | 检查 MathKernel 路径 |
| `FileNotFoundException` | 找不到 DLL | 检查 Wolfram.NETLink.dll 路径 |
| `Platform target mismatch` | 平台不匹配 | 设置为 x64 |
| `Timeout` | 计算时间过长 | 使用异步或增加超时时间 |
| `Invalid expression` | 语法错误 | 检查 Mathematica 语法 |

## 配置检查清单

- [ ] Mathematica 14.3 已安装
- [ ] .NET 8.0 SDK 已安装
- [ ] Wolfram.NETLink.dll 路径正确
- [ ] MathKernel.exe 路径正确
- [ ] 项目平台目标设置为 x64
- [ ] MathematicaFunctions.m 文件在输出目录
- [ ] 防火墙允许 MathKernel 运行

## 许可证检查

```csharp
// 检查 Mathematica 许可证
string license = math.ExecuteCommand("$LicenseExpirationDate");
Console.WriteLine($"许可证到期日: {license}");
```

## 快速故障排除

```bash
# 1. 验证 Mathematica 安装
"C:\Program Files\Wolfram Research\Mathematica\14.3\MathKernel.exe"

# 2. 编译项目
dotnet build

# 3. 检查引用
dotnet list reference

# 4. 运行程序
dotnet run
```

## 更多资源

- 官方文档: https://reference.wolfram.com/language/NETLink/
- API 参考: https://reference.wolfram.com/language/
- 社区: https://community.wolfram.com/
