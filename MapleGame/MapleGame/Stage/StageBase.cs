using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MapleGame.Core;

namespace MapleGame.Stage
{
    public abstract class StageBase
    {
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
