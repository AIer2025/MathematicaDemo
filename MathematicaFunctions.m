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

End[]

EndPackage[]