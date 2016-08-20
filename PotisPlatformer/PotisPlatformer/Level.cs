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

namespace Platformer
{
    public static class LevelManager
    {
        public static int BlockScale = 70;
        public static float EntitySize = 1.0f;
        public static bool Ending;
        public static bool SaidEndingPhrase;
        public static bool CameraFollowingOnYAxis;
        public static bool DebugMode;
        public static Level CurrentLevel;
        public static Vector2 Camera = Vector2.Zero;
        public static Player ThisPlayer = new Player(Vector2.Zero);
        public static int Score;
        public static int EndHeight = 2 * BlockScale;

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
            ThisPlayer = new Player(CurrentLevel.PlayerPos);
            MenuManager.GS = GameState.InGame;
            CurrentLevel.Background = Assets.GetRDMBackground();
            UpdateTextures();
            MediaPlayer.Play(Assets.InGameTheme);
            Ending = false;
            Score = 0;
        }
        public static void UpdateTextures()
        {
            foreach (Block B in CurrentLevel.BlockList) { B.UpdateTextureState(); }
            foreach (Block B in CurrentLevel.BlockList0) { B.UpdateTextureState(); }
        }
        public static bool NoBlockIntersectsThisRectangle(Rectangle Rect)
        {
            for (int i = 0; i < CurrentLevel.BlockList.Count; i++)
            {
                if (CurrentLevel.BlockList[i].Rect.Intersects(Rect))
                    return true;
            }
            return false;
        }

        public static void Update()
        {
            ThisPlayer.Update();

            for (int i = 0; i < CurrentLevel.BlockList.Count; i++)
            {
                CurrentLevel.BlockList[i].Update();
            }

            for (int i = 0; i < CurrentLevel.EnemyList.Count; i++)
            {
                CurrentLevel.EnemyList[i].Update();

                if (i < 0 || i >= CurrentLevel.EnemyList.Count)
                    break;

                // Enemy Death
                if (CurrentLevel.EnemyList[i].Rect.X - ThisPlayer.Rect.X < 100 && CurrentLevel.EnemyList[i].Rect.X - ThisPlayer.Rect.X > -100 &&
                        ThisPlayer.DeathTimer == 0 && CurrentLevel.EnemyList[i].Rect.Intersects(ThisPlayer.Rect))
                {
                    if (CurrentLevel.EnemyList[i].Rect.Y > ThisPlayer.Rect.Y + ThisPlayer.Rect.Height / 2 && ThisPlayer.Vel.Y > 0)
                    {
                        ThisPlayer.Jump(true);
                        CurrentLevel.EnemyList[i].OnDeath();
                    }
                    else
                    {
                        CurrentLevel.EnemyList[i].OnPlayerKill();
                    }
                }
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

            // Ending Anim | executed multiple times
            if (ThisPlayer.Rect.X > CurrentLevel.End.X - ThisPlayer.Rect.Width)
            {
                if (ThisPlayer.Rect.Y > CurrentLevel.End.Y &&
                    ThisPlayer.Rect.Y < CurrentLevel.End.Y + 30)
                {
                    Ending = true;
                    ThisPlayer.CanMove = false;
                    ThisPlayer.Rect.X += (int)((Values.WindowSize.X / 2) / (Assets.LevelEnd.Duration.TotalSeconds * 60));
                    ThisPlayer.UpdateWalkAnim();
                }
                else
                {
                    ThisPlayer.Rect.X = (int)CurrentLevel.End.X - ThisPlayer.Rect.Width;
                }
            }

            // Ending Soundeffect | only exectued once
            if (Ending && !SaidEndingPhrase)
            {
                ThisPlayer.Rect.X = (int)CurrentLevel.End.X;
                SaidEndingPhrase = true;
                if (StoredData.Default.SoundEffects)
                    Assets.LevelEnd.Play(0.75f, 0, 0);
                MediaPlayer.Stop();
                UpdateCameraPos();
            }

            // Camera will stand still when the player is at the end of the level
            if (!Ending)
                UpdateCameraPos();

            // Level ended
            if (Ending && ThisPlayer.Rect.X > CurrentLevel.End.X + Values.WindowSize.X / 2)
            {
                MenuManager.GS = GameState.MainMenu;
                MediaPlayer.Stop();
                ClearEnemys();
                Ending = false;
                SaidEndingPhrase = false;
            }
        }
        public static void UpdateCameraPos()
        {
            Camera.X = -(float)ThisPlayer.Rect.X + Values.WindowSize.X / 2;

            if (Camera.X > 0)
                Camera.X = 0;

            if (CameraFollowingOnYAxis)
            {
                Camera.Y = -ThisPlayer.Rect.Y + Values.WindowSize.Y / 2;

                if (Camera.Y < 0)
                    Camera.Y = 0;
            }
        }

        public static void Draw(SpriteBatch SB)
        {
            for (int i = 0; i < CurrentLevel.EnemyList.Count; i++)
            {
                CurrentLevel.EnemyList[i].Draw(SB);
            }
            for (int i = 0; i < CurrentLevel.BlockList.Count; i++)
            {
                CurrentLevel.BlockList[i].Draw(SB);
            }
            ThisPlayer.Draw(SB);
            SB.Draw(Assets.EndPipe, new Rectangle((int)CurrentLevel.End.X + (int)Camera.X, (int)CurrentLevel.End.Y + (int)Camera.Y + 4, (int)(Assets.EndPipe.Width * 3.62666666666664f), EndHeight), Color.White);
            if (DebugMode)
            {
                SB.DrawString(Assets.Font, new Vector2((Controls.CurMS.X - (int)Camera.X) / BlockScale, (Controls.CurMS.Y - (int)Camera.Y) / BlockScale).ToString(), new Vector2(12, 12), Color.White);
            }
            SB.DrawString(Assets.Font, "Score: " + Score.ToString(), new Vector2(Values.WindowSize.X - 12 - Assets.Font.MeasureString("Score: " + Score.ToString()).X, 12), Color.White);
        }
    }
    public class Level : ICloneable
    {
        public List<Block> BlockList = new List<Block>();
        public List<Block> BlockList0 = new List<Block>();
        public List<Enemy> EnemyList = new List<Enemy>();
        public List<Enemy> EnemyList0 = new List<Enemy>();
        public Vector2 End;
        
        public Vector2 PlayerPos;
        public Texture2D Background;

        public Level()
        {
            BlockList = new List<Block>();
            EnemyList = new List<Enemy>();
        }
        public Level(List<Block> BlockList, List<NPC> NPCList, List<Enemy> EnemyList, Vector2 PlaySpawnPos)
        {
            BlockList0 = BlockList;
            EnemyList0 = EnemyList;
        }
        
        public void Reset()
        {
            BlockList.Clear();
            foreach (Block B in BlockList0)
            {
                BlockList.Add((Block)B.Clone());
            }

            EnemyList.Clear();
            foreach (Enemy E in EnemyList0)
            {
                EnemyList.Add((Enemy)E.Clone());
            }
        }
        public object Clone()
        {
            Level L = (Level)MemberwiseClone();
            L.BlockList0 = new List<Block>();
            BlockList = new List<Block>();
            foreach (Block B in BlockList0)
            {
                L.BlockList0.Add((Block)B.Clone());
            }
            L.EnemyList0 = new List<Enemy>();
            EnemyList = new List<Enemy>();
            foreach (Enemy E in EnemyList0)
            {
                L.EnemyList0.Add((Enemy)E.Clone());
            }
            L.Reset();
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
