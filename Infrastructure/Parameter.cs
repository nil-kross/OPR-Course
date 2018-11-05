using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Parameter {
        private String nameString = null;
        private Nullable<Decimal> minValueNullable = null;
        private Nullable<Decimal> maxValueNullable = null;

        public Boolean IsHasName {
            get => this.nameString != null && this.nameString.Length > 0;
        }

        public Boolean IsHasMin {
            get => this.minValueNullable.HasValue;
        }

        public Boolean IsHasMax {
            get => this.maxValueNullable.HasValue;
        }

        public Decimal Min {
            get {
                Decimal minValue = 0;

                if (this.IsHasMin) {
                    minValue = this.minValueNullable.Value;
                } else {
                    throw new Exception("Min value was null.");
                }

                return minValue;
            }
        }

        public Decimal Max {
            get {
                Decimal maxValue = 0;

                if (this.IsHasMax) {
                    maxValue = this.maxValueNullable.Value;
                } else {
                    throw new Exception("Max value was null.");
                }

                return maxValue;
            }
        }

        public String Name {
            get => this.nameString;
        }

        public Parameter(Parameter.Options options) {
            this.nameString = options?.Name ?? null;
            this.minValueNullable = options?.Min ?? null;
            this.maxValueNullable = options?.Max ?? null;
        }

        public Argument GetRandomArgument() {
            var random = new Random(DateTime.Now.Millisecond);
            var value = (Decimal)(
                random.Next(
                    (this.IsHasMin ? (Int32)this.Min : Int32.MinValue),
                    (this.IsHasMax ? (Int32)this.Max : Int32.MaxValue)
                ) + random.NextDouble()
            );

            return new Argument(this, value);
        }

        public override string ToString() {
            String nameBlockString = this.IsHasName ? $"{this.Name}: " : "";
            String leftBorderString = (this.IsHasMin ? $"{this.Min} < " : "-∞") + " < ";
            String rightBorderStrign = " > " + (this.IsHasMax ? $"{this.Max}" : "+∞");

            return nameBlockString + $"({leftBorderString}x{rightBorderStrign})";
        }

        public class Options {
            public String Name { get; set; }
            public Nullable<Decimal> Min { get; set; }
            public Nullable<Decimal> Max { get; set; }
        }

        public class Argument {
            private Parameter parameter = null;
            private Decimal value = Decimal.Zero;

            public Parameter Parameter {
                get => this.parameter;
            }

            public Decimal Value {
                get => this.value;
            }

            public Argument(
                Parameter parameter,
                Decimal startingValue
            ) {
                this.parameter = parameter;
                this.value = startingValue;
            }

            public override string ToString()
            {
                return $"{this.Parameter.Name}: {this.Value:G2}";
            }
        }
    }
}