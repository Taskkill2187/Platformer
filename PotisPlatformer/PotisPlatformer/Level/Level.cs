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
using Platformer.Neural_Network;

namespace Platformer
{
    public static class LevelManager
    {
        
        public static bool Ending;
        public static bool SaidEndingPhrase;
        
        public static Level CurrentLevel;
        
        public static SpriteEffects FlipHorz = SpriteEffects.FlipHorizontally;
        public static SpriteEffects FlipVert = SpriteEffects.FlipVertically;

        public static void ClearEnemys()
        {
            CurrentLevel.EnemyList.Clear();
        }
        public static void LoadLevel(Level lvl)
        {
            lvl.Reset();
            CurrentLevel = lvl;
            CurrentLevel.ThisPlayer = new Player(CurrentLevel.PlayerPos, CurrentLevel);
            MenuManager.GS = GameState.InGame;
            CurrentLevel.Background = Assets.GetRDMBackground();
            UpdateTextures();
            MediaPlayer.Play(Assets.InGameTheme);
            Ending = false;
            CurrentLevel.Score = 0;
        }
        public static void UpdateTextures()
        {
            foreach (Block B in CurrentLevel.BlockList) { B.UpdateTextureState(); }
            foreach (Block B in CurrentLevel.BlockList0) { B.UpdateTextureState(); }
        }
        
        
        public static void Update()
        {
            CurrentLevel.Update();
        }
    }
    public class Level : ICloneable
    {
        public List<Block> BlockList = new List<Block>();
        public List<Block> BlockList0 = new List<Block>();
        public List<Enemy> EnemyList = new List<Enemy>();
        public List<Enemy> EnemyList0 = new List<Enemy>();
        public Vector2 End;
        public Player ThisPlayer;
        public bool Ending;
        public bool SaidEndingPhrase;
        public bool IsDisplayed;
        public bool GoToMainMenuAfterEnd = true;
        public int Score;
        public bool DebugMode;
        public Vector2 Camera = Vector2.Zero;
        public bool CameraFollowingOnYAxis;

        public static int BlockScale = 70;
        public static float EntitySize = 1.0f;
        public static int EndHeight = 2 * BlockScale;

        public Vector2 PlayerPos;
        public Texture2D Background;

        Player LastPlayerCopy;

        public Level()
        {
            BlockList = new List<Block>();
            EnemyList = new List<Enemy>();
            IsDisplayed = true;
        }
        public Level(Player ThisPlayer, bool IsDisplayed)
        {
            BlockList = new List<Block>();
            EnemyList = new List<Enemy>();
            this.ThisPlayer = ThisPlayer;
            this.IsDisplayed = IsDisplayed;
        }
        public Level(List<Block> BlockList, List<NPC> NPCList, List<Enemy> EnemyList, Vector2 PlaySpawnPos)
        {
            BlockList0 = BlockList;
            EnemyList0 = EnemyList;
            IsDisplayed = true;
        }
        
        public void Update()
        {
            ThisPlayer.Update();

            for (int i = 0; i < BlockList.Count; i++)
            {
                BlockList[i].Update();
            }

            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].Update();

                if (i < 0 || i >= EnemyList.Count)
                    break;

                // Enemy Death
                if (EnemyList[i].Rect.X - ThisPlayer.Rect.X < 100 && EnemyList[i].Rect.X - ThisPlayer.Rect.X > -100 &&
                        ThisPlayer.DeathTimer == 0 && EnemyList[i].Rect.Intersects(ThisPlayer.Rect))
                {
                    if (EnemyList[i].Rect.Y > ThisPlayer.Rect.Y + ThisPlayer.Rect.Height / 2 && ThisPlayer.Vel.Y > 0)
                    {
                        ThisPlayer.Jump(true);
                        ThisPlayer.Rect.Y = EnemyList[i].Rect.Y - ThisPlayer.Rect.Height;
                        EnemyList[i].OnDeath();
                    }
                    else
                    {
                        EnemyList[i].OnPlayerKill();
                    }
                }
            }

