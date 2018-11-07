using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Parameter {
        private String nameString = null;
        private Nullable<Decimal> minValue = null;
        private Nullable<Decimal> maxValue = null;

        public Boolean IsHasName {
            get => this.nameString != null && this.nameString.Length > 0;
        }

        public Decimal Min {
            get {
                Decimal minValue = 0;

                minValue = this.minValue.Value;

                return minValue;
            }
        }

        public Decimal Max {
            get {
                Decimal maxValue = 0;

                maxValue = this.maxValue.Value;

                return maxValue;
            }
        }

        public String Name {
            get => this.nameString;
        }

        public Parameter(String name, Decimal min, Decimal max) {
            this.nameString = name;
            this.minValue = min;
            this.maxValue = max;
        }

        public Argument GetRandomArgument() {
            var value = (Decimal)(
                GreatRandom.Next(
                    (Int32)this.Min,
                    (Int32)this.Max
                ) + GreatRandom.NextDouble()
            );

            return new Argument(this, value);
        }

        public override string ToString() {
            return $"{this.Min,3} < {this.Name} < {this.Max,3}";
        }
    }
}