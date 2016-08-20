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

        public FallingBlock(Vector2 Pos, bool Collision) : base(Assets.BlockGrass, Pos, Collision)
        {
            this.Rect = new Rectangle((int)Pos.X, (int)Pos.Y, LevelManager.BlockScale, LevelManager.BlockScale);
            this.Vel = Vector2.Zero;
            Falling = false;
        }

        public override void Update()
        {
            if (LevelManager.ThisPlayer.Rect.Intersects(new Rectangle(this.Rect.X, this.Rect.Y + LevelManager.BlockScale, this.Rect.Width, (int)Values.WindowSize.Y)))
            {
                Falling = true;
                Collision = false;
            }

            if (Falling)
            {
                Vel.Y += 1f;

                if (LevelManager.ThisPlayer.Rect.Intersects(this.Rect) && LevelManager.ThisPlayer.DeathTimer == 0)
                {
                    LevelManager.ThisPlayer.OnDeath();
                }
            }

            Rect = new Rectangle(Rect.X + (int)Vel.X, Rect.Y + (int)Vel.Y, Rect.Width, Rect.Height);
        }
    }
}
