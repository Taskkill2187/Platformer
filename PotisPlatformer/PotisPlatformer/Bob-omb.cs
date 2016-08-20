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
    public class Bob_omb : Enemy
    {
        int ExplodingCounter;
        static int ExplosionRadius = 150;

        public Bob_omb(int PosX, int PosY) : base(PosX, PosY, true, 2.5f)
        {
            WalkAnimStates = 2;
            Texture = Assets.Bob_omb;
        }

        public int Explode()
        {
            int a = 0;
            for (int i = 0; i < LevelManager.CurrentLevel.BlockList.Count; i++)
            {
                if (LevelManager.CurrentLevel.BlockList[i].GetType() == typeof(Brick) && Vector2.DistanceSquared(LevelManager.CurrentLevel.BlockList[i].GetPosVector2(), this.GetPosVector2()) < ExplosionRadius * ExplosionRadius)
                {
                    ParticleManager.CreateParticleExplosionFromEntityTexture(LevelManager.CurrentLevel.BlockList[i], new Rectangle(0, 0, 16, 16), 0.25f, 1.0f, false, true, false);
                    LevelManager.CurrentLevel.BlockList.Remove(LevelManager.CurrentLevel.BlockList[i]);
                    i--;
                }
            }

            for (int i = 0; i < LevelManager.CurrentLevel.EnemyList.Count; i++)
            {
                if (Vector2.DistanceSquared(LevelManager.CurrentLevel.EnemyList[i].GetPosVector2(), this.GetPosVector2()) < ExplosionRadius * ExplosionRadius)
                {
                    a++;
                    LevelManager.CurrentLevel.EnemyList[i].OnDeath();
                }
            }
            LevelManager.UpdateTextures();
            ParticleManager.CreateParticleExplosionFromEntityTexture(this, new Rectangle(0, 0, 16, 16), 0.05f, 4.0f, false, true, false);
            LevelManager.CurrentLevel.EnemyList.Remove(this);

            return a;
        }
        public override void OnDeath()
        {
            if (ExplodingCounter == 0)
            {
                ExplodingCounter = 1;

                //if (FacingRight)
                //    Vel += new Vector2(6, -10);
                //else
                //    Vel += new Vector2(-6, -10);
            }
        }
        public override void OnPlayerKill()
        {
            if (ExplodingCounter > 0)
            {
                if (LevelManager.ThisPlayer.FacingRight)
                    Vel = new Vector2(12, -10);
                else
                    Vel = new Vector2(-12, -10);
            }
            else
            {
                LevelManager.ThisPlayer.OnDeath();
            }
        }
        public override void CheckForCollision()
        {
            HasIntersectedBottomLeft = false;
            HasIntersectedBottomRight = false;

            for (int i = 0; i < LevelManager.CurrentLevel.BlockList.Count; i++)
            {
                if (LevelManager.CurrentLevel.BlockList[i].Rect.X - Rect.X < 200 && LevelManager.CurrentLevel.BlockList[i].Rect.X - Rect.X > -200)
                {
                    if (Rect.Intersects(LevelManager.CurrentLevel.BlockList[i].Rect) && LevelManager.CurrentLevel.BlockList[i].Collision)
                    {
                        if (this.Rect.Y + this.Rect.Height >= LevelManager.CurrentLevel.BlockList[i].Rect.Y + LevelManager.BlockScale)
                        {
                            if (this.Rect.X > LevelManager.CurrentLevel.BlockList[i].Rect.X)
                            {
                                FacingRight = true;
                                if (ExplodingCounter > 0)
                                {
                                    Vel.X = 0;
                                }
                                Rect.X = LevelManager.CurrentLevel.BlockList[i].Rect.X + LevelManager.CurrentLevel.BlockList[i].Rect.Width;
                            }
                            else
                            {
                                FacingRight = false;
                                if (ExplodingCounter > 0)
                                {
                                    Vel.X = 0;
                                }
                                Rect.X = LevelManager.CurrentLevel.BlockList[i].Rect.X - Rect.Width;
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

                if (RightCheck.Intersects(LevelManager.CurrentLevel.BlockList[i].Rect))
                {
                    HasIntersectedBottomRight = true;
                }
                if (LeftCheck.Intersects(LevelManager.CurrentLevel.BlockList[i].Rect))
                {
                    HasIntersectedBottomLeft = true;
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
        }

        public override void Update()
        {
            int a = 0;

            if (ExplodingCounter > 0)
                ExplodingCounter++;

            if (ExplodingCounter == 120)
                a = Explode();

            Vel.Y += 1f;
            Vel.X /= 1.01f;

            if (Rect.X < 0)
                FacingRight = true;

            if (ExplodingCounter == 0)
            {
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
            }

            Timer++;

            if (Timer % 10 == 0)
                WalkAnimState++;

            if (WalkAnimState >= WalkAnimStates)
                WalkAnimState = 0;

            CheckForCollision();

            Rect = new Rectangle(Rect.X + (int)Vel.X, Rect.Y + (int)Vel.Y, Rect.Width, Rect.Height);
        }
        public override void Draw(SpriteBatch SB)
        {
            SpriteEffects Flip;
            if (FacingRight)
                Flip = SpriteEffects.FlipHorizontally;
            else
                Flip = SpriteEffects.None;

            if (ExplodingCounter == 0)
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)LevelManager.Camera.X, Rect.Y + (int)LevelManager.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(17 * WalkAnimState, 0, 16, 16), Color.White, 0, new Vector2(0, 0), Flip, 0);
            }
            else
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)LevelManager.Camera.X, Rect.Y + (int)LevelManager.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(17 * (WalkAnimState + 1), 0, 16, 16), Color.White, 0, new Vector2(0, 0), Flip, 0);
            }
        }
    }
}
