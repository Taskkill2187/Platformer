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

namespace Platformer
{
    public class Enemy : Entity, ICloneable
    {
        public bool FacingRight;
        public int TimesJumped;
        public float MaxWalkSpeed;

        public Rectangle RightCheck;
        public Rectangle LeftCheck;

        public float GravForce = 1;

        public float Size;

        public bool HasIntersectedBottomRight;
        public bool HasIntersectedBottomLeft;
        public int Timer;

        internal int WalkAnimStates;
        public int WalkAnimState;

        public Enemy(int PosX, int PosY, bool FacingRight, float MaxWalkSpeed)
        {
            Size = 1;
            Rect = new Rectangle(PosX, PosY, (int)(LevelManager.BlockScale * LevelManager.EntitySize * Size), (int)(LevelManager.BlockScale * LevelManager.EntitySize * Size));
            this.MaxWalkSpeed = MaxWalkSpeed;
            this.FacingRight = FacingRight;
            WalkAnimStates = 0;
            if (FacingRight)
            {
                Vel = new Vector2(MaxWalkSpeed, 0);
            }
            else
            {
                Vel = new Vector2(-MaxWalkSpeed, 0);
            }
        }
    
        public virtual void OnDeath()
        {
            LevelManager.CurrentLevel.EnemyList.Remove(this);
            ParticleManager.CreateParticleExplosionFromEntityTexture(this, new Rectangle(0, 0, 16, 16), 0.3f, 0.2f, FacingRight, true, false);
        }
        public virtual void OnPlayerKill()
        {
            LevelManager.ThisPlayer.OnDeath();
        }
        public virtual void CheckForCollision()
        {
            HasIntersectedBottomLeft = false;
            HasIntersectedBottomRight = false;

            int CheckRange = LevelManager.BlockScale + Rect.Width;
            for (int i = 0; i < LevelManager.CurrentLevel.BlockList.Count; i++)
            {
                if (LevelManager.CurrentLevel.BlockList[i].Rect.X - Rect.X < CheckRange && LevelManager.CurrentLevel.BlockList[i].Rect.X - Rect.X > -CheckRange &&
                    Rect.Intersects(LevelManager.CurrentLevel.BlockList[i].Rect))
                {
                    if (LevelManager.CurrentLevel.BlockList[i].GetType() != typeof(JumpBlock))
                    {
                        if (this.Rect.Y + this.Rect.Height - LevelManager.BlockScale / 2 >= LevelManager.CurrentLevel.BlockList[i].Rect.Y + LevelManager.BlockScale / 2)
                        {
                            if (this.Rect.X > LevelManager.CurrentLevel.BlockList[i].Rect.X)
                            {
                                FacingRight = true;
                            }
                            else
                            {
                                FacingRight = false;
                            }
                            this.Vel.Y = 0;
                        }
                        else
                        {
                            this.Rect.Y = LevelManager.CurrentLevel.BlockList[i].Rect.Y - this.Rect.Height;
                            this.Vel.Y = 0;
                            Vel.X /= 1.4f;
                            TimesJumped = 0;
                        }
                    }
                }

                RightCheck = new Rectangle(Rect.X + Rect.Width / 2, Rect.Y + Rect.Height, Rect.Width / 2, Rect.Height / 2);
                LeftCheck = new Rectangle(Rect.X, Rect.Y + Rect.Height, Rect.Width / 2, Rect.Height / 2);

                if (LevelManager.CurrentLevel.BlockList[i].GetType() != typeof(JumpBlock))
                {
                    if (RightCheck.Intersects(LevelManager.CurrentLevel.BlockList[i].Rect))
                    {
                        HasIntersectedBottomRight = true;
                    }
                    if (LeftCheck.Intersects(LevelManager.CurrentLevel.BlockList[i].Rect))
                    {
                        HasIntersectedBottomLeft = true;
                    }
                }
            }
            if (!HasIntersectedBottomRight)
            {
                FacingRight = false;
            }
            if (!HasIntersectedBottomLeft)
            {
                FacingRight = true;
            }

            if (Rect.X < 0)
                FacingRight = true;
        }
        public override object Clone()
        {
            Enemy E = (Enemy)this.MemberwiseClone();
            E.RightCheck = new Rectangle();
            E.LeftCheck = new Rectangle();
            return E;
        }

        public virtual void Update()
        {
            Vel.Y += GravForce;
            Vel.X /= 1.01f;

            if (FacingRight)
            {
                if (Vel.X < MaxWalkSpeed)
                    Vel.X += 3;
            }
            else
            {
                if (Vel.X > -MaxWalkSpeed)
                    Vel.X -= 3;
            }

            Timer++;

            if (Timer % 3 == 0)
                WalkAnimState++;

            if (WalkAnimState >= WalkAnimStates)
                WalkAnimState = 0;

            CheckForCollision();

            Rect = new Rectangle(Rect.X + (int)Vel.X, Rect.Y + (int)Vel.Y, Rect.Width, Rect.Height);
        }
        public virtual new void Draw(SpriteBatch SB)
        {
            if (this.Texture == null)
            {
                SB.Draw(Assets.White, new Rectangle(Rect.X + (int)LevelManager.Camera.X, Rect.Y + (int)LevelManager.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(Rect.X + (int)LevelManager.Camera.X, Rect.Y + (int)LevelManager.Camera.Y, Rect.Width, Rect.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            else
            {
                if (FacingRight)
                {
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)LevelManager.Camera.X, Rect.Y + (int)LevelManager.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    SB.Draw(Texture, new Rectangle(Rect.X + (int)LevelManager.Camera.X, Rect.Y + (int)LevelManager.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                }
            }
        }
    }
}
