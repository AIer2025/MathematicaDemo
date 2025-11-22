using System;
using System.Threading.Tasks;
using MathematicaDemo;

namespace MathematicaDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=".PadRight(80, '='));
            Console.WriteLine("C# .NET 8.0 调用 Mathematica 14.3 示例程序");
            Console.WriteLine("=".PadRight(80, '='));
            Console.WriteLine();

            using (var math = new MathematicaHelper())
            {
                try
                {
                    // 初始化 Mathematica 内核
                    math.Initialize();
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 1: 单个整数参数，返回数值（同步）
                    // ====================================================================
                    Console.WriteLine("【示例 1】单个整数参数，返回数值（同步）");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    int num1 = 5;
                    int squareResult = math.ExecuteForInteger($"MathematicaFunctions`SquareNumber[{num1}]");
                    Console.WriteLine($"输入: {num1}");
                    Console.WriteLine($"SquareNumber({num1}) = {squareResult}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 2: 单个整数参数，返回数值（阶乘）
                    // ====================================================================
                    Console.WriteLine("【示例 2】单个整数参数，返回数值（阶乘）");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    int num2 = 6;
                    int factorialResult = math.ExecuteForInteger($"MathematicaFunctions`Factorial[{num2}]");
                    Console.WriteLine($"输入: {num2}");
                    Console.WriteLine($"Factorial({num2}) = {factorialResult}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 3: 单个字符串参数，返回字符串（同步）
                    // ====================================================================
                    Console.WriteLine("【示例 3】单个字符串参数，返回字符串（同步）");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    string userName = "张三";
                    string greeting = math.ExecuteForString($"MathematicaFunctions`GreetUser[\"{userName}\"]");
                    Console.WriteLine($"输入: \"{userName}\"");
                    Console.WriteLine($"结果: {greeting}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 4: 多个整数参数，返回数值（同步）
                    // ====================================================================
                    Console.WriteLine("【示例 4】多个整数参数，返回数值（同步）");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    int a = 15;
                    int b = 8;
                    int addResult = math.ExecuteForInteger($"MathematicaFunctions`AddNumbers[{a}, {b}]");
                    int multiplyResult = math.ExecuteForInteger($"MathematicaFunctions`Multiply[{a}, {b}]");
                    Console.WriteLine($"输入: a={a}, b={b}");
                    Console.WriteLine($"AddNumbers({a}, {b}) = {addResult}");
                    Console.WriteLine($"Multiply({a}, {b}) = {multiplyResult}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 5: 多个参数（整数、字符串、浮点数），返回字符串
                    // ====================================================================
                    Console.WriteLine("【示例 5】多个参数（整数、字符串、浮点数），返回字符串");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    int number = 100;
                    string description = "测试数据";
                    double factor = 1.5;
                    string processResult = math.ExecuteForString(
                        $"MathematicaFunctions`ProcessMultipleParams[{number}, \"{description}\", {factor}]");
                    Console.WriteLine($"输入: number={number}, description=\"{description}\", factor={factor}");
                    Console.WriteLine($"结果: {processResult}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 6: 两个字符串参数，返回字符串
                    // ====================================================================
                    Console.WriteLine("【示例 6】两个字符串参数，返回字符串");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    string str1 = "Hello";
                    string str2 = "World";
                    string concatResult = math.ExecuteForString(
                        $"MathematicaFunctions`ConcatStrings[\"{str1}\", \"{str2}\"]");
                    Console.WriteLine($"输入: str1=\"{str1}\", str2=\"{str2}\"");
                    Console.WriteLine($"结果: {concatResult}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 7: List 参数，返回数值（同步）
                    // ====================================================================
                    Console.WriteLine("【示例 7】List 参数，返回数值（同步）");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    int[] numbers = { 10, 20, 30, 40, 50 };
                    string listStr = "{" + string.Join(", ", numbers) + "}";
                    int sumResult = math.ExecuteForInteger($"MathematicaFunctions`SumList[{listStr}]");
                    Console.WriteLine($"输入列表: {listStr}");
                    Console.WriteLine($"SumList = {sumResult}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 8: List 参数，返回字符串（统计信息）
                    // ====================================================================
                    Console.WriteLine("【示例 8】List 参数，返回字符串（统计信息）");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    double[] dataList = { 12.5, 18.3, 25.7, 31.2, 45.8, 52.1 };
                    string dataListStr = "{" + string.Join(", ", dataList) + "}";
                    string analysisResult = math.ExecuteForString(
                        $"MathematicaFunctions`ProcessComplexList[{dataListStr}]");
                    Console.WriteLine($"输入列表: {dataListStr}");
                    Console.WriteLine($"分析结果: {analysisResult}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 9: 生成序列，返回数组
                    // ====================================================================
                    Console.WriteLine("【示例 9】生成序列，返回数组");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    int start = 1;
                    int end = 10;
                    int[] sequence = math.ExecuteForIntArray(
                        $"MathematicaFunctions`GenerateSequence[{start}, {end}]");
                    Console.WriteLine($"输入: start={start}, end={end}");
                    Console.WriteLine($"生成序列: [{string.Join(", ", sequence)}]");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 10: 复杂列表分析，返回 JSON 格式字符串
                    // ====================================================================
                    Console.WriteLine("【示例 10】复杂列表分析，返回 JSON 格式字符串");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    int[] dataSet = { 85, 92, 78, 95, 88, 73, 90, 86, 94, 89 };
                    string dataSetStr = "{" + string.Join(", ", dataSet) + "}";
                    string jsonResult = math.ExecuteForString(
                        $"MathematicaFunctions`AnalyzeList[{dataSetStr}]");
                    Console.WriteLine($"输入数据集: {dataSetStr}");
                    Console.WriteLine($"JSON 结果: {jsonResult}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 11: 复杂计算（多个数值参数），返回浮点数
                    // ====================================================================
                    Console.WriteLine("【示例 11】复杂计算（多个数值参数），返回浮点数");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    double x = 3.0;
                    double y = 4.0;
                    double z = 2.5;
                    double complexResult = math.ExecuteForDouble(
                        $"MathematicaFunctions`ComplexCalculation[{x}, {y}, {z}]");
                    Console.WriteLine($"输入: x={x}, y={y}, z={z}");
                    Console.WriteLine($"计算公式: sqrt(x^2 + y^2) * z");
                    Console.WriteLine($"结果: {complexResult:F4}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 12: 异步调用 - 单个整数参数
                    // ====================================================================
                    Console.WriteLine("【示例 12】异步调用 - 单个整数参数");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    int asyncNum = 8;
                    Task<int> asyncTask1 = math.ExecuteForIntegerAsync(
                        $"MathematicaFunctions`SquareNumber[{asyncNum}]");
                    Console.WriteLine($"正在异步计算 SquareNumber({asyncNum})...");
                    int asyncResult1 = await asyncTask1;
                    Console.WriteLine($"异步结果: {asyncResult1}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 13: 异步调用 - 字符串参数
                    // ====================================================================
                    Console.WriteLine("【示例 13】异步调用 - 字符串参数");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    string asyncUserName = "李四";
                    Task<string> asyncTask2 = math.ExecuteForStringAsync(
                        $"MathematicaFunctions`GreetUser[\"{asyncUserName}\"]");
                    Console.WriteLine($"正在异步处理字符串 \"{asyncUserName}\"...");
                    string asyncResult2 = await asyncTask2;
                    Console.WriteLine($"异步结果: {asyncResult2}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 14: 异步调用 - List 参数
                    // ====================================================================
                    Console.WriteLine("【示例 14】异步调用 - List 参数");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    int[] asyncList = { 100, 200, 300, 400, 500 };
                    string asyncListStr = "{" + string.Join(", ", asyncList) + "}";
                    Task<int> asyncTask3 = math.ExecuteForIntegerAsync(
                        $"MathematicaFunctions`SumList[{asyncListStr}]");
                    Console.WriteLine($"正在异步计算列表和: {asyncListStr}...");
                    int asyncResult3 = await asyncTask3;
                    Console.WriteLine($"异步结果: {asyncResult3}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 15: 并发异步调用多个函数
                    // ====================================================================
                    Console.WriteLine("【示例 15】并发异步调用多个函数");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    Console.WriteLine("同时启动 3 个异步任务...");
                    
                    var task1 = math.ExecuteForIntegerAsync("MathematicaFunctions`Factorial[10]");
                    var task2 = math.ExecuteForStringAsync("MathematicaFunctions`GreetUser[\"并发用户\"]");
                    var task3 = math.ExecuteForIntegerAsync("MathematicaFunctions`SumList[{1,2,3,4,5,6,7,8,9,10}]");
                    
                    await Task.WhenAll(task1, task2, task3);
                    
                    Console.WriteLine($"任务1结果 (Factorial(10)): {task1.Result}");
                    Console.WriteLine($"任务2结果 (GreetUser): {task2.Result}");
                    Console.WriteLine($"任务3结果 (SumList): {task3.Result}");
                    Console.WriteLine();

                    // ====================================================================
                    // 示例 16: 直接调用 Mathematica 内置函数
                    // ====================================================================
                    Console.WriteLine("【示例 16】直接调用 Mathematica 内置函数");
                    Console.WriteLine("-".PadRight(80, '-'));
                    
                    // 计算 Pi
                    string piValue = math.ExecuteCommand("N[Pi, 20]");
                    Console.WriteLine($"Pi (20位精度): {piValue}");
                    
                    // 计算矩阵行列式
                    string detResult = math.ExecuteCommand("Det[{{1,2},{3,4}}]");
                    Console.WriteLine("矩阵 {{1,2},{3,4}} 的行列式: " + detResult);
                    
                    // 求解方程
                    string solveResult = math.ExecuteCommand("Solve[x^2 - 5*x + 6 == 0, x]");
                    Console.WriteLine("方程 x^2 - 5x + 6 = 0 的解: " + solveResult);
                    Console.WriteLine();

                    // ====================================================================
                    Console.WriteLine("=".PadRight(80, '='));
                    Console.WriteLine("所有示例执行完毕！");
                    Console.WriteLine("=".PadRight(80, '='));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"错误: {ex.Message}");
                    Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
                }
            }

            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
