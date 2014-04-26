using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using AgentHeisenbug.Indexer.TaskbarApi;
using AshMind.Extensions;
using JetBrains.Annotations;

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

        private static readonly IntPtr _windowHandle = GetConsoleWindow();
        [NotNull] private static readonly ConcurrentDictionary<object, double> _progressBySource = new ConcurrentDictionary<object, double>();
        [NotNull] private static readonly ITaskbarList4 _taskbar = (ITaskbarList4)new CTaskbarList();
        [NotNull] private static Timer _progressTimer;
        [NotNull] private static string _savedTitle;

        static ConsoleMultiProgressReporter() {
            _taskbar.HrInit();
        }

        public static IDisposable Start([NotNull] params object[] sources) {
            _savedTitle = Console.Title;

            _progressBySource.Clear();
            sources.ForEach(s => _progressBySource[s] = 0);

            _progressTimer = new Timer(_ => DisplayProgress());
            try {
                _progressTimer.Change(0, 500);
                _taskbar.SetProgressState(_windowHandle, TaskbarProgressBarStatus.Normal);
            }
            catch (Exception) {
                _progressTimer.Dispose();
                throw;
            }

            return new Disposer();
        }

        public static void Progress([NotNull] object source, double progress) {
            _progressBySource[source] = progress;
        }

        public static void End() {
            _progressTimer.Dispose();
            _taskbar.SetProgressState(_windowHandle, TaskbarProgressBarStatus.NoProgress);
            Console.Title = _savedTitle;
        }

        private static void DisplayProgress() {
            var maximum = _progressBySource.Count;
            var current = _progressBySource.Values.Sum(v => v);
            Console.Title = string.Format("{0:F1} % {1}", 100 * current / maximum, _savedTitle);
            _taskbar.SetProgressValue(_windowHandle, (ulong)(1000 * current), (ulong)(1000 * maximum));
        }
    }
}
