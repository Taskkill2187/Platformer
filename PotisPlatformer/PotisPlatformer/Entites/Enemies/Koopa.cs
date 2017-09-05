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
    enum KoopaColor { Green, Red, Blue }

    class Koopa : Enemy
    {
        KoopaColor Col = KoopaColor.Green;
        public bool HasShell;

        public Koopa(int PosX, int PosY, KoopaColor Color, Level Parent) : base (PosX, PosY - 12, true, 3, Parent)
        {
            Rect.Height = (int)(28 * 3.62666f);
            Col = Color;
            Texture = Assets.GreenKoopa;
            HasShell = true;
            WalkAnimStates = 2;
        }

        public override object Clone()
        {
            Enemy E = (Enemy)this.MemberwiseClone();
            E.RightCheck = new Rectangle();
            E.LeftCheck = new Rectangle();
            return E;
        }
        public void OnShellLoss()
        {
            HasShell = false;
            Parent.EnemyList.Add(new Shell(Rect.X, Rect.Y + Rect.Height/2, Col, !FacingRight, Parent));

            if (FacingRight)
                Vel += new Vector2(7, 0);
            else
                Vel += new Vector2(-7, 0);

            Rect.Height = (int)(16 * 4.533333f);
            Rect.X += 48;
        }
        public override void OnDeath()
        {
            if (HasShell)
                OnShellLoss();
            else
            {
                ParticleManager.CreateParticleExplosionFromEntityTexture(this, new Rectangle(16 * WalkAnimState + 17 * 2, Texture.Height / 2, 16, Texture.Height / 2), 
                    0.3f, 0.6f, true, true, false, Parent);
                Parent.EnemyList.Remove(this);
            }
        }

        public override void Update()
        {
            Vel.Y += 1f;
            Vel.X /= 1.01f;

            if (Rect.X < 0)
                FacingRight = true;

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

            if (Timer % 10 == 0)
                WalkAnimState++;

            if (WalkAnimState >= WalkAnimStates)
                WalkAnimState = 0;
            
            Rect = new Rectangle(Rect.X + (int)Vel.X, Rect.Y + (int)Vel.Y, Rect.Width, Rect.Height);
            CheckForCollision();
        }
        public override void Draw(SpriteBatch SB)
        {
            SpriteEffects Flip;
            if (FacingRight)
                Flip = SpriteEffects.FlipHorizontally;
            else
                Flip = SpriteEffects.None;

            if (HasShell)
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(16 * WalkAnimState, -1, 16, Texture.Height), Color.White, 0, new Vector2(0, 0), Flip, 0);
            }
            else
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(16 * WalkAnimState + 17 * 2, Texture.Height / 2, 16, Texture.Height / 2), Color.White, 0, new Vector2(0, 0), Flip, 0);
            }
        }
    }
    class Shell : Enemy
    {
        KoopaColor Col = KoopaColor.Green;
        public Shell(int PosX, int PosY, KoopaColor Color, bool FacingRight, Level Parent) : base (PosX, PosY, FacingRight, 10, Parent)
        {
            Texture = Assets.GreenShell;
            Col = Color;
            WalkAnimStates = 4;
        }

        public override void OnDeath()
        {
            ParticleManager.CreateParticleExplosionFromEntityTexture(this, new Rectangle(17 * WalkAnimState, 12, 16, 16), 0.3f, 0.3f, FacingRight, true, false, Parent);
            Parent.EnemyList.Remove(this);
        }
        public override void CheckForCollision()
        {
            for (int i = 0; i < Parent.BlockList.Count; i++)
            {
                if (Parent.BlockList[i].Rect.X - Rect.X < 200 && Parent.BlockList[i].Rect.X - Rect.X > -200)
                {
                    if (Rect.Intersects(Parent.BlockList[i].Rect) && Parent.BlockList[i].Collision)
                    {
                        if (Rect.Y + this.Rect.Height - Level.BlockScale / 2 >= Parent.BlockList[i].Rect.Y + Level.BlockScale / 2)
                        {
                            if (Rect.X > Parent.BlockList[i].Rect.X)
                            {
                                FacingRight = true;
                                Rect.X = Parent.BlockList[i].Rect.X + Parent.BlockList[i].Rect.Width;
                            }
                            else
                            {
                                FacingRight = false;
                                Rect.X = Parent.BlockList[i].Rect.X - Rect.Width;
                            }
                            Vel.Y = 0;
                        }
                        else
                        {
                            Rect.Y = Parent.BlockList[i].Rect.Y - this.Rect.Height;
                            Vel.Y = 0;
                            Vel.X /= 1.4f;
                            TimesJumped = 0;
                        }
                    }
                }
            }
        }

        public override void Update()
        {
            Vel.Y += 1f;
            Vel.X /= 1.01f;

            if (Rect.X < 0)
                FacingRight = true;

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

            if (Timer > 10)
            {
                for (int i = 0; i < Parent.EnemyList.Count; i++)
                {
                    if (Parent.EnemyList[i] != this && Parent.EnemyList[i].Rect.Intersects(Rect))
                    {
                        Parent.EnemyList[i].OnDeath();
                    }
                }
            }

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

            SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(17 * WalkAnimState, 12, 16, 16), Color.White, 0, new Vector2(0, 0), Flip, 0);
        }
    }
}
