using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MapleGame.Core;
using MapleGame.Drawing;
using MapleGame.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using reNX.NXProperties;

namespace MapleGame.Stage
{
    public class GameStage : StageBase,IDisposable
    {
        public int Id { get; private set; }

        public Foothold[] Foothold;
        public LadderRope[] LadderRope;
        public Life[] Life;
        public MapProperties Properties;
        public Portal[] Portals;
        public Obj[][] Objs;
        public Tile[][] Tiles;

        private Dictionary<string, ZTexture> m_textureCache;

        public ZDictionary<int, Player> Players;

        public GameStage(int id)
        {
            Id = id;
            Objs = new Obj[7][];
            Tiles = new Tile[7][];

            m_textureCache = new Dictionary<string, ZTexture>();

            Players = new ZDictionary<int, Player>();

            Load();
        }

        public void Load()
        {
            var nx = Constants.MapNx;
            int sub = Id / 100000000;
            string path = string.Format("{0}/Map/Map{1}/{2}.img", nx.BaseNode.Name, sub, Id);
            var node = nx.ResolvePath(path);

            LoadFoothold(node);
            LoadProperties(node);
            LoadLadderRope(node);
            LoadPortal(node);

            LoadObjs(node);
            LoadTiles(node);

            LoadObjTextures();
            LoadTileTextures();
        }

        private void LoadProperties(NXNode node)
        {
            Properties = new MapProperties();
            Properties.Bgm = node["info"]["bgm"].ValueOrDie<string>();
            Properties.ReturnMap = (int)node["info"]["returnMap"].GetInt();
            Properties.ForcedReturnMap = node["info"]["forcedReturn"].GetInt();

            if (node["miniMap"] == null)
                return;

            Properties.Width = node["miniMap"]["width"].GetInt();
            Properties.Height = node["miniMap"]["height"].GetInt();
            Properties.CenterX = node["miniMap"]["centerX"].GetInt();
            Properties.CenterY = node["miniMap"]["centerY"].GetInt();


            //Properties.Botton = node["info"]["VRBottom"].GetInt();
            //Properties.Left = node["info"]["VRLeft"].GetInt();
            //Properties.Right = node["info"]["VRRight"].GetInt();
            //Properties.Top = node["info"]["VRTop"].GetInt();
        }
        private void LoadFoothold(NXNode node)
        {
            List<Foothold> footholds = new List<Foothold>();

            foreach (NXNode sub in node["foothold"])
            {
                foreach (NXNode sub2 in sub)
                {
                    foreach (NXNode sub3 in sub2)
                    {
                        Foothold fh = new Foothold();
                        fh.ID = int.Parse(sub3.Name);
                        fh.Next = sub3["next"].GetInt();
                        fh.Previous = sub3["prev"].GetInt();
                        fh.X1 = sub3["x1"].GetInt();
                        fh.X2 = sub3["x2"].GetInt();
                        fh.Y1 = sub3["y1"].GetInt();
                        fh.Y2 = sub3["y2"].GetInt();
                        footholds.Add(fh);
                    }
                }
            }

            Foothold = footholds.ToArray();
        }
        private void LoadLadderRope(NXNode node)
        {
            List<LadderRope> ladderropes = new List<LadderRope>();

            foreach (NXNode sub in node["ladderRope"])
            {
                LadderRope ladderrope = new LadderRope();
                ladderrope.ID = int.Parse(sub.Name);
                ladderrope.L = sub["l"].GetInt();
                ladderrope.X = sub["x"].GetInt();
                ladderrope.YBoundaryBottom = sub["y1"].GetInt();
                ladderrope.YBoundaryTop = sub["y2"].GetInt();
                ladderropes.Add(ladderrope);
            }
            LadderRope = ladderropes.ToArray();
        }
        private void LoadPortal(NXNode node)
        {
            List<Portal> portals = new List<Portal>();

            foreach (NXNode sub in node["portal"])
            { 
                Portal portal = new Portal();
                portal.ID = int.Parse(sub.Name);
                portal.PortalName = sub["pn"].ValueOrDie<string>();
                portal.PortalType = sub["pt"].GetInt();
                portal.DestinationMapID = sub["tm"].GetInt();
                portal.DestinationPortalName = sub["tn"].ValueOrDie<string>();
                portal.X = sub["x"].GetInt();
                portal.Y = sub["y"].GetInt();
                portals.Add(portal);
            }
            Portals = portals.ToArray();
        }
        private void LoadObjs(NXNode node)
        {
            for (int i = 0; i < 7; i++)
            {
                var objProp = node[i.ToString()]["obj"];
                var list = new List<Obj>();

                foreach (NXNode prop in objProp)
                {
                    Obj obj = new Obj();

                    obj.ImgName = prop["oS"].ValueOrDie<string>() + ".img";
                    obj.l0 = prop["l0"].ValueOrDie<string>();
                    obj.l1 = prop["l1"].ValueOrDie<string>();
                    obj.l2 = prop["l2"].ValueOrDie<string>();
                    obj.X = prop["x"].GetInt();
                    obj.Y = prop["y"].GetInt();

                    list.Add(obj);
                }

                Objs[i] = list.ToArray();
            }
        }
        private void LoadTiles(NXNode node)
        {
            for (int i = 0; i < 7; i++)
            {
                var numProp = node[i.ToString()];

                var infoProp = numProp["info"];

                if (infoProp.ChildCount == 0)
                {
                    Tiles[i] = new Tile[0];
                    continue;
                }

                var tileProp = numProp["tile"];

                if (tileProp.ChildCount == 0)
                {
                    Tiles[i] = new Tile[0];
                    continue;
                }


                string tileSetName = infoProp["tS"].ValueOrDie<string>();

                var list = new List<Tile>();

                foreach (NXNode prop in tileProp)
                {
                    Tile tile = new Tile();
                    tile.ImgName = tileSetName + ".img";
                    tile.X = prop["x"].GetInt();
                    tile.Y = prop["y"].GetInt();
                    tile.TilePackName = prop["u"].ValueOrDie<string>();
                    tile.TileId = prop["no"].GetInt().ToString();
                    list.Add(tile);
                }

                Tiles[i] = list.ToArray();
            }
        }

