using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Platformer.Neural_Network
{
    public static class Evolution_Manager
    {
        public static Level TestingLevel;
        public static Level DrawLevel;
        public static List<AI_Player> Population;
        public const int PopulationCount = 1000;
        public static int Generation;
        public static float GenerationCompletionPercentage;
        public static bool HasReachedTheEnd;
        static Vector2 RespawnPoint;
        static List<int> MaxFitness;
        static List<int> AverageFitness;

        // draw
        static VertexPositionColor[] pointList;
        static short[] lineListIndices;

        static int Threads;

        public static AI_Player Best;

        public static void CreateNewEvolution(Level TestingLevel, Vector2 RespawnPoint)
        {
            if (Population == null)
            {
                Population = new List<AI_Player>();
                MaxFitness = new List<int>();
                AverageFitness = new List<int>();
                MaxFitness.Add(0);
                AverageFitness.Add(0);
                Generation = 0;
                HasReachedTheEnd = false;
                AI_Player.MutationProbability = 0.5;
                AI_Player.MutationStepSize = 2;
                Evolution_Manager.TestingLevel = (Level)TestingLevel.Clone();
                Evolution_Manager.RespawnPoint = RespawnPoint;
                DrawLevel = (Level)TestingLevel.Clone();
                for (int i = 0; i < PopulationCount; i++)
                {
                    Population.Add(new AI_Player(TestingLevel.PlayerPos, TestingLevel,
                        new Vector2(Values.WindowSize.X / 2, 175), new Vector2(250, 100)));
                }
            }
            else
            {
                HasReachedTheEnd = false;
                MaxFitness = new List<int>();
                AverageFitness = new List<int>();
                MaxFitness.Add(0);
                AverageFitness.Add(0);
                //AI_Player.MutationProbability = 0.4;
                Evolution_Manager.TestingLevel = (Level)TestingLevel.Clone();
                Evolution_Manager.RespawnPoint = RespawnPoint;
                DrawLevel = (Level)TestingLevel.Clone();
            }
        }

        public static void TestCurrentGeneration()
        {
            foreach (AI_Player P in Population)
            {
                List<float> FitnessPerTick = new List<float>();
                Level LevelInstance = (Level)TestingLevel.Clone();
                LevelInstance.IsDisplayed = false;
                P.RespawnPoint = new Vector2(RespawnPoint.X, RespawnPoint.Y);
                P.AssignNewWorld(LevelInstance);
                P.Fitness = 0;
                foreach (Neuron N in P.InputNeurons)
                    N.value = 0;
                foreach (Neuron N in P.Neurons)
                    N.value = 0;
                P.Vel = Vector2.Zero;
                P.Rect = new Rectangle((int)RespawnPoint.X, (int)RespawnPoint.Y, Level.BlockScale, (int)(Level.BlockScale * 1.75f));
                P.JumpNeuron.value = 0;
                P.LeftNeuron.value = 0;
                P.RightNeuron.value = 0;
                P.SprintNeuron.value = 0;

                while (true)
                {
                    LevelInstance.Update();
                    FitnessPerTick.Add(P.Rect.X);

                    if (P.DeathTimer > 0)
                        break;
                    if (FitnessPerTick.Count > 350 && P.Rect.X - FitnessPerTick[FitnessPerTick.Count - 350] < 50)
                        break;
                    if (LevelInstance.Ending == true)
                    {
                        HasReachedTheEnd = true;
                        MessageBox.Show("EVOLUTION FINISHED!");
                        XNA.Show = true;
                        break;
                    }
                }
                
                P.Fitness = P.Rect.X - P.Timer / 2000f;

                GenerationCompletionPercentage = Population.IndexOf(P) / (float)Population.Count;

                if (HasReachedTheEnd)
                    break;

                if (LevelCreator.KillEvolutionThread)
                    break;
            }
            
            if (!LevelCreator.KillEvolutionThread)
            {
                Population = Population.OrderBy(x => x.Fitness).ToList();
                Best = (AI_Player)Population.Last().Clone();

                MaxFitness.Add((int)Population.Max(x => x.Fitness));
                AverageFitness.Add((int)Population.Average(x => x.Fitness));
            }
        }
        public static void TestCurrentGenerationMultiThreaded(int Threads)
        {
            // Start
            foreach (AI_Player P in Population)
                P.Fitness = 0;
            Evolution_Manager.Threads = Threads;
            List<Task> Tasks = new List<Task>();

            // Mutli Thread
            for (int i = 0; i < Evolution_Manager.Threads - 1; i++)
            {
                Tasks.Add(Task.Factory.StartNew(() => {
                    for (int j = PopulationCount * i / Threads; j < PopulationCount * (i + 1) / Threads; j++)
                    {
                        List<float> FitnessPerTick = new List<float>();
                        Level LevelInstance = (Level)TestingLevel.Clone();
                        LevelInstance.IsDisplayed = false;
                        Population[j].RespawnPoint = new Vector2(RespawnPoint.X, RespawnPoint.Y);
                        Population[j].AssignNewWorld(LevelInstance);
                        Population[j].Fitness = 0;

                        while (true)
                        {
                            LevelInstance.Update();
                            FitnessPerTick.Add(Population[j].Rect.X);

                            if (Population[j].DeathTimer > 0)
                                break;
                            if (FitnessPerTick.Count > 350 && Population[j].Rect.X - FitnessPerTick[FitnessPerTick.Count - 350] < 50)
                                break;
                            if (LevelInstance.Ending == true)
                            {
                                HasReachedTheEnd = true;
                                break;
                            }
                        }

                        Population[j].Fitness = Population[j].Rect.X - Population[j].Timer / 2000f;

                        GenerationCompletionPercentage = Population.IndexOf(Population[j]) / (float)Population.Count;

                        if (HasReachedTheEnd)
                            break;

                        if (LevelCreator.KillEvolutionThread)
                            break;
                    }
                }));
            }

            Task.WaitAll(Tasks.ToArray());

            // End
            if (!LevelCreator.KillEvolutionThread)
            {
                Population = Population.OrderBy(x => x.Fitness).ToList();
                Best = (AI_Player)Population.Last().Clone();

                MaxFitness.Add((int)Population.Max(x => x.Fitness));
                AverageFitness.Add((int)Population.Average(x => x.Fitness));
            }
        }
        public static void FinallizeGeneration()
        {
            // Kill animals with low fitness
            List<AI_Player> DeathMarker = new List<AI_Player>();
            for (int i = 0; i < Population.Count; i++)
                if (Values.RDM.NextDouble() < (PopulationCount - i) / (double)PopulationCount)
                    DeathMarker.Add(Population[i]);

            int j = 0;
            while (DeathMarker.Count < PopulationCount / 2)
            {
                if (!DeathMarker.Contains(Population[j]))
                    DeathMarker.Add(Population[j]);
                j++;
            }

            while (DeathMarker.Count > PopulationCount / 2)
                DeathMarker.Remove(DeathMarker.Last());

            foreach (AI_Player A in DeathMarker)
                Population.Remove(A);

            // Let the surviving animals bang
            for (int i = 0; i < PopulationCount / 2; i++)
            {
                AI_Player A = Population[i].CreateOffSpring();
                Population.Insert(Population.Count - 1, A);
            }

            // Last Steps
            AI_Player.MutationProbability *= 0.985f;
            if (AI_Player.MutationProbability < 0.05)
                AI_Player.MutationProbability = 0.05;
            AI_Player.MutationStepSize *= 0.99f;
            if (AI_Player.MutationStepSize < 0.2)
                AI_Player.MutationStepSize = 0.2;

            //Console.WriteLine("Generation " + Generation + " finalized!");

            Generation++;
            //SavePopulationToFile();
        }

        public static void PutBestOneIntoTestingLevel()
        {
            Best = Population.Find(x => x.Fitness == Population.Max(y => y.Fitness));
            TestingLevel = (Level)TestingLevel.Clone();
            Best.AssignNewWorld(TestingLevel);
            TestingLevel.IsDisplayed = true;
            TestingLevel.GoToMainMenuAfterEnd = false;
            Best.Ressurection();
        }
        public static void SavePopulationToFile()
        {
            FileStream DataStream = null;
            try
            {
                foreach (AI_Player P in Population)
                    foreach (Axon AX in P.Axons)
                        AX.PrepareForXMLSave();

                XmlSerializer writer = new XmlSerializer(typeof(List<AI_Player>), new Type[] { typeof(Block), typeof(Brick),
                            typeof (Coin), typeof(JumpBlock), typeof(Kugelwilli_Spawner), typeof(QuestionMarkBlock), typeof(SpinningBlock),
                            typeof(Bob_omb), typeof(Enemy), typeof(Goomba), typeof(Koopa), typeof(Kugelwilli), typeof(Player), typeof(AI_Player),
                            typeof(Axon), typeof(Neuron), typeof(NeuralNetworkEntity)});

                string folderpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//PlatformerAIs";
                string path = folderpath + "//AI_Population.xml";
                
                DataStream = File.Create(path);
                writer.Serialize(DataStream, Population);
                DataStream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n\n" + e.InnerException + "\n\n\n" + e.StackTrace);
                if (DataStream != null)
                    DataStream.Close();
            }
        }
        public static void LoadPopulationFromFile()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//PlatformerAIs//AI_Population.xml";
            if (File.Exists(path))
            {
                StreamReader DataStream = null;
                try
                {
                    XmlSerializer reader = new XmlSerializer(typeof(List<AI_Player>), new Type[] { typeof(Block), typeof(Brick),
                            typeof (Coin), typeof(JumpBlock), typeof(Kugelwilli_Spawner), typeof(QuestionMarkBlock), typeof(SpinningBlock),
                            typeof(Bob_omb), typeof(Enemy), typeof(Goomba), typeof(Koopa), typeof(Kugelwilli), typeof(Player), typeof(AI_Player),
                            typeof(Axon), typeof(Neuron), typeof(NeuralNetworkEntity)});

                    DataStream = new StreamReader(path);
                    Population = (List<AI_Player>)reader.Deserialize(DataStream);
                    
                    DataStream.Close();

                    foreach (AI_Player P in Population)
                    {
                        P.RightNeuron = P.Neurons.Find(X => X.Name == "RightNeuron");
                        P.LeftNeuron = P.Neurons.Find(X => X.Name == "LeftNeuron");
                        P.SprintNeuron = P.Neurons.Find(X => X.Name == "SprintNeuron");
                        P.JumpNeuron = P.Neurons.Find(X => X.Name == "JumpNeuron");

                        foreach (Axon AX in P.Axons)
                            AX.LoadAfterCreationFromXML(P);
                    }
                }
                catch (Exception e)
                {
                    if (DataStream != null)
                        DataStream.Close();
                    //MessageBox.Show("File: " + path + " is corrupted and will be terminated!");
                    //File.Delete(path);
                    MessageBox.Show(e.Message + "\n\n\n" + e.InnerException + "\n\n\n" + e.StackTrace);
                }
            }
        }

        public static void Update()
        {
            TestingLevel.Update();
        }
        public static void DrawProgress(SpriteBatch SB)
        {
            if (Best != null)
                DrawLevel.UpdateCameraPos(new Vector2(MaxFitness.Last(), Best.Rect.Y));
            else
                DrawLevel.UpdateCameraPos(new Vector2(MaxFitness.Last(), 0));
            DrawLevel.DrawWithoutPlayer(SB);

            if (Best != null)
                Best.DrawBrain(SB);

            // Lines
            SB.Draw(Assets.White, new Rectangle(MaxFitness.Last() + (int)DrawLevel.Camera.X, 0, 3, (int)Values.WindowSize.Y), Color.Gold);
            if (Best != null)
                SB.Draw(Assets.White, new Rectangle(MaxFitness.Last() + (int)DrawLevel.Camera.X - 10, Best.Rect.Y - 3, 23, 6), Color.Gold);
            SB.DrawString(Assets.Font, "MaxFitness: " + MaxFitness.Last().ToString(), new Vector2(MaxFitness.Last() + (int)DrawLevel.Camera.X + 5, Values.WindowSize.Y / 2 + 120), Color.Gold);
            SB.Draw(Assets.White, new Rectangle(AverageFitness.Last() + (int)DrawLevel.Camera.X, 0, 3, (int)Values.WindowSize.Y), Color.Black);
            SB.DrawString(Assets.Font, "AverageFitness: " + AverageFitness.Last().ToString(), new Vector2(AverageFitness.Last() + (int)DrawLevel.Camera.X + 5, Values.WindowSize.Y / 2 + 60), Color.Black);

            // Mutation
            SB.DrawString(Assets.Font, "Mutation:", new Vector2(12, 12), Color.White);
            SB.DrawString(Assets.Font, "Chance: " + Math.Round(AI_Player.MutationProbability, 3).ToString(), new Vector2(12, 12 + Assets.Font.MeasureString("[]").Y), Color.White);
            SB.DrawString(Assets.Font, "StepSize: " + Math.Round(AI_Player.MutationStepSize, 3).ToString(), new Vector2(12, 12 + Assets.Font.MeasureString("[]").Y * 2), Color.White);

            // Diagram
            Rectangle DiagramRect = new Rectangle((int)Values.WindowSize.X - 266, 17 + (int)Assets.Font.MeasureString("[]").Y, 254, 254);
            SB.DrawString(Assets.Font, "Progress:", new Vector2(DiagramRect.X, 12), Color.White);
            SB.Draw(Assets.White, new Rectangle(DiagramRect.X, DiagramRect.Y, DiagramRect.Width, DiagramRect.Height), Color.FromNonPremultiplied(59, 101, 61, 255));
            SB.Draw(Assets.White, new Rectangle(DiagramRect.X, DiagramRect.Y, 1, DiagramRect.Height), Color.Gray);
            SB.Draw(Assets.White, new Rectangle((int)Values.WindowSize.X - 12, DiagramRect.Y, 1, DiagramRect.Height), Color.Gray);
            SB.Draw(Assets.White, new Rectangle(DiagramRect.X, DiagramRect.Y, DiagramRect.Width, 1), Color.Gray);
            SB.Draw(Assets.White, new Rectangle(DiagramRect.X, DiagramRect.Y + DiagramRect.Height, DiagramRect.Width, 1), Color.Gray);
            float max = MaxFitness.Max();
            if (max != 0)
            {
                for (int i = 0; i < MaxFitness.Count - 1; i++)
                {
                    Assets.DrawLine(new Vector2(DiagramRect.X + 2 + (DiagramRect.Width - 2) * (i / (float)(MaxFitness.Count - 1)), 
                                        -MaxFitness[i] / max * (DiagramRect.Height - 2) + DiagramRect.Y + DiagramRect.Height),
                                    new Vector2(DiagramRect.X + 2 + (DiagramRect.Width - 2) * ((i + 1) / (float)(MaxFitness.Count - 1)), 
                                        -MaxFitness[i + 1] / max * (DiagramRect.Height - 2) + DiagramRect.Y + DiagramRect.Height), 1, Color.Gold, SB);
                }

                for (int i = 0; i < AverageFitness.Count - 1; i++)
                {
                    Assets.DrawLine(new Vector2(DiagramRect.X + 2 + (DiagramRect.Width - 2) * (i / (float)(AverageFitness.Count - 1)), 
                                        -AverageFitness[i] / max * (DiagramRect.Height - 2) + DiagramRect.Y + DiagramRect.Height),
                                    new Vector2(DiagramRect.X + 2 + (DiagramRect.Width - 2) * ((i + 1) / (float)(AverageFitness.Count - 1)), 
                                        -AverageFitness[i + 1] / max * (DiagramRect.Height - 2) + DiagramRect.Y + DiagramRect.Height), 1, Color.Black, SB);
                }
            }

            // Generation
            Vector2 StringSize = Assets.Font.MeasureString("Generation " + Generation.ToString() + ": " + ((int)(GenerationCompletionPercentage * 100)).ToString() + "%");
            SB.DrawString(Assets.Font, "Generation " + Generation.ToString() + ": " + (Math.Round(GenerationCompletionPercentage * 100, 2)).ToString() + "%", 
                new Vector2(Values.WindowSize.X / 2 - StringSize.X / 2, 30), Color.Black);
        }
        public static void Draw(SpriteBatch SB)
        {
            TestingLevel.Draw(SB);
        }
    }
}