            // Ending Anim | executed multiple times
            if (ThisPlayer.Rect.X > End.X - ThisPlayer.Rect.Width)
            {
                if (ThisPlayer.Rect.Y > End.Y &&
                    ThisPlayer.Rect.Y < End.Y + 30)
                {
                    Ending = true;
                    ThisPlayer.CanMove = false;
                    ThisPlayer.Rect.X += (int)((Values.WindowSize.X / 2) / (Assets.LevelEnd.Duration.TotalSeconds * 5));
                    ThisPlayer.UpdateWalkAnim();
                }
                else
                    ThisPlayer.Rect.X = (int)End.X - ThisPlayer.Rect.Width;
            }

            // Ending Soundeffect | only exectued once
            if (Ending && !SaidEndingPhrase)
            {
                ThisPlayer.Rect.X = (int)End.X;
                SaidEndingPhrase = true;
                if (IsDisplayed)
                {
                    if (StoredData.Default.SoundEffects && GoToMainMenuAfterEnd)
                        Assets.LevelEnd.Play(0.65f, 0, 0);
                    MediaPlayer.Stop();
                    UpdateCameraPos();
                }
            }

            // Camera will stand still when the player is at the end of the level
            if (!Ending && IsDisplayed)
                UpdateCameraPos();

            // Level ended
            if (Ending && ThisPlayer.Rect.X > End.X + Values.WindowSize.X / 2)
            {
                if (IsDisplayed && GoToMainMenuAfterEnd)
                {
                    MenuManager.GS = GameState.MainMenu;
                    MediaPlayer.Stop();
                }
                EnemyList.Clear();
                Ending = false;
                SaidEndingPhrase = false;
            }

