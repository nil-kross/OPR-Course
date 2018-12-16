using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Ans.ComponentSystem;
using Ans.ComponentSystem.Interop;
using Ansys.DesignXplorer.API.Common;
using Ansys.DesignXplorer.API.Optimization;
using Ans.DesignXplorer.InterProcessConnectionServer;
using Ans.DesignXplorer.InterProcessConnectionServices;
using Ans.DesignXplorer.CommonMatlabApplication;

namespace Ans.DesignXplorer.MatlabOptimizer {
    public class Parameter {
        public string ID;
        public double LowerBound;
        public double UpperBound;
        public double InitialValue;
    }
}