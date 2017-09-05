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
using System.Threading.Tasks;
using System.Threading;

namespace Platformer
{
    public class ParticleBatch
    {
        public Particle[] PArray;
        public Vector2 Middle;
        public float Force;
        public float ForceShift;
        public float DivergenceAngle;
        public float DivergenceAngleShift;

        public int Timer;

        public ParticleBatch(Particle[] PArray, Vector2 Middle, float Force, float ForceShift, float DivergenceAngle, float DivergenceAngleShift)
        {
            this.PArray = PArray;
            this.Force = Force;
            this.DivergenceAngle = DivergenceAngle;
            this.ForceShift = ForceShift;
            this.DivergenceAngleShift = DivergenceAngleShift;
            this.Middle = Middle;
        }

        public void Update()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            Timer++;

            lock (PArray)
            {
                for (int i = 0; i < PArray.Length; i++)
                {
                    if (PArray[i] != null)
                    {
                        PArray[i].Update();

                        if (Force != 0)
                            PArray[i].OrbitAround(Middle, DivergenceAngle, Force);

                        if (PArray[i].LifeTime > 255)
                            PArray[i] = null;
                    }
                }
            }

            Force += ForceShift;
            DivergenceAngle += DivergenceAngleShift;
        }
        public void Draw(SpriteBatch SB)
        {
            lock (PArray)
            {
                for (int i = 0; i < PArray.Length; i++)
                {
                    if (PArray[i] != null)
                        PArray[i].Draw(SB);
                }
            }
        }
    }
}
