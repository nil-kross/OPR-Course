using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu
{
    public class Parameter
    {
        private String nameString = null;
        private Nullable<Decimal> minValue = null;
        private Nullable<Decimal> maxValue = null;

        public Boolean IsHasName {
            get {
                return this.nameString != null && this.nameString.Length > 0;
            }
        }

        public Decimal Min {
            get {
                return this.minValue.HasValue ? this.minValue.Value : Decimal.MinValue;
            }
        }

        public Decimal Max {
            get {
                return this.maxValue.HasValue ? this.maxValue.Value : Decimal.MaxValue;
            }
        }

        public String Name {
            get {
                return this.nameString;
            }
        }

        public Parameter(String name, Decimal min, Decimal max)
        {
            this.nameString = name;
            this.minValue = min;
            this.maxValue = max;
        }

        public Argument GetRandomArgument()
        {
            var value = (Decimal)(
                GreatRandom.Next(
                    (Int32)this.Min,
                    (Int32)this.Max
                ) + GreatRandom.NextDouble()
            );

            return new Argument(this, value);
        }

        public override string ToString()
        {
            return String.Format("{0,3} < {1} < {2,3}", this.Min,  this.Name, this.Max);
        }
    }
}