﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Fitness : IComparable {
        private Chromosome chromosome = null;
        private Decimal value = 0;

        public Chromosome Chromosome {
            get => this.chromosome;
        }

        public Decimal Value {
            get => this.value;
        }

        public Fitness(Chromosome chromosome, Decimal value) {
            this.chromosome = chromosome;
            this.value = value;
        }

        public override String ToString() {
            return $"#{this.chromosome.Id}={this.value}";
        }

        public Int32 CompareTo(Object obj) {
            var comparationValue = 0;

            if (obj is Fitness otherFitness) {
                return this.Value.CompareTo(otherFitness.Value);
            }

            return comparationValue;
        }
    }
}