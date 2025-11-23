(* MathematicaFunctions.m *)

BeginPackage["MathematicaFunctions`"]

(* --- Usage Definitions (English only to prevent encoding errors) --- *)

AddNumbers::usage = "AddNumbers[a, b] returns the sum of two integers.";
ComputeFactorial::usage = "ComputeFactorial[n] calculates the factorial of n.";
SquareNumber::usage = "SquareNumber[n] returns the square of n.";
Multiply::usage = "Multiply[a, b] returns the product of two integers.";
GreetUser::usage = "GreetUser[name] returns a greeting string.";
ConcatStrings::usage = "ConcatStrings[s1, s2] concatenates two strings.";
ProcessMultipleParams::usage = "ProcessMultipleParams[n, s, f] demonstrates mixed types.";
SumList::usage = "SumList[list] returns the total sum of a list.";
ProcessComplexList::usage = "ProcessComplexList[list] returns a string summary of list stats.";
GenerateSequence::usage = "GenerateSequence[start, end] returns a list of integers.";
AnalyzeList::usage = "AnalyzeList[list] returns a JSON string with statistics.";
ComplexCalculation::usage = "ComplexCalculation[x, y, z] performs a math calculation.";

(* New function for plotting demo *)
GenerateSamplePlot::usage = "GenerateSamplePlot[] returns a 3D plot object.";

(* Complex nested function call demo *)
ComplexFuncCall::usage = "ComplexFuncCall[dataList, threshold] performs comprehensive analysis with nested function calls.";

Begin["`Private`"]

(* Basic Math *)
AddNumbers[a_Integer, b_Integer] := a + b
SquareNumber[n_Integer] := n^2
Multiply[a_Integer, b_Integer] := a * b

(* Renamed to avoid conflict with System`Factorial *)
ComputeFactorial[n_Integer] := n!

(* String Manipulation *)
GreetUser[name_String] := "Hello, " <> name <> "! Welcome to Mathematica."
ConcatStrings[str1_String, str2_String] := str1 <> " " <> str2
ProcessMultipleParams[num_Integer, str_String, factor_Real] := 
  Module[{result},
    result = num * factor;
    "Processing: " <> str <> ", Result: " <> ToString[result]
  ]

(* List Processing *)
SumList[list_List] := Total[list]

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

GenerateSequence[start_Integer, end_Integer] := Range[start, end]

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

ComplexCalculation[x_?NumericQ, y_?NumericQ, z_?NumericQ] := 
  Module[{result},
    result = (x^2 + y^2)^(1/2) * z;
    N[result]
  ]

(* Plotting Helper: Returns a Graphics3D object *)
GenerateSamplePlot[] := 
  Plot3D[Sin[x] Cos[y], {x, -3, 3}, {y, -3, 3}, 
   PlotStyle -> Directive[Orange, Specularity[White, 20]], 
   Mesh -> None, 
   ColorFunction -> "SunsetColors"]

(* ============================================ *)
(* Complex Nested Function Call - Helper Functions *)
(* ============================================ *)

(* Helper 1: Validate and clean data *)
ValidateData[dataList_List] := 
  Module[{cleaned},
    (* Remove any non-numeric values and keep only positive numbers *)
    cleaned = Select[dataList, NumericQ[#] && # > 0 &];
    cleaned
  ]

(* Helper 2: Calculate basic statistics *)
CalculateBasicStats[dataList_List] := 
  Module[{sum, mean, median, max, min},
    sum = Total[dataList];
    mean = Mean[dataList];
    median = Median[dataList];
    max = Max[dataList];
    min = Min[dataList];
    <|"sum" -> sum, "mean" -> mean, "median" -> median, 
      "max" -> max, "min" -> min|>
  ]

(* Helper 3: Calculate advanced statistics *)
CalculateAdvancedStats[dataList_List] := 
  Module[{variance, stdDev, range, q1, q3},
    variance = Variance[dataList];
    stdDev = StandardDeviation[dataList];
    range = Max[dataList] - Min[dataList];
    q1 = Quantile[dataList, 0.25];
    q3 = Quantile[dataList, 0.75];
    <|"variance" -> variance, "stdDev" -> stdDev, 
      "range" -> range, "Q1" -> q1, "Q3" -> q3|>
  ]

(* Helper 4: Filter data by threshold *)
FilterByThreshold[dataList_List, threshold_?NumericQ] := 
  Module[{aboveThreshold, belowThreshold},
    aboveThreshold = Select[dataList, # >= threshold &];
    belowThreshold = Select[dataList, # < threshold &];
    <|"above" -> aboveThreshold, "below" -> belowThreshold, 
      "aboveCount" -> Length[aboveThreshold], 
      "belowCount" -> Length[belowThreshold]|>
  ]

(* Helper 5: Generate summary report *)
GenerateReport[basicStats_, advancedStats_, filterResults_, originalCount_, cleanedCount_] := 
  Module[{report},
    report = "=== Data Analysis Report ===\n";
    report = report <> "Original data count: " <> ToString[originalCount] <> "\n";
    report = report <> "Cleaned data count: " <> ToString[cleanedCount] <> "\n";
    report = report <> "\n--- Basic Statistics ---\n";
    report = report <> "Sum: " <> ToString[N[basicStats["sum"], 4]] <> "\n";
    report = report <> "Mean: " <> ToString[N[basicStats["mean"], 4]] <> "\n";
    report = report <> "Median: " <> ToString[N[basicStats["median"], 4]] <> "\n";
    report = report <> "Max: " <> ToString[N[basicStats["max"], 4]] <> "\n";
    report = report <> "Min: " <> ToString[N[basicStats["min"], 4]] <> "\n";
    report = report <> "\n--- Advanced Statistics ---\n";
    report = report <> "Variance: " <> ToString[N[advancedStats["variance"], 4]] <> "\n";
    report = report <> "Std Dev: " <> ToString[N[advancedStats["stdDev"], 4]] <> "\n";
    report = report <> "Range: " <> ToString[N[advancedStats["range"], 4]] <> "\n";
    report = report <> "Q1 (25%): " <> ToString[N[advancedStats["Q1"], 4]] <> "\n";
    report = report <> "Q3 (75%): " <> ToString[N[advancedStats["Q3"], 4]] <> "\n";
    report = report <> "\n--- Threshold Analysis ---\n";
    report = report <> "Values above threshold: " <> ToString[filterResults["aboveCount"]] <> "\n";
    report = report <> "Values below threshold: " <> ToString[filterResults["belowCount"]] <> "\n";
    report = report <> "===========================";
    report
  ]

(* Main Complex Function: Calls multiple helper functions *)
ComplexFuncCall[dataList_List, threshold_?NumericQ] := 
  Module[{validData, basicStats, advancedStats, filterResults, report},
    (* Step 1: Validate and clean the data *)
    validData = ValidateData[dataList];
    
    (* Step 2: Calculate basic statistics on cleaned data *)
    basicStats = CalculateBasicStats[validData];
    
    (* Step 3: Calculate advanced statistics *)
    advancedStats = CalculateAdvancedStats[validData];
    
    (* Step 4: Filter data by threshold *)
    filterResults = FilterByThreshold[validData, threshold];
    
    (* Step 5: Generate comprehensive report *)
    report = GenerateReport[basicStats, advancedStats, filterResults, 
                           Length[dataList], Length[validData]];
    
    (* Return the report *)
    report
  ]

End[]

EndPackage[]