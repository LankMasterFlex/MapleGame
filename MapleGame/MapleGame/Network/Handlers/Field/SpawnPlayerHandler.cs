using MapleGame.Game;
using MapleGame.Stage;

namespace MapleGame.Network.Handlers.Field
{
    public class SpawnPlayerHandler : IPacketHandler
    {
        public void Handle(MaplePacketLib.PacketReader packet)
        {
            Player player = new Player();
            player.Id = packet.ReadInt();
            player.Level = packet.ReadByte();
            player.Name = packet.ReadMapleString();

            MainGame.Instance.Field.Players.Add(player.Id, player);
        }
    }
}
