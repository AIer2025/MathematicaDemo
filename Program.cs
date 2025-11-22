using System;
using System.Threading.Tasks;
using System.IO; // Required for saving images
using MathematicaDemo;

namespace MathematicaDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=".PadRight(80, '='));
            Console.WriteLine("C# .NET 8.0 Calling Mathematica 14.3 - Final Demo");
            Console.WriteLine("=".PadRight(80, '='));
            Console.WriteLine();

            using (var math = new MathematicaHelper())
            {
                try
                {
                    math.Initialize();
                    Console.WriteLine();

                    // --- Example 1: Integer ---
                    Console.WriteLine("--- Example 1: SquareNumber(5) ---");
                    int square = math.ExecuteForInteger("MathematicaFunctions`SquareNumber[5]");
                    Console.WriteLine($"Result: {square}");
                    Console.WriteLine();

                    // --- Example 2: Factorial (Using Renamed Function) ---
                    Console.WriteLine("--- Example 2: ComputeFactorial(6) ---");
                    int fact = math.ExecuteForInteger("MathematicaFunctions`ComputeFactorial[6]");
                    Console.WriteLine($"Result: {fact}");
                    Console.WriteLine();

                    // --- Example 3: String ---
                    Console.WriteLine("--- Example 3: GreetUser ---");
                    string greeting = math.ExecuteForString("MathematicaFunctions`GreetUser[\"Developer\"]");
                    Console.WriteLine($"Result: {greeting}");
                    Console.WriteLine();

                    // --- Example 7: List Sum ---
                    Console.WriteLine("--- Example 7: SumList ---");
                    int sum = math.ExecuteForInteger("MathematicaFunctions`SumList[{10, 20, 30}]");
                    Console.WriteLine($"Result: {sum}");
                    Console.WriteLine();

                    // --- Example 11: Complex Calculation ---
                    Console.WriteLine("--- Example 11: ComplexCalculation ---");
                    double complex = math.ExecuteForDouble("MathematicaFunctions`ComplexCalculation[3, 4, 2.5]");
                    Console.WriteLine($"Result: {complex}");
                    Console.WriteLine();

                    // --- Example 16: Built-in Functions ---
                    Console.WriteLine("--- Example 16: Built-in Pi & Solve ---");
                    Console.WriteLine($"Pi: {math.ExecuteCommand("N[Pi, 10]")}");
                    Console.WriteLine($"Solve: {math.ExecuteCommand("Solve[x^2==4, x]")}");
                    Console.WriteLine();

                    // ====================================================================
                    // Example 17: Image Generation (The New Feature)
                    // ====================================================================
                    Console.WriteLine("--- Example 17: Generating 3D Plot to File ---");
                    
                    // We can call a built-in Plot3D directly
                    string plotCmd1 = "Plot3D[Sin[x*y], {x,-2,2}, {y,-2,2}, ColorFunction->\"Rainbow\", Mesh->None, Boxed->False, Axes->False, Background->Black]";
                    
                    // OR we can call the function defined in our .m package
                    string plotCmd2 = "MathematicaFunctions`GenerateSamplePlot[]";

                    Console.WriteLine("Generating 3D Image...");
                    
                    // Generate bytes (using JPG format)
                    byte[] imageBytes = await math.ExecuteForImageBytesAsync(plotCmd1, "JPG");
                    
                    Console.WriteLine($"Image generated! Size: {imageBytes.Length} bytes.");

                    // Save to Desktop
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string filePath = Path.Combine(desktopPath, "Mathematica_Plot3D.jpg");
                    
                    await File.WriteAllBytesAsync(filePath, imageBytes);
                    Console.WriteLine($"Saved to: {filePath}");
                    Console.WriteLine();

                    Console.WriteLine("=".PadRight(80, '='));
                    Console.WriteLine("All tests completed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}