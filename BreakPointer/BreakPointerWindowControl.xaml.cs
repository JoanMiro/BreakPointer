namespace BreakPointer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows.Controls;
    using EnvDTE;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using Debugger = EnvDTE.Debugger;

    /// <summary>
    /// Interaction logic for BreakPointerWindowControl.
    /// </summary>
    public partial class BreakPointerWindowControl : UserControl
    {
        [Import] internal SVsServiceProvider ServiceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakPointerWindowControl"/> class.
        /// </summary>
        public BreakPointerWindowControl()
        {
            this.InitializeComponent();
            var breakpointManager = new BreakpointManager();
            ThreadHelper.ThrowIfNotOnUIThread();
            // var debuggerOld = this.GetInstances().First().Debugger;
            //var debugger = this.GetDebugger();
            Debug.WriteLine(breakpointManager.BreakpointsForClass());
            this.BreakpointsListView.ItemsSource = breakpointManager.Breakpoints;
        }

        private IEnumerable<DTE> GetInstances()
        {
            IRunningObjectTable rot;
            IEnumMoniker enumMoniker;
            var retVal = GetRunningObjectTable(0, out rot);
            ThreadHelper.ThrowIfNotOnUIThread();

            if (retVal == 0)
            {
                rot.EnumRunning(out enumMoniker);

                var moniker = new IMoniker[1];
                while (enumMoniker.Next(1, moniker, out _) == 0)
                {
                    IBindCtx bindCtx;
                    CreateBindCtx(0, out bindCtx);
                    string displayName;
                    moniker[0].GetDisplayName(bindCtx, null, out displayName);
                    Console.WriteLine("Display Name: {0}", displayName);
                    var isVisualStudio = displayName.StartsWith("!VisualStudio");
                    if (isVisualStudio)

                    {
                        rot.GetObject(moniker[0], out var dte);
                        yield return dte as DTE;
                    }
                }
            }
        }

        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);
    }
}