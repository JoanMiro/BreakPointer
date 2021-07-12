namespace BreakPointer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;

    public class BreakpointManager
    {
        public BreakpointManager()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var debugger = this.GetDebugger();
            if (debugger.Breakpoints != null)
            {
                this.Breakpoints = debugger.Breakpoints;
            }
        }

        public Breakpoints Breakpoints { get; }

        public List<Breakpoint> BreakpointsForClass()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var enumerator = Breakpoints.GetEnumerator();
            enumerator.Reset();
            var filteredBreakpoints = new List<Breakpoint>(Breakpoints.Count);
            while (enumerator.MoveNext())
            {
                filteredBreakpoints.Add(enumerator.Current as Breakpoint);
            }

            return filteredBreakpoints;
        }

        private Debugger GetDebugger()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = (DTE)Marshal.GetActiveObject("VisualStudio.DTE.16.0");
            //var dte2 = (DTE)this.ServiceProvider.GetService(typeof(DTE));
            if (dte == null)
            {
                throw new ArgumentNullException(nameof(dte));
            }

            return dte.Debugger;
        }
    }
}