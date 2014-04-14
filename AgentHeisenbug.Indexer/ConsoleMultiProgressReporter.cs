using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using AgentHeisenbug.Indexer.TaskbarApi;
using AshMind.Extensions;

namespace AgentHeisenbug.Indexer {
    public static class ConsoleMultiProgressReporter {
        #region Disposer
        private class Disposer : IDisposable {
            public void Dispose() {
                End();
            }
        }
        #endregion

        #region GetConsoleWindow
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
        #endregion

        private static readonly IntPtr windowHandle = GetConsoleWindow();
        private static readonly ConcurrentDictionary<object, double> progressBySource = new ConcurrentDictionary<object, double>();
        private static readonly ITaskbarList4 taskbar = (ITaskbarList4)new CTaskbarList();
        private static Timer progressTimer;

        static ConsoleMultiProgressReporter() {
            taskbar.HrInit();
        }
        
        public static IDisposable Start(params object[] sources) {
            progressBySource.Clear();
            sources.ForEach(s => progressBySource[s] = 0);

            progressTimer = new Timer(_ => DisplayProgress());
            try {
                progressTimer.Change(0, 500);
                taskbar.SetProgressState(windowHandle, TaskbarProgressBarStatus.Normal);
            }
            catch (Exception) {
                progressTimer.Dispose();
                throw;
            }

            return new Disposer();
        }

        public static void Progress(object source, double progress) {
            progressBySource[source] = progress;
        }

        public static void End() {
            progressTimer.Dispose();
            taskbar.SetProgressState(windowHandle, TaskbarProgressBarStatus.NoProgress);
        }

        private static void DisplayProgress() {
            var maximum = progressBySource.Count;
            var current = progressBySource.Values.Sum(v => v);
            taskbar.SetProgressValue(windowHandle, (ulong)(1000 * current), (ulong)(1000 * maximum));
        }
    }
}
