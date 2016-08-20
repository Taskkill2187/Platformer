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

namespace Platformer
{
    public static class LevelCreator
    {
        public static bool Testing;
        public static Level CurrentLevel = CreateNewLevel();
        public static Menu _Menu = CreateMenu();
        public static Entity DraggedEntity;
        public static List<Entity> SelectedEntities = new List<Entity>();
        public static List<Entity> Copy = new List<Entity>();
        public static Player ThisPlayer = new Player(new Vector2());
        static Rectangle SelectionRect;
        static int SnapRange = 15;

        static Menu CreateMenu()
        {
            Menu M = new Menu();
            // Create the Get-New-Entity-Buttons
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.GrassFront,     Assets.GrassFront));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.Brick,          Assets.Brick));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.QuestionMarkBlock,Assets.QuestionMarkBlock));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.Coin,           Assets.Coin));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.SpinningBlock,  Assets.SpinningBlock));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.JumpBlock,      Assets.JumpBlock));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.GrassFront,     Assets.GrassFront));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.KugelWilli,     Assets.KugelWilli));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.WalkerFront,    Assets.WalkerFront));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.KoopaFront,     Assets.KoopaFront));
            M.ControlElementList.Add(new Button("", new Rectangle(0, 0, LevelManager.BlockScale, LevelManager.BlockScale), Assets.Bob_omb,        Assets.Bob_omb));
            M.ArrangeButtons(MenuButtonLayout.TopHorz);
            int i = 0;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new Block(Assets.BlockGrass, Controls.GetMouseVector() - LevelManager.Camera, true));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new Brick(Controls.GetMouseVector() - LevelManager.Camera));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new QuestionMarkBlock(Controls.GetMouseVector() - LevelManager.Camera, new Coin(Vector2.Zero)));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new Coin(Controls.GetMouseVector() - LevelManager.Camera));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new SpinningBlock(Controls.GetMouseVector() - LevelManager.Camera));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new JumpBlock(Controls.GetMouseVector() - LevelManager.Camera, Direction.Up, 10));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new FallingBlock(Controls.GetMouseVector() - LevelManager.Camera, true));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.BlockList.Add(new Kugelwilli_Spawner(Controls.GetMouseVector() - LevelManager.Camera));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.EnemyList.Add(new Goomba((int)Controls.GetMouseVector().X - (int)LevelManager.Camera.X, (int)Controls.GetMouseVector().Y - (int)LevelManager.Camera.Y, false));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.EnemyList.Add(new Koopa((int)Controls.GetMouseVector().X - (int)LevelManager.Camera.X, (int)Controls.GetMouseVector().Y - (int)LevelManager.Camera.Y, KoopaColor.Green));
            };
            i++;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => {
                CurrentLevel.EnemyList.Add(new Bob_omb((int)Controls.GetMouseVector().X - (int)LevelManager.Camera.X, (int)Controls.GetMouseVector().Y - (int)LevelManager.Camera.Y));
            };
            i++;
            // Add the Save-Button
            M.ControlElementList.Add(new Button("Save", new Vector2(Values.WindowSize.X - 100, Values.WindowSize.Y - 100), Color.White, Assets.BigFont));
            //((Button)M.ControlElementList.Last()).Tex = Assets.GrassFront;
            ((Button)M.ControlElementList[i]).OnClick += (object sender, EventArgs e) => { SaveCurrentLevel(); };
            return M;
        }
        public static Level CreateNewLevel()
        {
            Level L = new Level();
            for (int i = 0; i < 200; i++)
            {
                L.BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(i * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale), true));
            }
            L.Reset();
            L.End = new Vector2(200 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale - LevelManager.EndHeight);
            return L;
        }
        public static void SaveCurrentLevel()
        {
            CurrentLevel.SaveCurrentEntityConfig();
            LevelDataStorage.LvlList.Add((Level)CurrentLevel.Clone());
            MenuManager.BuildMenus();
            MenuManager.GS = GameState.LevelMenu;
            CurrentLevel = CreateNewLevel();
        }
        static Entity GetMouseHoveredElement()
        {
            Rectangle M = Controls.GetMouseRect();
            M.X -= (int)LevelManager.Camera.X;
            M.Y -= (int)LevelManager.Camera.Y;
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
            if (Controls.CurKS.IsKeyDown(Keys.A))
                LevelManager.Camera.X += 10;

            if (Controls.CurKS.IsKeyDown(Keys.D))
                LevelManager.Camera.X -= 10;

            if (LevelManager.Camera.X > 200)
                LevelManager.Camera.X = 200;

            if (LevelManager.Camera.X < -CurrentLevel.End.X + (int)Values.WindowSize.X - 200)
                LevelManager.Camera.X = -CurrentLevel.End.X + (int)Values.WindowSize.X - 200;
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

            Rectangle R = new Rectangle(SelectionRect.X - (int)LevelManager.Camera.X, SelectionRect.Y - (int)LevelManager.Camera.Y, SelectionRect.Width, SelectionRect.Height);

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
            LevelManager.ThisPlayer = new Player(new Vector2(ThisPlayer.Rect.X, ThisPlayer.Rect.Y));
            LevelManager.Ending = false;
            LevelManager.SaidEndingPhrase = false;
            LevelManager.CurrentLevel.PlayerPos = new Vector2(ThisPlayer.Rect.X, ThisPlayer.Rect.Y);
            LevelManager.ThisPlayer.Vel = Vector2.Zero;
            LevelManager.UpdateTextures();
            Testing = true;
            MediaPlayer.Play(Assets.InGameTheme);
        }
        static void StopTesting()
        {
            CurrentLevel.Reset();
            LevelManager.Camera = ThisPlayer.RespawnPoint;
            //LevelManager.ClearEnemys();
            Testing = false;
            LevelManager.Ending = false;
            ParticleManager.Clear();
            MediaPlayer.Stop();
            LevelManager.Score = 0;
        }
        
        public static void Update()
        {
            if (Testing)
            {
                LevelManager.Update();

                if (Controls.WasKeyJustPressed(Keys.T))
                    StopTesting();

                if (LevelManager.Ending)
                    StopTesting();
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
                        DraggedEntity.CreatorClickPos = new Point(DraggedEntity.Rect.X - Controls.CurMS.X + (int)LevelManager.Camera.X, 
                            DraggedEntity.Rect.Y - Controls.CurMS.Y + (int)LevelManager.Camera.Y);

                        if (!Controls.CurKS.IsKeyDown(Keys.LeftControl) && !SelectedEntities.Contains(DraggedEntity))
                            SelectedEntities.Clear();

                        for (int i = 0; i < SelectedEntities.Count; i++)
                        {
                            if (SelectedEntities[i] != DraggedEntity)
                                SelectedEntities[i].CreatorClickPos = new Point(SelectedEntities[i].Rect.X - DraggedEntity.Rect.X + (int)LevelManager.Camera.X,
                                    SelectedEntities[i].Rect.Y - DraggedEntity.Rect.Y + (int)LevelManager.Camera.Y);
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
                    DraggedEntity.Rect = new Rectangle(Controls.CurMS.X - (int)LevelManager.Camera.X + DraggedEntity.CreatorClickPos.X, 
                        Controls.CurMS.Y - (int)LevelManager.Camera.Y + DraggedEntity.CreatorClickPos.Y, DraggedEntity.Rect.Width, DraggedEntity.Rect.Height);

                    SnapEntityToBlocks(DraggedEntity);

                    if (DraggedEntity.Rect.X < 0)
                        DraggedEntity.Rect.X = 0;

                    for (int i = 0; i < SelectedEntities.Count; i++)
                    {
                        // SelectedEntities will be positioned relative to the DraggedEntity
                        if (SelectedEntities[i] != DraggedEntity)
                            SelectedEntities[i].Rect = new Rectangle(DraggedEntity.Rect.X - (int)LevelManager.Camera.X + SelectedEntities[i].CreatorClickPos.X,
                                DraggedEntity.Rect.Y - (int)LevelManager.Camera.Y + SelectedEntities[i].CreatorClickPos.Y, SelectedEntities[i].Rect.Width, 
                                SelectedEntities[i].Rect.Height);

                        if (SelectedEntities[i].Rect.X < 0)
                            SelectedEntities[i].Rect.X = 0;
                    }
                }
            }
        }
        public static void Draw(SpriteBatch SB)
        {
            if (Testing)
            {
                LevelManager.Draw(SB);
            }
            else
            {
                // Draw Selected Entity Outline
                for (int i = 0; i < SelectedEntities.Count; i++)
                {
                    Rectangle R = Values.CopyRectangle(SelectedEntities[i].Rect);
                    R.X += (int)LevelManager.Camera.X;
                    R.Inflate(5, 5);
                    //SB.Draw(Assets.White, R, Color.Black * 0.5f);
                    Assets.DrawRoundedRectangle(R, 20, Color.Black, SB);
                }

                // Draw X Zero - Red Zone
                SB.Draw(Assets.White, new Rectangle(0, 0, (int)LevelManager.Camera.X, (int)Values.WindowSize.Y), Color.Red * 0.25f);

                // Draw X End - Red Zone
                SB.Draw(Assets.White, new Rectangle((int)CurrentLevel.End.X + (int)LevelManager.Camera.X, 0, (int)CurrentLevel.End.X - (int)LevelManager.Camera.X, 
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
                        SB.Draw(CurrentLevel.BlockList[i].Texture, new Rectangle(CurrentLevel.BlockList[i].Rect.X + (int)LevelManager.Camera.X, 
                            CurrentLevel.BlockList[i].Rect.Y + (int)LevelManager.Camera.Y, CurrentLevel.BlockList[i].Rect.Width, 
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
