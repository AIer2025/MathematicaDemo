using Wolfram.NETLink;
using System;
using System.Threading.Tasks;
using System.IO;

namespace MathematicaDemo
{
    /// <summary>
    /// Mathematica 辅助类 (修复版)
    /// 解决了 Error 1002 图片生成问题，使用 Base64 传输二进制数据
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
                    // 默认路径，请根据实际安装位置调整
                    mathematicaPath = @"C:\Program Files\Wolfram Research\Wolfram\14.3\MathKernel.exe";
                }

                Console.WriteLine("正在启动 Mathematica 内核...");
                
                string[] args = { "-linkmode", "launch", "-linkname", $"\"{mathematicaPath}\"" };
                _kernelLink = MathLinkFactory.CreateKernelLink(args);

                // 丢弃初始 InputNamePacket
                _kernelLink.WaitAndDiscardAnswer();

                Console.WriteLine("Mathematica 内核启动成功！");

                LoadCustomPackage();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"初始化 Mathematica 内核失败: {ex.Message}", ex);
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
                    
                    // 使用 EvaluateToOutputForm 安全加载包
                    string result = _kernelLink.EvaluateToOutputForm($"Get[\"{packagePath}\"]", 0);
                    
                    if (result.Contains("$Failed"))
                    {
                        Console.WriteLine($"严重警告: 函数包加载失败，Mathematica 返回: {result}");
                    }
                    else
                    {
                        Console.WriteLine("自定义函数包加载成功！");
                    }
                }
                else
                {
                    Console.WriteLine($"警告: 未找到函数包文件: {packagePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载函数包时出错: {ex.Message}");
            }
        }

        // ============================================================
        // 核心执行方法
        // ============================================================

        /// <summary>
        /// 通用执行方法：返回字符串结果
        /// </summary>
        public string ExecuteCommand(string command)
        {
            if (_kernelLink == null)
                throw new InvalidOperationException("内核未初始化");

            try
            {
                // 0 表示不换行，返回标准 OutputForm 字符串
                return _kernelLink.EvaluateToOutputForm(command, 0);
            }
            catch (MathLinkException ex)
            {
                throw new InvalidOperationException($"Mathematica 通信错误: {ex.Message}", ex);
            }
        }

        public int ExecuteForInteger(string command)
        {
            string resultStr = ExecuteCommand(command);
            if (int.TryParse(resultStr, out int result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"期望返回整数，但 Mathematica 返回了: '{resultStr}'");
            }
        }

        public double ExecuteForDouble(string command)
        {
            string resultStr = ExecuteCommand(command);
            // 处理科学计数法 (1.2*^3 -> 1.2E3)
            resultStr = resultStr.Replace("*^", "E");

            if (double.TryParse(resultStr, out double result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"期望返回浮点数，但 Mathematica 返回了: '{resultStr}'");
            }
        }

        public string ExecuteForString(string command)
        {
            return ExecuteCommand(command);
        }

        public int[] ExecuteForIntArray(string command)
        {
            if (_kernelLink == null) throw new InvalidOperationException("内核未初始化");

            try 
            {
                _kernelLink.Evaluate(command);
                _kernelLink.WaitForAnswer();
                return _kernelLink.GetInt32Array();
            }
            catch (MathLinkException)
            {
                _kernelLink.ClearError(); 
                _kernelLink.NewPacket(); 
                throw new InvalidOperationException("获取数组失败。请确保函数返回的是整数列表 (List)。");
            }
        }

        // ============================================================
        // 【关键修复】图像生成方法
        // ============================================================

        /// <summary>
        /// 生成图像并返回二进制数据
        /// 修复方案：使用 Base64 字符串传输，避免 Error 1002 数组深度错误
        /// </summary>
        public byte[] ExecuteForImageBytes(string plotCommand, string format = "JPG")
        {
            if (_kernelLink == null) throw new InvalidOperationException("内核未初始化");

            try
            {
                // 【修复逻辑】
                // 1. ExportByteArray 生成 ByteArray 对象
                // 2. Base64String 将其转换为纯文本字符串
                // 这样 C# 只需要读取 String，绝对安全
                string command = $"Base64String[ExportByteArray[{plotCommand}, \"{format}\"]]";
                
                _kernelLink.Evaluate(command);
                _kernelLink.WaitForAnswer();
                
                // 读取 Base64 字符串
                string base64Result = _kernelLink.GetString();

                // 在 C# 端解码为二进制
                return Convert.FromBase64String(base64Result);
            }
            catch (MathLinkException ex)
            {
                _kernelLink.ClearError();
                _kernelLink.NewPacket();
                throw new InvalidOperationException($"图像生成失败: {ex.Message}", ex);
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException($"图像数据解码失败 (Base64 格式错误): {ex.Message}", ex);
            }
        }

        // ============================================================
        // 异步封装
        // ============================================================
        public async Task<string> ExecuteCommandAsync(string command) => await Task.Run(() => ExecuteCommand(command));
        public async Task<int> ExecuteForIntegerAsync(string command) => await Task.Run(() => ExecuteForInteger(command));
        public async Task<double> ExecuteForDoubleAsync(string command) => await Task.Run(() => ExecuteForDouble(command));
        public async Task<string> ExecuteForStringAsync(string command) => await Task.Run(() => ExecuteForString(command));
        public async Task<int[]> ExecuteForIntArrayAsync(string command) => await Task.Run(() => ExecuteForIntArray(command));
        
        // 异步图像生成
        public async Task<byte[]> ExecuteForImageBytesAsync(string command, string format = "JPG") => 
            await Task.Run(() => ExecuteForImageBytes(command, format));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_kernelLink != null)
                    {
                        _kernelLink.Close();
                        _kernelLink = null;
                        Console.WriteLine("Mathematica 内核已关闭");
                    }
                }
                _disposed = true;
            }
        }

        ~MathematicaHelper()
        {
            Dispose(false);
        }
    }
}
