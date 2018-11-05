using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Gene {
        private Parameter.Argument argument;

        public Parameter.Argument Value {
            get => this.argument;
        }

        public Gene(Parameter.Argument startingArgument)
        {
            this.argument = startingArgument;
        }
    }
}