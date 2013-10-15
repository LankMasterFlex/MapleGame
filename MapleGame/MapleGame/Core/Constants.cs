using System.Drawing;
using System.Linq;
using MapleGame.Drawing;
using MaplePacketLib;
using Microsoft.Xna.Framework;
using reNX;
using reNX.NXProperties;
using Microsoft.Xna.Framework.Graphics;

namespace MapleGame.Core
{
    public class Constants
    {
       public static readonly byte[] Userkey = new byte[32]
            {
                0x13, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0xB4, 0x00, 0x00, 0x00,
                0x1B, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x33, 0x00, 0x00, 0x00, 0x52, 0x00, 0x00, 0x00
            };

        public const string LoginIp = "209.188.19.131"; //JealousMs

        public const string Username = "rajan";
        public const string Password = "123456";

        public const int Width = 800;
        public const int Height = 600;

        public static ZAnimation PortalAnimation;
        public static ZAnimation PlayerAnimation;

        public static NXFile MapNx;

        public static void Load()
        {
            Factory.EmptyRect = Factory.Rectangle(1, 1,Microsoft.Xna.Framework.Color.White);
            Logger.Initializer("Loading NX files", LoadNX);
            Logger.Initializer("Loading animation constants", LoadAnimation);
        }

        private static void LoadAnimation()
        {
            LoadPortalAnimation();
            LoadPlayerAnimation();
        }

        private static void LoadPortalAnimation()
        {
            var parent = MapNx.ResolvePath("MapHelper.img/portal/game/pv");

            ZFrame[] frames = new ZFrame[parent.ChildCount];

            for (int i = 0; i < frames.Length; i++)
            {
                var node = parent.ElementAt(i);
                var bitmap = node.ValueOrDie<Bitmap>();
                var origin = node["origin"].ValueOrDie<System.Drawing.Point>();

                var frame = new ZFrame();
                frame.Delay = 100;
                frame.Texture.Texture = Factory.FromBitmap(bitmap);
                frame.Texture.Origin = new Vector2(origin.X, origin.Y);

                frames[i] = frame;
            }

            PortalAnimation = new ZAnimation(frames);
        }
        private static void LoadPlayerAnimation()
        {
            var frames = new ZFrame[3];

            for (int i = 0; i < frames.Length; i++)
            {
                string path = string.Concat("Textures/Player/", i); //2040016.img

                ZFrame frame = new ZFrame();
                frame.Delay = 500;

                frame.Texture.Origin = new Vector2(20, 66);
                frame.Texture.Texture =  MainGame.Instance.Content.Load<Texture2D>(path);

                frames[i] = frame;
            }

            PlayerAnimation = new ZAnimation(frames);
        }

        private static void LoadNX()
        {
            MapNx = new NXFile(@"C:\Nexon\NX 83\Map.nx");
        }
    }
}
