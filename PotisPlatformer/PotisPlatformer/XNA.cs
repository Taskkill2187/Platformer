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
    public class XNA : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public XNA()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = (int)Values.WindowSize.X;
            graphics.PreferredBackBufferHeight = (int)Values.WindowSize.Y;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            LevelDataStorage.BuildLevels();
            MenuManager.BuildMenus();
            MenuManager.GS = GameState.MainMenu;
        }
        protected override void OnExiting(Object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            StoredData.Default.Save();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Assets.Load(Content, GraphicsDevice);
            LevelCreator.UpdateTextures();
            if (StoredData.Default.Music)
                MediaPlayer.Volume = 0.15f;
            else
                MediaPlayer.Volume = 0f;
        }
        
        protected override void Update(GameTime gameTime)
        {
            Controls.Update();

            ParticleManager.Update();

            switch (MenuManager.GS)
            {
                case GameState.MainMenu:
                    MenuManager.MainMenu.Update();
                    if (Controls.CurKS.IsKeyDown(Keys.Escape) && Controls.LastKS.IsKeyUp(Keys.Escape) || MenuManager.Exiting)
                        Exit();
                    break;

                case GameState.Options:
                    MenuManager.Options.Update();
                    if (Controls.CurKS.IsKeyDown(Keys.Escape))
                        MenuManager.GS = GameState.MainMenu;
                    break;

                case GameState.LevelMenu:
                    MenuManager.LevelMenu.Update();
                    if (Controls.CurKS.IsKeyDown(Keys.Escape))
                        MenuManager.GS = GameState.MainMenu;
                    break;

                case GameState.LevelCreator:
                    LevelCreator.Update();
                    if (Controls.CurKS.IsKeyDown(Keys.Escape))
                    {
                        MenuManager.GS = GameState.MainMenu;
                        ParticleManager.Clear();
                        MediaPlayer.Stop();
                    }
                    break;

                case GameState.InGame:
                    LevelManager.Update();
                    if (Controls.CurKS.IsKeyDown(Keys.Escape))
                    {
                        MenuManager.GS = GameState.MainMenu;
                        ParticleManager.Clear();
                        LevelManager.ClearEnemys();
                        MediaPlayer.Stop();
                    }
                    break;
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

            if (MenuManager.GS == GameState.InGame || MenuManager.GS == GameState.LevelCreator)
            {
                // Draw Level-Background
                int SnippetSizeX = 550;
                int SnippetSizeY = 309;
                if (LevelManager.CurrentLevel.Background != null)
                {
                    int x = (int)(-LevelManager.Camera.X / 20 % LevelManager.CurrentLevel.Background.Width);
                    if (x > LevelManager.CurrentLevel.Background.Width - SnippetSizeX) {
                        // if x is bigger than the Background Texture => Draw two textures
                        spriteBatch.Draw(LevelManager.CurrentLevel.Background, new Rectangle(0, 0, (int)Values.WindowSize.X, (int)Values.WindowSize.Y), 
                            new Rectangle(x, LevelManager.CurrentLevel.Background.Height - SnippetSizeY, SnippetSizeX, SnippetSizeY), Color.White);
                        //                                                     End of the Window                        - the difference of x and the texture size
                        spriteBatch.Draw(LevelManager.CurrentLevel.Background, new Rectangle((int)(Values.WindowSize.X) + 
                            (int)(((LevelManager.CurrentLevel.Background.Width - SnippetSizeX) - x) * (Values.WindowSize.X / SnippetSizeX)), 0, 
                            (int)Values.WindowSize.X, (int)Values.WindowSize.Y), 
                            new Rectangle(0, LevelManager.CurrentLevel.Background.Height - SnippetSizeY, SnippetSizeX, SnippetSizeY), Color.White);
                    }
                    else // if x is smaller than the background teture => Draw normally
                        spriteBatch.Draw(LevelManager.CurrentLevel.Background, new Rectangle(0, 0, (int)Values.WindowSize.X, (int)Values.WindowSize.Y), 
                            new Rectangle(x, LevelManager.CurrentLevel.Background.Height - SnippetSizeY, SnippetSizeX, SnippetSizeY), Color.White);
                }
                else
                {
                    Texture2D DefaultBackground = Assets.LevelBackgroundMountains;
                    // Texture snippet is 300 x 200
                    int x = (int)(-LevelManager.Camera.X / 20 % DefaultBackground.Width);
                    if (x > DefaultBackground.Width - SnippetSizeX)
                    {
                        // if x is bigger than the Background Texture => Draw two textures
                        spriteBatch.Draw(DefaultBackground, new Rectangle(0, 0, (int)Values.WindowSize.X + (int)(((DefaultBackground.Width - SnippetSizeX) - x) * 
                            (Values.WindowSize.X / SnippetSizeX)), (int)Values.WindowSize.Y),
                            new Rectangle(x, DefaultBackground.Height - SnippetSizeY, SnippetSizeX + ((DefaultBackground.Width - SnippetSizeX) - x), SnippetSizeY), Color.White);
                        //                                                     End of the Window            - the difference of x and the texture size
                        spriteBatch.Draw(DefaultBackground, new Rectangle((int)(Values.WindowSize.X) + (int)(((DefaultBackground.Width - SnippetSizeX) - x) * 
                            (Values.WindowSize.X / SnippetSizeX)), 0, (int)Values.WindowSize.X, (int)Values.WindowSize.Y),
                            new Rectangle(0, DefaultBackground.Height - SnippetSizeY, SnippetSizeX, SnippetSizeY), Color.White);
                    }
                    else // if x is smaller than the background teture => Draw normally
                        spriteBatch.Draw(DefaultBackground, new Rectangle(0, 0, (int)Values.WindowSize.X, (int)Values.WindowSize.Y),
                            new Rectangle(x, DefaultBackground.Height - SnippetSizeY, SnippetSizeX, SnippetSizeY), Color.White);
                }
            }
            else
            {
                // Draw Menu-Background
                if (Assets.MenuBackground != null)
                {
                    spriteBatch.Draw(Assets.MenuBackground, new Rectangle(0, 0, (int)Values.WindowSize.X, (int)Values.WindowSize.Y), Color.White);
                }
            }

            ParticleManager.Draw(spriteBatch);

            if (MenuManager.GS == GameState.MainMenu)
                MenuManager.MainMenu.Draw(spriteBatch);

            if (MenuManager.GS == GameState.Options)
                MenuManager.Options.Draw(spriteBatch);

            if (MenuManager.GS == GameState.LevelMenu)
                MenuManager.LevelMenu.Draw(spriteBatch);

            if (MenuManager.GS == GameState.InGame)
                LevelManager.Draw(spriteBatch);

            if (MenuManager.GS == GameState.LevelCreator)
                LevelCreator.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
