namespace MapleGame.Network.Handlers.Login
{
    public class LoginStatusHandler : IPacketHandler
    {
        public void Handle(MaplePacketLib.PacketReader packet)
        {
            byte result = packet.ReadByte();

            if (result != 0)
            {
                Logger.Write(LogLevel.Error, "Unable to login: {0}", result);
            }
            else
            {
                Logger.Write(LogLevel.Info, "Login successful");

                var p = new MaplePacketLib.PacketWriter((short)SendOps.CHARLIST_REQUEST, 9);
                p.WriteByte(2);
                p.WriteByte(0); //scania
                p.WriteByte(0); //channel
                p.WriteInt(System.Environment.TickCount); //hwid?

                MainGame.Instance.Socket.Send(p);
            }
        }
    }
}
