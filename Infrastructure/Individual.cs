using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Individual {
        private IList<Chromosome> chromosomesList = null;

        protected IEnumerable<Chromosome> Chromosomes {
            get => this.chromosomesList;
        }
    }
}