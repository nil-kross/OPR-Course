using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lomtseu.Parameter;

namespace Lomtseu {
    public class Gene {
        private Parameter parameter;
        private Int32 positionValue = 0;
        private Int32 lengthValue = 0;

        public Decimal this[Boolean[] alleles] {
            get {
                Decimal value = 0;

                {
                    var mulpiplicatorValue = 1;

                    for (var i = this.positionValue; i < (this.positionValue + this.lengthValue); i++)
                    {
                        value += (alleles[i] ? 1 : 0) * mulpiplicatorValue;
                        mulpiplicatorValue *= 2;
                    }
                    value += this.parameter.Min;
                }

                return value;
            }
            set {
                if (this.IsValid(alleles, value)) {
                    var currValue = value + Math.Abs(this.parameter.Min);
                    var i = this.positionValue;

                    for (var j = this.positionValue; j < this.positionValue + this.lengthValue; j++) {
                        alleles[j] = false;
                    }
                    while (currValue > 0) {
                        var div = (Int32)currValue / 2;
                        var mod = (Int32)currValue % 2;

                        alleles[i] = mod == 1 ? true : false;
                        currValue = div;
                        i++;
                    }
                }
            }
        }

        public Int32 Position {
            get {
                return this.positionValue;
            }
            set {
                this.positionValue = value;
            }
        }

        public Int32 Length {
            get {
                if (this.lengthValue == 0) {
                    Int32 length = 0;

                    {
                        Int32 amplitudeValue = (Int32)(this.parameter.Max - this.parameter.Min);
                        
                        while (amplitudeValue > 0) {
                            amplitudeValue /= 2;
                            length++;
                        }
                    }

                    this.lengthValue = length;
                }

                return this.lengthValue;
            }
            protected set {
                this.lengthValue = value;
            }
        }

        public Parameter Parameter {
            get => this.parameter;
        }

        public Gene(Parameter parameter) {
            this.parameter = parameter;
            this.lengthValue = this.Length;
        }

        public Boolean IsValid(Boolean[] alleles, Nullable<Decimal> value = null) {
            var isValid = false;

            if (value == null) {
                value = this[alleles];
            }
            if (value <= this.parameter.Max && value >= this.parameter.Min) {
                isValid  = true;
            }

            return isValid;
        }

        public override String ToString() {
            return $"{this.parameter} | P: {this.Position,3} L: {this.Length,3}";
        }
    }
}