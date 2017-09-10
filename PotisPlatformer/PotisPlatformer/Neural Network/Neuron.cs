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

namespace Platformer.Neural_Network
{
    public class Neuron : NeuralNetworkEntity, ICloneable
    {
        public float value; // should be between -1 and 1
        public Vector2 Pos;
        public const float Size = 10;
        public string Name;
        public float startValue;

        public Neuron(Vector2 Pos)
        {
            this.Pos = Pos;
            value = 0;
            startValue = 0;
        }

        public override void Mutate()
        {
            while (Values.RDM.NextDouble() < AI_Player.MutationProbability / 5)
            {
                startValue += (float)Values.RDM.NextDouble() - 0.5f;

                if (startValue > 1)
                    startValue = 1;
                if (startValue < -1)
                    startValue = -1;
            }
        }
        public override float GetArragementIndex()
        {
            return Pos.X;
        }
        public object Clone()
        {
            Neuron N = (Neuron)this.MemberwiseClone();
            N.Pos = new Vector2(Pos.X, Pos.Y);
            return N;
        }

        public override void Update()
        {
            value = (float)(2 / (1 + Math.Exp(-2 * value)) - 1) + startValue;
        }
        public override void Draw(SpriteBatch SB, Vector2 NeuronGrid_Middle, Vector2 NeuronGrid_Size)
        {
            if (value == 0)
                Assets.DrawCircle(Pos * NeuronGrid_Size + NeuronGrid_Middle, Size, Color.White * 0.5f, SB);
            else if (value > 0)
                Assets.DrawCircle(Pos * NeuronGrid_Size + NeuronGrid_Middle, Size, Color.White, SB);
            else
                Assets.DrawCircle(Pos * NeuronGrid_Size + NeuronGrid_Middle, Size, Color.Black, SB);

            if (!string.IsNullOrEmpty(Name))
                SB.DrawString(Assets.SmallFont, Name, Pos * NeuronGrid_Size + NeuronGrid_Middle + new Vector2(5, -Assets.SmallFont.MeasureString(Name).Y / 2), Color.Black);
        }
    }
}
