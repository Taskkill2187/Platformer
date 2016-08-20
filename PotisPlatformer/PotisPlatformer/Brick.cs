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
    public class Brick : Block
    {
        int Timer;
        int AnimState;
        const int AnimStates = 4;

        public Brick(Vector2 Pos) : base(Assets.Brick, Pos, true) { }

        public override void Activate()
        {
            ParticleManager.CreateParticleExplosionFromEntityTexture(this, new Rectangle(0, 0, 16, 16), 0.3f, 1.3f, false, true, false);
            LevelManager.CurrentLevel.BlockList.Remove(this);
            LevelManager.ThisPlayer.Vel.Y = 0;
        }

        public override void Update()
        {
            Timer++;

            if (Timer % 120 == 0)
                AnimState = 1;

            if (AnimState > 0 && Timer % 7 == 0)
                AnimState++;

            if (AnimState >= AnimStates)
                AnimState = 0;

            base.Update();
        }
        public override void Draw(SpriteBatch SB)
        {
            SB.Draw(Texture, new Rectangle(Rect.X + (int)LevelManager.Camera.X, Rect.Y + (int)LevelManager.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(AnimState * 17, 0, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