        private void LoadObjTextures()
        {
            foreach (Obj[] collection in Objs)
            {
                foreach (Obj obj in collection)
                {
                    var nx = Constants.MapNx;
                    string path = string.Format("Obj/{0}/{1}/{2}/{3}/0", obj.ImgName, obj.l0, obj.l1, obj.l2);

                    if (m_textureCache.ContainsKey(path))
                    {
                        obj.Texture = m_textureCache[path];
                    }
                    else
                    {
                        var prop = nx.ResolvePath(path);

                        if (prop != null && prop is NXCanvasNode)
                        {
                            var bitmap = prop.ValueOrDie<Bitmap>();
                            var origin = prop["origin"].ValueOrDie<System.Drawing.Point>();

                            obj.Texture.Texture = Factory.FromBitmap(bitmap);
                            obj.Texture.Origin = new Vector2(origin.X, origin.Y);

                            m_textureCache.Add(path, obj.Texture);
                        }
                    }
                }
            }
        }
        private void LoadTileTextures()
        {
            foreach (Tile[] collection in Tiles)
            {
                foreach (Tile tile in collection)
                {
                    var nx = Constants.MapNx;
                    string path = string.Format("/Tile/{0}/{1}/{2}", tile.ImgName, tile.TilePackName, tile.TileId);

                    if (m_textureCache.ContainsKey(path))
                    {
                        tile.Texture = m_textureCache[path];
                    }
                    else
                    {
                        var prop = nx.ResolvePath(path);

                        if (prop != null && prop is NXCanvasNode)
                        {
                            var bitmap = prop.ValueOrDie<Bitmap>();
                            var origin = prop["origin"].ValueOrDie<System.Drawing.Point>();

                            tile.Texture.Texture = Factory.FromBitmap(bitmap);
                            tile.Texture.Origin = new Vector2(origin.X, origin.Y);

                            m_textureCache.Add(path, tile.Texture);
                        }
                    }
                }
            }
        }

        public Foothold FindBelow(Portal portal)
        {
            foreach (Foothold fh in Foothold)
            {
                if (fh.X1 <= portal.X && fh.X2 >= portal.X && !fh.Wall)
                {
                    if (fh.Y1 != fh.Y2)
                    {
                        int calcY;
                        double s1 = Math.Abs(fh.Y2 - fh.Y1);
                        double s2 = Math.Abs(fh.X2 - fh.X1);
                        double s4 = Math.Abs(portal.X - fh.X1);
                        double alpha = Math.Atan(s2 / s1);
                        double beta = Math.Atan(s1 / s2);
                        double s5 = Math.Cos(alpha) * (s4 / Math.Cos(beta));

                        if (fh.Y2 < fh.Y1)
                        {
                            calcY = fh.Y1 - ((int)s5);
                        }
                        else
                        {
                            calcY = fh.Y1 + ((int)s5);
                        }

                        if (calcY >= portal.Y)
                        {
                            return fh;
                        }
                    }
                    else if (fh.Y1 >= portal.Y)
                    {
                        return fh;
                    }
                }
            }
            return null;
        }
        public Portal GetSpawn(byte id)
        {
            return Portals.Single((p) => p.ID == id);
        }

        public override void Update(GameTime gameTime)
        {
            Players.ForEach(UpdateSinglePlayer, gameTime);
            MainGame.Instance.Player.Update();
            Constants.PortalAnimation.Update(gameTime);
            Constants.PlayerAnimation.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            var offset = MainGame.Instance.Offset;

            foreach (Obj[] collection in Objs)
            {
                foreach (Obj obj in collection)
                {
                    obj.Draw(spriteBatch, offset);
                }
            }

            foreach (Tile[] collection in Tiles)
            {
                foreach (Tile tile in collection)
                {
                    tile.Draw(spriteBatch, offset);
                }
            }

            foreach (Portal portal in Portals)
            {
                portal.Draw(spriteBatch, offset);
            }

            Players.ForEach(DrawSinglePlayer, spriteBatch);

            MainGame.Instance.Player.Draw(spriteBatch);
        }

        private void UpdateSinglePlayer(KeyValuePair<int, Player> kvp, object state)
        {
            GameTime time = (GameTime)state;
            kvp.Value.Update(time);
        }
        private void DrawSinglePlayer(KeyValuePair<int, Player> kvp, object state)
        {
            Player player = kvp.Value;
            SpriteBatch spriteBatch = state as SpriteBatch;
            player.Draw(spriteBatch);
        }

        public void Dispose()
        {
            Foothold = null;
            LadderRope = null;
            Life = null;
            Properties = null;
            Portals = null;
            Objs = null;
            Tiles = null;
            m_textureCache.Clear();
            Players.Clear();
        }
    }
}