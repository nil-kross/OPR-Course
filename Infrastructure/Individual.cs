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
            var zigote = Chromosome.Cross(this.Chromosome, otherIndividual.Chromosome);

            return new Individual(zigote.);
        }

        public class Fitness {
            public Individual Individual { get; protected set; }
            public IEnumerable<Parameter.Argument> Arguments { get; protected set; }

            public Fitness(Individual individual, IEnumerable<Parameter.Argument> arguments) {
                this.Individual = individual;
                this.Arguments = arguments;
            }
        }
    }
}