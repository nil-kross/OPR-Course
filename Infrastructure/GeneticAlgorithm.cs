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

        public GeneticAlgorithm(
            IEnumerable<Parameter> parameters,
            Population startingPopulation, 
            FitnessDelegate fitnessFunction
        ) {
            this.parametersList = new List<Parameter>(parameters);
            this.population = startingPopulation;
            this.fitnessDelegate = fitnessFunction;
        }

        public Task<IEnumerable<Parameter.Argument>> Compute() {
            this.population.Current.

            return null; // IMP
        }
    }
}