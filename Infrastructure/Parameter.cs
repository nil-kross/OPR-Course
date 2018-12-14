using System;

namespace Lomtseu {
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

        public Nullable<Byte> Accuracy { get; protected set; }

        public Parameter(String name, Decimal? min = null, Decimal? max = null, Byte? accuracy = 0)
        {
            this.nameString = name;
            this.minValue = min;
            this.maxValue = max;
            this.Accuracy = accuracy;
        }

        public Argument GetRandomArgument()
        {
            var value = (Decimal)(
                GreatRandom.Next(
                    (Int32)this.Min,
                    (Int32)this.Max
                ) + GreatRandom.NextDouble()
            );

            return new Argument(this, Math.Round(value, (Int32)this.Accuracy));
        }

        public override String ToString()
        {
            return String.Format("{0,3} < {1} < {2,3} .{3}", this.Min,  this.Name, this.Max, this.Accuracy);
        }
    }
}