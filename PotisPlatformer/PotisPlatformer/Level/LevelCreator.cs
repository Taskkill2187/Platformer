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
using System.Reflection;
using Platformer.Neural_Network;
using System.Threading.Tasks;

namespace Platformer
{
    public static class LevelCreator
    {
        public static bool Testing;
        public static bool AIPlayerPlaying;
        public static bool AIPlayerLearning;
        public static Level CurrentLevel = CreateNewLevel();
        public static Menu _Menu = CreateMenu();
        public static Entity DraggedEntity;
        public static List<Entity> SelectedEntities = new List<Entity>();
        public static List<Entity> Copy = new List<Entity>();
        public static Player ThisPlayer = new Player(new Vector2(), CurrentLevel);
        static Rectangle SelectionRect;
        static int SnapRange = 15;
        public static bool KillEvolutionThread;
        static bool WindowHasBeenShownAfterFinishedEvolution = false;

        static Menu CreateMenu()
        {
            Menu M = new Menu();
            // Create the Get-New-Entity-Buttons
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.GrassFront,     Assets.GrassFront));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.Brick,          Assets.Brick));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.QuestionMarkBlock,Assets.QuestionMarkBlock));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.Coin,           Assets.Coin));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.SpinningBlock,  Assets.SpinningBlock));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.JumpBlock,      Assets.JumpBlock));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.GrassFront,     Assets.GrassFront));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.KugelWilli,     Assets.KugelWilli));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.WalkerFront,    Assets.WalkerFront));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.KoopaFront,     Assets.KoopaFront));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, Level.BlockScale, Level.BlockScale), Assets.Bob_omb,        Assets.Bob_omb));
            M.ArrangeButtons(MenuButtonLayout.TopHorz);
            int i = 0;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new Block(Assets.BlockGrass, Controls.GetMouseVector() - CurrentLevel.Camera, true, CurrentLevel));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new Brick(Controls.GetMouseVector() - CurrentLevel.Camera, CurrentLevel));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new QuestionMarkBlock(Controls.GetMouseVector() - CurrentLevel.Camera, new Coin(Vector2.Zero, CurrentLevel), CurrentLevel));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new Coin(Controls.GetMouseVector() - CurrentLevel.Camera, CurrentLevel));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new SpinningBlock(Controls.GetMouseVector() - CurrentLevel.Camera, CurrentLevel));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new JumpBlock(Controls.GetMouseVector() - CurrentLevel.Camera, Direction.Up, 10, CurrentLevel));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new FallingBlock(Controls.GetMouseVector() - CurrentLevel.Camera, true, CurrentLevel));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new Kugelwilli_Spawner(Controls.GetMouseVector() - CurrentLevel.Camera, CurrentLevel));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.EnemyList.Add(new Goomba((int)Controls.GetMouseVector().X - (int)CurrentLevel.Camera.X, (int)Controls.GetMouseVector().Y - (int)CurrentLevel.Camera.Y, false, CurrentLevel));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.EnemyList.Add(new Koopa((int)Controls.GetMouseVector().X - (int)CurrentLevel.Camera.X, (int)Controls.GetMouseVector().Y - (int)CurrentLevel.Camera.Y, KoopaColor.Green, CurrentLevel));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.EnemyList.Add(new Bob_omb((int)Controls.GetMouseVector().X - (int)CurrentLevel.Camera.X, (int)Controls.GetMouseVector().Y - (int)CurrentLevel.Camera.Y, CurrentLevel));
            };
            i++;
            // Add the Save-Button
            M.ControlElementList.Add(new Button("Save", new Vector2(Values.WindowSize.X - 100, Values.WindowSize.Y - 100), Color.White, Assets.BigFont));
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => { SaveCurrentLevel(); };
            i++;
            // Add the AI-Button
            M.ControlElementList.Add(new Button("AI", new Vector2(100, Values.WindowSize.Y - 100), Color.White, Assets.BigFont));
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => { AIlevelSolving(); };
            return M;
        }

        private static void AIlevelSolving()
        {
            AIPlayerLearning = true;
            CurrentLevel.SaveCurrentEntityConfig();
            Evolution_Manager.CreateNewEvolution(CurrentLevel, new Vector2(ThisPlayer.Rect.X, ThisPlayer.Rect.Y));
            WindowHasBeenShownAfterFinishedEvolution = false;

            Task.Factory.StartNew(() =>
            {
                while (!Evolution_Manager.HasReachedTheEnd)
                {
                    Evolution_Manager.TestCurrentGeneration();

                    if (KillEvolutionThread)
                        break;

                    Evolution_Manager.FinallizeGeneration();

                    if (KillEvolutionThread)
                        break;
                }
                
                if (!KillEvolutionThread)
                {
                    Evolution_Manager.PutBestOneIntoTestingLevel();
                    AIPlayerPlaying = true;
                    AIPlayerLearning = false;
                }

                KillEvolutionThread = false;
            });
        }
        public static Level CreateNewLevel()
        {
            Level L = new Level();
            L.End = new Vector2(30 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale - Level.EndHeight);
            for (int i = 0; i < L.End.X / Level.BlockScale + 10; i++)
            {
                L.BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(i * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale), true, L));
            }
            /*for (int i = 0; i < 500; i++)
            {
                L.BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(i * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2 - LevelManager.ThisPlayer.Rect.Height), true));
            }*/
            L.Reset();
            return L;
        }
        public static void SaveCurrentLevel()
        {
            CurrentLevel.SaveCurrentEntityConfig();
            LevelDataStorage.LvlList.Add((Level)CurrentLevel.Clone());
            MenuManager.BuildMenus();

            try
            {
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Level));
                int Number = 0;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//PlatformerLevels//Level" + (Number + 1).ToString() + ".xml";
                string folderpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//PlatformerLevels";

                if (System.IO.Directory.Exists(folderpath))
                    Number = System.IO.Directory.GetFiles(folderpath).Length;
                else
                    System.IO.Directory.CreateDirectory(folderpath);
                
                System.IO.FileStream DataStream = System.IO.File.Create(path);
                writer.Serialize(DataStream, CurrentLevel);
                DataStream.Close();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n\n\n" + e.InnerException + "\n\n\n" + e.StackTrace);
            }

            //CurrentLevel = CreateNewLevel();
        }
        static Entity GetMouseHoveredElement()
        {
            Rectangle M = Controls.GetMouseRect();
            M.X -= (int)CurrentLevel.Camera.X;
            M.Y -= (int)CurrentLevel.Camera.Y;
            if (ThisPlayer.Rect.Intersects(M))
            {
                return ThisPlayer;
            }

            foreach (Block B in CurrentLevel.BlockList)
            {
                if (B.Rect.Intersects(M))
                    return B;
            }

            foreach (Enemy E in CurrentLevel.EnemyList)
            {
                if (E.Rect.Intersects(M))
                    return E;
            }

            return null;
        }
        static void SnapEntityToBlocks(Entity E)
        {
            for (int i = 0; i < CurrentLevel.BlockList.Count; i++)
            {
                if (E != CurrentLevel.BlockList[i])
                {
                    Rectangle R = Values.CopyRectangle(CurrentLevel.BlockList[i].Rect);
                    if (E.Rect.X > R.X - SnapRange && E.Rect.X < R.X + SnapRange)
                    {
                        E.Rect.X = R.X;
                    }
                    if (E.Rect.X > R.X + R.Width - SnapRange && E.Rect.X < R.X + R.Width + SnapRange)
                    {
                        E.Rect.X = R.X + R.Width;
                    }
                    if (E.Rect.X + E.Rect.Width > R.X - SnapRange && E.Rect.X + E.Rect.Width < R.X + SnapRange)
                    {
                        E.Rect.X = R.X - E.Rect.Width;
                    }

                    if (E.Rect.Y > R.Y - SnapRange && E.Rect.Y < R.Y + SnapRange)
                    {
                        E.Rect.Y = R.Y;
                    }
                    if (E.Rect.Y > R.Y + R.Height - SnapRange && E.Rect.Y < R.Y + R.Height + SnapRange)
                    {
                        E.Rect.Y = R.Y + R.Height;
                    }
                    if (E.Rect.Y + E.Rect.Height > R.Y - SnapRange && E.Rect.Y + E.Rect.Height < R.Y + SnapRange)
                    {
                        E.Rect.Y = R.Y - E.Rect.Height;
                    }
                }
            }
        }
        static void HandleCameraControls()
        {
            if (Controls.CurKS.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                CurrentLevel.Camera.X += 10;

            if (Controls.CurKS.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                CurrentLevel.Camera.X -= 10;

            if (CurrentLevel.Camera.X > 200)
                CurrentLevel.Camera.X = 200;

            if (CurrentLevel.Camera.X < -CurrentLevel.End.X + (int)Values.WindowSize.X - 200)
                CurrentLevel.Camera.X = -CurrentLevel.End.X + (int)Values.WindowSize.X - 200;
        }
        static void DeleteAllSelectedItems()
        {
            for (int i = 0; i < SelectedEntities.Count; i++)
            {
                try { CurrentLevel.BlockList.Remove((Block)SelectedEntities[i]); } catch { }
                try { CurrentLevel.EnemyList.Remove((Enemy)SelectedEntities[i]); } catch { }
            }
            SelectedEntities.Clear();
        }
        public static void UpdateTextures()
        {
            LevelManager.CurrentLevel = CurrentLevel;
            LevelManager.UpdateTextures();
        }
        static void SwitchMainPropertiesOfAllSelectedItems()
        {
            for (int i = 0; i < SelectedEntities.Count; i++)
            {
                if (SelectedEntities[i].GetType() == typeof(JumpBlock))
                {
                    switch (((JumpBlock)SelectedEntities[i]).Direction)
                    {
                        case Direction.Up:
                            ((JumpBlock)SelectedEntities[i]).Direction = Direction.Right;
                            break;

                        case Direction.Right:
                            ((JumpBlock)SelectedEntities[i]).Direction = Direction.Down;
                            break;

                        case Direction.Down:
                            ((JumpBlock)SelectedEntities[i]).Direction = Direction.Left;
                            break;

                        case Direction.Left:
                            ((JumpBlock)SelectedEntities[i]).Direction = Direction.Up;
                            break;
                    }
                }
                else if(SelectedEntities[i].GetType() == typeof(Block))
                {
                    ((Block)SelectedEntities[i]).Collision = !((Block)SelectedEntities[i]).Collision;
                }
                else if (SelectedEntities[i].GetType() == typeof(Enemy))
                {
                    ((Enemy)SelectedEntities[i]).FacingRight = !((Enemy)SelectedEntities[i]).FacingRight;
                }
            }
        }
        static void UpdateSelectionRect()
        {
            SelectionRect.Width = Controls.CurMS.X - SelectionRect.X;
            SelectionRect.Height = Controls.CurMS.Y - SelectionRect.Y;

            Rectangle R = new Rectangle(SelectionRect.X - (int)CurrentLevel.Camera.X, SelectionRect.Y - (int)CurrentLevel.Camera.Y, SelectionRect.Width, SelectionRect.Height);

            SelectedEntities.Clear();
            for (int i = 0; i < CurrentLevel.BlockList.Count; i++)
            {
                if (CurrentLevel.BlockList[i].Rect.Intersects(R)) { SelectedEntities.Add(CurrentLevel.BlockList[i]); }
            }
            for (int i = 0; i < CurrentLevel.EnemyList.Count; i++)
            {
                if (CurrentLevel.EnemyList[i].Rect.Intersects(R)) { SelectedEntities.Add(CurrentLevel.EnemyList[i]); }
            }
            if (ThisPlayer.Rect.Intersects(R)) { SelectedEntities.Add(ThisPlayer); }
        }
        static void CopySelectedElements()
        {
            Copy = new List<Entity>();
            foreach (Entity E in SelectedEntities)
            {
                if (E.GetType() == typeof(Enemy))
                {
                    Copy.Add((Enemy)E.Clone());
                }
                else if (E.GetType() == typeof(Block))
                {
                    Copy.Add((Block)E.Clone());
                }
                else if (E.GetType() == typeof(FallingBlock))
                {
                    Copy.Add((FallingBlock)E.Clone());
                }
                else if (E.GetType() == typeof(JumpBlock))
                {
                    Copy.Add((JumpBlock)E.Clone());
                }
            }
        }
        static void PasteCopiedElements()
        {
            SelectedEntities.Clear();
            DraggedEntity = null;
            foreach (Entity E in Copy)
            {
                if (E.GetType() == typeof(Enemy))
                {
                    CurrentLevel.EnemyList.Add((Enemy)E);
                }
                else if (E.GetType() == typeof(Block))
                {
                    CurrentLevel.BlockList.Add((Block)E);
                }
                else if (E.GetType() == typeof(FallingBlock))
                {
                    CurrentLevel.BlockList.Add((FallingBlock)E);
                }
                else if (E.GetType() == typeof(JumpBlock))
                {
                    CurrentLevel.BlockList.Add((JumpBlock)E);
                }
                SelectedEntities.Add(E);
            }
            Copy.Clear();
        }
        static void StartTesting()
        {
            CurrentLevel.SaveCurrentEntityConfig();
            LevelManager.CurrentLevel = CurrentLevel;
            CurrentLevel.ThisPlayer = new Player(new Vector2(ThisPlayer.Rect.X, ThisPlayer.Rect.Y), CurrentLevel);
            LevelManager.Ending = false;
            LevelManager.SaidEndingPhrase = false;
            LevelManager.CurrentLevel.PlayerPos = new Vector2(ThisPlayer.Rect.X, ThisPlayer.Rect.Y);
            CurrentLevel.ThisPlayer.Vel = Vector2.Zero;
            LevelManager.UpdateTextures();
            Testing = true;
            MediaPlayer.Play(Assets.InGameTheme);
        }
        static void StopTesting()
        {
            CurrentLevel.Reset();
            CurrentLevel.Camera = ThisPlayer.RespawnPoint;
            //LevelManager.ClearEnemys();
            Testing = false;
            CurrentLevel.Ending = false;
            ParticleManager.Clear();
            MediaPlayer.Stop();
            CurrentLevel.Score = 0;
        }
        
        public static void Update()
        {
            if (!AIPlayerLearning)
            {
                if (Testing)
                {
                    LevelManager.Update();

                    if (CurrentLevel.Ending)
                        StopTesting();

                    if (Controls.WasKeyJustPressed(Keys.T))
                        StopTesting();
                }
                else
                {
                    if (AIPlayerPlaying)
                    {
                        if (Controls.WasKeyJustPressed(Keys.T) || Controls.WasKeyJustPressed(Keys.Escape))
                            AIPlayerPlaying = false;

                        if (!WindowHasBeenShownAfterFinishedEvolution)
                        {
                            Values.ShowWindow(XNA.gameWindowForm.Handle, 5);
                            WindowHasBeenShownAfterFinishedEvolution = true;
                        }

                        if (Controls.WasKeyJustPressed(Keys.R) || Evolution_Manager.TestingLevel.Ending)
                        {
                            Evolution_Manager.TestingLevel.Reset();
                            Evolution_Manager.TestingLevel.ThisPlayer = Evolution_Manager.Best;
                        }

                        Evolution_Manager.Update();
                    }
                    else
                    {
                        if (Controls.WasKeyJustPressed(Keys.T) || Controls.WasKeyJustPressed(Keys.Tab))
                            StartTesting();

                        HandleCameraControls();

                        if (Controls.CurKS.IsKeyDown(Keys.LeftControl) && Controls.WasKeyJustPressed(Keys.C))
                            CopySelectedElements();

                        if (Controls.CurKS.IsKeyDown(Keys.LeftControl) && Controls.WasKeyJustPressed(Keys.V))
                            PasteCopiedElements();

                        if (Controls.CurKS.IsKeyDown(Keys.X))
                            DeleteAllSelectedItems();

                        _Menu.Update();

                        // Updating DraggedEntity + SelectedEntities and Creating SelectionRect if needed
                        if (Controls.WasLMBJustPressed())
                        {
                            DraggedEntity = GetMouseHoveredElement();
                            if (DraggedEntity != null)
                            {
                                DraggedEntity.CreatorClickPos = new Point(DraggedEntity.Rect.X - Controls.CurMS.X + (int)CurrentLevel.Camera.X,
                                    DraggedEntity.Rect.Y - Controls.CurMS.Y + (int)CurrentLevel.Camera.Y);

                                if (!Controls.CurKS.IsKeyDown(Keys.LeftControl) && !SelectedEntities.Contains(DraggedEntity))
                                    SelectedEntities.Clear();

                                for (int i = 0; i < SelectedEntities.Count; i++)
                                {
                                    if (SelectedEntities[i] != DraggedEntity)
                                        SelectedEntities[i].CreatorClickPos = new Point(SelectedEntities[i].Rect.X - DraggedEntity.Rect.X + (int)CurrentLevel.Camera.X,
                                            SelectedEntities[i].Rect.Y - DraggedEntity.Rect.Y + (int)CurrentLevel.Camera.Y);
                                }

                                SelectedEntities.Add(DraggedEntity);
                            }
                            else
                            {
                                SelectedEntities.Clear();
                                SelectionRect = Controls.GetMouseRect();
                            }
                        }

                        if (SelectionRect.X != 0 && SelectionRect.Y != 0)
                            UpdateSelectionRect();

                        if (Controls.WasRMBJustPressed())
                            SwitchMainPropertiesOfAllSelectedItems();

                        if (Controls.WasLMBJustReleased())
                        {
                            DraggedEntity = null;
                            UpdateTextures();
                            SelectionRect = new Rectangle();
                        }

                        // Update Positions of the DraggedEntity + SelectedEntities
                        if (DraggedEntity != null)
                        {
                            DraggedEntity.Rect = new Rectangle(Controls.CurMS.X - (int)CurrentLevel.Camera.X + DraggedEntity.CreatorClickPos.X,
                                Controls.CurMS.Y - (int)CurrentLevel.Camera.Y + DraggedEntity.CreatorClickPos.Y, DraggedEntity.Rect.Width, DraggedEntity.Rect.Height);

                            SnapEntityToBlocks(DraggedEntity);

                            if (DraggedEntity.Rect.X < 0)
                                DraggedEntity.Rect.X = 0;

                            for (int i = 0; i < SelectedEntities.Count; i++)
                            {
                                // SelectedEntities will be positioned relative to the DraggedEntity
                                if (SelectedEntities[i] != DraggedEntity)
                                    SelectedEntities[i].Rect = new Rectangle(DraggedEntity.Rect.X - (int)CurrentLevel.Camera.X + SelectedEntities[i].CreatorClickPos.X,
                                        DraggedEntity.Rect.Y - (int)CurrentLevel.Camera.Y + SelectedEntities[i].CreatorClickPos.Y, SelectedEntities[i].Rect.Width,
                                        SelectedEntities[i].Rect.Height);

                                if (SelectedEntities[i].Rect.X < 0)
                                    SelectedEntities[i].Rect.X = 0;
                            }
                        }
                    }
                }
            }
        }
        public static void Draw(SpriteBatch SB)
        {
            if (Testing)
            {
                CurrentLevel.Draw(SB);
            }
            else if (AIPlayerLearning)
            {
                if (Controls.WasKeyJustPressed(Keys.T))
                {
                    KillEvolutionThread = true;
                    while (KillEvolutionThread) { }
                    AIPlayerPlaying = false;
                    AIPlayerLearning = false;
                }
                Evolution_Manager.DrawProgress(SB);
            }
            else if (AIPlayerPlaying)
            {
                Evolution_Manager.Draw(SB);
            }
            else
            {
                // Draw Selected Entity Outline
                for (int i = 0; i < SelectedEntities.Count; i++)
                {
                    Rectangle R = Values.CopyRectangle(SelectedEntities[i].Rect);
                    R.X += (int)CurrentLevel.Camera.X;
                    R.Inflate(5, 5);
                    //SB.Draw(Assets.White, R, Color.Black * 0.5f);
                    Assets.DrawRoundedRectangle(R, 20, Color.Black, SB);
                }

                // Draw X Zero - Red Zone
                SB.Draw(Assets.White, new Rectangle(0, 0, (int)CurrentLevel.Camera.X, (int)Values.WindowSize.Y), Color.Red * 0.25f);

                // Draw X End - Red Zone
                SB.Draw(Assets.White, new Rectangle((int)CurrentLevel.End.X + (int)CurrentLevel.Camera.X, 0, (int)CurrentLevel.End.X - (int)CurrentLevel.Camera.X, 
                    (int)Values.WindowSize.Y), Color.Red * 0.25f);

                // Draw all Entities
                for (int i = 0; i < CurrentLevel.BlockList.Count; i++)
                {
                    if (CurrentLevel.BlockList[i].Collision || CurrentLevel.BlockList[i].GetType() == typeof(JumpBlock))
                    {
                        CurrentLevel.BlockList[i].Draw(SB);
                    }
                    else
                    {
                        SB.Draw(CurrentLevel.BlockList[i].Texture, new Rectangle(CurrentLevel.BlockList[i].Rect.X + (int)CurrentLevel.Camera.X, 
                            CurrentLevel.BlockList[i].Rect.Y + (int)CurrentLevel.Camera.Y, CurrentLevel.BlockList[i].Rect.Width, 
                            CurrentLevel.BlockList[i].Rect.Height), Color.White * 0.5f);
                    }
                }
                for (int i = 0; i < CurrentLevel.EnemyList.Count; i++)
                {
                    if (CurrentLevel.EnemyList[i].FacingRight)
                    {
                        CurrentLevel.EnemyList[i].Draw(SB, Color.White, SpriteEffects.FlipHorizontally);
                    }
                    else
                    {
                        CurrentLevel.EnemyList[i].Draw(SB);
                    }
                }
                ThisPlayer.Draw(SB);

                // Draw HUD
                SB.Draw(Assets.White, SelectionRect, Color.Black * 0.5f);
                int Width = _Menu.ControlElementList.Count * 100;
                Assets.DrawRoundedRectangle(new Rectangle((int)Values.WindowSize.X / 2 - Width / 2, 100 - Width, Width, Width), 30, Color.Black, SB);
                for (int i = 0; i < _Menu.ControlElementList.Count; i++) { _Menu.ControlElementList[i].Draw(SB); }
            }
        }
    }
}
