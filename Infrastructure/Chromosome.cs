using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lomtseu.Parameter;

namespace Lomtseu {
    public class Chromosome {
        private static Byte idLengthValue = 3;
        private static ISet<String> idsSet = new HashSet<String>();

        private String identifierString = null;
        private IList<Gene> genesList = null;
        private Boolean[] allelesArray = null;

        public String Id {
            get {
                return this.identifierString;
            }
            protected set {
                this.identifierString = value;
            }
        }

        public IEnumerable<Gene> Genes {
            get {
                return this.genesList;
            }
        }

        public Decimal this[Parameter parameter] {
            get {
                Nullable<Decimal> value = null;

                foreach (var gene in this.genesList) {
                    if (gene.Parameter == parameter) {
                        value = gene[this.allelesArray];
                    }
                }

                return value.Value;
            }
        }

        public IList<Chromosome> Parents { get; internal set; }

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

            this.GenerateId();
        }

        public Chromosome(Boolean[] alleles, IList<Gene> genes) {
            this.allelesArray = alleles;
            this.genesList = new List<Gene>(genes);

            this.GenerateId();
        }

        public void Mutate() {
            if (this.allelesArray != null) {
                var isDone = false;

                while (!isDone) {
                    var indexValue = GreatRandom.Next(0, this.allelesArray.Length - 1);
                    var prevValue = this.allelesArray[indexValue];

                    this.allelesArray[indexValue] = !(this.allelesArray[indexValue]);
                    {
                        var isValid = true;

                        foreach (var gene in this.genesList) {
                            if (gene.IsValid(this.allelesArray) == false) {
                                isValid = false;
                            }
                        }
                        isDone = isValid;
                        if (!isValid) {
                            this.allelesArray[indexValue] = prevValue;
                        }
                    }
                }
            } else {
                throw new Exception("Can't mutate this chromosome. Alleles not created yet!");
            }
        }

        public override String ToString() {
            var allelesString = "";
            var argumentsString = "";

            for (var i = 0; i < this.allelesArray.Length; i++) {
                var allel = this.allelesArray[i];

                allelesString += allel == true ? "1" : "0";
                if (i > 0) {
                    var isGeneEnd = false;

                    foreach (var gene in this.genesList) {
                        if (gene.Position - 1 == i) {
                            isGeneEnd = true;
                        }
                    }

                    if (isGeneEnd) {
                        allelesString += '|';
                    }
                }
            }
            foreach (var gene in this.genesList) {
                var value = gene[this.allelesArray];
                argumentsString += $"{gene.Parameter.Name}={value};";
            }

            return $"#{this.Id} [{allelesString}]=({argumentsString})";
        }

        public static IEnumerable<Chromosome> Cross(Chromosome first, Chromosome second) {
            List<Chromosome> childChromosomesList = null;

            {
                var lengthValue = first.allelesArray.Length;
                var firstAllelesArray = new Boolean[lengthValue];
                var secondAllelesArray = new Boolean[lengthValue];
                var indexValue = GreatRandom.Next(1, lengthValue - 1);

                for (var i = 0; i < indexValue; i++) {
                    firstAllelesArray[i] = first.allelesArray[i];
                    secondAllelesArray[i] = second.allelesArray[i];
                }
                for (var i = indexValue; i < lengthValue; i++) {
                    firstAllelesArray[i] = second.allelesArray[i];
                    secondAllelesArray[i] = first.allelesArray[i];
                }
                childChromosomesList = new List<Chromosome>() {
                    new Chromosome(firstAllelesArray, first.genesList),
                    new Chromosome(secondAllelesArray, first.genesList),
                };
                foreach (var childChromosome in childChromosomesList) {
                    childChromosome.Parents = new Chromosome[2] { first, second };
                }
            }

            return childChromosomesList;
        }

        private void GenerateId() {
            String identifierString = null;
            var isDone = false;

            while (!isDone) {
                identifierString = "";
                for (var i = 0; i < Chromosome.idLengthValue; i++) {
                    var currChar = '-';

                    if (GreatRandom.Next(0, 100) < 60) {
                        currChar = (Char)GreatRandom.Next('A', 'Z');
                    } else {
                        currChar = (Char)GreatRandom.Next('0', '9');
                        ;
                    }
                    identifierString += currChar;
                }
                isDone = Chromosome.idsSet.Add(identifierString);
            }

            this.identifierString = identifierString;
        }
    }
}