using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu.Abstractions
{
    public abstract class Population
    {
        private static byte currentGenerationOrderValue = 0;

        private IList<Chromosome> chromosomesList = null;
        private Byte generationOrderValue = 0;

        public IEnumerable<Chromosome> Chromosomes {
            get => this.chromosomesList;
        }

        public Byte GenerationOrder {
            get => this.generationOrderValue;
        }

        public Population(IEnumerable<Chromosome> chromosomes)
        {
            this.chromosomesList = new List<Chromosome>(chromosomes);
            this.generationOrderValue = currentGenerationOrderValue++;
        }
    }
}