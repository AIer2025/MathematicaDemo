using Wolfram.NETLink;
using System;
using System.Threading.Tasks;
using System.IO;

namespace MathematicaDemo
{
    /// <summary>
    /// Mathematica 辅助类 (文件桥接版)
    /// 通过临时文件交换数据，彻底解决内存传输导致的文件损坏/文本化问题
    /// </summary>
    public class MathematicaHelper : IDisposable
    {
        private IKernelLink? _kernelLink;
        private bool _disposed = false;

        public void Initialize(string? mathematicaPath = null)
        {
            try
            {
                if (string.IsNullOrEmpty(mathematicaPath))
                {
                    mathematicaPath = @"C:\Program Files\Wolfram Research\Wolfram\14.3\MathKernel.exe";
                }

                Console.WriteLine("正在启动 Mathematica 内核...");
                
                string[] args = { "-linkmode", "launch", "-linkname", $"\"{mathematicaPath}\"" };
                _kernelLink = MathLinkFactory.CreateKernelLink(args);
                _kernelLink.WaitAndDiscardAnswer();

                Console.WriteLine("Mathematica 内核启动成功！");
                LoadCustomPackage();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"初始化失败: {ex.Message}", ex);
            }
        }

        private void LoadCustomPackage()
        {
            if (_kernelLink == null) return;
            try
            {
                string packagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MathematicaFunctions.m");
                if (File.Exists(packagePath))
                {
                    packagePath = packagePath.Replace("\\", "/");
                    string result = _kernelLink.EvaluateToOutputForm($"Get[\"{packagePath}\"]", 0);
                    if (result.Contains("$Failed")) Console.WriteLine($"严重警告: 包加载失败: {result}");
                    else Console.WriteLine("自定义函数包加载成功！");
                }
            }
            catch (Exception ex) { Console.WriteLine($"加载包出错: {ex.Message}"); }
        }

        // ============================================================
        // 基础执行方法
        // ============================================================

        public string ExecuteCommand(string command)
        {
            if (_kernelLink == null) throw new InvalidOperationException("内核未初始化");
            try { return _kernelLink.EvaluateToOutputForm(command, 0); }
            catch (MathLinkException ex) { throw new InvalidOperationException($"通信错误: {ex.Message}", ex); }
        }

        public int ExecuteForInteger(string command)
        {
            string res = ExecuteCommand(command);
            if (int.TryParse(res, out int val)) return val;
            throw new FormatException($"期望整数，返回了: '{res}'");
        }

        public double ExecuteForDouble(string command)
        {
            string res = ExecuteCommand(command).Replace("*^", "E");
            if (double.TryParse(res, out double val)) return val;
            throw new FormatException($"期望浮点数，返回了: '{res}'");
        }

        public string ExecuteForString(string command) => ExecuteCommand(command);

        public int[] ExecuteForIntArray(string command)
        {
            if (_kernelLink == null) throw new InvalidOperationException("内核未初始化");
            _kernelLink.Evaluate(command);
            _kernelLink.WaitForAnswer();
            return _kernelLink.GetInt32Array();
        }

        // ============================================================
        // 【终极方案】图像生成方法 (File Bridge)
        // ============================================================

        public byte[] ExecuteForImageBytes(string plotCommand, string format = "JPG")
        {
            if (_kernelLink == null) throw new InvalidOperationException("内核未初始化");

            // 1. 生成唯一的临时文件路径 (例如 C:\Users\xxx\AppData\Local\Temp\tmp1234.tmp)
            string tempFilePath = Path.GetTempFileName();
            
            try
            {
                // 2. 处理路径格式，Mathematica 需要双反斜杠
                string mathPath = tempFilePath.Replace("\\", "\\\\");

                // 3. 构造 Export 命令：让 Mathematica 直接把图写到这个文件里
                // Export["C:\\Temp\\tmp.jpg", Plot[...], "JPG"]
                // 并在最后返回 "OK" 字符串以便确认
                string command = $"Export[\"{mathPath}\", {plotCommand}, \"{format}\"]; \"OK\"";

                // 4. 执行命令
                string result = _kernelLink.EvaluateToOutputForm(command, 0);

                // 5. 检查是否成功
                if (!result.Contains("OK"))
                {
                    throw new Exception($"Mathematica 导出失败，返回: {result}");
                }

                // 6. C# 从硬盘读取这个真正的图片文件
                if (File.Exists(tempFilePath) && new FileInfo(tempFilePath).Length > 0)
                {
                    return File.ReadAllBytes(tempFilePath);
                }
                else
                {
                    throw new Exception("临时文件未生成或为空。");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"图像生成失败: {ex.Message}", ex);
            }
            finally
            {
                // 7. 清理现场：删除临时文件
                try
                {
                    if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
                }
                catch { /* 忽略删除失败，由系统自动清理 */ }
            }
        }

        // 异步封装
        public async Task<string> ExecuteCommandAsync(string cmd) => await Task.Run(() => ExecuteCommand(cmd));
        public async Task<int> ExecuteForIntegerAsync(string cmd) => await Task.Run(() => ExecuteForInteger(cmd));
        public async Task<double> ExecuteForDoubleAsync(string cmd) => await Task.Run(() => ExecuteForDouble(cmd));
        public async Task<string> ExecuteForStringAsync(string cmd) => await Task.Run(() => ExecuteForString(cmd));
        public async Task<int[]> ExecuteForIntArrayAsync(string cmd) => await Task.Run(() => ExecuteForIntArray(cmd));
        public async Task<byte[]> ExecuteForImageBytesAsync(string cmd, string fmt="JPG") => await Task.Run(() => ExecuteForImageBytes(cmd, fmt));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing && _kernelLink != null)
            {
                _kernelLink.Close();
                _kernelLink = null;
            }
            _disposed = true;
        }

        ~MathematicaHelper() => Dispose(false);
    }
}