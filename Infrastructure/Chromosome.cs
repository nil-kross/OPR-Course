using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lomtseu.Parameter;

namespace Lomtseu {
    public class Chromosome {
        private IList<Gene> genesList = null;
        private Boolean[] allelesArray = null;

        public Chromosome(IList<Argument> arguments) {
            this.genesList = new List<Gene>();

            foreach (var arg in arguments) {
                var gene = new Gene(arg.Parameter);

                genesList.Add(gene);
            }
            {
                Int32 totalLengthValue = 0;
                Int32 currPositionValue = 0;

                foreach (var gene in this.genesList) {
                    gene.Position = currPositionValue;
                    totalLengthValue += gene.Length;
                    currPositionValue += gene.Length;
                }
                this.allelesArray = new Boolean[totalLengthValue];
            }
            for (var i = 0; i < arguments.Count(); i++) { 
                var gene = this.genesList[i];
                var arg = arguments[i];

                gene[this.allelesArray] = arg.Value;
            }
        }

        public void Mutate() {
            if (this.allelesArray != null) {
                var indexValue = GreatRandom.Next(0, this.allelesArray.Length - 1);

                this.allelesArray[indexValue] = !(this.allelesArray[indexValue]);
            } else {
                throw new Exception("Can't mutate this chromosome. Alleles not created yet!");
            }
        }

        public override String ToString() {
            var allelesString = "";
            var argumentsString = "";

            foreach (var allel in this.allelesArray) {
                allelesString += allel == true ? "1" : "0";
            }
            foreach (var gene in this.genesList) {
                argumentsString += $"{gene[this.allelesArray].ToString()};";
            }

            return $"[{allelesString}]=({argumentsString})";
        }
    }
}