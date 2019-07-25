using System.Drawing;

namespace DrawWork
{
    public class PathCommands
    {
        public PointF P;
        public char Pc;

        public PathCommands(PointF p, char pc)
        {
            P = p;
            Pc = pc;
        }
    }
}