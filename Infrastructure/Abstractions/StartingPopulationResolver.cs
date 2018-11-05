using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lomtseu.Parameter;

namespace Lomtseu.Abstractions {
    public class StartingPopulationResolver {
        public Population StartingPopulation { get; protected set; }

        public StartingPopulationResolver(Options options) {
            this.StartingPopulation = options.StartingPopulation;
        }

        public abstract class Options {
            public Population StartingPopulation { get; protected set; }
        }

        public class ManualOptions : Options {
            public ManualOptions(Population starting) {
                this.StartingPopulation = starting;
            }
        }

        public class RandomOptions : Options {
            public RandomOptions(Int32 populationSize, IEnumerable<Parameter> parameters) {
                var random = new Random(DateTime.Now.Millisecond);
                Population startingPopulation = null;

                {
                    IList<Chromosome> chromosomesList = new List<Chromosome>();

                    for (var i = 0; i < populationSize; i++) {
                        Chromosome chromosome = null;

                        {
                            IList<Argument> arguments = new List<Argument>();

                            foreach (var parameter in parameters) {
                                arguments.Add(new Argument(parameter, parameter.GetRandomArgument().Value));
                            }

                            chromosome = new Chromosome(arguments);
                        }
                    }
                }

                this.StartingPopulation = startingPopulation;
            }
        }
    }
}