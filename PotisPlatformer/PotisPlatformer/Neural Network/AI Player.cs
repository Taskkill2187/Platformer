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
    public class AI_Player : Player, ICloneable
    {
        const int InputSizeSqrt = 11;
        public float Fitness = 0;

        public Neuron[,] InputNeurons = new Neuron[InputSizeSqrt, InputSizeSqrt];
        public List<Neuron> Neurons = new List<Neuron>();
        public List<Axon> Axons = new List<Axon>();
        public List<NeuralNetworkEntity> Entities = new List<NeuralNetworkEntity>();

        // Mutations
        public static double MutationProbability = 0.3; // is changed by evolution manager
        public static float MinAxonWeight = -1;
        public static float MaxAxonWeight = 1;

        // Outputs
        public Neuron SprintNeuron;
        public Neuron RightNeuron;
        public Neuron LeftNeuron;
        public Neuron JumpNeuron;

        Vector2 NeuronGrid_Middle, NeuronGrid_Size;

        public AI_Player(Vector2 StartingPos, Level Parent, Vector2 NeuronGrid_Middle, Vector2 NeuronGrid_Size) : base(StartingPos, Parent)
        {
            InputNeurons = new Neuron[InputSizeSqrt, InputSizeSqrt];
            Neurons = new List<Neuron>();
            Axons = new List<Axon>();
            Entities = new List<NeuralNetworkEntity>();
            this.NeuronGrid_Middle = NeuronGrid_Middle;
            this.NeuronGrid_Size = NeuronGrid_Size;

            // Add Input Neurons
            for (int x = 0; x < InputSizeSqrt; x++)
                for (int y = 0; y < InputSizeSqrt; y++)
                    InputNeurons[x, y] = new Neuron(new Vector2((x / NeuronGrid_Size.X * Neuron.Size * 1.5f) - 2.1f, 
                        y / NeuronGrid_Size.Y * Neuron.Size * 1.5f));
            
            // Add Neurons
            int NeuronCount = Values.RDM.Next(0, 1);
            for (int i = 0; i < NeuronCount; i++)
                Neurons.Add(new Neuron(new Vector2((float)(Values.RDM.NextDouble() * 2 - 1), (float)(Values.RDM.NextDouble() * 2 - 1))));

            RightNeuron = new Neuron(new Vector2(1.1f, Neuron.Size / NeuronGrid_Size.Y + 0.02f));
            LeftNeuron = new Neuron(new Vector2(1.1f, (Neuron.Size / NeuronGrid_Size.Y + 0.02f) * 2));
            JumpNeuron = new Neuron(new Vector2(1.1f, (Neuron.Size / NeuronGrid_Size.Y + 0.02f) * 3));
            SprintNeuron = new Neuron(new Vector2(1.1f, (Neuron.Size / NeuronGrid_Size.Y + 0.02f) * 4));

            Neurons.Add(SprintNeuron);
            Neurons.Add(RightNeuron);
            Neurons.Add(LeftNeuron);
            Neurons.Add(JumpNeuron);

            RightNeuron.Name = nameof(RightNeuron);
            LeftNeuron.Name = nameof(LeftNeuron);
            JumpNeuron.Name = nameof(JumpNeuron);
            SprintNeuron.Name = nameof(SprintNeuron);

            int CompleteNeuronCount = Neurons.Count + InputNeurons.GetLength(0) * InputNeurons.GetLength(1);
            float InputNeuronAmountPercentage = InputNeurons.GetLength(0) * InputNeurons.GetLength(1) / (float)CompleteNeuronCount;

            // Add Axons
            int AxonCount = Values.RDM.Next(1, 3);
            for (int i = 0; i < AxonCount; i++)
            {
                if ((float)Values.RDM.NextDouble() < InputNeuronAmountPercentage)
                {
                    // Axon input is a inputNeuron
                    Axons.Add(new Axon(GetRandomInputNeuron(), GetRandomNonInputNeuron()));
                }
                else
                {
                    // Axon input is not a inputNeuron
                    Neuron Input = GetRandomNonInputNeuron();
                    Neuron Output = GetRandomNonInputNeuron(Input);
                    Axons.Add(new Axon(Input, Output));
                }
            }

            RemoveUnusedNeurons();
            UpdateEntitiesList();
        }
        
        public AI_Player CreateOffSpring()
        {
            AI_Player P = (AI_Player)this.Clone();
            P.Mutate();
            P.Fitness = 0;
            return P;
        }

        public void AssignNewWorld(Level World)
        {
            Parent = World;
            Parent.ThisPlayer = this;

            Vel = Vector2.Zero;
            Rect = new Rectangle((int)RespawnPoint.X, (int)RespawnPoint.Y, Level.BlockScale, (int)(Level.BlockScale * 1.75f));
            Parent.Reset();
            CanMove = true;
            DeathTimer = 0;
        }

        public Neuron GetRandomInputNeuron()
        {
            return InputNeurons[Values.RDM.Next(0, InputNeurons.GetLength(0)), Values.RDM.Next(0, InputNeurons.GetLength(1))];
        }
        public Neuron GetRandomNonInputNeuron()
        {
            return Neurons[Values.RDM.Next(0, Neurons.Count)];
        }
        public Neuron GetRandomNonInputNeuron(Neuron NotThisOne)
        {
            while (true)
            {
                Neuron N = Neurons[Values.RDM.Next(0, Neurons.Count)];
                if (N != NotThisOne)
                    return N;
            }
        }

        public void RemoveUnusedNeurons()
        {
            Neurons.RemoveAll(x => NeuronIsNoLongerInUse(x));
        }
        public bool NeuronIsNoLongerInUse(Neuron N)
        {
            if (N == RightNeuron || N == LeftNeuron || N == JumpNeuron || N == SprintNeuron)
                return false;

            foreach (Axon Ax in Axons)
                if (Ax.Input == N || Ax.Output == N)
                    return false;

            if (N.Name != null)
                N.Name.Split();

            return true;
        }

        public void UpdateEntitiesList()
        {
            Entities = new List<NeuralNetworkEntity>();
            foreach (Neuron N in Neurons)
                Entities.Add(N);
            foreach (Axon Ax in Axons)
                Entities.Add(Ax);

            if (!Neurons.Contains(RightNeuron))
                throw new Exception();

            if (!Entities.Contains(RightNeuron))
                throw new Exception();
        }
        public void UpdateInputNeurons()
        {
            if (!Entities.Contains(RightNeuron))
                throw new Exception();

            Vector2 PlayerBlockCoords = new Vector2(Rect.X + Rect.Width / 2f - Level.BlockScale / 2f, Rect.Y + Rect.Height - Level.BlockScale);
            Vector2 UpperRightCheckZone = new Vector2(PlayerBlockCoords.X - InputSizeSqrt / 2f * Level.BlockScale, 
                PlayerBlockCoords.Y - InputSizeSqrt / 2f * Level.BlockScale);

            for (int x = 0; x < InputSizeSqrt; x++)
                for (int y = 0; y < InputSizeSqrt; y++)
                {
                    InputNeurons[x, y].value = Parent.GetNeuronValueForTheseCoords(new Point((int)UpperRightCheckZone.X + x * (Level.BlockScale), (int)UpperRightCheckZone.Y + y * (Level.BlockScale)));
                }

            if (!Entities.Contains(RightNeuron))
                throw new Exception();
        }
        public bool InputNeuronsContain(Neuron N)
        {
            for (int x = 0; x < InputSizeSqrt; x++)
                for (int y = 0; y < InputSizeSqrt; y++)
                    if (N == InputNeurons[x, y])
                        return true;

            return false;
        }

        void Mutate()
        {
            if (!Entities.Contains(RightNeuron))
                throw new Exception();

            foreach (Axon Ax in Axons)
                Ax.Mutate();

            // Add Neuron
            if (Values.RDM.NextDouble() < MutationProbability)
            {
                Neuron N = new Neuron(new Vector2((float)(Values.RDM.NextDouble() * 2 - 1), (float)(Values.RDM.NextDouble() * 2 - 1)));

                int Connections = Values.RDM.Next(1, Neurons.Count / 2);
                for (int i = 0; i < Connections; i++)
                    Axons.Add(new Axon(N, Neurons[Values.RDM.Next(Neurons.Count)]));

                Neurons.Add(N);
            }

            // Remove Neuron
            if (Values.RDM.NextDouble() < MutationProbability)
            {
                Neuron N = Neurons[Values.RDM.Next(Neurons.Count)];

                for (int i = 0; i < Axons.Count; i++)
                {
                    if (Axons[i].Output == N || Axons[i].Input == N)
                    {
                        Axons.RemoveAt(i);
                        i--;
                    }
                }

                if (N != RightNeuron && N != LeftNeuron && N != JumpNeuron && N != SprintNeuron)
                    Neurons.Remove(N);
            }

            // Add Axon
            if (Values.RDM.NextDouble() < MutationProbability)
            {
                int CompleteNeuronCount = Neurons.Count + InputNeurons.GetLength(0) * InputNeurons.GetLength(1);
                float InputNeuronAmountPercentage = InputNeurons.GetLength(0) * InputNeurons.GetLength(1) / (float)CompleteNeuronCount;

                if ((float)Values.RDM.NextDouble() < InputNeuronAmountPercentage)
                    Axons.Add(new Axon(GetRandomInputNeuron(), GetRandomNonInputNeuron()));
                else
                {
                    Neuron Input = GetRandomNonInputNeuron();
                    Neuron Output = GetRandomNonInputNeuron(Input);
                    Axons.Add(new Axon(Input, Output));
                }
            }

            if (!Entities.Contains(RightNeuron))
                throw new Exception();
        }

        public void UpdateNeuralNetwork()
        {
            foreach (Neuron N in InputNeurons)
                N.value = 0;
            foreach (Neuron N in Neurons)
                N.value = 0;

            UpdateInputNeurons();
            Entities.OrderBy(x => x.GetArragementIndex());

            foreach (NeuralNetworkEntity E in Entities)
                E.Update();
        }
        public override void Update()
        {
            UpdateNeuralNetwork();

            #region GamePhysics
            Timer++;
            TochedFloorTimer++;
            if (CanMove)
            {
                Vel.X /= 1.05f;

                if (RightNeuron.value > 0)
                {
                    if (Vel.X < MaxWalkSpeed)
                    {
                        Vel.X += Values.PlayerWalkAcceleration;
                    }
                    UpdateWalkAnim();
                    FacingRight = true;
                }
                if (LeftNeuron.value > 0)
                {
                    if (Vel.X > -MaxWalkSpeed)
                        Vel.X -= Values.PlayerWalkAcceleration;

                    UpdateWalkAnim();
                    FacingRight = false;
                }

                // Reset Walking state in case the player is standing still
                if (!(LeftNeuron.value > 0) && !(RightNeuron.value > 0))
                {
                    WalkAnimState = 0;
                    Vel.X /= 1.05f;
                }

                if (SprintNeuron.value > 0)
                    MaxWalkSpeed = Level.BlockScale / 6.25f;
                else
                    MaxWalkSpeed = Level.BlockScale / 9.375f;
            }

            if (!Parent.DebugMode)
                Vel.Y += 1f;

            if (JumpNeuron.value > 0)
            {
                if (Parent.DebugMode)
                    Vel.Y -= 10;
                else
                    Jump(false);
            }

            if (Rect.Y > Values.WindowSize.Y && DeathTimer == 0)
                OnDeath();

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
            #endregion
        }

        public override void Draw(SpriteBatch SB)
        {
            if (!Entities.Contains(RightNeuron))
                throw new Exception();

            foreach (NeuralNetworkEntity E in Entities)
                E.Draw(SB, NeuronGrid_Middle, NeuronGrid_Size);
            foreach (Neuron N in InputNeurons)
                N.Draw(SB, NeuronGrid_Middle, NeuronGrid_Size);

            SB.DrawString(Assets.Font, "Fitness: " + GetPosVector2().X, 
                new Vector2(Values.WindowSize.X / 2 - Assets.Font.MeasureString("Fitness: " + GetPosVector2().X).X / 2, 50), Color.Black);

            base.Draw(SB);
        }
        public override object Clone()
        {
            if (!Entities.Contains(RightNeuron))
                throw new Exception();

            AI_Player P = (AI_Player)base.Clone();
            P.InputNeurons = new Neuron[InputSizeSqrt, InputSizeSqrt];
            P.Neurons = new List<Neuron>();
            P.Axons = new List<Axon>();
            P.Entities = new List<NeuralNetworkEntity>();
            P.Fitness = 0;

            for (int x = 0; x < InputSizeSqrt; x++)
                for (int y = 0; y < InputSizeSqrt; y++)
                    P.InputNeurons[x, y] = (Neuron)InputNeurons[x, y].Clone();

            foreach (Neuron N in Neurons)
            {
                Neuron Clone = (Neuron)N.Clone();
                if (Clone.Name == "SprintNeuron")
                    P.SprintNeuron = Clone;
                if (Clone.Name == "RightNeuron")
                    P.RightNeuron = Clone;
                if (Clone.Name == "LeftNeuron")
                    P.LeftNeuron = Clone;
                if (Clone.Name == "JumpNeuron")
                    P.JumpNeuron = Clone;
                P.Neurons.Add(Clone);
            }

            foreach (Axon A in Axons)
                P.Axons.Add((Axon)A.Clone(P, this));

            P.UpdateEntitiesList();

            return P;
        }
    }
}
