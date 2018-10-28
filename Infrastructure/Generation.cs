using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Generation {
        private UInt32 orderValue = 0;
        private IList<Individual> individualsList = null;

        public IEnumerable<Individual> Individuals {
            get => this.individualsList;
        }

        public UInt32 GenerationOrder {
            get => this.orderValue;
        }

        public Int32 Size {
            get => this.individualsList?.Count() ?? 0;
        }

        public Generation(IEnumerable<Individual> startPopulation = null) {
            this.individualsList = new List<Individual>(startPopulation);
        }

        public void Cross() {
            var childIndividualsList = new List<Individual>();
            var crossedIndividualsList = new List<Individual>();
            var uncrossedIndividualsList = new List<Individual>(this.individualsList);

            {
                var isDone = false;
                var random = new Random(DateTime.Now.Millisecond);

                while (!isDone) {
                    if (uncrossedIndividualsList.Count >= 2) {
                        var firstIndividual 
                            = uncrossedIndividualsList[random.Next(0, uncrossedIndividualsList.Count - 1)];

                        uncrossedIndividualsList.Remove(firstIndividual);

                        {
                            var secondIndividual = uncrossedIndividualsList[random.Next(0, uncrossedIndividualsList.Count - 1)];

                            uncrossedIndividualsList.Remove(secondIndividual);

                            {
                                var childIndividual = firstIndividual.Cross(secondIndividual);
                            }
                        }
                    } else {
                        isDone = true;
                    }
                }
            }

            this.individualsList = new List<Individual>(
                crossedIndividualsList
                .Concat(uncrossedIndividualsList)
                .Concat(childIndividualsList)
            );
        }

        public override string ToString() {
            return $"Generation: {this.GenerationOrder}-nd order, Size: {this.Size}";
        }
    }
}