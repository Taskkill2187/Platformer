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
    public class Axon : NeuralNetworkEntity, ICloneable
    {
        public Neuron Input, Output;
        public float weight; // should be between -1 and 1

        public Axon(Neuron Input, Neuron Output)
        {
            this.Input = Input;
            this.Output = Output;
            weight = (float)Values.RDM.NextDouble() * 2 - 1;

            if (Input.Pos.X > Output.Pos.X)
            {
                Neuron Test = Input;
                Input = Output;
                Output = Test;
            }
        }

        public override void Mutate()
        {
            if (Values.RDM.NextDouble() < AI_Player.MutationProbability)
            {
                weight += (float)Values.RDM.NextDouble() - 0.5f;

                if (weight > AI_Player.MaxAxonWeight)
                    weight = AI_Player.MaxAxonWeight;
                if (weight < AI_Player.MinAxonWeight)
                    weight = AI_Player.MinAxonWeight;
            }
        }
        
        public override float GetArragementIndex()
        {
            if (Input.Pos.X > Output.Pos.X)
            {
                Neuron Test = Input;
                Input = Output;
                Output = Test;
            }

            return Input.Pos.X + float.Epsilon;
        }

        public override void Update()
        {
            Output.value += Input.value * weight;
        }

        public override void Draw(SpriteBatch SB, Vector2 NeuronGrid_Middle, Vector2 NeuronGrid_Size)
        {
            if (weight == 0)
                Assets.DrawLine(Input.Pos * NeuronGrid_Size + NeuronGrid_Middle,
                    Output.Pos * NeuronGrid_Size + NeuronGrid_Middle, 2, Color.Black, SB);
            else if (weight > 0)
                Assets.DrawLine(Input.Pos * NeuronGrid_Size + NeuronGrid_Middle,
                    Output.Pos * NeuronGrid_Size + NeuronGrid_Middle, 2, Color.Green, SB);
            else
                Assets.DrawLine(Input.Pos * NeuronGrid_Size + NeuronGrid_Middle,
                    Output.Pos * NeuronGrid_Size + NeuronGrid_Middle, 2, Color.Red, SB);
        }

        public object Clone(AI_Player ClonePlayer, AI_Player CurrentPlayer)
        {
            Axon A = (Axon)this.MemberwiseClone();

            int InputIndex = 0;
            int OutputIndex = CurrentPlayer.Neurons.IndexOf(Output);

            if (OutputIndex == -1)
            {
                bool wat = CurrentPlayer.InputNeuronsContain(Output);

            }

            if (CurrentPlayer.Neurons.Contains(Input))
            {
                InputIndex = CurrentPlayer.Neurons.IndexOf(Input);
                A.Input = ClonePlayer.Neurons[InputIndex];
                A.Output = ClonePlayer.Neurons[OutputIndex];
            }
            else
            {
                int InputIndexY = 0;
                for (int x = 0; x < CurrentPlayer.InputNeurons.GetLength(0); x++)
                    for (int y = 0; y < CurrentPlayer.InputNeurons.GetLength(1); y++)
                        if (CurrentPlayer.InputNeurons[x, y] == Input)
                        {
                            InputIndex = x;
                            InputIndexY = y;
                        }
                
                A.Input = ClonePlayer.InputNeurons[InputIndex, InputIndexY];
                A.Output = ClonePlayer.Neurons[OutputIndex];
            }

            return A;
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
