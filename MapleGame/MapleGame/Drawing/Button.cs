using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 

namespace MapleGame.Drawing
{
    public class Button
    {
        private SpriteFont m_font;
        private Texture2D m_image;
        private Rectangle m_location;

        private Vector2 m_textLocation; 

        private MouseState m_mouse;
        private MouseState m_oldMouse;

        public bool Enabled { get; set; }
        public string Text { get; private set; }
        public event Action<Button> OnClicked;

        public Button(SpriteFont font,int x ,int y ,int width,int height)
        {
            m_font = font;
            m_image = Factory.Rectangle(width, height,Color.White);
            m_location = new Rectangle(x,y, width, height);

            Text = string.Empty;
            Enabled = true;
        }

        public void SetText(string text)
        {
            Text = text;

            Vector2 size = m_font.MeasureString(text);

            m_textLocation = new Vector2();
            m_textLocation.Y = m_location.Y + ((m_image.Height / 2) - (size.Y / 2));
            m_textLocation.X = m_location.X + ((m_image.Width / 2) - (size.X / 2));
        }

        public void Update()
        {
            m_mouse = Mouse.GetState();

            if (m_mouse.LeftButton == ButtonState.Released && m_oldMouse.LeftButton == ButtonState.Pressed)
            {
                if (m_location.Contains(m_mouse.X, m_mouse.Y))
                {
                    if (Enabled && OnClicked != null)
                        OnClicked(this);
                }
            }

            m_oldMouse = m_mouse;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Enabled)
            {
                spriteBatch.Draw(m_image, m_location, Color.DarkSlateGray);
            }
            else if (m_location.Contains(m_mouse.X, m_mouse.Y))
            {
                spriteBatch.Draw(m_image, m_location, Color.Silver);
            }
            else
            {
                spriteBatch.Draw(m_image, m_location, Color.White);
            }

            if (Text != string.Empty && Text != null)
            {
                spriteBatch.DrawString(m_font, Text, m_textLocation, Color.Black);
            }
        }
    }
}
