using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class GeneticAlgorithm {
        private Population startingPopulation;
        private Func<IEnumerable<Fitness>, Population> selectionDelegate = null;
        private Func<Chromosome, Fitness> fitnessDelegate = null;
        private Double mutationChanceValue = 0.0f;
        private UInt16 maxGenerationsCountValue = 1;
        private Logger logger = null;

        public GeneticAlgorithm(
            StartingPopulationResolver resolver,
            Func<IEnumerable<Fitness>, Population> selection,
            UInt16 maxGenerationsCount,
            Func<Chromosome, Fitness> fitness,
            Double mutationChance,
            Logger logger = null
        ) {
            this.startingPopulation = resolver.StartingPopulation;
            this.maxGenerationsCountValue = maxGenerationsCount;
            this.selectionDelegate = selection;
            this.fitnessDelegate = fitness;
            this.logger = logger ?? new Logger();
        }

        public Population Compute() {
            var currentPopulation = this.startingPopulation;

            /*
             TO DO: завершение по истечении времени
             */
            while (currentPopulation.GenerationOrder <= this.maxGenerationsCountValue)
            {
                IList<Fitness> fitnessesList = new List<Fitness>();

                this.logger.Log(currentPopulation.ToString());
                foreach (var chromosome in currentPopulation.Chromosomes)
                {
                    var fitness = this.fitnessDelegate(chromosome);

                    fitnessesList.Add(fitness);
                }

                {
                    var newPopulation = this.selectionDelegate(fitnessesList);

                    currentPopulation = newPopulation;
                }

                foreach (var chromosome in currentPopulation.Chromosomes) {
                    if (GreatRandom.NextDouble() > this.mutationChanceValue) {
                        chromosome.Mutate();
                    }
                }
            }

            return currentPopulation;
        }
    }
}