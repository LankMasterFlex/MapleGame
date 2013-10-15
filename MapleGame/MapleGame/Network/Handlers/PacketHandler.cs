using MapleGame.Network.Handlers.Login;
using MapleGame.Network.Handlers.Field;

namespace MapleGame.Network.Handlers
{
    interface IPacketHandler
    {
        void Handle(MaplePacketLib.PacketReader packet);
    }
    internal class PacketHandler
    {
        private static IPacketHandler[] m_handlers;

        public static void Load()
        {
            m_handlers = new IPacketHandler[0xFFFF + 1];

            Add(RecvOps.PING, new PingHandler());
            Add(RecvOps.CHARLIST, new CharListHandler());
            Add(RecvOps.LOGIN_STATUS, new LoginStatusHandler());
            Add(RecvOps.SERVER_IP, new ServerIpHandler());

            Add(RecvOps.WARP_TO_MAP, new WarpToMapHandler());

            Add(RecvOps.SPAWN_PLAYER, new SpawnPlayerHandler());
            Add(RecvOps.MOVE_PLAYER, new MovePlayerHandler());
            Add(RecvOps.REMOVE_PLAYER_FROM_MAP, new RemovePlayerHandler());
        }

        public static void Add(RecvOps opcode, IPacketHandler handler)
        {
            m_handlers[(short)opcode] = handler;
        }

        public static void Remove(RecvOps opcode)
        {
            m_handlers[(short)opcode] = null;
        }

        public static IPacketHandler Get(RecvOps opcode)
        {
            return m_handlers[(short)opcode];
        }
    }
}
