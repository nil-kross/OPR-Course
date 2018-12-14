using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    class Program {
        static void Main(string[] args)
        {
            var range = 100;
            Logger logger = new Logger();
            Parameter x = new Parameter("X", -1 * range , range, 1);
            Parameter y = new Parameter("Y", -1 * range, range, 1);
            var @params = new List<Parameter>{
                x, y
            };
            Func<Chromosome, Fitness> fitness = (ind) => new Fitness(ind, (-1 * ind[x] * ind[x] + -1 * ind[y] * ind[y]));
            Func<IEnumerable<Fitness>, Double, IEnumerable<Fitness>> selection = (IEnumerable<Fitness> fitnesses, Double part) => {
                var chromosomesList = new List<Fitness>();

                if (true) {
                    IList<Fitness> fitnessesArray = fitnesses.ToArray();

                    Array.Sort((Array)fitnessesArray);
                    fitnessesArray = new List<Fitness>(fitnessesArray.Reverse());
                    {
                        var length = Math.Min(fitnessesArray.Count, (Int32)(part * fitnessesArray.Count));

                        for (var i = 0; i < length; i++) {
                            chromosomesList.Add(fitnessesArray[i]);
                        }
                    }
                }
                if (false) {
                    foreach (var chromosomeFitness in fitnesses) {

                        chromosomesList.Add(chromosomeFitness);
                    }
                }

                return chromosomesList;
            };

            while (true)
            {StartingPopulationResolver populationResolver = new RandomStartingPopulationResolver(10, @params);
                GeneticAlgorithm ga = new GeneticAlgorithm(
                    populationResolver,
                    selection,
                    99,
                    fitness,
                    0.10,
                    0.50,
                    null,
                    new TimeSpan(0, 0, 10, 0)
                );

                Console.Clear();
                Console.WriteLine(" Параметры:");
                foreach (var par in @params)
                {
                    logger.WriteWithColor(String.Format("\t{0},\n", par), ConsoleColor.Gray);
                }
                logger.WriteWithColor(" Алгоритм начинает выполнение..\n", ConsoleColor.Gray);
                var res = ga.Compute();
                logger.WriteWithColor("\n Оптимальное решение:\n", ConsoleColor.Green);
                Console.WriteLine(String.Format("Приспособленность={0} | {1}", res.Value, res.Chromosome));

                Console.ReadKey();
            }
        }
    }
}