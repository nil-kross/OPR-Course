using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Chromosome {
        private IList<Gene> genes;

        public IEnumerable<Gene> Genes {
            get => this.genes;
        }

        public Chromosome(IEnumerable<Gene> genes) {
            this.genes = new List<Gene>(genes);
        }
    }
}