using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Pflegehaushaltsbuch
{
    public static class ApplicationLogger
    {
        private static readonly object SyncRoot = new object();

        public static string LogFilePath
        {
            get
            {
                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, "Richter Pflegehaushaltsbuch", "logs", "exceptions.log");
            }
        }

        public static void LogException(Exception exception, string context = null)
        {
            if (exception == null)
                return;

            WriteEntry(builder =>
            {
                builder.AppendLine("Exception");
                if (!string.IsNullOrWhiteSpace(context))
                    builder.AppendLine("Context: " + context);
                builder.AppendLine(exception.ToString());
            });
        }

        public static void LogUnhandledObject(object exceptionObject, string context = null)
        {
            Exception exception = exceptionObject as Exception;
            if (exception != null)
            {
                LogException(exception, context);
                return;
            }

            WriteEntry(builder =>
            {
                builder.AppendLine("Unhandled object");
                if (!string.IsNullOrWhiteSpace(context))
                    builder.AppendLine("Context: " + context);
                builder.AppendLine(exceptionObject == null ? "<null>" : exceptionObject.ToString());
            });
        }

        private static void WriteEntry(Action<StringBuilder> writeDetails)
        {
            try
            {
                lock (SyncRoot)
                {
                    string logFile = LogFilePath;
                    Directory.CreateDirectory(Path.GetDirectoryName(logFile));

                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine("============================================================");
                    builder.AppendLine("Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz", CultureInfo.InvariantCulture));
                    builder.AppendLine("Application: " + GetApplicationName());
                    builder.AppendLine("Version: " + GetApplicationVersion());
                    builder.AppendLine("ProcessId: " + Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture));
                    builder.AppendLine("ThreadId: " + Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture));
                    builder.AppendLine("Culture: " + CultureInfo.CurrentCulture.Name);
                    builder.AppendLine("UICulture: " + CultureInfo.CurrentUICulture.Name);
                    writeDetails(builder);
                    builder.AppendLine();

                    File.AppendAllText(logFile, builder.ToString(), Encoding.UTF8);
                }
            }
            catch
            {
            }
        }

        private static string GetApplicationName()
        {
            Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            return assembly.GetName().Name;
        }

        private static string GetApplicationVersion()
        {
            Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            return version == null ? string.Empty : version.ToString();
        }
    }
}
