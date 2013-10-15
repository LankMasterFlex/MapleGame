using MapleGame.Game;

namespace MapleGame.Network.Handlers.Field
{
    public class MovePlayerHandler : IPacketHandler
    {
        public void Handle(MaplePacketLib.PacketReader packet)
        {
            int cid = packet.ReadInt();

            Player player = MainGame.Instance.Field .Players.Get(cid);

            if (player == null)
                return;

            packet.Skip(4);

            int loop = packet.ReadByte();

            for (int i = 0; i < loop; i++)
            {
                byte command = packet.ReadByte();
                MovementSegment segment = new MovementSegment();

                switch (command)
                {
                    case 0:
                    case 5:
                    case 17:
                        {
                            segment.X = packet.ReadShort();
                            segment.Y = packet.ReadShort();
                            packet.Skip(7);
                            segment.Duration = packet.ReadShort();

                        }
                        break;
                    case 1:
                    case 2:
                    case 6: // fj
                    case 12:
                    case 13: // Shot-jump-back thing
                    case 16: //Float
                        {
                            player.X = packet.ReadShort();
                            player.Y = packet.ReadShort();
                            packet.Skip(1);
                            segment.Duration = packet.ReadShort();
                        }
                        break;
                    case 3:
                    case 4: // tele... -.-
                    case 7: // assaulter
                    case 8: // assassinate
                    case 9: // rush
                    case 14:  // Before Jump Down - fixes item/mobs dissappears
                        {
                            packet.Skip(9);
                        }
                        break;
                    case 10:// Change Equip
                        {
                            packet.Skip(1);
                        }
                        break;
                    case 11: //Chair
                        {
                            player.X = packet.ReadShort();
                            player.Y = packet.ReadShort();
                            packet.Skip(3);
                            segment.Duration = packet.ReadShort();
                        }
                        break;
                    case 15:
                        {
                            player.X = packet.ReadShort();
                            player.Y = packet.ReadShort();
                            packet.Skip(9);
                            segment.Duration = packet.ReadShort();
                        }
                        break;
                    case 21:
                        {
                            packet.Skip(3);
                        }
                        break;
                    default:
                        return;
                }

                player.Segments.Enqueue(segment);
            }
        }
    }
}
