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
            ISet<Chromosome> chromosomesSet = new HashSet<Chromosome>();
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
                // Вычисление значений приспособленности:
                this.logger.WriteWithColor(" Приспособленность: ", ConsoleColor.Gray);
                foreach (var chromosome in currentPopulation.Chromosomes)
                {
                    var fitness = this.fitnessDelegate(chromosome);

                    logger.WriteWithColor($"{fitness}, ", ConsoleColor.Blue);
                    fitnessesList.Add(fitness);
                }
                this.logger.WriteWithColor("\n", ConsoleColor.White);
                // Мутация:
                {
                    var mutatedChromosomesSet = new HashSet<Chromosome>();
                    var mutatedCountValue = (Int32)(this.mutationChanceValue * currentPopulation.Chromosomes.Count());

                    for (var k = 0; k < mutatedCountValue; k++) {
                        var isDone = false;

                        while (!isDone) {
                            var indexValue = GreatRandom.Next(0, currentPopulation.Chromosomes.Count());
                            var chromosome = currentPopulation.Chromosomes[indexValue];

                            if (mutatedChromosomesSet.Add(chromosome)) {
                                isDone = true;
                                chromosome.Mutate();
                                this.logger.WriteWithColor($" Хромосома #{chromosome.Id} мутировала!\n", ConsoleColor.Red);
                            }
                        }
                    }
                }
                // Нахождение оптимального решения:
                foreach (var fitness in fitnessesList) {
                    if (optimalSolutionFitness == null ||
                        fitness.Value > optimalSolutionFitness.Value) {
                        optimalSolutionFitness = fitness;
                    }
                }
                // Скрещивание:
                {
                    var uncrossedChromosomesList = currentPopulation.Chromosomes;
                    var crossedChromosomesList = new List<Chromosome>();

                }
                // Селекция:
                {
                    var newPopulation = this.selectionDelegate(fitnessesList, this.selectionPartValue);

                    currentPopulation = newPopulation;
                }
                // Условия окончания алгормтма:
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