using MapleGame.Core;
using MapleGame.Drawing;
using MapleGame.Game;
using MapleGame.Network;
using MapleGame.Network.Handlers;
using MapleGame.Stage;
using MaplePacketLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapleGame
{
    public class MainGame : Microsoft.Xna.Framework.Game, IMapleClient
    {
        public static MainGame Instance { get; private set; }

        private StageBase m_stage;
        private bool m_loading;

        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        public CClientSocket Socket
        {
            get;
            private set;
        }
        public CUserLocal Player
        {
            get;
            private set;
        }
        public GameStage Field
        {
            get
            {
                return m_stage as GameStage;
            }
        }
        public bool LoginServer { get; set; }

        public Vector2 Offset
        {
            get
            {
                return new Vector2(Player.X, Player.Y);
            }
        }

        public MainGame()
        {
            Instance = this;

            Content.RootDirectory = "Content";

            m_graphics = new GraphicsDeviceManager(this);
            m_graphics.IsFullScreen = false;
            m_graphics.PreferredBackBufferWidth = Constants.Width;
            m_graphics.PreferredBackBufferHeight = Constants.Height;
            m_graphics.ApplyChanges();

            LoginServer = true;
        }

        protected override void Initialize()
        {
            CClientSocket.SetAesKey(Constants.Userkey);
            m_stage = new LoginStage();
            Player = new CUserLocal();
            Socket = new CClientSocket(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            Core.Constants.Load();
            PacketHandler.Load();
            Cursor.Load();
            Player.Load();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            m_stage.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_spriteBatch.Begin();

            if (!m_loading)
            {
                m_stage.Draw(m_spriteBatch);
                Cursor.Draw(m_spriteBatch);
            }

            m_spriteBatch.End();

            base.Draw(gameTime);
        }

        public void OnConnect(bool success, object state)
        {
            Logger.Write(LogLevel.Connection,success ? "Connected" : "Unable to connect");
        }

        public void OnHandshake(short major, string minor, byte locale)
        {
            Logger.Write(LogLevel.Info, "Logging in...");

            if (LoginServer)
            {
                PacketWriter p = new PacketWriter((short)SendOps.LOGIN_PASSWORD);
                p.WriteMapleString(Constants.Username);
                p.WriteMapleString(Constants.Password);

                Socket.Send(p);
            }
            else
            {
                PacketWriter p = new PacketWriter((short)SendOps.PLAYER_LOGGEDIN, 8);
                p.WriteInt(Player.Id);
                p.WriteShort();

                Socket.Send(p);
            }
        }

        public void OnPacket(byte[] packet)
        {
            PacketReader reader = new PacketReader(packet);

            var opcode = (RecvOps)reader.ReadShort();

            IPacketHandler handler = PacketHandler.Get(opcode);

            if (handler != null)
                handler.Handle(reader);
        }

        public void OnDisconnected()
        {
            Logger.Write(LogLevel.Connection,"Disconnected");
        }

        public void LoadMap()
        {
            m_loading = true;

            var gameStage = new GameStage(Player.Map);
            gameStage.Load();

            var spawn = gameStage.GetSpawn(Player.Spawn);
            Player.X = 0;// spawn.X;
            Player.Y = 0;// spawn.Y;
            Player.Move(spawn);

            m_stage = gameStage;

            m_loading = false;
        }
    }
}
