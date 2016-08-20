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
    public class Button : ControlElement
    {
        public event EventHandler OnClick;

        public Texture2D Tex;
        public Texture2D TexPressed;
        
        public Color Color;
        private string text;
        public string Text
        {
            set { text = value; SnapRectangleToText(); }
            get { return text; }
        }
        public SpriteFont Font = Assets.BigFont;
        public Vector2 TopRight;
        public Vector2 Center;
        public object Storage;

        public Button(string Text, Rectangle rectangle, Color color)
        {
            Tex = null;
            TexPressed = null;
            this.Text = Text;
            Rect = rectangle;
            Color = color;
            Font = Assets.BigFont;
            TopRight = new Vector2(Rect.X, Rect.Y);
            Center = TopRight + new Vector2(Rect.Width / 2, Rect.Height / 2);
        }
        public Button(string Text, Rectangle rectangle, Texture2D Texture, Texture2D TexturePressed)
        {
            Tex = Texture;
            TexPressed = TexturePressed;
            this.Text = Text;
            Rect = rectangle;
            Font = Assets.BigFont;
            TopRight = new Vector2(Rect.X, Rect.Y);
            Center = TopRight + new Vector2(Rect.Width / 2, Rect.Height / 2);
        }
        public Button(string Text, Vector2 Center, Color color, SpriteFont Font)
        {
            Tex = null;
            TexPressed = null;
            this.Text = Text;
            Color = color;
            this.Font = Font;
            this.Center = Center;
            SnapRectangleToText();
        }

        public override void Update()
        {
            if (Controls.CurMS.LeftButton == ButtonState.Pressed && Controls.LastMS.LeftButton == ButtonState.Released && Rect.Intersects(new Rectangle(Controls.CurMS.X, Controls.CurMS.Y, 1, 1)))
            {
                if (OnClick != null)
                {
                    if (StoredData.Default.SoundEffects)
                        Assets.MenuButton.Play(0.6f, 0, 0);

                    OnClick(this, EventArgs.Empty);
                }
            }
        }
        public void SnapRectangleToText()
        {
            TopRight = new Vector2(Center.X - Font.MeasureString(Text).X / 2, Center.Y - Font.MeasureString(Text).Y / 2);
            Rect = new Rectangle((int)TopRight.X, (int)TopRight.Y, (int)Font.MeasureString(Text).X, (int)Font.MeasureString(Text).Y);
        }
        public void FitRectangleToCenter()
        {
            Rect.X = (int)Center.X - Rect.Width / 2;
            Rect.Y = (int)Center.Y - Rect.Height / 2;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Controls.CurMS.LeftButton == ButtonState.Pressed && Rect.Intersects(new Rectangle(Controls.CurMS.X, Controls.CurMS.Y, 1, 1)))
            {
                if (TexPressed != null)
                    spriteBatch.Draw(TexPressed, Rect, Color.White);
                spriteBatch.DrawString(Assets.BigFont, Text, new Vector2(Rect.X + Rect.Width / 2 - Assets.BigFont.MeasureString(Text).X / 2, Rect.Y + Rect.Height / 2 - Assets.BigFont.MeasureString(Text).Y / 2), Color);
            }
            else
            {
                if (Tex != null)
                    spriteBatch.Draw(Tex, Rect, Color.White);
                spriteBatch.DrawString(Assets.BigFont, Text, new Vector2(Rect.X + Rect.Width / 2 - Assets.BigFont.MeasureString(Text).X / 2, Rect.Y + Rect.Height / 2 - Assets.BigFont.MeasureString(Text).Y / 2), Color);
            }
        }
    }
}