            if (Controls.CurKS.IsKeyDown(Keys.K) && Controls.LastKS.IsKeyUp(Keys.K))
            {
                switch (DebugMode)
                {
                    case true:
                        DebugMode = false;
                        Values.PlayerWalkAcceleration = 3.5f;
                        break;

                    case false:
                        DebugMode = true;
                        Values.PlayerWalkAcceleration = 30;
                        break;
                }
            }
        }
        public float GetNeuronValuesForTheseCoords(Point Coords)
        {
            Coords = new Point(Coords.X / BlockScale, Coords.Y / BlockScale);

            foreach (Enemy E in EnemyList)
                if (E.Rect.X / BlockScale == Coords.X && E.Rect.Y / BlockScale == Coords.Y ||
                    E.Rect.Height > BlockScale && E.Rect.X / BlockScale == Coords.X && E.Rect.Y / BlockScale + 1 == Coords.Y)
                    return -1;

            foreach (Block B in BlockList)
                if (B.Rect.X / BlockScale == Coords.X && B.Rect.Y / BlockScale == Coords.Y ||
                    B.Rect.Height > BlockScale && B.Rect.X / BlockScale == Coords.X && B.Rect.Y / BlockScale + 1 == Coords.Y)
                    return 1;
            
            return 0;
        }
        Neuron[,] GetNeuronValuesForTheseCoords(Point[,] Coords, Neuron[,] InputArray)
        {
            Neuron[,] NeuronArray = new Neuron[Coords.GetLength(0), Coords.GetLength(1)];

            for (int x = 0; x < Coords.GetLength(0); x++)
                for (int y = 0; y < Coords.GetLength(1); y++)
                {
                    NeuronArray[x, y] = new Neuron(InputArray[x, y].Pos);

                }

            return NeuronArray;
        }
        public bool NoBlockIntersectsThisRectangle(Rectangle Rect)
        {
            for (int i = 0; i < BlockList.Count; i++)
            {
                if (BlockList[i].Rect.Intersects(Rect))
                    return false;
            }
            return true;
        }

        public void UpdateCameraPos()
        {
            Camera.X = -ThisPlayer.Rect.X + Values.WindowSize.X / 2;

            if (Camera.X > 0)
                Camera.X = 0;

            if (CameraFollowingOnYAxis)
            {
                Camera.Y = -ThisPlayer.Rect.Y + Values.WindowSize.Y / 2;

                if (Camera.Y < 0)
                    Camera.Y = 0;
            }
        }
        public void UpdateCameraPos(Vector2 ManualTarget)
        {
            Camera.X = -ManualTarget.X + Values.WindowSize.X / 2;

            if (Camera.X > 0)
                Camera.X = 0;

            if (CameraFollowingOnYAxis)
            {
                Camera.Y = -ManualTarget.Y + Values.WindowSize.Y / 2;

                if (Camera.Y < 0)
                    Camera.Y = 0;
            }
        }
        public void Draw(SpriteBatch SB)
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].Draw(SB);
            }
            for (int i = 0; i < BlockList.Count; i++)
            {
                BlockList[i].Draw(SB);
            }
            ThisPlayer.Draw(SB);
            SB.Draw(Assets.EndPipe, new Rectangle((int)End.X + (int)Camera.X, (int)End.Y + (int)Camera.Y + 4, (int)(Assets.EndPipe.Width * 3.62666666666664f), EndHeight), Color.White);
            if (DebugMode)
            {
                SB.DrawString(Assets.Font, new Vector2((Controls.CurMS.X - (int)Camera.X) / BlockScale, (Controls.CurMS.Y - (int)Camera.Y) / BlockScale).ToString(), new Vector2(12, 12), Color.White);
            }
            SB.DrawString(Assets.Font, "Score: " + Score.ToString(), new Vector2(Values.WindowSize.X - 12 - Assets.Font.MeasureString("Score: " + Score.ToString()).X, 12), Color.White);
        }
        public void DrawWithoutPlayer(SpriteBatch SB)
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].Draw(SB);
            }
            for (int i = 0; i < BlockList.Count; i++)
            {
                BlockList[i].Draw(SB);
            }
            SB.Draw(Assets.EndPipe, new Rectangle((int)End.X + (int)Camera.X, (int)End.Y + (int)Camera.Y + 4, (int)(Assets.EndPipe.Width * 3.62666666666664f), EndHeight), Color.White);
            if (DebugMode)
            {
                SB.DrawString(Assets.Font, new Vector2((Controls.CurMS.X - (int)Camera.X) / BlockScale, (Controls.CurMS.Y - (int)Camera.Y) / BlockScale).ToString(), new Vector2(12, 12), Color.White);
            }
            SB.DrawString(Assets.Font, "Score: " + Score.ToString(), new Vector2(Values.WindowSize.X - 12 - Assets.Font.MeasureString("Score: " + Score.ToString()).X, 12), Color.White);
        }

        public void Reset()
        {
            if (ThisPlayer != null)
            {
                ThisPlayer.Rect = new Rectangle((int)ThisPlayer.RespawnPoint.X, (int)ThisPlayer.RespawnPoint.Y, BlockScale, (int)(BlockScale * 1.75f));
                ThisPlayer.Vel = Vector2.Zero;
                ThisPlayer.CanMove = true;
                ThisPlayer.MaxWalkSpeed = 0;
                ThisPlayer.Timer = 0;
                LastPlayerCopy = (Player)ThisPlayer.Clone();
            }
            Ending = false;
            SaidEndingPhrase = false;
            BlockList.Clear();
            foreach (Block B in BlockList0)
            {
                BlockList.Add((Block)B.Clone());
                BlockList.Last().Parent = this;
            }

            EnemyList.Clear();
            foreach (Enemy E in EnemyList0)
            {
                EnemyList.Add((Enemy)E.Clone());
                EnemyList.Last().Parent = this;
            }
            Score = 0;
        }
        public object Clone()
        {
            Level L = (Level)MemberwiseClone();
            L.BlockList0 = new List<Block>();
            BlockList = new List<Block>();
            foreach (Block B in BlockList0)
            {
                L.BlockList0.Add((Block)B.Clone());
                L.BlockList0.Last().Parent = L;
            }
            L.EnemyList0 = new List<Enemy>();
            EnemyList = new List<Enemy>();
            foreach (Enemy E in EnemyList0)
            {
                L.EnemyList0.Add((Enemy)E.Clone());
                L.EnemyList0.Last().Parent = L;
            }
            L.Reset();
            Reset();
            L.ThisPlayer = null;
            L.End = new Vector2(End.X, End.Y);
            L.Score = 0;
            return L;
        }
        public void SaveCurrentEntityConfig()
        {
            BlockList0.Clear();
            foreach (Block B in BlockList)
            {
                BlockList0.Add((Block)B.Clone());
            }
            EnemyList0.Clear();
            foreach (Enemy E in EnemyList)
            {
                EnemyList0.Add((Enemy)E.Clone());
            }
            Reset();
        }
    }
}
