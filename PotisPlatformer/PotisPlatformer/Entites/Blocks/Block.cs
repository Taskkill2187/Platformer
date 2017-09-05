using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Platformer
{
    public enum BlockTextureState
    {
        Default,
        FreeTop,
        FreeTopRight,
        FreeRight,
        FreeTopLeft,
        FreeLeft,
        FreeLeftRight,
        FreeLeftRightTop,
    }
    public class Block : Entity, ICloneable
    {
        public bool Collision;
        public bool Destroyed;

        bool ClearBottom;
        bool TopLeftEdge;
        bool TopRightEdge;
        BlockTextureState TexState = BlockTextureState.Default;

        public Block(Texture2D Texture, Vector2 Pos, bool Collision, Level Parent)
        {
            Rect = new Rectangle((int)Pos.X, (int)Pos.Y, Level.BlockScale, Level.BlockScale);
            Vel = Vector2.Zero;
            this.Collision = Collision;
            this.Texture = Texture;
            this.Parent = Parent;

            if (Texture == null)
                Texture = Assets.BlockGrass;
        }

        public void UpdateTextureState()
        {
            Rectangle Right = new Rectangle(Rect.X + Level.BlockScale, Rect.Y, 1, Rect.Height);
            Rectangle Left = new Rectangle(Rect.X - 1, Rect.Y, 1, Rect.Height);
            Rectangle Top = new Rectangle(Rect.X, Rect.Y - 1, Rect.Width, 1);
            Rectangle Bottom = new Rectangle(Rect.X + Rect.Width / 2, Rect.Y + Rect.Height, 1, 1);

            if (Parent != null && Parent.BlockList != null)
            {
                // Look through the BlockList of the Current Level
                bool DetectedRight = false;
                bool DetectedLeft = false;
                bool DetectedTop = false;
                bool DetectedBottom = false;

                for (int i = 0; i < Parent.BlockList.Count; i++)
                {
                    if (Parent.BlockList[i].Rect.X - Rect.X < 100 && Parent.BlockList[i].Rect.X - Rect.X > -100 &&
                        Parent.BlockList[i].GetType() == typeof(Block) ||
                        Parent.BlockList[i].Rect.X - Rect.X < 100 && Parent.BlockList[i].Rect.X - Rect.X > -100 &&
                        Parent.BlockList[i].GetType() == typeof(FallingBlock))
                    {
                        if (Right.Intersects(Parent.BlockList[i].Rect))
                        {
                            DetectedRight = true;
                        }

                        if (Left.Intersects(Parent.BlockList[i].Rect))
                        {
                            DetectedLeft = true;
                        }

                        if (Top.Intersects(Parent.BlockList[i].Rect))
                        {
                            DetectedTop = true;
                        }

                        if (Bottom.Intersects(Parent.BlockList[i].Rect))
                        {
                            DetectedBottom = true;
                        }
                    }
                }

                // Update TextureState
                if (DetectedRight)
                {
                    if (DetectedLeft)
                    {
                        if (DetectedTop)
                        {
                            TexState = BlockTextureState.Default;
                        }
                        else
                        {
                            TexState = BlockTextureState.FreeTop;
                        }
                    }
                    else
                    {
                        if (DetectedTop)
                        {
                            TexState = BlockTextureState.FreeLeft;
                        }
                        else
                        {
                            TexState = BlockTextureState.FreeTopLeft;
                        }
                    }
                }
                else
                {
                    if (DetectedLeft)
                    {
                        if (DetectedTop)
                        {
                            TexState = BlockTextureState.FreeRight;
                        }
                        else
                        {
                            TexState = BlockTextureState.FreeTopRight;
                        }
                    }
                    else
                    {
                        if (DetectedTop)
                        {
                            TexState = BlockTextureState.FreeLeftRight;
                        }
                        else
                        {
                            TexState = BlockTextureState.FreeLeftRightTop;
                        }
                    }
                }

                // Update ClearBottom
                if (DetectedBottom)
                    ClearBottom = false;
                else
                    ClearBottom = true;

                // Update Edge Dots
                if (TexState == BlockTextureState.FreeRight)
                {
                    TopLeftEdge = true;
                    for (int i = 0; i < Parent.BlockList.Count; i++)
                    {
                        if (Parent.BlockList[i].GetType() != typeof(JumpBlock) && Parent.BlockList[i].Rect.Intersects(new Rectangle(Rect.X - 5, Rect.Y - 5, 5, 5))) { TopLeftEdge = false; }
                    }
                }
                else if (TexState == BlockTextureState.FreeLeft)
                {
                    TopRightEdge = true;
                    for (int i = 0; i < Parent.BlockList.Count; i++)
                    {
                        if (Parent.BlockList[i].GetType() != typeof(JumpBlock) && Parent.BlockList[i].Rect.Intersects(new Rectangle(Rect.X + Rect.Width, Rect.Y - 5, 5, 5))) { TopRightEdge = false; }
                    }
                }
                else if (TexState == BlockTextureState.Default)
                {
                    TopLeftEdge = true;
                    for (int i = 0; i < Parent.BlockList.Count; i++)
                    {
                        if (Parent.BlockList[i].GetType() != typeof(JumpBlock) && Parent.BlockList[i].Rect.Intersects(new Rectangle(Rect.X - 5, Rect.Y - 5, 5, 5))) { TopLeftEdge = false; }
                    }
                    TopRightEdge = true;
                    for (int i = 0; i < Parent.BlockList.Count; i++)
                    {
                        if (Parent.BlockList[i].GetType() != typeof(JumpBlock) && Parent.BlockList[i].Rect.Intersects(new Rectangle(Rect.X + Rect.Width, Rect.Y - 5, 5, 5))) { TopRightEdge = false; }
                    }
                }
                else
                {
                    TopLeftEdge = false;
                    TopRightEdge = false;
                }
            }
        }
        public virtual void Activate()
        {

        }
        public override object Clone()
        {
            return MemberwiseClone();
        }

        public virtual void Update()
        {
            if (Parent.ThisPlayer.Rect.X > Rect.X - Parent.ThisPlayer.Rect.Width && Parent.ThisPlayer.Rect.X < Rect.X + Rect.Width &&
                Parent.ThisPlayer.Rect.Y == Rect.Y + Rect.Height && Parent.ThisPlayer.TimesJumped > 0 && Parent.ThisPlayer.Vel.Y <= 1)
                    Activate();
        }
        public override void Draw(SpriteBatch SB)
        {
            // Bottom Line
            if (ClearBottom)
                SB.Draw(Assets.White, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + Rect.Height + (int)Parent.Camera.Y, Rect.Width, 4), Color.Black);

            // Texture Drawing
            switch (TexState)
            {
                case BlockTextureState.Default:
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(17, 17, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;

                case BlockTextureState.FreeTop:
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(17, 0, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;

                case BlockTextureState.FreeTopRight:
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(34, 0, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;

                case BlockTextureState.FreeTopLeft:
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(0, 0, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;

                case BlockTextureState.FreeLeft:
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(0, 17, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;

                case BlockTextureState.FreeRight:
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(34, 17, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;

                case BlockTextureState.FreeLeftRightTop:
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X + Rect.Width / 2, Rect.Y + (int)Parent.Camera.Y, Rect.Width / 2, Rect.Height),
                        new Rectangle(42, 0, 8, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width / 2, Rect.Height),
                        new Rectangle(0, 0, 8, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;

                case BlockTextureState.FreeLeftRight:
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X + Rect.Width / 2, Rect.Y + (int)Parent.Camera.Y, Rect.Width / 2, Rect.Height),
                        new Rectangle(42, 17, 8, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width / 2, Rect.Height),
                        new Rectangle(0, 17, 8, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;

                default:
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(17, 17, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;
            }

            if (TopLeftEdge)
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(17 * 3, 17, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
            }

            if (TopRightEdge)
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                       new Rectangle(17 * 3, 17, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
        }
    }
}
