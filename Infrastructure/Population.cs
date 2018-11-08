using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Population {
        internal static byte currentGenerationOrderValue = 0;

        private IList<Chromosome> chromosomesList = null;
        private Byte generationOrderValue = 0;

        public IList<Chromosome> Chromosomes {
            get => this.chromosomesList;
        }

        public Byte GenerationOrder {
            get => this.generationOrderValue;
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

            return $"№ {this.GenerationOrder,-3} S: {this.chromosomesList.Count,-3}\n{chromosomesString}";
        }
    }
}