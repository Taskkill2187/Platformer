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
    public class QuestionMarkBlock : Block
    {
        int Timer;
        int AnimState;
        const int AnimStates = 4;

        Entity Content;

        public QuestionMarkBlock(Vector2 Pos, Entity Content, Level Parent) : base(Assets.QuestionMarkBlock, Pos, true, Parent)
        {
            this.Content = Content;
        }

        public override void Activate()
        {
            if (Content != null)
            {
                Content.Rect.X = Rect.X;
                Content.Rect.Y = Rect.Y - Content.Rect.Height;
                if (Content.GetType() == typeof(Coin))
                {
                    ParticleManager.CreateParticleExplosionFromEntityTexture(new Coin(new Vector2(Rect.X, Rect.Y - Level.BlockScale), Parent), new Rectangle(0, 0, 16, 16),
                        -0.2f, 5f, false, true, false, Parent);
                    if (StoredData.Default.SoundEffects && Parent.IsDisplayed)
                        Assets.CoinSound.Play(0.75f, 0, 0);
                    Parent.Score += 100;
                }
                else
                {
                    try { Parent.BlockList.Add((Block)Content); } catch { }
                    try { Parent.EnemyList.Add((Enemy)Content); } catch { }
                }
                Content = null;
            }
        }

        public override void Update()
        {
            Timer++;

            if (Timer % 7 == 0)
                AnimState++;

            if (AnimState >= AnimStates)
                AnimState = 0;

            base.Update();
        }
        public override void Draw(SpriteBatch SB)
        {
            if (Content == null)
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(0, 0, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            else
            {
                SB.Draw(Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height),
                        new Rectangle(AnimState * 17 + 17, 0, 16, 16), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
        }
    }
}
