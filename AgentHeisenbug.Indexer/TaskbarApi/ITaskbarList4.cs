using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AgentHeisenbug.Indexer.TaskbarApi {
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("c43dc798-95d1-4bea-9030-bb99e2983a1a")]
    [ComImport]
    internal interface ITaskbarList4 {
        [MethodImpl(MethodImplOptions.PreserveSig)]
        void HrInit();

        [MethodImpl(MethodImplOptions.PreserveSig)]
        void AddTab(IntPtr hwnd);

        [MethodImpl(MethodImplOptions.PreserveSig)]
        void DeleteTab(IntPtr hwnd);

        [MethodImpl(MethodImplOptions.PreserveSig)]
        void ActivateTab(IntPtr hwnd);

        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetActiveAlt(IntPtr hwnd);

        [MethodImpl(MethodImplOptions.PreserveSig)]
        void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

        [MethodImpl(MethodImplOptions.PreserveSig)]
        void SetProgressState(IntPtr hwnd, TaskbarProgressBarStatus tbpFlags);

        // other methods, skipped
    }
}
