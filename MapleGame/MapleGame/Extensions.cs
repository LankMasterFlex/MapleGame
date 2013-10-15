using Microsoft.Xna.Framework;
using reNX.NXProperties;

namespace MapleGame
{
    public static class Extensions
    {
        public static bool Contains(this Rectangle rectangle, Vector2 vector)
        {
            return rectangle.Contains((int)vector.X, (int)vector.Y);
        }

        public static int GetInt(this NXNode node)
        {
            return (int)node.ValueOrDie<long>();
        }
    }
}
