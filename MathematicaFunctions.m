(* Mathematica 函数定义文件 *)
(* MathematicaFunctions.m *)

BeginPackage["MathematicaFunctions`"]

(* 声明公共函数 *)
AddNumbers::usage = "AddNumbers[a, b] 返回两个数的和";
Multiply::usage = "Multiply[a, b] 返回两个数的乘积";
GreetUser::usage = "GreetUser[name] 返回问候语";
ConcatStrings::usage = "ConcatStrings[str1, str2] 连接两个字符串";
ProcessMultipleParams::usage = "ProcessMultipleParams[num, str, factor] 处理多个参数";
SumList::usage = "SumList[list] 计算列表元素之和";
ProcessComplexList::usage = "ProcessComplexList[list] 处理复杂列表并返回统计信息";
SquareNumber::usage = "SquareNumber[n] 返回数字的平方";
Factorial::usage = "Factorial[n] 计算阶乘";
GenerateSequence::usage = "GenerateSequence[start, end] 生成序列";
AnalyzeList::usage = "AnalyzeList[list] 分析列表并返回详细信息";
ComplexCalculation::usage = "ComplexCalculation[x, y, z] 执行复杂计算";

Begin["`Private`"]

(* 1. 单个整数参数，返回数值 *)
AddNumbers[a_Integer, b_Integer] := a + b

(* 2. 单个整数参数，返回数值 *)
SquareNumber[n_Integer] := n^2

(* 3. 单个整数参数，返回数值 *)
Factorial[n_Integer] := n!

(* 4. 两个整数参数，返回数值 *)
Multiply[a_Integer, b_Integer] := a * b

(* 5. 单个字符串参数，返回字符串 *)
GreetUser[name_String] := "Hello, " <> name <> "! Welcome to Mathematica."

(* 6. 两个字符串参数，返回字符串 *)
ConcatStrings[str1_String, str2_String] := str1 <> " " <> str2

(* 7. 多个参数（整数和字符串），返回字符串 *)
ProcessMultipleParams[num_Integer, str_String, factor_Real] := 
  Module[{result},
    result = num * factor;
    "Processing: " <> str <> ", Result: " <> ToString[result]
  ]

(* 8. List 参数，返回数值 *)
SumList[list_List] := Total[list]

(* 9. List 参数，返回字符串 *)
ProcessComplexList[list_List] := 
  Module[{sum, mean, max, min},
    sum = Total[list];
    mean = Mean[list];
    max = Max[list];
    min = Min[list];
    "Sum: " <> ToString[sum] <> 
    ", Mean: " <> ToString[mean] <> 
    ", Max: " <> ToString[max] <> 
    ", Min: " <> ToString[min]
  ]

(* 10. 生成序列，返回 List *)
GenerateSequence[start_Integer, end_Integer] := Range[start, end]

(* 11. 分析列表，返回详细 JSON 格式字符串 *)
AnalyzeList[list_List] := 
  Module[{sum, mean, median, stdDev},
    sum = Total[list];
    mean = Mean[list];
    median = Median[list];
    stdDev = StandardDeviation[list];
    "{\"sum\":" <> ToString[sum] <> 
    ",\"mean\":" <> ToString[N[mean]] <> 
    ",\"median\":" <> ToString[N[median]] <> 
    ",\"stdDev\":" <> ToString[N[stdDev]] <> 
    ",\"count\":" <> ToString[Length[list]] <> "}"
  ]

(* 12. 复杂计算，多个数值参数 *)
ComplexCalculation[x_?NumericQ, y_?NumericQ, z_?NumericQ] := 
  Module[{result},
    result = (x^2 + y^2)^(1/2) * z;
    N[result]
  ]

(* 13. 矩阵运算示例 *)
MatrixDeterminant[matrix_List] := Det[matrix]

(* 14. 符号计算示例 *)
DifferentiateExpression[expr_String, var_String] := 
  Module[{expression, variable, derivative},
    expression = ToExpression[expr];
    variable = ToExpression[var];
    derivative = D[expression, variable];
    ToString[derivative, InputForm]
  ]

(* 15. 求解方程 *)
SolveEquation[equation_String, var_String] := 
  Module[{eq, variable, solution},
    eq = ToExpression[equation];
    variable = ToExpression[var];
    solution = Solve[eq == 0, variable];
    ToString[solution, InputForm]
  ]

End[]

EndPackage[]

(* 打印加载信息 *)
Print["MathematicaFunctions package loaded successfully!"]
