using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ansys.ACT.Interfaces.Common;
using Ansys.ACT.Interfaces.Mechanical;
using Ansys.ACT.Interfaces.Mesh;
using Ansys.ACT.Interfaces.UserObject;

namespace Ansys {
    public class Load {
        private readonly IMechanicalExtAPI _api;
        private readonly IMechanicalUserLoad _load;

        public Load(IExtAPI api, IUserLoad load) {
            _api = (IMechanicalExtAPI)api;
            _load = (IMechanicalUserLoad)load;
        }

        public virtual IEnumerable<double> getnodalvaluesfordisplay(IUserLoad load, IEnumerable<int> nodeIds) {
            var res = new List<double>();
            IMeshData mesh = _load.Analysis.MeshData;
            foreach (int nodeId in nodeIds) {
                INode node = mesh.NodeById(nodeId);
                res.Add(Math.Sqrt(node.X * node.X + node.Y * node.Y + node.Z * node.Z));
            }
            return res;
        }
    }
}