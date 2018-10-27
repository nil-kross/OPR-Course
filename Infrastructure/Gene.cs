using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Gene
    {
        private Parameter.Point value;

        public Parameter.Point Value {
            get => this.value;
        }

        public Gene(Parameter.Point value)
        {
            this.value = value;
        }
    }
}