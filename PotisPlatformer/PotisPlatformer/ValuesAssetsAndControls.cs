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
    public static class Values
    {
        public static Vector2 WindowSize = new Vector2(1360, 768);
        public static Random RDM = new Random();
        public static int TimesPlayerCanJump = 1;
        public static float PlayerWalkAcceleration = 4f;
        public static int PlayerAnimSpeed = 5;
        public static Rectangle GetScreenRect()
        {
            return new Rectangle(0, 0, (int)WindowSize.X, (int)WindowSize.Y);
        }
        public static Rectangle CopyRectangle(Rectangle R)
        {
            return new Rectangle(R.X, R.Y, R.Width, R.Height);
        }
    }
    public static class Controls
    {
        public static MouseState CurMS;
        public static MouseState LastMS;
        public static KeyboardState CurKS;
        public static KeyboardState LastKS;

        public static int BackSpaceHoldingCounter;

        public static void Update()
        {
            LastKS = CurKS;
            LastMS = CurMS;

            CurKS = Keyboard.GetState();
            CurMS = Mouse.GetState();

            if (CurKS.IsKeyDown(Keys.Back))
            {
                BackSpaceHoldingCounter++;
            }
            else
            {
                BackSpaceHoldingCounter = 0;
            }
        }

        public static Rectangle GetMouseRect() { return new Rectangle(CurMS.X, CurMS.Y, 1, 1); }
        public static bool WasKeyJustPressed(Keys K) { return CurKS.IsKeyDown(K) && LastKS.IsKeyUp(K); }
        public static Vector2 GetMouseVector() { return new Vector2(CurMS.X, CurMS.Y); }
        public static bool WasLMBJustPressed() { return CurMS.LeftButton == ButtonState.Pressed && LastMS.LeftButton == ButtonState.Released; }
        public static bool WasLMBJustReleased() { return CurMS.LeftButton == ButtonState.Released && LastMS.LeftButton == ButtonState.Pressed; }
        public static bool WasRMBJustPressed() { return CurMS.RightButton == ButtonState.Pressed && LastMS.RightButton == ButtonState.Released; }
    }
    public static class Assets
    {
        public static Texture2D PlayerWalk;
        public static Texture2D BluSpy;
        public static Texture2D BlockGrass;
        public static Texture2D GrassFront;
        public static Texture2D White;
        public static Texture2D WalkAnim;
        public static Texture2D Standing;
        public static Texture2D JumpBlock;
        public static Texture2D LevelBackgroundLeaves;
        public static Texture2D LevelBackgroundMountains;
        public static Texture2D LevelBackgroundMountains2;
        public static Texture2D MenuBackground;
        public static Texture2D Walker;
        public static Texture2D WalkerFront;
        public static Texture2D PlayerJumpFall;
        public static Texture2D Brick;
        public static Texture2D KoopaFront;
        public static Texture2D GreenKoopa;
        public static Texture2D GreenShell;
        public static Texture2D Bob_omb;
        public static Texture2D EndPipe;
        public static Texture2D Coin;
        public static Texture2D SpinningBlock;
        public static Texture2D KugelWilli;
        public static Texture2D KugelWilli_Spawner;
        public static Texture2D QuestionMarkBlock;

        public static SpriteFont Font;
        public static SpriteFont BigFont;

        public static SoundEffect CoinSound;
        public static SoundEffect Death;
        public static SoundEffect Jump;
        public static SoundEffect LevelEnd;
        public static SoundEffect MenuButton;
        public static SoundEffect JumpBlockSound;

        public static Song InGameTheme;

        public static void Load(ContentManager Content, GraphicsDevice GD)
        {
            BlockGrass = Content.Load<Texture2D>(@"Sprites\BlockGrass");
            GrassFront = Content.Load<Texture2D>(@"Sprites\GrassFront");
            JumpBlock = Content.Load<Texture2D>(@"Sprites\JumpBlock");
            PlayerWalk = Content.Load<Texture2D>(@"Sprites\PlayerWalk");
            BluSpy = Content.Load<Texture2D>(@"Sprites\BluSpy");
            Walker = Content.Load<Texture2D>(@"Sprites\Walker");
            WalkerFront = Content.Load<Texture2D>(@"Sprites\WalkerFront");
            PlayerJumpFall = Content.Load<Texture2D>(@"Sprites\PlayerJumpFall");
            Brick = Content.Load<Texture2D>(@"Sprites\Bricks");
            GreenKoopa = Content.Load<Texture2D>(@"Sprites\GreenKoopa");
            GreenShell = Content.Load<Texture2D>(@"Sprites\GreenShell");
            KoopaFront = Content.Load<Texture2D>(@"Sprites\KoopaFront");
            Bob_omb = Content.Load<Texture2D>(@"Sprites\Bob-omb");
            EndPipe = Content.Load<Texture2D>(@"Sprites\EndPipe");
            Coin = Content.Load<Texture2D>(@"Sprites\Coin");
            SpinningBlock = Content.Load<Texture2D>(@"Sprites\SpinningBlock");
            KugelWilli = Content.Load<Texture2D>(@"Sprites\Kugelwilli");
            KugelWilli_Spawner = Content.Load<Texture2D>(@"Sprites\Kugelwilli-Spawner");
            QuestionMarkBlock = Content.Load<Texture2D>(@"Sprites\QuestionMarkBlock");

            LevelBackgroundLeaves = Content.Load<Texture2D>(@"Sprites\LevelBackgroundLeaves");
            LevelBackgroundMountains = Content.Load<Texture2D>(@"Sprites\LevelBackgroundMountains");
            LevelBackgroundMountains2 = Content.Load<Texture2D>(@"Sprites\LevelBackgroundMountains2");
            MenuBackground = Content.Load<Texture2D>(@"Sprites\MenuBackground");

            Font = Content.Load<SpriteFont>("SmallFont");
            BigFont = Content.Load<SpriteFont>("BigFont");
            
            Jump = Content.Load<SoundEffect>(@"SoundEffects\JumpSound");
            CoinSound = Content.Load<SoundEffect>(@"SoundEffects\Coin");
            Death = Content.Load<SoundEffect>(@"SoundEffects\Death");
            LevelEnd = Content.Load<SoundEffect>(@"SoundEffects\LevelEnd");
            MenuButton = Content.Load<SoundEffect>(@"SoundEffects\MenuButtonSound");
            JumpBlockSound = Content.Load<SoundEffect>(@"SoundEffects\JumpBlockSound");

            InGameTheme = Content.Load<Song>("InGameTheme");
            
            Color[] Col = new Color[1];
            Col[0] = Color.White;
            White = new Texture2D(GD, 1, 1);
            White.SetData<Color>(Col);
        }
        public static Texture2D GetRDMBackground()
        {
            switch (Values.RDM.Next(3))
            {
                case 0:
                    return LevelBackgroundLeaves;

                case 1:
                    return LevelBackgroundMountains;

                case 2:
                    return LevelBackgroundMountains2;
            }
            return LevelBackgroundLeaves;
        }

        public static void DrawLine(Vector2 End1, Vector2 End2, int Thickness, Color Col, SpriteBatch SB)
        {
            Vector2 Line = End1 - End2;
            SB.Draw(White, End1, null, Col, -(float)Math.Atan2(Line.X, Line.Y) - (float)Math.PI / 2, new Vector2(0, 0.5f), new Vector2(Line.Length(), Thickness), SpriteEffects.None, 0f);
        }
        public static void DrawCircle(Vector2 Pos, float Radius, Color Col, SpriteBatch SB)
        {
            if (Radius < 0)
                Radius *= -1;

            for (int i = -(int)Radius; i < (int)Radius; i++)
            {
                int HalfHeight = (int)Math.Sqrt(Radius * Radius - i * i);
                SB.Draw(White, new Rectangle((int)Pos.X + i, (int)Pos.Y - HalfHeight, 1, HalfHeight * 2), Col);
            }
        }
        public static void DrawCircle(Vector2 Pos, float Radius, float HeightMultiplikator, Color Col, SpriteBatch SB)
        {
            if (Radius < 0)
                Radius *= -1;

            for (int i = -(int)Radius; i < (int)Radius; i++)
            {
                int HalfHeight = (int)Math.Sqrt(Radius * Radius - i * i);
                SB.Draw(White, new Rectangle((int)Pos.X + i, (int)Pos.Y, 1, (int)(HalfHeight * HeightMultiplikator)), Col);
            }

            for (int i = -(int)Radius; i < (int)Radius; i++)
            {
                int HalfHeight = (int)Math.Sqrt(Radius * Radius - i * i);
                SB.Draw(White, new Rectangle((int)Pos.X + i + 1, (int)Pos.Y, -1, (int)(-HalfHeight * HeightMultiplikator)), Col);
            }
        }
        public static void DrawRoundedRectangle(Rectangle Rect, float PercentageOfRounding, Color Col, SpriteBatch SB)
        {
            float Rounding = PercentageOfRounding / 100;
            Rectangle RHorz = new Rectangle(Rect.X, (int)(Rect.Y + Rect.Height * (Rounding / 2)), Rect.Width, (int)(Rect.Height * (1 - Rounding)));
            Rectangle RVert = new Rectangle((int)(Rect.X + Rect.Width * (Rounding / 2)), Rect.Y, (int)(Rect.Width * (1 - Rounding)), (int)(Rect.Height * 0.999f));

            int RadiusHorz = (int)(Rect.Width * (Rounding / 2));
            int RadiusVert = (int)(Rect.Height * (Rounding / 2));

            if (RadiusHorz != 0)
            {
                // Top-Left
                DrawCircle(new Vector2(Rect.X + RadiusHorz, Rect.Y + RadiusVert), RadiusHorz, RadiusVert / (float)RadiusHorz, Col, SB);

                // Top-Right
                DrawCircle(new Vector2(Rect.X + Rect.Width - RadiusHorz - 1, Rect.Y + RadiusVert), RadiusHorz, RadiusVert / (float)RadiusHorz, Col, SB);

                // Bottom-Left
                DrawCircle(new Vector2(Rect.X + RadiusHorz, Rect.Y + RadiusVert + (int)(Rect.Height * (1 - Rounding))), RadiusHorz, RadiusVert / (float)RadiusHorz, Col, SB);

                // Bottom-Right
                DrawCircle(new Vector2(Rect.X + Rect.Width - RadiusHorz - 1, Rect.Y + RadiusVert + (int)(Rect.Height * (1 - Rounding))), RadiusHorz, RadiusVert / (float)RadiusHorz, Col, SB);
            }

            SB.Draw(White, RHorz, Col);
            SB.Draw(White, RVert, Col);
        }
    }
}
