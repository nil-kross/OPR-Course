using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    class Program {
        static void Main(string[] args)
        {
            Parameter x = new Parameter(new Parameter.Options() { Name = "X", Min = 0, Max = 42 });
            Parameter y = new Parameter(new Parameter.Options() { Name = "Y", Min = 0, Max = 128 });
            var @params = new List<Parameter>{
                x, y
            };
            FitnessDelegate fitness = (ind) => 42.0M;
            Population pop = new Population(new Population.Options() { Starting =  });

            GeneticAlgorithm ga = new GeneticAlgorithm(@params, pop, fitness);

            var res = ga.Compute().Result;
            
            foreach (var value in res) {
                Console.WriteLine(value);
            }
            Console.ReadKey();
        }
    }
}