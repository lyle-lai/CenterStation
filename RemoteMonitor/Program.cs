using System;
using System.Collections.Generic;
using System.Threading;
//using System.Linq;
using System.Windows.Forms;

namespace RemoteMonitor
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionEventHandler);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmMain());
            }
            catch(Exception  ex)
            {
                
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 线程异常信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            //作为示例，这里用消息框显示异常的信息 
            MessageBox.Show(e.Exception.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 捕获系统中trycatch不能捕获的异常信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var currentDir = AppDomain.CurrentDomain.BaseDirectory;
                using (var fs = new System.IO.FileStream($@"{currentDir}\testme.log", System.IO.FileMode.Append, System.IO.FileAccess.Write))
                {
                    using (var w = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8))
                    {
                        w.WriteLine(e.ExceptionObject);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
