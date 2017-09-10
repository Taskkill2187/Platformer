using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

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
        static int MaxFitness;
        static int AverageFitness;

        public static AI_Player Best;

        public static void CreateNewEvolution(Level TestingLevel, Vector2 RespawnPoint)
        {
            if (Population == null)
            {
                Population = new List<AI_Player>();
                MaxFitness = 0;
                AverageFitness = 0;
                Generation = 0;
                HasReachedTheEnd = false;
                AI_Player.MutationProbability = 0.5;
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
                AI_Player.MutationProbability = 0.4;
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

                while (true)
                {
                    LevelInstance.Update();
                    FitnessPerTick.Add(P.Rect.X);

                    if (P.DeathTimer > 0)
                        break;
                    if (FitnessPerTick.Count > 300 && P.Rect.X - FitnessPerTick[FitnessPerTick.Count - 300] < 50)
                        break;
                    if (LevelInstance.Ending == true)
                    {
                        HasReachedTheEnd = true;
                        break;
                    }
                }
                
                P.Fitness = P.Rect.X + P.Rect.Y / Values.WindowSize.Y / 6f;

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

                MaxFitness = (int)Population.Max(x => x.Fitness);
                AverageFitness = (int)Population.Average(x => x.Fitness);
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
                AI_Player B = Population[i];
                if (Population[i].Fitness == Population.Max(x => x.Fitness))
                    A.Clone();
                A.Clone();
                Population.Add(A);
            }

            // Last Steps
            AI_Player.MutationProbability *= 0.985f;
            if (AI_Player.MutationProbability < 0.1)
                AI_Player.MutationProbability = 0.1;

            //Console.WriteLine("Generation " + Generation + " finalized!");

            Generation++;
        }

        public static void PutBestOneIntoTestingLevel()
        {
            Best = Population.Find(x => x.Fitness == Population.Max(y => y.Fitness));
            TestingLevel = (Level)TestingLevel.Clone();
            Best.AssignNewWorld(TestingLevel);
            TestingLevel.IsDisplayed = true;
            TestingLevel.GoToMainMenuAfterEnd = false;
        }

        public static void Update()
        {
            TestingLevel.Update();
        }
        public static void DrawProgress(SpriteBatch SB)
        {
            DrawLevel.UpdateCameraPos(new Vector2(MaxFitness, 0));
            DrawLevel.DrawWithoutPlayer(SB);

            if (Best != null)
                Best.DrawBrain(SB);

            SB.Draw(Assets.White, new Rectangle(MaxFitness + (int)DrawLevel.Camera.X, 0, 3, (int)Values.WindowSize.Y), Color.Gold);
            if (Best != null)
                SB.Draw(Assets.White, new Rectangle(MaxFitness + (int)DrawLevel.Camera.X - 10, Best.Rect.Y - 3, 23, 6), Color.Gold);
            SB.DrawString(Assets.Font, "MaxFitness: " + MaxFitness.ToString(), new Vector2(MaxFitness + (int)DrawLevel.Camera.X + 5, Values.WindowSize.Y / 2 + 30), Color.Gold);
            SB.Draw(Assets.White, new Rectangle(AverageFitness + (int)DrawLevel.Camera.X, 0, 3, (int)Values.WindowSize.Y), Color.Black);
            SB.DrawString(Assets.Font, "AverageFitness: " + AverageFitness.ToString(), new Vector2(AverageFitness + (int)DrawLevel.Camera.X + 5, Values.WindowSize.Y / 2 - 30), Color.Black);

            Vector2 StringSize = Assets.Font.MeasureString("Generation " + Generation.ToString() + ": " + ((int)(GenerationCompletionPercentage * 100)).ToString() + "%");
            SB.DrawString(Assets.Font, "Generation " + Generation.ToString() + ": " + ((int)(GenerationCompletionPercentage * 100)).ToString() + "%", 
                new Vector2(Values.WindowSize.X / 2 - StringSize.X / 2, 30), Color.Black);
        }
        public static void Draw(SpriteBatch SB)
        {
            TestingLevel.Draw(SB);
        }
    }
}
