using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    class Program {
        static void Main(string[] args)
        {
            Parameter x = new Parameter("X", 0, 16);
            Parameter y = new Parameter("Y", -6, 4);
            var @params = new List<Parameter>{
                x, y
            };
            Func<Chromosome, Fitness> fitness = (ind) => new Fitness(ind, GreatRandom.Next());
            Func<IEnumerable<Fitness>, Population> selection = (IEnumerable<Fitness> fitnesses) => {
                Population newPopulation = null;

                {
                    var chromosomesList = new List<Chromosome>();

                    foreach (var chromosomeFitness in fitnesses) {
                        chromosomesList.Add(chromosomeFitness.Chromosome);
                    }

                    newPopulation = new Population(chromosomesList);
                }

                return newPopulation;
            };
            StartingPopulationResolver populationResolver = new StartingPopulationResolver(new StartingPopulationResolver.RandomOptions(3, @params));
            GeneticAlgorithm ga = new GeneticAlgorithm(populationResolver, selection, 9, fitness, 25);

            var res = ga.Compute();
            Console.WriteLine();
            Console.WriteLine("Оптимальное решение:");
            Console.WriteLine(res);

            Console.ReadKey();
        }
    }
}