using MapleGame.Stage;

namespace MapleGame.Network.Handlers.Field
{
    public class RemovePlayerHandler : IPacketHandler
    {
        public void Handle(MaplePacketLib.PacketReader packet)
        {
            int id = packet.ReadInt();
            MainGame.Instance.Field.Players.Remove(id);
        }
    }
}
