namespace BreakPointer
{
    using System.Collections;

    using EnvDTE;

    using Microsoft.VisualStudio.Shell;

    public class FilteredBreakpoints : Breakpoints
    {
        private readonly Breakpoints breakpoints;

        public FilteredBreakpoints(Breakpoints breakpoints)
        {
            this.breakpoints = breakpoints;
        }

        public Breakpoint Item(object index)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return this.breakpoints.Item(index);
        }

        IEnumerator Breakpoints.GetEnumerator()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return this.breakpoints.GetEnumerator();
        }

        public Breakpoints Add(
            string Function = "",
            string File = "",
            int Line = 1,
            int Column = 1,
            string Condition = "",
            dbgBreakpointConditionType ConditionType = dbgBreakpointConditionType.dbgBreakpointConditionTypeWhenTrue,
            string Language = "",
            string Data = "",
            int DataCount = 1,
            string Address = "",
            int HitCount = 0,
            dbgHitCountType HitCountType = dbgHitCountType.dbgHitCountTypeNone)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return this.breakpoints.Add(Function,
                File,
                Line,
                Column,
                Condition,
                ConditionType,
                Language,
                Data,
                DataCount,
                Address,
                HitCount,
                HitCountType);
        }

        public DTE DTE
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return this.breakpoints.DTE;
            }
        }

        public Debugger Parent
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return this.breakpoints.Parent;
            }
        }

        public int Count
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return this.breakpoints.Count;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return this.breakpoints.GetEnumerator();
        }

        public Breakpoints FilterByFilename(string filterFilename)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var breakpointsEnumerator = this.breakpoints.GetEnumerator();
            while (breakpointsEnumerator.MoveNext())
            {
                if (breakpointsEnumerator.Current is Breakpoint current && current.File.Contains(filterFilename))
                {
                    this.Add(current.FunctionName,
                        current.File,
                        current.FileLine,
                        current.FileColumn,
                        current.Condition,
                        current.ConditionType,
                        current.Language,
                        HitCount: current.HitCountTarget,
                        HitCountType: current.HitCountType);
                }
            }

            return this.breakpoints;
        }
    }
}