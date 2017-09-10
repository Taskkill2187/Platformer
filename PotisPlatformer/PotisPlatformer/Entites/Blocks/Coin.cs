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
    public class Coin : Block
    {
        int Timer;
        int AnimState;
        const int AnimStates = 4;

        public Coin()
        {

        }
        public Coin(Vector2 Pos, Level Parent) : base(Assets.Coin, Pos, false, Parent)
        {

        }

        public override void Update()
        {
            if (Rect.Intersects(Parent.ThisPlayer.Rect))
            {
                Parent.BlockList.Remove(this);
                if (StoredData.Default.SoundEffects && Parent.IsDisplayed)
                    Assets.CoinSound.Play(0.75f, 0, 0);
                ParticleManager.CreateParticleExplosionFromEntityTexture(this, new Rectangle(AnimState * 16, 0, 16, 16), 0, 5f, false, true, true, Parent);
                Parent.Score += 100;
            }

            Timer++;

            if (Timer % 7 == 0)
                AnimState++;

            if (AnimState >= AnimStates)
                AnimState = 0;
        }
        public override void Draw(SpriteBatch SB)
        {
            SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(AnimState * 16, 0, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
