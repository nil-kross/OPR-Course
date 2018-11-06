using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public static class GreatRandom {
        private readonly static Random random = new Random(GreatRandom.GetSeed);

        private static Int32 GetSeed {
            get {
                return DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Millisecond;
            }
        }

        public static Int32 Next() {
            return GreatRandom.random.Next();
        }

        public static Int32 Next(Int32 min, Int32 max) {
            return GreatRandom.random.Next(min, max);
        }

        public static Double NextDouble() {
            return GreatRandom.random.NextDouble();
        }
    }
}