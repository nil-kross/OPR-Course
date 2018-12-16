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
    public class Objective {
        public string ID;
        public enumGoalType ObjectiveType;
        public double ObjectiveValue;
        public double Importance;

        public double Compute(IOptimizationPoint point) {
            if (ObjectiveType == enumGoalType.eGT_MinimumPossible)
                return Convert.ToDouble(point.get_Value(ID));
            if (ObjectiveType == enumGoalType.eGT_MaximumPossible)
                return -Convert.ToDouble(point.get_Value(ID));
            if (ObjectiveType == enumGoalType.eGT_SeekTarget)
                return Math.Abs(Convert.ToDouble(ObjectiveValue - Convert.ToDouble(point.get_Value(ID))));
            return 0.0;
        }
    }
}