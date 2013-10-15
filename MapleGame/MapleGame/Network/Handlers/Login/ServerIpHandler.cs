namespace MapleGame.Network.Handlers.Login
{
    public class ServerIpHandler : IPacketHandler
    {
        public void Handle(MaplePacketLib.PacketReader packet)
        {
            packet.Skip(2);
            string ip = string.Join(".",packet.ReadBytes(4));
            short port = packet.ReadShort();
            MainGame.Instance.Player.Id = packet.ReadInt();

            MainGame.Instance.Socket.Disconnect();

            MainGame.Instance.LoginServer = false;
            MainGame.Instance.Socket.Connect(ip, port);
        }
    }
}
