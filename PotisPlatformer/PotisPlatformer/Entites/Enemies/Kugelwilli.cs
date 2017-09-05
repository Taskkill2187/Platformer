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
    public class Kugelwilli : Enemy
    {
        public Kugelwilli(int PosX, int PosY, bool FacingRight, Level Parent) : base(PosX, PosY, FacingRight, 7.5f, Parent) { GravForce = 0; Texture = Assets.KugelWilli; }

        public override void CheckForCollision() { }
        public override void OnDeath()
        {
            Parent.EnemyList.Remove(this);
            ParticleManager.CreateParticleExplosionFromEntityTexture(this, new Rectangle(0, 0, 17, 14), 0.4f, 0, FacingRight, false, false, new Vector2(0, -4), Parent);
        }
        public override void Draw(SpriteBatch SB)
        {
            SpriteEffects E;

            if (FacingRight)
                E = SpriteEffects.FlipHorizontally;
            else
                E = SpriteEffects.None;

            base.Draw(SB, Color.White, E);
        }
    }
}
