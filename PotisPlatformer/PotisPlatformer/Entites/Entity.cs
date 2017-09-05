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
    public class Entity : ICloneable
    {
        public Rectangle Rect;
        public Vector2 Vel;
        public Texture2D Texture;

        public Point CreatorClickPos;

        public Level Parent;

        public Vector2 GetPosVector2() { return new Vector2(Rect.X, Rect.Y); }
        public Vector2 GetSizeVector2() { return new Vector2(Rect.Width, Rect.Height); }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.Texture == null)
            {
                spriteBatch.Draw(Assets.White, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), Color.White);
            }
            else
            {
                spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), Color.White);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch, Color Tint, SpriteEffects Effect)
        {
            if (this.Texture == null)
            {
                spriteBatch.Draw(Assets.White, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), Tint, 0, new Vector2(0, 0), Effect, 0);
            }
            else
            {
                spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), new Rectangle(0, 0, Texture.Width, Texture.Height), Tint, 0, new Vector2(0, 0), Effect, 0);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch, Rectangle SourceRect, Color Tint, float Rotation, SpriteEffects Effect)
        {
            if (this.Texture == null)
            {
                spriteBatch.Draw(Assets.White, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), SourceRect, Tint, Rotation, new Vector2(0, 0), Effect, 0);
            }
            else
            {
                spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), SourceRect, Tint, Rotation, new Vector2(0, 0), Effect, 0);
            }
        }

        public virtual object Clone()
        {
            Entity a = new Entity();
            a.Rect = new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            a.Vel = new Vector2(Vel.X, Vel.Y);
            a.CreatorClickPos = new Point(CreatorClickPos.X, CreatorClickPos.Y);
            a.Texture = Texture;
            return a;
        }
    }
}
