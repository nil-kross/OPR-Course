using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class GeneticAlgorithm {
        private IList<Parameter> parametersList = null;
        private Population population = null;
        private FitnessDelegate fitnessDelegate = null;
        private CrossingDelegate crossingDelegate = null;

        public GeneticAlgorithm(GeneticAlgorithm.Options options) {
            this.parametersList = new List<Parameter>(options.Parameters);
            this.population = options.Population;
            this.fitnessDelegate = options.Fitness;
        }

        public Task<IEnumerable<Parameter.Argument>> Compute() {
            var currentGeneration = this.population.Current;

            var parametersArguments = currentGeneration;

            return null; // IMP
        }

        public class Options {
            public IEnumerable<Parameter> Parameters { get; set; }
            public Population Population { get; set; }
            public FitnessDelegate Fitness { get; set; }
            public CrossingDelegate Crossing { get; set; }
        }
    }
}