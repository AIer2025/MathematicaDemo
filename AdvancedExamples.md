# 高级示例和最佳实践

## 目录
1. [错误处理](#错误处理)
2. [性能优化](#性能优化)
3. [数据类型转换](#数据类型转换)
4. [复杂数据结构](#复杂数据结构)
5. [图形和可视化](#图形和可视化)
6. [并行计算](#并行计算)

## 错误处理

### 示例：优雅的错误处理

```csharp
public class SafeMathematicaHelper : MathematicaHelper
{
    public async Task<(bool success, int result, string error)> SafeExecuteForIntegerAsync(string command)
    {
        try
        {
            int result = await ExecuteForIntegerAsync(command);
            return (true, result, string.Empty);
        }
        catch (MathLinkException ex)
        {
            return (false, 0, $"MathLink 错误: {ex.Message}");
        }
        catch (Exception ex)
        {
            return (false, 0, $"未知错误: {ex.Message}");
        }
    }
}

// 使用示例
var (success, result, error) = await helper.SafeExecuteForIntegerAsync("1/0");
if (!success)
{
    Console.WriteLine($"计算失败: {error}");
}
```

### 示例：超时处理

```csharp
public async Task<T> ExecuteWithTimeoutAsync<T>(
    Func<Task<T>> function, 
    int timeoutMs = 5000)
{
    using var cts = new CancellationTokenSource();
    var task = function();
    var timeoutTask = Task.Delay(timeoutMs, cts.Token);
    
    var completedTask = await Task.WhenAny(task, timeoutTask);
    
    if (completedTask == timeoutTask)
    {
        throw new TimeoutException("Mathematica 计算超时");
    }
    
    cts.Cancel();
    return await task;
}

// 使用示例
try
{
    var result = await ExecuteWithTimeoutAsync(
        () => math.ExecuteForIntegerAsync("Factorial[100000]"),
        timeoutMs: 10000
    );
}
catch (TimeoutException)
{
    Console.WriteLine("计算超时");
}
```

## 性能优化

### 示例：批量处理

```csharp
// 在 Mathematica 端定义批量处理函数
// BatchProcess[data_List] := Map[SomeFunction, data]

public async Task<int[]> BatchProcessAsync(int[] data)
{
    string listStr = "{" + string.Join(", ", data) + "}";
    return await ExecuteForIntArrayAsync($"BatchProcess[{listStr}]");
}
```

### 示例：结果缓存

```csharp
public class CachedMathematicaHelper : MathematicaHelper
{
    private readonly Dictionary<string, object> _cache = new();
    
    public async Task<T> ExecuteWithCacheAsync<T>(
        string command, 
        Func<string, Task<T>> executor)
    {
        if (_cache.TryGetValue(command, out var cached))
        {
            return (T)cached;
        }
        
        var result = await executor(command);
        _cache[command] = result!;
        return result;
    }
}

// 使用示例
var result = await cachedHelper.ExecuteWithCacheAsync(
    "Factorial[100]",
    cmd => math.ExecuteForIntegerAsync(cmd)
);
```

## 数据类型转换

### 示例：处理二维数组（矩阵）

```csharp
// Mathematica 函数
MatrixSum[matrix_List] := Total[Flatten[matrix]]

// C# 调用
public int CalculateMatrixSum(int[,] matrix)
{
    int rows = matrix.GetLength(0);
    int cols = matrix.GetLength(1);
    
    StringBuilder sb = new StringBuilder("{");
    for (int i = 0; i < rows; i++)
    {
        sb.Append("{");
        for (int j = 0; j < cols; j++)
        {
            sb.Append(matrix[i, j]);
            if (j < cols - 1) sb.Append(",");
        }
        sb.Append("}");
        if (i < rows - 1) sb.Append(",");
    }
    sb.Append("}");
    
    return ExecuteForInteger($"MatrixSum[{sb}]");
}

// 使用
int[,] matrix = { {1, 2, 3}, {4, 5, 6}, {7, 8, 9} };
int sum = CalculateMatrixSum(matrix);
```

### 示例：处理浮点数数组

```csharp
public double[] ExecuteForDoubleArray(string command)
{
    _kernelLink.Evaluate(command);
    _kernelLink.WaitForAnswer();
    return _kernelLink.GetDoubleArray();
}

// 使用
double[] array = math.ExecuteForDoubleArray("N[{Pi, E, Sqrt[2]}, 10]");
```

## 复杂数据结构

### 示例：处理关联数组（Association）

```mathematica
(* Mathematica 函数 *)
CreateUserProfile[name_String, age_Integer, email_String] := 
  <|"name" -> name, "age" -> age, "email" -> email|>

GetProfileJSON[profile_Association] := 
  ExportString[profile, "JSON"]
```

```csharp
// C# 调用
public string CreateUserProfileJSON(string name, int age, string email)
{
    string command = $@"
        profile = CreateUserProfile[""{name}"", {age}, ""{email}""];
        GetProfileJSON[profile]
    ";
    return ExecuteForString(command);
}

// 使用
string json = CreateUserProfileJSON("张三", 30, "zhangsan@example.com");
Console.WriteLine(json);
// 输出: {"name":"张三","age":30,"email":"zhangsan@example.com"}
```

### 示例：处理嵌套结构

```csharp
// Mathematica: 返回嵌套列表
GenerateNestedData[n_Integer] := 
  Table[{i, i^2, i^3}, {i, 1, n}]

// C# 解析
public List<(int x, int y, int z)> GetNestedData(int n)
{
    string result = ExecuteCommand($"GenerateNestedData[{n}]");
    // 解析 Mathematica 输出: {{1, 1, 1}, {2, 4, 8}, ...}
    // 这里需要自定义解析逻辑
    return ParseMathematicaList(result);
}
```

## 图形和可视化

### 示例：导出图形为 PNG

```mathematica
(* Mathematica 函数 *)
PlotAndExport[func_String, path_String] := 
  Module[{plot, expr},
    expr = ToExpression[func];
    plot = Plot[expr, {x, -10, 10}, ImageSize -> 800];
    Export[path, plot, "PNG"]
  ]
```

```csharp
// C# 调用
public void CreatePlot(string function, string outputPath)
{
    outputPath = outputPath.Replace("\\", "/");
    ExecuteCommand($@"PlotAndExport[""{function}"", ""{outputPath}""]");
}

// 使用
CreatePlot("Sin[x] * Cos[x]", "C:/temp/plot.png");
```

### 示例：生成 3D 图形

```csharp
public void Create3DPlot(string function, string outputPath)
{
    outputPath = outputPath.Replace("\\", "/");
    string command = $@"
        plot = Plot3D[{function}, {{x, -3, 3}}, {{y, -3, 3}}, 
                      ImageSize -> 800, PlotStyle -> Automatic];
        Export[""{outputPath}"", plot, ""PNG""]
    ";
    ExecuteCommand(command);
}
```

## 并行计算

### 示例：并行映射

```mathematica
(* Mathematica 并行函数 *)
ParallelMapFunc[func_, data_List] := 
  ParallelMap[func, data]

ParallelSquareList[data_List] := 
  ParallelMap[#^2 &, data]
```

```csharp
// C# 调用并行函数
public async Task<int[]> ParallelSquareAsync(int[] data)
{
    string listStr = "{" + string.Join(", ", data) + "}";
    return await ExecuteForIntArrayAsync(
        $"ParallelSquareList[{listStr}]"
    );
}

// 使用
int[] data = Enumerable.Range(1, 1000).ToArray();
int[] results = await ParallelSquareAsync(data);
```

### 示例：分布式计算配置

```csharp
public void ConfigureParallelKernels(int numKernels = 4)
{
    ExecuteCommand($"LaunchKernels[{numKernels}]");
    Console.WriteLine($"已启动 {numKernels} 个并行内核");
}

public void CloseParallelKernels()
{
    ExecuteCommand("CloseKernels[]");
    Console.WriteLine("已关闭所有并行内核");
}
```

## 完整示例：数据分析管道

```csharp
public class DataAnalysisPipeline
{
    private readonly MathematicaHelper _math;
    
    public DataAnalysisPipeline()
    {
        _math = new MathematicaHelper();
        _math.Initialize();
    }
    
    public async Task<AnalysisResult> AnalyzeDataAsync(double[] data)
    {
        string listStr = "{" + string.Join(", ", data) + "}";
        
        // 并行执行多个统计分析
        var meanTask = _math.ExecuteForDoubleAsync($"Mean[{listStr}]");
        var medianTask = _math.ExecuteForDoubleAsync($"Median[{listStr}]");
        var stdTask = _math.ExecuteForDoubleAsync($"StandardDeviation[{listStr}]");
        var minTask = _math.ExecuteForDoubleAsync($"Min[{listStr}]");
        var maxTask = _math.ExecuteForDoubleAsync($"Max[{listStr}]");
        
        await Task.WhenAll(meanTask, medianTask, stdTask, minTask, maxTask);
        
        return new AnalysisResult
        {
            Mean = meanTask.Result,
            Median = medianTask.Result,
            StandardDeviation = stdTask.Result,
            Min = minTask.Result,
            Max = maxTask.Result
        };
    }
    
    public void Dispose()
    {
        _math.Dispose();
    }
}

public class AnalysisResult
{
    public double Mean { get; set; }
    public double Median { get; set; }
    public double StandardDeviation { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    
    public override string ToString()
    {
        return $"Mean: {Mean:F2}, Median: {Median:F2}, " +
               $"StdDev: {StandardDeviation:F2}, " +
               $"Range: [{Min:F2}, {Max:F2}]";
    }
}

// 使用
using var pipeline = new DataAnalysisPipeline();
var data = new double[] { 12.5, 18.3, 25.7, 31.2, 45.8, 52.1 };
var result = await pipeline.AnalyzeDataAsync(data);
Console.WriteLine(result);
```

## 调试技巧

### 启用详细日志

```csharp
public void EnableDebugMode()
{
    ExecuteCommand("$MessagePrePrint = InputForm");
    ExecuteCommand("On[Assert]");
}
```

### 查看 Mathematica 输出

```csharp
public string GetLastOutput()
{
    return ExecuteCommand("Out[-1]");
}
```

### 检查内核状态

```csharp
public string GetKernelInfo()
{
    return ExecuteCommand("$Version");
}

public int GetMemoryUsage()
{
    return ExecuteForInteger("MemoryInUse[]");
}
```

## 最佳实践总结

1. **始终使用 using 语句**确保资源正确释放
2. **异步优先**：对于耗时操作使用异步方法
3. **批量处理**：尽量减少调用次数
4. **错误处理**：捕获并处理所有可能的异常
5. **缓存结果**：对于重复计算使用缓存
6. **超时控制**：为长时间运行的任务设置超时
7. **连接复用**：避免频繁创建销毁连接
8. **类型安全**：使用强类型方法而不是通用方法
9. **路径格式**：Windows 路径转换为 Mathematica 格式（正斜杠）
10. **并行计算**：利用 Mathematica 的并行能力处理大数据

## 性能基准

典型操作的性能参考：

| 操作 | 时间 |
|------|------|
| 启动内核 | 2-5 秒 |
| 简单计算（加法） | <1 毫秒 |
| 复杂计算（阶乘1000） | 10-50 毫秒 |
| 列表处理（10000元素） | 50-200 毫秒 |
| 符号计算 | 100-1000 毫秒 |
| 图形生成 | 500-2000 毫秒 |

注意：实际性能取决于硬件配置和具体操作复杂度。
