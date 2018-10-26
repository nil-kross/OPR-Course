using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class GeneticAlgorithm {
        private Population population = null;
        private FitnessDelegate fitnessDelegate = null;

        public GeneticAlgorithm(
            Population startingPopulation, 
            FitnessDelegate fitnessFunction
        ) {
            this.population = startingPopulation;
            this.fitnessDelegate = fitnessFunction;
        }
    }
}