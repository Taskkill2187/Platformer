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
    public static class MenuManager
    {
        public static GameState GS;

        public static ControlElement CurrentSelectedElement;
        public static bool Exiting;

        public static Menu MainMenu = new Menu();
        public static Menu Options = new Menu();
        public static Menu LevelMenu = new Menu();

        public static void BuildMenus()
        {
            MainMenu.ControlElementList.Clear();
            Options.ControlElementList.Clear();
            LevelMenu.ControlElementList.Clear();
            MainMenu.ControlElementList.Add(new Button("Select Level", new Vector2(), Color.White, Assets.BigFont));
            MainMenu.ControlElementList.Add(new Button("Level Creator", new Vector2(), Color.White, Assets.BigFont));
            MainMenu.ControlElementList.Add(new Button("Options", new Vector2(), Color.White, Assets.BigFont));
            MainMenu.ControlElementList.Add(new Button("Exit", new Vector2(), Color.White, Assets.BigFont));
            MainMenu.ArrangeButtons(MenuButtonLayout.MiddleVert);

            ((Button)MainMenu.ControlElementList[0]).OnClick += (object sender, EventArgs e) => { GS = GameState.LevelMenu; };
            ((Button)MainMenu.ControlElementList[1]).OnClick += (object sender, EventArgs e) => {
                GS = GameState.LevelCreator;
                if (LevelCreator.Testing)
                {
                    MediaPlayer.Play(Assets.InGameTheme);
                }
                LevelCreator.AIPlayerPlaying = false;
            };
            ((Button)MainMenu.ControlElementList[2]).OnClick += (object sender, EventArgs e) => { GS = GameState.Options; };
            ((Button)MainMenu.ControlElementList[3]).OnClick += (object sender, EventArgs e) => { Exiting = true; };


            foreach (Level L in LevelDataStorage.LvlList)
            {
                LevelMenu.ControlElementList.Add(new Button((LevelDataStorage.LvlList.IndexOf(L) + 1).ToString(), new Vector2(), Color.White, Assets.BigFont));
                ((Button)LevelMenu.ControlElementList.Last()).OnClick += (object sender, EventArgs e) => { LevelManager.LoadLevel(L); };
            }
            LevelMenu.ArrangeButtons(MenuButtonLayout.MiddleHorz);

            Options.ControlElementList.Add(new Button("SoundEffects: " + StoredData.Default.SoundEffects.ToString(), new Vector2(), Color.White, Assets.BigFont));
            Options.ControlElementList.Add(new Button("Music: " + StoredData.Default.Music.ToString(), new Vector2(), Color.White, Assets.BigFont));
            Options.ControlElementList.Add(new Button("Particle Effects: " + StoredData.Default.ParticleEffects.ToString(), new Vector2(), Color.White, Assets.BigFont));
            Options.ArrangeButtons(MenuButtonLayout.MiddleVert);

            ((Button)Options.ControlElementList[0]).OnClick += new EventHandler(ToggleSound);
            ((Button)Options.ControlElementList[1]).OnClick += new EventHandler(ToggleMusic);
            ((Button)Options.ControlElementList[2]).OnClick += new EventHandler(ToggleParticles);
        }
        
        public static void ToggleSound(Object sender, EventArgs e)
        {
            switch (StoredData.Default.SoundEffects)
            {
                case true:
                    StoredData.Default.SoundEffects = false;
                    ((Button)Options.ControlElementList[0]).Text = "SoundEffects: " + StoredData.Default.SoundEffects.ToString();
                    break;

                case false:
                    StoredData.Default.SoundEffects = true;
                    ((Button)Options.ControlElementList[0]).Text = "SoundEffects: " + StoredData.Default.SoundEffects.ToString();
                    break;
            }
        }
        public static void ToggleMusic(Object sender, EventArgs e)
        {
            switch (StoredData.Default.Music)
            {
                case true:
                    StoredData.Default.Music = false;
                    MediaPlayer.Volume = 0;
                    ((Button)Options.ControlElementList[1]).Text = "Music: " + StoredData.Default.Music.ToString();
                    break;

                case false:
                    StoredData.Default.Music = true;
                    MediaPlayer.Volume = 0.15f;
                    ((Button)Options.ControlElementList[1]).Text = "Music: " + StoredData.Default.Music.ToString();
                    break;
            }
        }
        public static void ToggleParticles(Object sender, EventArgs e)
        {
            switch (StoredData.Default.ParticleEffects)
            {
                case true:
                    StoredData.Default.ParticleEffects = false;
                    ((Button)Options.ControlElementList[2]).Text = "Particle Effects: " + StoredData.Default.ParticleEffects.ToString();
                    break;

                case false:
                    StoredData.Default.ParticleEffects = true;
                    ((Button)Options.ControlElementList[2]).Text = "Particle Effects: " + StoredData.Default.ParticleEffects.ToString();
                    break;
            }
        }
    }

    public enum GameState
    {
        InGame,
        MainMenu,
        LevelMenu,
        LevelCreator,
        Options
    }
}
