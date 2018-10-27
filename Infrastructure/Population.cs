using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Population {
        private UInt32 generationOrderValue = 0;
        private IList<Individual> individualsList = null;

        public IEnumerable<Individual> Individuals {
            get => this.individualsList;
        }

        public UInt32 GenerationOrder {
            get => this.generationOrderValue;
        }

        public Int32 Size {
            get => this.individualsList?.Count() ?? 0;
        }

        public Population(IEnumerable<Individual> startPopulation = null) {
            this.individualsList = new List<Individual>(startPopulation);
        }

        public override string ToString()
        {
            return $"Generation: {this.GenerationOrder}-nd order, Size: {this.Size}";
        }
    }
}