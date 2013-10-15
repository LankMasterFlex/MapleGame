namespace MapleGame.Network.Handlers.Field
{
    public class WarpToMapHandler : IPacketHandler
    {
        public void Handle(MaplePacketLib.PacketReader packet)
        {
            int channel = packet.ReadInt();

            bool init = packet.ReadBool();

            if (init)
            {
                packet.Skip(15);
                packet.Skip(9);
                MainGame.Instance.Player.ReadStats(packet);
                packet.Skip(1); //buddy list capacity

                if (packet.ReadBool())
                {
                    packet.Skip(packet.ReadShort());
                }

                MainGame.Instance.Player.ReadInventory(packet);
            }
            else
            {
                packet.Skip(3);
                packet.ReadByte();
                int dest = packet.ReadInt();
                byte spaw = packet.ReadByte();
                MainGame.Instance.Player.CurHP = packet.ReadShort();
            }

            string msg = string.Concat("Loading map ", MainGame.Instance.Player.Map);
            Logger.Initializer(msg, MainGame.Instance.LoadMap);
        }
    }
}
