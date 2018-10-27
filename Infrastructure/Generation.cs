using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Generation {
        private UInt32 orderValue = 0;
        private IList<Individual> individualsList = null;

        public IEnumerable<Individual> Individuals {
            get => this.individualsList;
        }

        public UInt32 GenerationOrder {
            get => this.orderValue;
        }

        public Int32 Size {
            get => this.individualsList?.Count() ?? 0;
        }

        public Generation(IEnumerable<Individual> startPopulation = null) {
            this.individualsList = new List<Individual>(startPopulation);
        }

        public override string ToString() {
            return $"Generation: {this.GenerationOrder}-nd order, Size: {this.Size}";
        }
    }
}