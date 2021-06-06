using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kamisado
{
    class Coordinate
    {
        public byte X { get; set; }
        public byte Y { get; set; }

        public Coordinate(byte x = 0, byte y = 0) => (this.X, this.Y) = (x, y);

        public static bool operator ==(Coordinate c1, Coordinate c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Coordinate c1, Coordinate c2)
        {
            return !c1.Equals(c2);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Coordinate);
        }

        public bool Equals(Coordinate coor)
        {
            if (ReferenceEquals(coor, null)) return false;
            return X == coor.X && Y == coor.Y;
        }
    }
}
