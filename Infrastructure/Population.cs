using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Population {
        private UInt32 maxGenerationCountValue = 0;
        private IList<Generation> generationsList = null;

        public Generation Current {
            get => this.generationsList[(Int32)this.maxGenerationCountValue];
        }

        public UInt32 MaxGenerationsCount {
            get => this.maxGenerationCountValue;
        }

        public IEnumerable<Generation> Generations {
            get => this.generationsList;
        }

        public Population(Population.Options options) {
            this.maxGenerationCountValue = options.MaxGenerationsCount ?? 0;
            this.generationsList = new List<Generation>();

            if (options.Starting != null) {
                this.generationsList.Add(options.Starting);
            }
        }

        public class Options {
            public Nullable<UInt32> MaxGenerationsCount { get; set; }

            public Generation Starting { get; set; }
        }
    }
}