using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MapleGame.Drawing
{
    public static class Cursor
    {
        private static Texture2D m_cursorIdle;
        private static Texture2D m_cursorDown;

        public static void Load()
        {
            m_cursorIdle = MainGame.Instance.Content.Load<Texture2D>("Textures/Cursor/idle");
            m_cursorDown = MainGame.Instance.Content.Load<Texture2D>("Textures/Cursor/down");
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            var state = Mouse.GetState();
            var position = new Vector2(state.X, state.Y);
            var texture = state.LeftButton == ButtonState.Pressed ? m_cursorDown : m_cursorIdle;

            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
