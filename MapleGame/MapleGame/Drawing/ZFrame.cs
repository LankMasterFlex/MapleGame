using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapleGame.Drawing
{
    public class ZFrame
    {
        public ZTexture Texture { get; set; }
        public int Delay { get; set; }

        public ZFrame()
        {
            Texture = new ZTexture();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Vector2 offset)
        {
            Texture.Draw(spriteBatch, location, offset);
        }
    }
}
