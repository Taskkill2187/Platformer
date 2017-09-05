using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading.Tasks;

namespace Platformer
{
    public static class ParticleManager
    {
        public static List<ParticleBatch> ParticleBatchList = new List<ParticleBatch>();

        public static void Clear()
        {
            ParticleBatchList = new List<ParticleBatch>();
        }
        public static void AddParticleBatch(ParticleBatch PB)
        {
            ParticleBatchList.Add(PB);
        }

        public static void Update()
        {
            foreach (ParticleBatch PB in ParticleBatchList)
                Task.Factory.StartNew(() => PB.Update());

            lock (ParticleBatchList)
            {
                for (int i = 0; i < ParticleBatchList.Count; i++)
                {
                    if (ParticleBatchList[i].Timer > 255)
                        ParticleBatchList.RemoveAt(i);
                }

                //if (ParticleBatchList.Count > 10)
                //    ParticleBatchList.Remove(ParticleBatchList.First());
            }
        }

        public static void CreateParticleExplosionFromEntityTexture(Entity E, Rectangle TexelField, float GravForce, float DriftSpeed, bool FlipHorz, bool Collision, bool Anim, Level Parent)
        {
            if (StoredData.Default.ParticleEffects && Parent.IsDisplayed)
            {
                float PixelSize = E.Rect.Width / TexelField.Width;

                Color[] Col1D = new Color[E.Texture.Width * E.Texture.Height];
                E.Texture.GetData(Col1D);

                Color[,] Col2D = new Color[E.Texture.Width, E.Texture.Height];

                for (int i = 0; i < Col1D.Length; i++)
                    Col2D[i % E.Texture.Width, i / E.Texture.Width] = Col1D[i];

                Particle[] PArray = new Particle[Col2D.Length];

                for (int ix = TexelField.X; ix < TexelField.Width + TexelField.X; ix++)
                {
                    for (int iy = TexelField.Y; iy < TexelField.Height + TexelField.Y; iy++)
                    {
                        // Only create a Particle if it's a visible Texel
                        if (Col2D[ix, iy].A != 0 || Col2D[ix, iy].ToVector3() != Vector3.Zero)
                        {
                            Vector2 Point;

                            if (FlipHorz)
                                Point = new Vector2(TexelField.Width - (ix - TexelField.X), iy - TexelField.Y);
                            else
                                Point = new Vector2(ix - TexelField.X, iy - TexelField.Y);

                            int LifeTime;

                            if (Anim)
                                LifeTime = Values.RDM.Next(175, 230);
                            else
                                LifeTime = Values.RDM.Next(0, 50);

                            // Construktor                                                              Adding the Entity Position to the Point
                            PArray[(int)Point.X + (int)Point.Y * Col2D.GetLength(1) - 1] = new Particle(Point * PixelSize + E.GetPosVector2(),
                                // Lifetime
                                Values.RDM.Next(175, 230),
                                // Texture    Color
                                Assets.White, Col2D[ix, iy],
                                // Velocity
                                new Vector2(((Point.X - TexelField.Width / 2) * 2f + Values.RDM.Next(4)) / 20f * DriftSpeed,
                                            ((Point.Y - TexelField.Height / 2) * 2f + Values.RDM.Next(4)) / 20f * DriftSpeed) + E.Vel, GravForce,
                                // Friction                Size
                                new Vector2(1.02f, 1.003f), new Vector2(PixelSize), Collision, Parent);
                        }
                    }
                }

                if (Anim)
                    AddParticleBatch(new ParticleBatch(PArray, E.GetPosVector2() + E.GetSizeVector2() / 2, 0, 100, 0.5f, 0.02f));
                else
                    AddParticleBatch(new ParticleBatch(PArray, E.GetPosVector2() + E.GetSizeVector2() / 2, 0, 0, 0, 0));
            }
        }
        public static void CreateParticleExplosionFromEntityTexture(Entity E, Rectangle TexelField, float GravForce, float DriftSpeed, bool FlipHorz, bool Collision, bool Anim, Vector2 StartSpeed, Level Parent)
        {
            if (StoredData.Default.ParticleEffects && Parent.IsDisplayed)
            {
                float PixelSize = E.Rect.Width / (float)TexelField.Width;

                Color[] Col1D = new Color[E.Texture.Width * E.Texture.Height];
                E.Texture.GetData(Col1D);

                Color[,] Col2D = new Color[E.Texture.Width, E.Texture.Height];

                for (int i = 0; i < Col1D.Length; i++)
                    Col2D[i % E.Texture.Width, i / E.Texture.Width] = Col1D[i];

                Particle[] PArray = new Particle[Col2D.Length];

                for (int ix = TexelField.X; ix < TexelField.Width + TexelField.X; ix++)
                {
                    for (int iy = TexelField.Y; iy < TexelField.Height + TexelField.Y; iy++)
                    {
                        // Only create a Particle if it's a visible Texel
                        if (Col2D[ix, iy].A != 0 || Col2D[ix, iy].ToVector3() != Vector3.Zero)
                        {
                            Vector2 Point;

                            if (FlipHorz)
                                Point = new Vector2(TexelField.Width - (ix - TexelField.X), iy - TexelField.Y);
                            else
                                Point = new Vector2(ix - TexelField.X, iy - TexelField.Y);

                            int LifeTime;

                            if (Anim)
                                LifeTime = Values.RDM.Next(175, 230);
                            else
                                LifeTime = Values.RDM.Next(0, 50);

                            // Construktor                                                              Adding the Entity Position to the Point
                            PArray[(int)Point.X + (int)Point.Y * Col2D.GetLength(1) - 1] = new Particle(Point * PixelSize + E.GetPosVector2(),
                                // Lifetime
                                Values.RDM.Next(175, 230),
                                // Texture    Color
                                Assets.White, Col2D[ix, iy],
                                // Velocity
                                new Vector2(((Point.X - TexelField.Width / 2) * 2f + Values.RDM.Next(4)) / 20f * DriftSpeed,
                                            ((Point.Y - TexelField.Height / 2) * 2f + Values.RDM.Next(4)) / 20f * DriftSpeed) + E.Vel + StartSpeed, GravForce,
                                // Friction                Size
                                new Vector2(1.02f, 1.003f), new Vector2(PixelSize), Collision, Parent);
                        }
                    }
                }

                if (Anim)
                    AddParticleBatch(new ParticleBatch(PArray, E.GetPosVector2() + E.GetSizeVector2() / 2, 0, 100, 0.5f, 0.02f));
                else
                    AddParticleBatch(new ParticleBatch(PArray, E.GetPosVector2() + E.GetSizeVector2() / 2, 0, 0, 0, 0));
            }
        }
        public static void CreateExplosion(Vector2 Pos, Vector2 Vel, Color Col, Level Parent)
        {
            if (StoredData.Default.ParticleEffects && Parent.IsDisplayed)
            {
                Particle[] PArray = new Particle[20];
                for (int ip = 0; ip < 20; ip++)
                {
                    float Angle = Values.RDM.Next(361);
                    float Strength = Values.RDM.Next(0, 1000);
                    Strength /= 100;
                    PArray[ip] = new Particle(Pos, new Vector2((float)Math.Sin(Angle) * Strength,
                        (float)Math.Cos(Angle) * Strength) + Vel + new Vector2(0, -2), Col, 0.02f, 50 + Values.RDM.Next(30), Parent);
                }
                AddParticleBatch(new ParticleBatch(PArray, Pos, 0, 0, 0, 0));
            }
        }
        public static void CreateDescendingParticleOfEntity(Entity E, Rectangle TexelField, float GravForce, bool FlipHorz, Level Parent)
        {
            if (StoredData.Default.ParticleEffects && Parent.IsDisplayed)
            {
                Particle[] PArray = new Particle[1];
                PArray[0] = new Particle(E.GetPosVector2() + E.GetSizeVector2() / 2, 0, E.Texture, Color.White, E.Vel + new Vector2(0, -3), 0.3f, new Vector2(1.02f, 1.003f), E.GetSizeVector2() / 15, false, Parent);
                ParticleBatchList.Add(new ParticleBatch(PArray, E.GetPosVector2() + E.GetSizeVector2() / 2, 0, 0, 0, 0));
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < ParticleBatchList.Count; i++)
            {
                ParticleBatchList[i].Draw(spriteBatch);
            }
        }
    }
}
