using MaplePacketLib;

namespace MapleGame.Network.Handlers
{
    internal sealed class PingHandler : IPacketHandler
    {
        public void Handle(PacketReader packet)
        {
            MainGame.Instance.Socket.Send(new PacketWriter((short)SendOps.PONG,2));
        }
    }
}
