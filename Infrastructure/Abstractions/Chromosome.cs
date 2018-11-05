using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu.Abstractions
{
    public abstract class Chromosome
    {
        private IList<Gene> genesList = null;

        public abstract void Mutate();
    }
}