using Ansys.ACT.Core;
using Ansys.ACT.Interfaces.Common;
using System;
using Ansys.ACT.WorkBench.Project;

namespace Ansys {
    public class GenA {
        private readonly IExtAPI api;

        public GenA(IExtAPI api = null) {
            this.api = api;
        }

        public override String ToString() {
            var str = "GenA says " + this.api?.Context ?? "null";

            dynamic e = (api as dynamic).DataModel;
            if (e != null) {
                str += " also " + e.ToString();
            }

            return str;
        }

        public String ToString(params dynamic[] args) {
            return this.ToString();
        }
    }
}