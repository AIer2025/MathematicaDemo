using Wolfram.NETLink;
using System;
using System.Threading.Tasks;

namespace MathematicaDemo
{
    /// <summary>
    /// Mathematica 辅助类，封装常用操作
    /// </summary>
    public class MathematicaHelper : IDisposable
    {
        private IKernelLink? _kernelLink;
        private bool _disposed = false;

        /// <summary>
        /// 初始化 Mathematica 内核连接
        /// </summary>
        /// <param name="mathematicaPath">Mathematica 可执行文件路径（可选）</param>
        public void Initialize(string? mathematicaPath = null)
        {
            try
            {
                // 如果未指定路径，使用默认路径
                if (string.IsNullOrEmpty(mathematicaPath))
                {
                    // Windows 默认路径
                    mathematicaPath = @"C:\Program Files\Wolfram Research\Wolfram\14.3\MathKernel.exe";
                    
                    // 如果是 Mac 或 Linux，需要修改路径
                    // mathematicaPath = "/Applications/Mathematica.app/Contents/MacOS/MathKernel";
                }

                Console.WriteLine("正在启动 Mathematica 内核...");
                
                // 创建内核链接
                string[] args = { "-linkmode", "launch", "-linkname", $"\"{mathematicaPath}\"" };
                _kernelLink = MathLinkFactory.CreateKernelLink(args);

                // 丢弃初始输入提示符
                _kernelLink.WaitAndDiscardAnswer();

                Console.WriteLine("Mathematica 内核启动成功！");

                // 加载自定义函数包
                LoadCustomPackage();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"初始化 Mathematica 内核失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 加载自定义函数包
        /// </summary>
        private void LoadCustomPackage()
        {
            try
            {
                string packagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MathematicaFunctions.m");
                
                if (File.Exists(packagePath))
                {
                    // 将反斜杠转换为正斜杠（Mathematica 格式）
                    packagePath = packagePath.Replace("\\", "/");
                    
                    string command = $"Get[\"{packagePath}\"]";
                    ExecuteCommand(command);
                    Console.WriteLine("自定义函数包加载成功！");
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

        /// <summary>
        /// 执行 Mathematica 命令（同步）
        /// </summary>
        public string ExecuteCommand(string command)
        {
            if (_kernelLink == null)
                throw new InvalidOperationException("Mathematica 内核未初始化");

            try
            {
                _kernelLink.Evaluate(command);
                _kernelLink.WaitForAnswer();
                
                // 使用 ToString 方法将结果转换为字符串
                _kernelLink.Evaluate("ToString[%]");
                _kernelLink.WaitForAnswer();
                return _kernelLink.GetString();
            }
            catch (MathLinkException ex)
            {
                throw new InvalidOperationException($"执行命令失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 执行命令并直接返回字符串结果（不经过ToString转换）
        /// </summary>
        private string ExecuteCommandRaw(string command)
        {
            if (_kernelLink == null)
                throw new InvalidOperationException("Mathematica 内核未初始化");

            _kernelLink.Evaluate(command);
            _kernelLink.WaitForAnswer();
            return _kernelLink.GetString();
        }

        /// <summary>
        /// 执行 Mathematica 命令并返回整数（同步）
        /// </summary>
        public int ExecuteForInteger(string command)
        {
            if (_kernelLink == null)
                throw new InvalidOperationException("Mathematica 内核未初始化");

            _kernelLink.Evaluate(command);
            _kernelLink.WaitForAnswer();
            return _kernelLink.GetInteger();
        }

        /// <summary>
        /// 执行 Mathematica 命令并返回浮点数（同步）
        /// </summary>
        public double ExecuteForDouble(string command)
        {
            if (_kernelLink == null)
                throw new InvalidOperationException("Mathematica 内核未初始化");

            _kernelLink.Evaluate(command);
            _kernelLink.WaitForAnswer();
            return _kernelLink.GetDouble();
        }

        /// <summary>
        /// 执行 Mathematica 命令并返回字符串（同步）
        /// </summary>
        public string ExecuteForString(string command)
        {
            if (_kernelLink == null)
                throw new InvalidOperationException("Mathematica 内核未初始化");

            try
            {
                _kernelLink.Evaluate(command);
                _kernelLink.WaitForAnswer();
                return _kernelLink.GetString();
            }
            catch (MathLinkException ex)
            {
                // 如果 GetString 失败，尝试使用 ToString 转换
                try
                {
                    _kernelLink.Evaluate("ToString[%]");
                    _kernelLink.WaitForAnswer();
                    return _kernelLink.GetString();
                }
                catch
                {
                    throw new InvalidOperationException($"执行命令失败: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// 执行 Mathematica 命令并返回数组（同步）
        /// </summary>
        public int[] ExecuteForIntArray(string command)
        {
            if (_kernelLink == null)
                throw new InvalidOperationException("Mathematica 内核未初始化");

            _kernelLink.Evaluate(command);
            _kernelLink.WaitForAnswer();
            return _kernelLink.GetInt32Array();
        }

        /// <summary>
        /// 执行 Mathematica 命令（异步）
        /// </summary>
        public async Task<string> ExecuteCommandAsync(string command)
        {
            return await Task.Run(() => ExecuteCommand(command));
        }

        /// <summary>
        /// 执行 Mathematica 命令并返回整数（异步）
        /// </summary>
        public async Task<int> ExecuteForIntegerAsync(string command)
        {
            return await Task.Run(() => ExecuteForInteger(command));
        }

        /// <summary>
        /// 执行 Mathematica 命令并返回浮点数（异步）
        /// </summary>
        public async Task<double> ExecuteForDoubleAsync(string command)
        {
            return await Task.Run(() => ExecuteForDouble(command));
        }

        /// <summary>
        /// 执行 Mathematica 命令并返回字符串（异步）
        /// </summary>
        public async Task<string> ExecuteForStringAsync(string command)
        {
            return await Task.Run(() => ExecuteForString(command));
        }

        /// <summary>
        /// 执行 Mathematica 命令并返回数组（异步）
        /// </summary>
        public async Task<int[]> ExecuteForIntArrayAsync(string command)
        {
            return await Task.Run(() => ExecuteForIntArray(command));
        }

        /// <summary>
        /// 清理资源
        /// </summary>
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
