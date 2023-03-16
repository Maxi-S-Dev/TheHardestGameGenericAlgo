using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheHardestGame
{
    internal class Chromosome
    {
        public Rectangle mesh { get; set; }
        
        public List<MoveDirections> genes { get; set; }
    }

    internal enum MoveDirections
    {
        up, down, left, right
    }
}
