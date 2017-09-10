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
    public class Goomba : Enemy
    {
        public Goomba() { }
        public Goomba(int PosX, int PosY, bool FacingRight, Level Parent) : base (PosX, PosY, FacingRight, 2.5f, Parent)
        {
            Texture = Assets.Walker;
            WalkAnimStates = 10;
        }

        public override void OnDeath()
        {
            int index = Parent.EnemyList.IndexOf(this);
            Parent.EnemyList.RemoveAt(index);
            ParticleManager.CreateParticleExplosionFromEntityTexture(this, new Rectangle(0, 0, 16, 16), 0.3f, -10f, !FacingRight, true, false, Parent);
        }
        public override void Draw(SpriteBatch SB)
        {
            if (Texture == null)
            {
                SB.Draw(Assets.White, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                    new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            else if (FacingRight)
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                new Rectangle(16 * WalkAnimState, 0, 16, Texture.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            else
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                new Rectangle(16 * WalkAnimState, 0, 16, Texture.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
            }
        }
    }
}
