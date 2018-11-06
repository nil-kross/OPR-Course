using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
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

        public override String ToString() {
            return $"{this.Parameter.Name}={this.Value:G2}";
        }
    }
}