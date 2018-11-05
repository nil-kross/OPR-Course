using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Chromosome {
        private IList<Gene> genes;
        private Boolean[] allelesValuesArray = null;

        public Boolean[] Alleles {
            get => this.allelesValuesArray;
            protected set {
                this.allelesValuesArray = value;
            }
        }
        public IEnumerable<Gene> Genes {
            get => this.genes;
        }

        public Chromosome(IEnumerable<Gene> genes) {
            this.genes = new List<Gene>(genes);
        }

        public static CrossingResult Cross(Chromosome first, Chromosome second) {
            Random random = new Random(DateTime.Now.Millisecond);
            Int32 genesCountValue = first.Genes.Count();
            Int32 crossingPointValue = random.Next(0, first.Genes.Count() - 1);
            var firstGenesList = new List<Gene>();
            var secondGenesList = new List<Gene>();

            firstGenesList.AddRange(second.Genes.Take(crossingPointValue));
            secondGenesList.AddRange(first.Genes.Take(crossingPointValue));
            firstGenesList.AddRange(second.Genes.Skip(crossingPointValue).Take(genesCountValue - crossingPointValue));
            secondGenesList.AddRange(first.Genes.Skip(crossingPointValue).Take(genesCountValue - crossingPointValue));

            return new CrossingResult() {
                First = new Chromosome(firstGenesList), 
                Second = new Chromosome(secondGenesList)
            };
        }

        public class CrossingResult {
            public Chromosome First { get; set; }
            public Chromosome Second { get; set; }
        }
    }
}