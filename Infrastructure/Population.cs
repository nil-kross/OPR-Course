using System;
using System.Collections.Generic;

namespace Lomtseu {
    public class Population {
        internal static byte currentGenerationOrderValue = 0;

        private IList<Chromosome> chromosomesList = null;
        private Byte generationOrderValue = 0;

        public IList<Chromosome> Chromosomes {
            get {
                return this.chromosomesList;
            }
        }

        public Byte GenerationOrder {
            get {
                return this.generationOrderValue;
            }
        }

        public Population(IEnumerable<Chromosome> chromosomes) {
            this.chromosomesList = new List<Chromosome>(chromosomes);
            this.generationOrderValue = currentGenerationOrderValue++;
        }

        public override String ToString() {
            var chromosomesString = "";

            foreach (var chromosome in this.chromosomesList) {
                chromosomesString += "\t" + chromosome.ToString() + "\n";
            }

            return String.Format(
                "№ {0,-3} S: {1,-3}\n{2}",
                this.GenerationOrder,
                this.chromosomesList.Count,
                chromosomesString
            );
        }
    }
}