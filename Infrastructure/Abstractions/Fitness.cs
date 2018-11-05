using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu.Abstractions
{
    public abstract class Fitness
    {
        private Chromosome chromosome = null;
        private Decimal value = 0;

        public Chromosome Chromosome 
        {
            get => this.chromosome;
        }

        public Decimal Value 
        {
            get => this.value;
        }

        public Fitness(Chromosome chromosome, Decimal value)
        {
            this.chromosome = chromosome;
            this.value = value;
        }
    }
}