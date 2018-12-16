using Ansys.DesignXplorer.API.Optimization;
using System;

namespace Ansys {
    public class Objective {
        public String ID;
        public enumGoalType ObjectiveType;
        public Double ObjectiveValue;
        public Double Importance;

        public Double Compute(IOptimizationPoint point) {
            if (this.ObjectiveType == enumGoalType.eGT_MinimumPossible) {
                return Convert.ToDouble(point.get_Value(this.ID));
            }

            if (this.ObjectiveType == enumGoalType.eGT_MaximumPossible) {
                return -Convert.ToDouble(point.get_Value(this.ID));
            }

            return this.ObjectiveType == enumGoalType.eGT_SeekTarget
                ? Math.Abs(Convert.ToDouble(this.ObjectiveValue - Convert.ToDouble(point.get_Value(this.ID))))
                : 0.0;
        }
    }
}