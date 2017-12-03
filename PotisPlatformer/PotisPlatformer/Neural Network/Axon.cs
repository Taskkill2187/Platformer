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
using System.Xml.Serialization;

namespace Platformer.Neural_Network
{
    [XmlInclude(typeof(Axon))]
    public class Axon : NeuralNetworkEntity, ICloneable
    {
        bool InputFromInputNeurons;
        int InputIndex, OutputIndex, ParentIndex;
        public float weight; // should be between -1 and 1

        [XmlIgnore]
        public AI_Player Parent;
        [XmlIgnore]
        public Neuron Input;
        [XmlIgnore]
        public Neuron Output;

        public Axon () { }
        public Axon(Neuron Input, Neuron Output, AI_Player Parent)
        {
            this.Parent = Parent;
            this.Output = Output;
            this.Input = Input;
            
            weight = (float)Values.RDM.NextDouble() * 2 - 1;

            if (!Parent.Neurons.Contains(Output))
                throw new Exception();

            if (!Parent.Neurons.Contains(Input) && !Parent.InputNeurons.Contains(Input))
                throw new Exception();

            if (Input.Pos.X > Output.Pos.X)
            {
                Neuron Test = Input;
                Input = Output;
                Output = Test;
            }
        }

        public override void Mutate()
        {
            while (Values.RDM.NextDouble() < AI_Player.MutationProbability)
            {
                weight += (float)((Values.RDM.NextDouble() - 0.5f) * AI_Player.MutationStepSize);

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
                Neuron Temp = Input;
                Input = Output;
                Output = Temp;
            }

            return Input.Pos.X + float.Epsilon;
        }

        public void PrepareForXMLSave()
        {
            ParentIndex = Evolution_Manager.Population.IndexOf(Parent);

            if (Parent.Neurons.Contains(Input))
            {
                InputFromInputNeurons = false;
                InputIndex = Parent.Neurons.IndexOf(Input);
            }
            else
            {
                InputFromInputNeurons = true;
                InputIndex = Parent.InputNeurons.ToList().IndexOf(Input);
            }

            if (InputIndex == -1 || InputIndex >= Parent.Neurons.Count && InputFromInputNeurons == false)
                throw new Exception();

            this.OutputIndex = Parent.Neurons.IndexOf(Output);

            if (OutputIndex == -1 || OutputIndex >= Parent.Neurons.Count)
                throw new Exception();
        }
        public void LoadAfterCreationFromXML(AI_Player Parent)
        {
            this.Parent = Parent;

            if (InputFromInputNeurons)
                Input = Parent.InputNeurons[InputIndex];
            else
                Input = Parent.Neurons[InputIndex];

            Output = Parent.Neurons[OutputIndex];
        }

        public override void Update()
        {
            Output.value += Input.value * weight;
        }

        public override void Draw(SpriteBatch SB, Vector2 NeuronGrid_Middle, Vector2 NeuronGrid_Size)
        {
            float Obiacy = 1;
            if (Input.value == 0)
                Obiacy = 0.1f;

            if (weight == 0)
                Assets.DrawLine(Input.Pos * NeuronGrid_Size + NeuronGrid_Middle,
                    Output.Pos * NeuronGrid_Size + NeuronGrid_Middle, 2, Color.Black * Obiacy, SB);
            else if (weight > 0)
                Assets.DrawLine(Input.Pos * NeuronGrid_Size + NeuronGrid_Middle,
                    Output.Pos * NeuronGrid_Size + NeuronGrid_Middle, 2, Color.Green * Obiacy, SB);
            else
                Assets.DrawLine(Input.Pos * NeuronGrid_Size + NeuronGrid_Middle,
                    Output.Pos * NeuronGrid_Size + NeuronGrid_Middle, 2, Color.Red * Obiacy, SB);
        }

        public object Clone(AI_Player ClonePlayer, AI_Player CurrentPlayer)
        {
            Axon A = (Axon)this.MemberwiseClone();
            A.Parent = ClonePlayer;
            
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
                for (int x = 0; x < CurrentPlayer.InputNeurons.GetLength(0); x++)
                    if (CurrentPlayer.InputNeurons[x] == Input)
                    {
                        InputIndex = x;
                    }
                
                A.Input = ClonePlayer.InputNeurons[InputIndex];
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
