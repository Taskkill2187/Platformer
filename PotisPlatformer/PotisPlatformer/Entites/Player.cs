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
    public class Player : Entity, ICloneable
    {
        public int TimesJumped;
        public float MaxWalkSpeed;
        public bool CanMove;
        public bool FacingRight;
        public int TochedFloorTimer;
        public Vector2 RespawnPoint;
        internal int WalkAnimState;
        public const int WalkAnimStates = 4;
        internal int Timer;
        public int DeathTimer;

        public static int CollisionDetectionRectangleInflation = -20;
        public static int CollisionDetectionRectangle2Inflation = -25;

        public Player() { }
        public Player(Vector2 Pos, Level Parent)
        {
            this.Texture = Assets.PlayerWalk;
            this.Vel = Vector2.Zero;
            RespawnPoint = Pos;
            this.Rect = new Rectangle((int)RespawnPoint.X, (int)RespawnPoint.Y, Level.BlockScale, (int)(Level.BlockScale * 1.75f));
            CanMove = true;
            WalkAnimState = 0;
            this.Parent = Parent;
        }

        public virtual void Update()
        {
            Timer++;
            TochedFloorTimer++;
            if (CanMove)
            {
                Vel.X /= 1.05f;
                
                Vector2 AntiGravVel = Vel;
                if (AntiGravVel.Y > -5)
                    AntiGravVel.Y = -5;
                Rectangle HalfFuture = new Rectangle(Rect.X + (int)AntiGravVel.X / 2, Rect.Y + (int)AntiGravVel.Y / 2, Rect.Width, Rect.Height);
                HalfFuture.Inflate(CollisionDetectionRectangleInflation, CollisionDetectionRectangleInflation);
                if (!Parent.NoBlockIntersectsThisRectangle(HalfFuture))
                    Vel = Vector2.Zero;

                if (Controls.CurKS.IsKeyDown(Keys.D))
                {
                    if (Vel.X < MaxWalkSpeed)
                    {
                        Vel.X += Values.PlayerWalkAcceleration;
                    }
                    UpdateWalkAnim();
                    FacingRight = true;
                }
                if (Controls.CurKS.IsKeyDown(Keys.A))
                {
                    if (Vel.X > -MaxWalkSpeed)
                        Vel.X -= Values.PlayerWalkAcceleration;

                    UpdateWalkAnim();
                    FacingRight = false;
                }

                // Reset Walking state in case the player is standing still
                if (!Controls.CurKS.IsKeyDown(Keys.A) && !Controls.CurKS.IsKeyDown(Keys.D))
                {
                    WalkAnimState = 0;
                    Vel.X /= 1.05f;
                }

                if (Controls.CurKS.IsKeyDown(Keys.LeftShift))
                    MaxWalkSpeed = Level.BlockScale / 6.25f;
                else
                    MaxWalkSpeed = Level.BlockScale / 9.375f;
            }
            
            if (!Parent.DebugMode)
                Vel.Y += 1f;
            
            if (Controls.CurKS.IsKeyDown(Keys.W))
            {
                if (Parent.DebugMode)
                    Vel.Y -= 10;
                else
                    Jump(false);
            }

            if (Controls.CurKS.IsKeyDown(Keys.S) && Parent.DebugMode)
                Vel.Y += 10;

            if (Controls.CurKS.IsKeyDown(Keys.Space) && Controls.LastKS.IsKeyUp(Keys.Space))
                Jump(false);

            if (Rect.Y > Values.WindowSize.Y && DeathTimer == 0)
            {
                Vel.Y = -15;
                OnDeath();
            }

            Vector2 AntiGravVel2 = Vel;
            if (AntiGravVel2.Y > -5)
                AntiGravVel2.Y = -5;
            Rectangle HalfFuture2 = new Rectangle(Rect.X + (int)AntiGravVel2.X / 2, Rect.Y + (int)AntiGravVel2.Y / 2, Rect.Width, Rect.Height);
            HalfFuture2.Inflate(CollisionDetectionRectangle2Inflation, CollisionDetectionRectangle2Inflation);
            if (!Parent.NoBlockIntersectsThisRectangle(HalfFuture2))
                Vel = -Vel;

            Rect = new Rectangle(Rect.X + (int)Vel.X, Rect.Y + (int)Vel.Y, Rect.Width, Rect.Height);

            CheckForCollision();

            if (Rect.X < 0)
                Rect.X = 0;

            if (Parent.DebugMode)
                Vel = Vector2.Zero;

            if (DeathTimer > 0)
                DeathTimer++;

            if (DeathTimer > Assets.Death.Duration.Seconds * 60 + 30)
                Ressurection();
        }
        public override void Draw(SpriteBatch SB)
        {
            SpriteEffects Flip;
            if (FacingRight)
                Flip = SpriteEffects.None;
            else
                Flip = SpriteEffects.FlipHorizontally;

            int SinlgeSegmentWidth = Texture.Width / WalkAnimStates;
            if (Texture == null)
            {
                SB.Draw(Assets.White, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            else if (TochedFloorTimer > 3)
            {
                if (Vel.Y < 0)
                {
                    SB.Draw(Assets.PlayerJumpFall, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(0, 0, Assets.PlayerJumpFall.Width / 2, Assets.PlayerJumpFall.Height), Color.White, 0, new Vector2(0, 0), Flip, 0);
                }
                else
                {
                    SB.Draw(Assets.PlayerJumpFall, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(Assets.PlayerJumpFall.Width / 2, 0, Assets.PlayerJumpFall.Width / 2, Assets.PlayerJumpFall.Height), Color.White, 0, new Vector2(0, 0), Flip, 0);
                }
            }
            else
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(SinlgeSegmentWidth * WalkAnimState, 0, SinlgeSegmentWidth, Texture.Height), Color.White, 0, new Vector2(0, 0), Flip, 0);
            }
        }

        public void UpdateWalkAnim()
        {
            if (Timer % Values.PlayerAnimSpeed == 0 && !LevelManager.Ending || Timer % (Values.PlayerAnimSpeed * 2) == 0 && LevelManager.Ending)
            {
                if (WalkAnimState < WalkAnimStates - 1)
                {
                    WalkAnimState++;
                }
                else
                {
                    WalkAnimState = 0;
                }
            }
        }
        public void CheckForCollision()
        {
            Rectangle LastPos = Values.CopyRectangle(Rect);

            for (int i = 0; i < Parent.BlockList.Count; i++)
            {
                // Collision will only be calculated on nearby Blocks with Collision enabled or FallingBlocks which aren't falling yet
                if (Parent.BlockList[i].Collision)
                {
                    //Top
                    if (Parent.BlockList[i].Rect.Intersects(new Rectangle(this.Rect.X + this.Rect.Width / 2, this.Rect.Y, 1, 1)))
                    {
                        Rect.Y = Parent.BlockList[i].Rect.Y + Parent.BlockList[i].Rect.Height;
                        if (Vel.Y < 0)
                            Vel.Y = 0;
                    }

                    //TopLeft
                    if (Parent.BlockList[i].Rect.Intersects(new Rectangle(this.Rect.X + 17, this.Rect.Y, 1, 1)))
                    {
                        Rect.Y = Parent.BlockList[i].Rect.Y + Parent.BlockList[i].Rect.Height;
                        if (Vel.Y < 0)
                            Vel.Y = 0;
                    }

                    //TopRight
                    if (Parent.BlockList[i].Rect.Intersects(new Rectangle(this.Rect.X + this.Rect.Width - 17, this.Rect.Y, 1, 1)))
                    {
                        Rect.Y = Parent.BlockList[i].Rect.Y + Parent.BlockList[i].Rect.Height;
                        if (Vel.Y < 0)
                            Vel.Y = 0;
                    }

                    //Left
                    if (Parent.BlockList[i].Rect.Intersects(new Rectangle(this.Rect.X, this.Rect.Y + this.Rect.Height / 2, 1, 1)))
                    {
                        Rect.X = Parent.BlockList[i].Rect.X + Parent.BlockList[i].Rect.Width;
                    }

                    //Right
                    if (Parent.BlockList[i].Rect.Intersects(new Rectangle(this.Rect.X + this.Rect.Width, this.Rect.Y + this.Rect.Height / 2, -1, 1)))
                    {
                        Rect.X = Parent.BlockList[i].Rect.X - Parent.BlockList[i].Rect.Width;
                    }

                    //Bottom
                    if (Parent.BlockList[i].Rect.Intersects(new Rectangle(this.Rect.X + this.Rect.Width / 2, this.Rect.Y + this.Rect.Height, 1, -1)))
                    {
                        Rect.Y = Parent.BlockList[i].Rect.Y - this.Rect.Height;
                        TimesJumped = 0;
                        TochedFloorTimer = 0;
                        Vel.X /= 1.4f;
                        if (Vel.Y > 0)
                            Vel.Y = 0;
                    }

                    //LeftBottom
                    if (Parent.BlockList[i].Rect.Intersects(new Rectangle(this.Rect.X, this.Rect.Y + this.Rect.Height - 17, 1, -1)))
                    {
                        Rect.X = Parent.BlockList[i].Rect.X + Parent.BlockList[i].Rect.Width;
                    }

                    //RightBottom
                    if (Parent.BlockList[i].Rect.Intersects(new Rectangle(this.Rect.X + this.Rect.Width, this.Rect.Y + this.Rect.Height - 17, 1, -1)))
                    {
                        Rect.X = Parent.BlockList[i].Rect.X - Parent.BlockList[i].Rect.Width;
                    }

                    //BottomLeft
                    if (Parent.BlockList[i].Rect.Intersects(new Rectangle(this.Rect.X + 17, this.Rect.Y + this.Rect.Height, 1, -1)))
                    {
                        Rect.Y = Parent.BlockList[i].Rect.Y - this.Rect.Height;
                        TimesJumped = 0;
                        TochedFloorTimer = 0;
                        Vel.X /= 1.4f;
                        if (Vel.Y > 0)
                            Vel.Y = 0;
                    }

                    //BottomRight
                    if (Parent.BlockList[i].Rect.Intersects(new Rectangle(this.Rect.X + this.Rect.Width - 17, this.Rect.Y + this.Rect.Height, 1, -1)))
                    {
                        Rect.Y = Parent.BlockList[i].Rect.Y - this.Rect.Height;
                        TimesJumped = 0;
                        TochedFloorTimer = 0;
                        Vel.X /= 1.4f;
                        if (Vel.Y > 0)
                            Vel.Y = 0;
                    }
                }
            }
        }
        public void Jump(bool ForcedJump)
        {
            if (ForcedJump)
            {
                Vel.Y = -25;
                if (StoredData.Default.SoundEffects && Parent.IsDisplayed)
                {
                    Assets.Jump.Play(0.4f, 0, 0);
                }
            }
            else
            {
                if (TochedFloorTimer < 5 || Values.TimesPlayerCanJump != 1 || TimesJumped != 0)
                {
                    if (TimesJumped < Values.TimesPlayerCanJump && CanMove && Vel.Y >= 0)
                    {
                        TimesJumped++;

                        Vel.X *= 1.75f;
                        Vel.Y = -25;
                        if (StoredData.Default.SoundEffects && Parent.IsDisplayed)
                        {
                            Assets.Jump.Play(0.4f, 0, 0);
                        }
                    }
                }
            }
        }
        public void OnDeath()
        {
            if (StoredData.Default.SoundEffects && Parent.IsDisplayed)
                Assets.Death.Play(0.8f, 0, 0);
            DeathTimer = 1;
            int SinlgeSegmentWidth = Texture.Width / WalkAnimStates;
            ParticleManager.CreateParticleExplosion(this, new Rectangle(SinlgeSegmentWidth * WalkAnimState, 0, SinlgeSegmentWidth, Texture.Height), 
                0.3f, -12f, !FacingRight, true, false, Parent);
            Rect.Width = 0;
            CanMove = false;
            MediaPlayer.Stop();
            Vel = Vector2.Zero;
            Parent.Ending = false;
        }
        public virtual void Ressurection()
        {
            Vel = Vector2.Zero;
            Rect = new Rectangle((int)RespawnPoint.X, (int)RespawnPoint.Y, Level.BlockScale, (int)(Level.BlockScale * 1.75f));
            Parent.Reset();
            CanMove = true;
            DeathTimer = 0;
            MediaPlayer.Play(Assets.InGameTheme);
            LevelManager.UpdateTextures();
            LevelManager.Ending = false;
        }

        public override object Clone()
        {
            Player P = (Player)this.MemberwiseClone();
            P.Rect = new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            P.Vel = new Vector2(Vel.X, Vel.Y);
            P.CreatorClickPos = new Point(CreatorClickPos.X, CreatorClickPos.Y);
            P.Texture = Texture;
            P.RespawnPoint = new Vector2(RespawnPoint.X, RespawnPoint.Y);
            return P;
        }
    }
}
