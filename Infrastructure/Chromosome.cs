using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lomtseu.Parameter;

namespace Lomtseu {
    public class Chromosome {
        private IList<Gene> genesList = null;

        public Chromosome(IEnumerable<Argument> arguments) {
            throw new NotImplementedException();
        }

        public void Mutate() {
            throw new NotImplementedException();
        }
    }
}