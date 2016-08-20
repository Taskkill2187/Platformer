using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    public class Particle
    {
        public float GravForce;
        public int LifeTime;
        public Vector2 Friction = new Vector2(1.04f, 1.04f);
        public bool HasCollision;
        public Vector2 Size;
        Texture2D Tex;
        Vector2 Pos;
        Vector2 Vel;
        Color Color;

        public float PullLength;
        public float ForceMagnitude;
        public float ForceAngle;

        public Particle(Vector2 Pos, Vector2 Vel, Color Color, float GravForce, int LifeTime)
        {
            this.Pos = Pos;
            this.Vel = Vel;

            Friction = new Vector2(1.03f, 1.03f);
            Tex = Assets.White;
            this.Color = Color;
            this.LifeTime = LifeTime;
            this.GravForce = GravForce;
            Size = new Vector2(4.53333f);
        }
        public Particle(Vector2 Pos, int LifeTime, Texture2D Tex, Color Col, Vector2 Vel, float GravitiationForce,
            Vector2 Friction, Vector2 Size, bool HasCollision)
        {
            this.Pos = Pos;
            this.Vel = Vel;
            Color = Col;
            this.Tex = Tex;
            this.LifeTime = LifeTime;
            GravForce = GravitiationForce;
            this.Friction = Friction;
            this.HasCollision = HasCollision;
            this.Size = Size;
        }

        public void Update()
        {
            Vel.X /= Friction.X;
            Vel.Y /= Friction.Y;

            LifeTime++;

            Vel.Y += GravForce;

            Pos += Vel;
            Collision();
        }
        public void OrbitAround(Vector2 Point, float DivergenceAngle, float Force)
        {
            PullLength = Vector2.Distance(Point, Pos);
            Vector2 PullVektor = Point - Pos;
            ForceMagnitude = Force / (PullLength);
            ForceAngle = (float)Math.Atan2(PullVektor.X, PullVektor.Y) + (float)Math.PI - DivergenceAngle;

            if (ForceMagnitude > 0.7f)
            {
                ForceMagnitude = 0.7f;
            }

            Vel.X -= ForceMagnitude * (float)Math.Sin(ForceAngle);
            Vel.Y -= ForceMagnitude * (float)Math.Cos(ForceAngle);

            Vel.X /= 1.05f;
            Vel.Y /= 1.05f;
        }
        public void Collision()
        {
            if (HasCollision)
            {
                lock (LevelManager.CurrentLevel.BlockList)
                {
                    for (int i = 0; i < LevelManager.CurrentLevel.BlockList.Count; i++)
                    {
                        if (i > LevelManager.CurrentLevel.BlockList.Count || LevelManager.CurrentLevel.BlockList[i] == null)
                            break;

                        if (LevelManager.CurrentLevel.BlockList[i].Collision &&
                            LevelManager.CurrentLevel.BlockList[i].Rect.X > Pos.X - LevelManager.BlockScale &&
                            LevelManager.CurrentLevel.BlockList[i].Rect.X < Pos.X + LevelManager.BlockScale)
                        {
                            Rectangle R = LevelManager.CurrentLevel.BlockList[i].Rect;
                            if (LevelManager.CurrentLevel.BlockList[i].GetType() != typeof(JumpBlock))
                            {
                                // Top
                                if (new Rectangle(R.X, R.Y, R.Width, 10).Intersects(new Rectangle((int)Pos.X - (int)Size.X / 2, (int)Pos.Y - (int)Size.Y / 2, (int)Size.X, (int)Size.Y)))
                                {
                                    Pos.Y = R.Y - Size.Y / 2;
                                    Vel.Y *= -0.5f;
                                }

                                // Left
                                if (new Rectangle(R.X, R.Y, 10, R.Height).Intersects(new Rectangle((int)Pos.X - (int)Size.X / 2, (int)Pos.Y - (int)Size.Y / 2, (int)Size.X, (int)Size.Y)))
                                {
                                    Pos.X = LevelManager.CurrentLevel.BlockList[i].Rect.X - Size.X / 2;
                                    Vel.X *= -0.5f;
                                }

                                // Bottom
                                if (new Rectangle(R.X, R.Y + R.Height - 10, R.Width, 10).Intersects(new Rectangle((int)Pos.X - (int)Size.X / 2, (int)Pos.Y - (int)Size.Y / 2, (int)Size.X, (int)Size.Y)))
                                {
                                    Pos.Y = R.Y + R.Height + Size.Y / 2;
                                    Vel.Y *= -0.5f;
                                }

                                // Right
                                if (new Rectangle(R.X + R.Width - 10, R.Y, 10, R.Height).Intersects(new Rectangle((int)Pos.X - (int)Size.X / 2, (int)Pos.Y - (int)Size.Y / 2, (int)Size.X, (int)Size.Y)))
                                {
                                    Pos.X = R.X + R.Width + Size.X / 2;
                                    Vel.X *= -0.5f;
                                }
                            }
                            else
                            {
                                if (new Rectangle((int)Pos.X - (int)Size.X / 2, (int)Pos.Y - (int)Size.Y / 2, (int)Size.X, (int)Size.Y).Intersects(R))
                                {
                                    switch (((JumpBlock)LevelManager.CurrentLevel.BlockList[i]).Direction)
                                    {
                                        case Direction.Up:
                                            Vel.Y += -((JumpBlock)LevelManager.CurrentLevel.BlockList[i]).Strength / 5;
                                            Vel.X /= ((JumpBlock)LevelManager.CurrentLevel.BlockList[i]).Friction;
                                            break;

                                        case Direction.Down:
                                            Vel.Y += ((JumpBlock)LevelManager.CurrentLevel.BlockList[i]).Strength / 5;
                                            Vel.X /= ((JumpBlock)LevelManager.CurrentLevel.BlockList[i]).Friction;
                                            break;

                                        case Direction.Left:
                                            Vel.X += -((JumpBlock)LevelManager.CurrentLevel.BlockList[i]).Strength / 5;
                                            Vel.Y /= ((JumpBlock)LevelManager.CurrentLevel.BlockList[i]).Friction;
                                            break;

                                        case Direction.Right:
                                            Vel.X += ((JumpBlock)LevelManager.CurrentLevel.BlockList[i]).Strength / 5;
                                            Vel.Y /= ((JumpBlock)LevelManager.CurrentLevel.BlockList[i]).Friction;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Tex != null)
            {
                spriteBatch.Draw(Tex, Pos + LevelManager.Camera, null, Color, 0,
                        new Vector2(Tex.Width / 2, Tex.Height / 2), Size, SpriteEffects.None, 0);
            }
        }
    }
}
