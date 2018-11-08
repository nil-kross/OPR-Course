using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lomtseu.Parameter;

namespace Lomtseu {
    public abstract class StartingPopulationResolver {
        public abstract Population StartingPopulation { get; protected set; }
    }

    public class ManualStartingPopulationResolver : StartingPopulationResolver {
        private Population startingPopulation = null;

        public override Population StartingPopulation { 
            get {
                return this.startingPopulation;
            }
            protected set {
                this.startingPopulation = value;
            }
        }

        public ManualStartingPopulationResolver(Population starting) {
            this.StartingPopulation = starting;
        }
    }

    public class RandomStartingPopulationResolver : StartingPopulationResolver {
        private Int32 populationSizeValue;
        private IEnumerable<Parameter> parametersEnumerable;

        public override Population StartingPopulation {
            get {
                Population startingPopulation = null;
                IList<Chromosome> chromosomesList = new List<Chromosome>();

                for (var i = 0; i < populationSizeValue; i++) {
                    Chromosome chromosome = null;

                    {
                        IList<Argument> arguments = new List<Argument>();

                        foreach (var parameter in parametersEnumerable) {
                            arguments.Add(new Argument(parameter, parameter.GetRandomArgument().Value));
                        }

                        chromosome = new Chromosome(arguments);
                        chromosomesList.Add(chromosome);
                    }
                }
                startingPopulation = new Population(chromosomesList);

                return startingPopulation;
            }
            protected set { }
        }

        public RandomStartingPopulationResolver(Int32 populationSize, IEnumerable<Parameter> parameters) {
            this.populationSizeValue = populationSize;
            this.parametersEnumerable = parameters;
        }
    }
}