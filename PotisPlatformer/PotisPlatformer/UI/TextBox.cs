using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    public class TextBox : ControlElement
    {
        Rectangle InnerLayer;
        public Color RectColor;
        public string Text;
        Color InnerLayerColor;
        int BorderWidth;
        int TextToBorderDistance;
        int Width;

        event EventHandler PressedEnter;
        event EventHandler TextChanged;

        public TextBox(Rectangle Rect)
        {
            InnerLayerColor = Color.DarkGray;
            Width = 100;
            Text = "";
            AdaptTheInnerLayerToTheOuterOne(2, 2);
        }

        public TextBox(Rectangle Rect, EventHandler PressedEnterEvent, EventHandler TextChangedEvent)
        {
            InnerLayerColor = Color.DarkGray;
            Width = 100;
            Text = "";
            AdaptTheInnerLayerToTheOuterOne(2, 2);
            PressedEnter += PressedEnterEvent;
            TextChanged += TextChangedEvent;
        }

        public TextBox(Rectangle Rect, Color BorderColor, Color InnerColor, string Text, int Width, int BorderWidth, int TextToBorderDistance, 
            EventHandler PressedEnterEvent)
        {
            this.Rect = Rect;
            RectColor = BorderColor;
            this.Text = Text;
            this.Width = Width;
            AdaptTheInnerLayerToTheOuterOne(BorderWidth, TextToBorderDistance);
            PressedEnter += PressedEnterEvent;
        }

        public void AdaptTheInnerLayerToTheOuterOne(int BorderWidth, int TextToBorderDistance)
        {
            Width = (int)Assets.Font.MeasureString(Text).X;
            if (Width < 100) { Width = 100; }
            this.BorderWidth = BorderWidth;
            this.TextToBorderDistance = TextToBorderDistance;

            InnerLayer = new Rectangle(Rect.X + BorderWidth, Rect.Y + BorderWidth, Width - BorderWidth * 2, 
                (int)Assets.Font.MeasureString("O").Y - BorderWidth * 2);

            Rect = new Rectangle(Rect.X, Rect.Y, Width, (int)Assets.Font.MeasureString("O").Y);
        }

        public override void Update()
        {
            AdaptTheInnerLayerToTheOuterOne(BorderWidth, TextToBorderDistance);

            if (Controls.CurMS.LeftButton == ButtonState.Pressed && Rect.Intersects(new Rectangle(Controls.CurMS.X, Controls.CurMS.Y, 1, 1)))
            {
                MenuManager.CurrentSelectedElement = this;
            }

            if (this == MenuManager.CurrentSelectedElement)
            {
                if (Controls.CurKS.IsKeyDown(Keys.Enter) && Controls.LastKS.IsKeyUp(Keys.Enter))
                {
                    PressedEnter.Invoke(this, EventArgs.Empty);
                }

                if (Controls.CurKS.GetPressedKeys().GetLength(0) > 0)
                    TextChanged.Invoke(this, EventArgs.Empty);

                foreach (Keys key in Controls.CurKS.GetPressedKeys())
                {
                    if (!Controls.LastKS.GetPressedKeys().Contains(key) && key != Keys.Back && key != Keys.Enter && key != Keys.LeftShift && key != Keys.Space &&
                        key != Keys.NumPad0 && key != Keys.NumPad1 && key != Keys.NumPad2 && key != Keys.NumPad3 && key != Keys.NumPad4 && key != Keys.NumPad5 &&
                        key != Keys.NumPad6 && key != Keys.NumPad7 && key != Keys.NumPad8 && key != Keys.NumPad9)
                    {
                        Text = string.Concat(Text, key.ToString());
                    }
                }

                if (Controls.CurKS.IsKeyDown(Keys.Back) && Controls.LastKS.IsKeyUp(Keys.Back))
                {
                    if (Text.Length > 0)
                    {
                        Text = Text.Remove(Text.Length - 1, 1);
                    }
                }

                if (Controls.BackSpaceHoldingCounter > 60)
                {
                    if (Text.Length > 0)
                    {
                        Text = Text.Remove(Text.Length - 1, 1);
                    }
                }

                if (Controls.CurKS.IsKeyDown(Keys.Space) && Controls.LastKS.IsKeyUp(Keys.Space))
                {
                    Text = string.Concat(Text, " ");
                }

                if (Controls.CurKS.IsKeyDown(Keys.NumPad0) && Controls.LastKS.IsKeyUp(Keys.NumPad0))
                {
                    Text = string.Concat(Text, "0");
                }

                if (Controls.CurKS.IsKeyDown(Keys.NumPad1) && Controls.LastKS.IsKeyUp(Keys.NumPad1))
                {
                    Text = string.Concat(Text, "1");
                }

                if (Controls.CurKS.IsKeyDown(Keys.NumPad2) && Controls.LastKS.IsKeyUp(Keys.NumPad2))
                {
                    Text = string.Concat(Text, "2");
                }

                if (Controls.CurKS.IsKeyDown(Keys.NumPad3) && Controls.LastKS.IsKeyUp(Keys.NumPad3))
                {
                    Text = string.Concat(Text, "3");
                }

                if (Controls.CurKS.IsKeyDown(Keys.NumPad4) && Controls.LastKS.IsKeyUp(Keys.NumPad4))
                {
                    Text = string.Concat(Text, "4");
                }

                if (Controls.CurKS.IsKeyDown(Keys.NumPad5) && Controls.LastKS.IsKeyUp(Keys.NumPad5))
                {
                    Text = string.Concat(Text, "5");
                }

                if (Controls.CurKS.IsKeyDown(Keys.NumPad6) && Controls.LastKS.IsKeyUp(Keys.NumPad6))
                {
                    Text = string.Concat(Text, "6");
                }

                if (Controls.CurKS.IsKeyDown(Keys.NumPad7) && Controls.LastKS.IsKeyUp(Keys.NumPad7))
                {
                    Text = string.Concat(Text, "7");
                }

                if (Controls.CurKS.IsKeyDown(Keys.NumPad8) && Controls.LastKS.IsKeyUp(Keys.NumPad8))
                {
                    Text = string.Concat(Text, "8");
                }

                if (Controls.CurKS.IsKeyDown(Keys.NumPad9) && Controls.LastKS.IsKeyUp(Keys.NumPad9))
                {
                    Text = string.Concat(Text, "9");
                }
            }
        }

        public override void Draw(SpriteBatch SB)
        {
            SB.Draw(Assets.White, Rect, RectColor);
            SB.Draw(Assets.White, InnerLayer, InnerLayerColor);
            SB.DrawString(Assets.Font, Text, new Vector2(InnerLayer.X, InnerLayer.Y), Color.White);
        }
    }
}
