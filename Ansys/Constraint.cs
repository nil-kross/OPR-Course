using Ansys.DesignXplorer.API.Optimization;
using System;

namespace Ansys {
    public class Constraint {
        public String ID;
        public enumConstraintType ConstraintType;
        public Double Value1;
        public Double Value2;
        public Double Importance;
        public Boolean IsStrict;

        public Double Compute(IOptimizationPoint point) {
            if (this.ConstraintType == enumConstraintType.eCT_LessThanTarget)
                return Convert.ToDouble(point.get_Value(this.ID)) - this.Value1;
            if (this.ConstraintType == enumConstraintType.eCT_GreaterThanTarget)
                return this.Value1 - Convert.ToDouble(point.get_Value(this.ID));
            if (this.ConstraintType == enumConstraintType.eCT_NearTarget)
                return Math.Abs(Convert.ToDouble(point.get_Value(this.ID)) - this.Value1);
            if (this.ConstraintType == enumConstraintType.eCT_InsideBounds) {
                Double c1 = this.Value1 - Convert.ToDouble(point.get_Value(this.ID));
                Double c2 = Convert.ToDouble(point.get_Value(this.ID)) - this.Value2;

                return c1 > c2 ? c1 : c2;
            }
            return 0.0;
        }
    }
}