using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    class Program {
        static void Main(string[] args)
        {
            Parameter x = new Parameter(new Parameter.Options() { Name = "X", Min = 0, Max = 8 });
            Parameter y = new Parameter(new Parameter.Options() { Name = "Y", Min = 0, Max = 32 });
            var @params = new List<Parameter>{
                x, y
            };
            Func<Chromosome, Fitness> fitness = (ind) => new Fitness(ind, DateTime.Now.Ticks);
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
            StartingPopulationResolver populationResolver = new StartingPopulationResolver(new StartingPopulationResolver.RandomOptions(5, @params));
            GeneticAlgorithm ga = new GeneticAlgorithm(populationResolver, selection, 10, fitness, 0.01);

            var res = ga.Compute();
            Console.WriteLine(res);

            Console.ReadKey();
        }
    }
}