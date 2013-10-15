using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Concurrent;
using MapleGame.Drawing;
using MapleGame.Core;

namespace MapleGame.Game
{
    public class MapProperties
    {
        public string Bgm { get; set; }
        public int ReturnMap { get; set; }
        public int ForcedReturnMap { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int CenterX { get; set; }
        public int CenterY { get; set; }

        public int Botton {get;set;}
        public int Top { get; set; }
        public int Right { get; set; }
        public int Left { get; set; }
    }

    public class Foothold
    {
        public int ID { get; set; }
        public int Previous { get; set; }
        public int Next { get; set; }
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }

        public bool Wall
        {
            get
            {
                return X2 == X1;
            }
        }
    }

    public class LadderRope
    {
        public int ID { get; set; }
        public int L { get; set; }
        public int X { get; set; }
        public int YBoundaryBottom { get; set; }
        public int YBoundaryTop { get; set; }
    }

    public class Life
    {
        public int ID { get; set; }
        public int CenterY { get; set; }
        public int Face { get; set; }
        public int Foothold { get; set; }
        public int LifeID { get; set; }
        public int XBoundaryRight { get; set; }
        public int XBoundaryLeft { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Portal
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string PortalName { get; set; }
        public int PortalType { get; set; }
        public int DestinationMapID { get; set; }
        public string DestinationPortalName { get; set; }

        public void Draw(SpriteBatch spriteBatch,Vector2 offset)
        {
            if (PortalType == 1 || PortalType == 2)
                Core.Constants.PortalAnimation.Draw(spriteBatch, new Vector2(X, Y), offset);
        }
/*
        PortalType TypeByInt(int type)
        {
            switch (type)
            {
                case 1:
                case 2:
                    return PortalType.Normal;
                case 0:
                    return PortalType.Spawn;
                case 3:
                    return PortalType.Auto;
                case 9:
                    return PortalType.Teleport;
                default:
                    return PortalType.Normal;
            }
        }

        public enum PortalType
        {
            Normal,
            Spawn,
            Auto,
            Teleport
        }
 */
    }

    public class Obj
    {
        public string ImgName { get; set; }
        public string l0 { get; set; }
        public string l1 { get; set; }
        public string l2 { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ZTexture Texture { get; set; }

        public Obj()
        {
            ImgName = string.Empty;
            l0 = string.Empty;
            l1 = string.Empty;
            l2 = string.Empty;
            Texture = new ZTexture();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture.Draw(spriteBatch, new Vector2(X,Y), offset);
        }
    }

    public class Tile
    {
        public string ImgName { get; set; }
        public byte Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string TilePackName { get; set; }
        public string TileId { get; set; }
        public ZTexture Texture { get; set; }

        public Tile()
        {
            ImgName = string.Empty;
            TilePackName = string.Empty;
            TileId = string.Empty;
            Texture = new ZTexture();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            var location = new Vector2(X, Y);
            Texture.Draw(spriteBatch, location, offset);
        }
    }

    public class MovementSegment
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Duration { get; set; }
    }

    public class Player
    {
        public ConcurrentQueue<MovementSegment> Segments { get; set; }

        public int UpdateTime { get; private set; }

        public int Id { get; set; }
        public byte Level { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Player()
        {
            Segments = new ConcurrentQueue<MovementSegment>();
        }

        public void Update(GameTime gameTime)
        {
            MovementSegment segment;

            if(Segments.TryPeek(out segment))
            {
                UpdateTime += gameTime.ElapsedGameTime.Milliseconds;

                if (UpdateTime >= segment.Duration) //Duration is over
                {
                    UpdateTime = 0;
                    Segments.TryDequeue(out segment);

                    X = segment.X;
                    Y = segment.Y;
                }
                else
                {
                    var curPos = new Vector2(X,Y);
                    var destPos = new Vector2(segment.X,segment.Y);

                    int displacement = (int)Vector2.Distance(curPos, destPos);

                    int velocity = displacement / (segment.Duration - UpdateTime);

                    if (segment.X > X)
                    {
                        X += velocity;
                    }
                    else if (segment.X < X)
                    {
                        X -= velocity;
                    }

                    if (segment.Y > Y)
                    {
                        Y += velocity;
                    }
                    else if (segment.Y < Y)
                    {
                        Y -= velocity;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //FIX THIS
            var offset = MainGame.Instance.Offset;

            var animation = Constants.PlayerAnimation;

            var font = MainGame.Instance.Content.Load<SpriteFont>("Fonts/SmallFont");

            var charPos = new Vector2(X, Y + animation.CurrentFrame.Texture.Height);
            var textPos = new Vector2(X+ offset.X,Y+ offset.Y);

            animation.Draw(spriteBatch, charPos, offset);
            spriteBatch.DrawString(font, Name, textPos, Color.Black);
        }
    }
}
