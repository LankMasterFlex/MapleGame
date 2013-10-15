using MaplePacketLib;
using Microsoft.Xna.Framework.Input;
using MapleGame.Network;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MapleGame.Game
{
    public class CUserLocal
    {
        private Texture2D m_texture;

        public int X { get; set; }
        public int Y { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte Gender { get; set; }
        public byte Skin { get; set; }
        public int Face { get; set; }
        public int Hair { get; set; }
        public byte Level { get; set; }
        public short Job { get; set; }

        public short MaxHP { get; set; }
        public short CurHP { get; set; }
        public short MaxMP { get; set; }
        public short CurMP { get; set; }

        public short Str { get; set; }
        public short Dex { get; set; }
        public short Luk { get; set; }
        public short Int { get; set; }

        public short AP { get; set; }
        public short SP { get; set; }

        public int Exp { get; set;}
        public short Fame { get; set; }

        public int Map { get; set; }
        public byte Spawn { get; set; }

        public int Meso { get; set; }

        public void Load()
        {
            m_texture = MainGame.Instance.Content.Load<Texture2D>("Textures/localPlayer");
        }

        public void Update()
        {
            var state = Keyboard.GetState();
            var properties = MainGame.Instance.Field.Properties;

            if (state.IsKeyDown(Keys.Up))
            {
                Y += 10;
            }

            if (state.IsKeyDown(Keys.Down))
            {
                Y -= 10;
            }

            if (state.IsKeyDown(Keys.Left))
            {
                X += 10;
            }

            if (state.IsKeyDown(Keys.Right))
            {
                X -= 10;
            }
        }

        public void ReadStats(PacketReader packet)
        {
            Id = packet.ReadInt();
            Name = packet.ReadString(13);
            Gender = packet.ReadByte();
            Skin = packet.ReadByte();
            Face = packet.ReadInt();
            Hair = packet.ReadInt();

            packet.Skip(24); //pets

            Level = packet.ReadByte();
            Job = packet.ReadShort();
            Str = packet.ReadShort();
            Dex = packet.ReadShort();
            Int = packet.ReadShort();
            Luk = packet.ReadShort();
            CurHP = packet.ReadShort();
            MaxHP = packet.ReadShort();
            CurMP = packet.ReadShort();
            MaxMP = packet.ReadShort();
            AP = packet.ReadShort();
            SP = packet.ReadShort();
            Exp = packet.ReadInt();
            Fame = packet.ReadShort();
            packet.Skip(4); //Gacha Exp
            Map = packet.ReadInt();
            Spawn = packet.ReadByte();
            packet.Skip(4); //zero
        }

        public void ReadInventory(PacketReader packet)
        {
            Meso = packet.ReadInt();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var prop = MainGame.Instance.Field.Properties;
            var position = new Vector2(X, Y - m_texture.Height);
            spriteBatch.Draw(m_texture, position, Color.White);
        }

        public void Move(Portal portal)
        {
            var field = MainGame.Instance.Field;

            if (field == null)
                return;

            Foothold fh = field.FindBelow(portal);

            int x = (fh.X1 + fh.X2) / 2;
            int y = (fh.Y1 + fh.Y2) / 2;

            Move(x, y);
        }

        public void Move(int x, int y)
        {
            PacketWriter p = new PacketWriter((short)SendOps.MOVE_PLAYER);
            p.WriteZero(9); //skipped in moopledev

            p.WriteByte(1); //loop count

            p.WriteByte(11); //Chair ;)
            p.WriteShort((short)x);
            p.WriteShort((short)y);
            p.WriteShort(); //fh
            p.WriteByte(4); //stance
            p.WriteShort(10000); //duration

            //no trail bc un parsed in moopledev

            //X = x;
            //Y = y;

            MainGame.Instance.Socket.Send(p);
        }
    }
}
