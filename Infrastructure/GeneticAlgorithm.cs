using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class GeneticAlgorithm {
        private Population startingPopulation;
        private Func<IEnumerable<Fitness>, Double, Population> selectionDelegate = null;
        private Func<Chromosome, Fitness> fitnessDelegate = null;
        private Double mutationChanceValue = 0.0f;
        private Double selectionPartValue = 1.0f;
        private UInt16 maxGenerationsCountValue = 1;
        private Logger logger = null;
        private Nullable<TimeSpan> maxTimeValue = null;

        public GeneticAlgorithm(
            StartingPopulationResolver resolver,
            Func<IEnumerable<Fitness>, Double, Population> selection,
            UInt16 maxGenerationsCount,
            Func<Chromosome, Fitness> fitness,
            Double mutationChance,
            Double selectionPart,
            Logger logger = null,
            Nullable<TimeSpan> maxTime = null
        ) {
            if (!(mutationChance >= 0 && mutationChance <= 1)) {
                throw new Exception("Шанс мутации должен принадлежать интервалу [0; 1]!");
            }
            if (!(selectionPart > 0 && selectionPart <= 1)) {
                throw new Exception("Доля отбора должена принадлежать полуинтервалу (0; 1]!");
            }

            this.startingPopulation = resolver.StartingPopulation;
            this.maxGenerationsCountValue = maxGenerationsCount;
            this.selectionDelegate = selection;
            this.fitnessDelegate = fitness;
            this.mutationChanceValue = mutationChance;
            this.selectionPartValue = selectionPart;
            this.logger = logger ?? new Logger();
            this.maxTimeValue = maxTime;
        }

        public Fitness Compute() {
            Fitness optimalSolutionFitness = null;
            var startingTimeValue = DateTime.Now;
            var currentPopulation = this.startingPopulation;
            var isEnd = false;

            /*
             TO DO: завершение по истечении времени
             */
            while (!isEnd)
            {
                IList<Fitness> fitnessesList = new List<Fitness>();

                this.logger.Log(currentPopulation.ToString());
                foreach (var chromosome in currentPopulation.Chromosomes)
                {
                    var fitness = this.fitnessDelegate(chromosome);

                    fitnessesList.Add(fitness);
                }
                {
                    this.logger.WriteWithColor(" Приспособленность: ", ConsoleColor.Gray);
                    foreach (var fitness in fitnessesList) {
                        logger.WriteWithColor($"{fitness}, ", ConsoleColor.Blue);
                    }
                    this.logger.WriteWithColor("\n", ConsoleColor.White);
                }
                foreach (var fitness in fitnessesList) {
                    if (optimalSolutionFitness == null ||
                        fitness.Value > optimalSolutionFitness.Value) {
                        optimalSolutionFitness = fitness;
                    }
                }
                {
                    var newPopulation = this.selectionDelegate(fitnessesList, this.selectionPartValue);

                    currentPopulation = newPopulation;
                }

                foreach (var chromosome in currentPopulation.Chromosomes) {
                    Double chance = (Double)((GreatRandom.Next(0, 100) * 1.0f) / 100);
                    if (chance < this.mutationChanceValue) {
                        chromosome.Mutate();
                        logger.WriteWithColor($" Хромосома #{chromosome.Id} мутировала!\n", ConsoleColor.Red);
                    }
                }

                if (currentPopulation.Chromosomes.Count() <= 1) {
                    this.logger.WriteWithColor(" Осталась одна хромосома.", ConsoleColor.Red);
                    isEnd = true;
                }
                if (currentPopulation.GenerationOrder >= this.maxGenerationsCountValue) {
                    this.logger.WriteWithColor(" Достигнуто ограничение количества поколений.", ConsoleColor.Red);
                    isEnd = true;
                }
                if (this.maxTimeValue.HasValue &&
                    (DateTime.Now - this.maxTimeValue) > startingTimeValue) {
                    this.logger.WriteWithColor(" Достигнуто ограничение времени выполнения.", ConsoleColor.Red);
                    isEnd = true;
                }
            }

            return optimalSolutionFitness;
        }
    }
}