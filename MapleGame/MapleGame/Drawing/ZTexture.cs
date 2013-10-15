using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapleGame.Drawing
{
    public class ZTexture
    {
        public Vector2 Origin { get; set; }
        public Texture2D Texture { get; set; }

        public int Width
        {
            get { return Texture.Width; }
        }
        public int Height
        {
            get { return Texture.Height; }
        }

        public ZTexture()
        {
            Origin = Vector2.Zero;
            Texture = Drawing.Factory.EmptyRect;
        }

        public ZTexture(Vector2 origin, Texture2D texture)
        {
            Origin = origin;
            Texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Vector2 offset)
        {
            var position = new Vector2(location.X - Origin.X + offset.X, location.Y - Origin.Y + offset.Y);
            spriteBatch.Draw(Texture, position, Color.White);
        }
    }
}
