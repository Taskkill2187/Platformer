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
    [XmlInclude(typeof(Kugelwilli_Spawner))]
    public class Kugelwilli_Spawner : Block
    {
        int SpawnTimer;
        const int SpawnTime = 100;

        public Kugelwilli_Spawner() { }
        public Kugelwilli_Spawner(Vector2 Pos, Level Parent) : base(Assets.KugelWilli_Spawner, Pos, true, Parent) { Rect.Height *= 2; }

        public override void UpdateTextureReference()
        {
            Texture = Assets.KugelWilli_Spawner;
        }

        public override void Update()
        {
            SpawnTimer++;

            if (SpawnTimer > SpawnTime)
            {
                if (Parent.ThisPlayer.Rect.X > Rect.X)
                    Parent.EnemyList.Add(new Kugelwilli(Rect.X, Rect.Y, true, Parent));
                else
                    Parent.EnemyList.Add(new Kugelwilli(Rect.X, Rect.Y, false, Parent));

                SpawnTimer = 0;
            }
        }

        public override void Draw(SpriteBatch SB)
        {
            SB.Draw(Assets.KugelWilli_Spawner, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), Color.White);
        }
    }
}
