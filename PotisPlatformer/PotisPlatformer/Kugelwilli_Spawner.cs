﻿using System;
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
    public class Kugelwilli_Spawner : Block
    {
        int SpawnTimer;
        const int SpawnTime = 180;
        public Kugelwilli_Spawner(Vector2 Pos) : base(Assets.KugelWilli_Spawner, Pos, true) { Rect.Height *= 2; }

        public override void Update()
        {
            SpawnTimer++;

            if (SpawnTimer > SpawnTime)
            {
                if (LevelManager.ThisPlayer.Rect.X > Rect.X)
                    LevelManager.CurrentLevel.EnemyList.Add(new Kugelwilli(Rect.X, Rect.Y, true));
                else
                    LevelManager.CurrentLevel.EnemyList.Add(new Kugelwilli(Rect.X, Rect.Y, false));

                SpawnTimer = 0;
            }
        }

        public override void Draw(SpriteBatch SB)
        {
            SB.Draw(Assets.KugelWilli_Spawner, new Rectangle(Rect.X + (int)LevelManager.Camera.X, Rect.Y + (int)LevelManager.Camera.Y, Rect.Width, Rect.Height), Color.White);
        }
    }
}