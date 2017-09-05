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
    public class FallingBlock : Block
    {
        public bool Falling;

        public FallingBlock(Vector2 Pos, bool Collision, Level Parent) : base(Assets.BlockGrass, Pos, Collision, Parent)
        {
            this.Rect = new Rectangle((int)Pos.X, (int)Pos.Y, Level.BlockScale, Level.BlockScale);
            this.Vel = Vector2.Zero;
            Falling = false;
        }

        public override void Update()
        {
            if (Parent.ThisPlayer.Rect.Intersects(new Rectangle(this.Rect.X, this.Rect.Y + Level.BlockScale, this.Rect.Width, (int)Values.WindowSize.Y)))
            {
                Falling = true;
                Collision = false;
            }

            if (Falling)
            {
                Vel.Y += 1f;

                if (Parent.ThisPlayer.Rect.Intersects(this.Rect) && Parent.ThisPlayer.DeathTimer == 0)
                {
                    Parent.ThisPlayer.OnDeath();
                }
            }

            Rect = new Rectangle(Rect.X + (int)Vel.X, Rect.Y + (int)Vel.Y, Rect.Width, Rect.Height);
        }
    }
}
