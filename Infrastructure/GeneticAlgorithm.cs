using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class GeneticAlgorithm {
        private Population population = null;

        public GeneticAlgorithm(Population startingPopulation)
        {
            this.population = startingPopulation;
        }
    }
}