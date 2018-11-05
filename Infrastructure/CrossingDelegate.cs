using System.Collections.Generic;

namespace Lomtseu
{
    public delegate IEnumerable<Chromosome> CrossingDelegate(IEnumerable<Chromosome> individuals);
}