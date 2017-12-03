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
using System.Xml.Serialization;

namespace Platformer
{
    [XmlInclude(typeof(SpinningBlock))]
    public class SpinningBlock : Block
    {
        int Timer;
        int AnimState;
        public static new int AnimStates = 8;

        public SpinningBlock() { }
        public SpinningBlock(Vector2 Pos, Level Parent) : base(Assets.SpinningBlock, Pos, true, Parent) { }

        public override void UpdateTextureReference()
        {
            Texture = Assets.SpinningBlock;
        }

        public override void Update()
        {
            if (Parent.ThisPlayer.Rect.X > Rect.X - Parent.ThisPlayer.Rect.Width && Parent.ThisPlayer.Rect.X < Rect.X + Rect.Width &&
                Parent.ThisPlayer.Rect.Y == Rect.Y + Rect.Height && Parent.ThisPlayer.TimesJumped > 0 && Parent.ThisPlayer.Vel.Y <= 0)
            {
                AnimState = 1;
                Collision = false;
            }

            Timer++;

            if (AnimState > 0 && Timer % (int)(25 * (AnimState / (float)AnimStates)) == 0)
                AnimState++;

            if (AnimState >= AnimStates)
            {
                AnimState = 0;
                Collision = true;
            }
        }
        public override void Draw(SpriteBatch SB)
        {
            SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(AnimState * 16, 0, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
