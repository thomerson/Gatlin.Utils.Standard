using System;
using System.IO;
using System.Threading;

namespace Gatlin.Utils.Standard.Log
{
    public class Logger
    {
        /// <summary>
        /// 表示用于管理资源访问的锁定状态，可实现多线程读取或进行独占式写入访问
        /// </summary>
        private static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        public static void Write(string content)
        {
            var txtPath = Environment.CurrentDirectory + "\\" + DateTime.Now.ToString("yyyyMMdd");

            if (!Directory.Exists(txtPath))
            {
                Directory.CreateDirectory(txtPath);
            }

            var filePath = txtPath + "\\log.txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            try
            {
                LogWriteLock.EnterWriteLock();

                using (FileStream stream = new FileStream(filePath, FileMode.Append))
                {
                    StreamWriter write = new StreamWriter(stream);

                    write.WriteLine($"{DateTime.Now}:{content}");

                    //关闭并销毁流写入文件
                    write.Close();
                    write.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LogWriteLock.ExitWriteLock();
            }
        }
    }
}
