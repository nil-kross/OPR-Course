using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Individual {
        private Chromosome chromosome = null;
        private Nullable<Decimal> fitnessValueNullable  = null;

        public Nullable<Decimal> Fitness {
            get => this.fitnessValueNullable;
        }

        public Chromosome Chromosome {
            get => this.chromosome;
        }

        public Individual(Chromosome chromosome) {
            this.chromosome = chromosome;
        }

        public Individual Cross(Individual otherIndividual) {
            var childChromosome = new Chromosome();

            return new Individual(childChromosome);
        }
    }
}