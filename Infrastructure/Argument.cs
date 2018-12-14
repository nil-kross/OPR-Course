using System;

namespace Lomtseu {
    public class Argument {
        private Parameter parameter = null;
        private Decimal value = Decimal.Zero;

        public Parameter Parameter {
            get {
                return this.parameter;
            }
        }

        public Decimal Value {
            get {
                return this.value;
            }
        }

        public Argument(
            Parameter parameter,
            Decimal startingValue
        ) {
            this.parameter = parameter;
            this.value = startingValue;
        }

        public override String ToString() {
            return String.Format(
                "{0}={1,3}",
                this.Parameter.Name,
                this.Value
            );
        }
    }
}