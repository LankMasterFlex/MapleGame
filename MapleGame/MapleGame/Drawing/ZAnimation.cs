using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapleGame.Drawing
{
    public class ZAnimation
    {
        public ZFrame[] Frames { get; private set; }

        private int m_curFrame;
        private int m_updateTime;

        public ZFrame CurrentFrame
        {
            get
            {
                return Frames[m_curFrame];
            }
        }

        public ZAnimation(ZFrame[] frames)
        {
            Frames = frames;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Vector2 offset)
        {
            Frames[m_curFrame].Draw(spriteBatch, location, offset);
        }

        public void Update(GameTime gameTime)
        {
            if (Frames.Length <= 1) //no point in even updating
                return;

            m_updateTime += gameTime.ElapsedGameTime.Milliseconds;

            if (m_updateTime >= Frames[m_curFrame].Delay)
            {
                m_updateTime = 0;

                if ((m_curFrame + 1) >= Frames.Length)
                {
                    m_curFrame = 0;
                }
                else
                {
                    m_curFrame++;
                }
            }
        }
    }
}
