using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    class Program {
        static void Main(string[] args)
        {
            Logger logger = new Logger();
            Parameter x = new Parameter("X", 0, 3);
            Parameter y = new Parameter("Y", -3, 0);
            var @params = new List<Parameter>{
                x, y
            };
            Func<Chromosome, Fitness> fitness = (ind) => new Fitness(ind, ind[x] + ind[y]);
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
            StartingPopulationResolver populationResolver = new StartingPopulationResolver(new StartingPopulationResolver.RandomOptions(10, @params));
            GeneticAlgorithm ga = new GeneticAlgorithm(
                populationResolver,
                selection,
                99,
                fitness,
                0.10,
                0.40,
                null,
                new TimeSpan(0, 0, 1, 0)
            );

            Console.WriteLine(" Параметры:");
            foreach (var par in @params) {
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