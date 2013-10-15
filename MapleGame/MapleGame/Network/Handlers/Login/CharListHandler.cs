namespace MapleGame.Network.Handlers.Login
{
    public class CharListHandler : IPacketHandler
    {
        public void Handle(MaplePacketLib.PacketReader packet)
        {
            packet.Skip(1);
            byte chars = packet.ReadByte();

            if (chars == 0)
            {
                Logger.Write(LogLevel.Warning, "No characters on account");
                return;
            }

            int charId = packet.ReadInt();

            var p = new MaplePacketLib.PacketWriter((short)SendOps.CHAR_SELECT_WITH_PIC);
            p.WriteMapleString("000000");
            p.WriteInt(charId);
            p.WriteMapleString("nope");
            p.WriteMapleString("nope");

            MainGame.Instance.Socket.Send(p);
        }
    }
}
