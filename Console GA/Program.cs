using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    class Program {
        static void Main(string[] args)
        {
            var range = 1000;
            Logger logger = new Logger();
            Parameter x = new Parameter("X", -1 * range , range);
            Parameter y = new Parameter("Y", -1 * range, range);
            var @params = new List<Parameter>{
                x, y
            };
            Func<Chromosome, Fitness> fitness = (ind) => new Fitness(ind, (-1 * ind[x] * ind[x] -1 * ind[y] * ind[y] + 4));
            Func<IEnumerable<Fitness>, Double, Population> selection = (IEnumerable<Fitness> fitnesses, Double part) => {
                Population newPopulation = null;

                {
                    var chromosomesList = new List<Chromosome>();

                    if (true) {
                        var fitnessesArray = fitnesses.Reverse().ToArray();
                        Array.Sort(fitnessesArray);

                        {
                            var length = Math.Min(fitnessesArray.Length, (Int32)(part * fitnessesArray.Length));

                            for (var i = 0; i < length; i++) {
                                chromosomesList.Add(fitnessesArray[i].Chromosome);
                            }
                        }
                    }
                    if (false) {
                        foreach (var chromosomeFitness in fitnesses) {

                            chromosomesList.Add(chromosomeFitness.Chromosome);
                        }
                    }

                    newPopulation = new Population(chromosomesList);
                }

                return newPopulation;
            };
            StartingPopulationResolver populationResolver = new StartingPopulationResolver(new StartingPopulationResolver.RandomOptions(1000, @params));
            GeneticAlgorithm ga = new GeneticAlgorithm(
                populationResolver,
                selection,
                99,
                fitness,
                0.10,
                0.50,
                null,
                new TimeSpan(0, 0, 1, 0)
            );

            while (true)
            {
                Console.Clear();
                Console.WriteLine(" Параметры:");
                foreach (var par in @params)
                {
                    logger.WriteWithColor($"\t{par},\n", ConsoleColor.Gray);
                }
                logger.WriteWithColor(" Алгоритм начинает выполнение..\n", ConsoleColor.Gray);
                var res = ga.Compute();
                logger.WriteWithColor("\n Оптимальное решение:\n", ConsoleColor.Green);
                Console.WriteLine($"Приспособленность={res.Value} | {res.Chromosome}");

                Console.ReadKey();
            }
        }
    }
}