using System;
using System.Collections.Generic;
using System.Linq;

namespace Lomtseu {
    public class GeneticAlgorithm {
        private StartingPopulationResolver startingPopulationResolver = null;
        private Func<IEnumerable<Fitness>, Double, IEnumerable<Fitness>> selectionDelegate = null;
        private Func<Chromosome, Fitness> fitnessDelegate = null;
        private Double mutationChanceValue = 0.0f;
        private Double selectionPartValue = 1.0f;
        private UInt16 maxGenerationsCountValue = 1;
        private Logger logger = null;
        private Nullable<TimeSpan> maxTimeValue = null;

        public GeneticAlgorithm(
            StartingPopulationResolver resolver,
            Func<IEnumerable<Fitness>, Double, IEnumerable<Fitness>> selection,
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

            this.startingPopulationResolver = resolver;
            this.maxGenerationsCountValue = maxGenerationsCount;
            this.selectionDelegate = selection;
            this.fitnessDelegate = fitness;
            this.mutationChanceValue = mutationChance;
            this.selectionPartValue = selectionPart;
            this.logger = logger ?? new Logger();
            this.maxTimeValue = maxTime;
        }

        public Fitness Compute() {
            if (this.startingPopulationResolver == null) {
                throw new Exception("Starting population resolver was null!");
            }

            Fitness optimalSolutionFitness = null;
            ISet<Chromosome> chromosomesSet = new HashSet<Chromosome>();
            var startingTimeValue = DateTime.Now;
            var currentPopulation = this.startingPopulationResolver.StartingPopulation;
            var isEnd = false;

            /*
             TO DO: завершение по истечении времени
             */
            Population.currentGenerationOrderValue = 1;
            while (!isEnd) {
                IList<Fitness> fitnessesList = new List<Fitness>();

                this.logger.Log(currentPopulation.ToString());
                // Вычисление значений приспособленности:
                {
                    this.logger.WriteWithColor(" Приспособленность: ", ConsoleColor.Gray);
                    foreach (var chromosome in currentPopulation.Chromosomes)
                    {
                        var fitness = this.fitnessDelegate(chromosome);

                        this.logger.WriteWithColor($"#{fitness.Chromosome.Id}", ConsoleColor.Cyan);
                        this.logger.WriteWithColor("=", ConsoleColor.Gray);
                        this.logger.WriteWithColor($"{fitness.Value}, ", ConsoleColor.Yellow);
                        fitnessesList.Add(fitness);
                    }
                    this.logger.WriteWithColor("\n", ConsoleColor.White);
                }
                // Нахождение оптимального решения:
                foreach (var fitness in fitnessesList)
                {
                    if (optimalSolutionFitness == null ||
                        fitness.Value > optimalSolutionFitness.Value)
                    {
                        optimalSolutionFitness = fitness;
                    }
                }
                {
                    IList<Chromosome> chromosomesPoolList = null;
                    var crossedChromosomesList = new List<Chromosome>();
                    var childChromosomesList = new List<Chromosome>();
                    var mutatedChromosomesSet = new HashSet<Chromosome>();

                    // Селекция:
                    {
                        var selectedFitnessesList = this.selectionDelegate(fitnessesList, this.selectionPartValue);

                        foreach (var fitness in selectedFitnessesList) {
                            if (chromosomesPoolList == null) {
                                chromosomesPoolList = new List<Chromosome>();
                            }
                            chromosomesPoolList.Add(fitness.Chromosome);
                        }
                    }
                    // Мутация:
                    {
                        var mutatedCountValue = (Int32)(this.mutationChanceValue * chromosomesPoolList.Count());

                        for (var k = 0; k < mutatedCountValue; k++) {
                            var isDone = false;

                            while (!isDone) {
                                var indexValue = GreatRandom.Next(0, chromosomesPoolList.Count());
                                var chromosome = chromosomesPoolList[indexValue];

                                chromosomesPoolList.Remove(chromosome);
                                if (mutatedChromosomesSet.Add(chromosome)) {
                                    isDone = true;
                                    chromosome.Mutate();
                                }
                            }
                        }
                        if (mutatedChromosomesSet.Count > 0) {
                            this.logger.WriteWithColor(" Хромосомы ", ConsoleColor.Red);
                            foreach (var chromosome in mutatedChromosomesSet) {
                                this.logger.WriteWithColor(String.Format("#{0}", chromosome.Id), ConsoleColor.Cyan);
                                this.logger.WriteWithColor(", ", ConsoleColor.Gray);
                            }
                            this.logger.WriteWithColor(" мутировали!\n", ConsoleColor.Red);
                        }
                        chromosomesPoolList = new List<Chromosome>(chromosomesPoolList.Union(mutatedChromosomesSet));
                    }
                    // Размножение:
                    {
                        while (chromosomesPoolList.Count > 1) {
                            var firstIndexValue = GreatRandom.Next(0, chromosomesPoolList.Count);
                            var firstChromosome = chromosomesPoolList[firstIndexValue];

                            chromosomesPoolList.Remove(firstChromosome);
                            crossedChromosomesList.Add(firstChromosome);
                            {
                                var secondIndexValue = GreatRandom.Next(0, chromosomesPoolList.Count);
                                var secondChromosomes = chromosomesPoolList[secondIndexValue];

                                chromosomesPoolList.Remove(secondChromosomes);
                                crossedChromosomesList.Add(secondChromosomes);

                                {
                                    var childChromosomes = Chromosome.Cross(firstChromosome, secondChromosomes);

                                    childChromosomesList.AddRange(childChromosomes);
                                }
                            }
                        }
                        if (childChromosomesList.Count > 0) {
                            this.logger.WriteWithColor(" Скрещивание: ", ConsoleColor.Red);
                            foreach (var chromosome in childChromosomesList) {
                                this.logger.WriteWithColor(String.Format("#{0}", chromosome.Parents[0].Id), ConsoleColor.Cyan);
                                this.logger.WriteWithColor("+", ConsoleColor.Gray);
                                this.logger.WriteWithColor(String.Format("#{0}", chromosome.Parents[1].Id), ConsoleColor.Cyan);
                                this.logger.WriteWithColor("=>", ConsoleColor.Gray);
                                this.logger.WriteWithColor(String.Format("{0}", chromosome), ConsoleColor.White);
                                this.logger.WriteWithColor(", ", ConsoleColor.Gray);
                            }
                            this.logger.WriteWithColor("\n", ConsoleColor.Gray);
                        }
                    }
                    // Формирование новой популяции:
                    {
                        currentPopulation = new Population(
                            chromosomesPoolList
                                .Union(crossedChromosomesList)
                                .Union(childChromosomesList)
                        );
                    }
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