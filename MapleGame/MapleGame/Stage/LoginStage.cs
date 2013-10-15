using System;
using MapleGame.Core;
using MapleGame.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapleGame.Stage
{
    public class LoginStage : StageBase
    {
        private SpriteFont m_font;
        private Button m_btnConnect;
        private Button m_btnExit;

        public LoginStage()
        {
            m_font = MainGame.Instance.Content.Load<SpriteFont>("Fonts/DefaultFont");

            m_btnConnect = new Button(m_font, 100, 50, 100, 100);
            m_btnConnect.OnClicked += new Action<Button>(OnButtonClicked);
            m_btnConnect.SetText("Connect");

            m_btnExit = new Button(m_font, 210, 50, 100, 100);
            m_btnExit.OnClicked += new Action<Button>(OnButtonClicked);
            m_btnExit.SetText("Exit");
        }

        private void OnButtonClicked(Button sender)
        {
            if (sender == m_btnConnect)
            {
                Logger.Write(LogLevel.Connection, "Connecting");
                MainGame.Instance.Socket.Connect(Constants.LoginIp, 8484);
                m_btnConnect.Enabled = false;
            }
            else if (sender == m_btnExit)
            {
                MainGame.Instance.Socket.Dispose();
                MainGame.Instance.Exit();
            }
        }

        public override void Update(GameTime gameTime)
        {
            m_btnConnect.Update();
            m_btnExit.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            m_btnConnect.Draw(spriteBatch);
            m_btnExit.Draw(spriteBatch);
        }
    }
}
