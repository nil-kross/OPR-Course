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
    public class Constraint {
        public string ID;
        public enumConstraintType ConstraintType;
        public double Value1;
        public double Value2;
        public double Importance;
        public bool IsStrict;

        public double Compute(IOptimizationPoint point) {
            if (ConstraintType == enumConstraintType.eCT_LessThanTarget)
                return Convert.ToDouble(point.get_Value(ID)) - Value1;
            if (ConstraintType == enumConstraintType.eCT_GreaterThanTarget)
                return Value1 - Convert.ToDouble(point.get_Value(ID));
            if (ConstraintType == enumConstraintType.eCT_NearTarget)
                return Math.Abs(Convert.ToDouble(point.get_Value(ID)) - Value1);
            if (ConstraintType == enumConstraintType.eCT_InsideBounds) {
                double c1 = Value1 - Convert.ToDouble(point.get_Value(ID));
                double c2 = Convert.ToDouble(point.get_Value(ID)) - Value2;
                return c1 > c2 ? c1 : c2;
            }
            return 0.0;
        }
    }
}