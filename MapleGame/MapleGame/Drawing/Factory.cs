using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing.Imaging;

namespace MapleGame.Drawing
{
    public class Factory
    {
        public static Texture2D EmptyRect;

        public static Texture2D Rectangle(int width, int height,Color color)
        {
            var texture = new Texture2D(MainGame.Instance.GraphicsDevice, width, height);

            Color[] colors = new Color[width * height];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }

            texture.SetData(colors);

            return texture;
        }

        public static Texture2D FromBitmap(System.Drawing.Bitmap value)
        {
            Texture2D tx = null;
            using (MemoryStream s = new MemoryStream())
            {
                value.Save(s,ImageFormat.Png);
                s.Position = 0;
                tx = Texture2D.FromStream(MainGame.Instance.GraphicsDevice, s);
            }

            return tx;
        }

        public static void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }
    }
}
