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
    public static class LevelDataStorage
    {
        public static List<Level> LvlList = new List<Level>();

        public static void BuildLevels()
        {
            LvlList.Add(new Level());
            //LvlList.Add(new Level());
            //LvlList.Add(new Level());

            BuildLevel1();
            //BuildLevel2();
            //BuildLevel3();

            SpawnLevel1Enemys();
            //SpawnLevel2Enemys();
            //SpawnLevel3Enemys();
        }

        public static void BuildLevel1()
        {
            LvlList[0] = new Level();
            LvlList[0].PlayerPos = new Vector2(5 * LevelManager.BlockScale, 3 * LevelManager.BlockScale - 131);
            LvlList[0].End.X = 80 * LevelManager.BlockScale;
            LvlList[0].End.Y = Values.WindowSize.Y - LevelManager.BlockScale * 7 - LevelManager.EndHeight;

            for (int i = 1; i < 1000; i++)
            {
                LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(i * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale), true));

                if (i == 10)
                    i = 15;

                if (i == 21)
                    i = 26;

                if (i == 28)
                    i = 37;

                if (i == 40)
                    i = 56;

                if (i == 63)
                    i = 1000;
            }

            //SpawnPlatform
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(4 * LevelManager.BlockScale, 6 * LevelManager.BlockScale), true));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(5 * LevelManager.BlockScale, 6 * LevelManager.BlockScale), true));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(6 * LevelManager.BlockScale, 6 * LevelManager.BlockScale), true));

            // Stairs
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(19 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), true));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(20 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), true));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(20 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 3), true));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(21 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), true));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(21 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 3), true));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(21 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), true));

            //FakeBlocks
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(0 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale), false));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(33 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale), false));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(34 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale), false));

            //BluSpyPlatform
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(40 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), true));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(41 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), true));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(42 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), true));

            //EnemyPlatform
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(44 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), false));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(45 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), false));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(46 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), false));

            //EnemyPlatform
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(48 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), false));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(49 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), false));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(50 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), false));

            //EnemyPlatforms
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(67 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), false));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(70 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), false));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(73 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), false));

            //EndPlatform
            for (int i = 0; i < 15; i++ )
            {
                LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2((78 + i) * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), true));
            }
        }
        public static void SpawnLevel1Enemys()
        {
            //FallingPlatform
            LvlList[0].BlockList0.Add(new FallingBlock(new Vector2(59 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));
            LvlList[0].BlockList0.Add(new FallingBlock(new Vector2(60 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));
            LvlList[0].BlockList0.Add(new FallingBlock(new Vector2(61 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));

            //Enemys
            LvlList[0].EnemyList0.Add(new Goomba(1 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 2, true));
            LvlList[0].EnemyList0.Add(new Goomba(17 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 2, true));
            LvlList[0].EnemyList0.Add(new Goomba(33 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 2, true));
            LvlList[0].EnemyList0.Add(new Goomba(36 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 2, true));
            LvlList[0].EnemyList0.Add(new Goomba(45 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 6, true));
            LvlList[0].EnemyList0.Add(new Goomba(49 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 5, true));
            LvlList[0].EnemyList0.Add(new Goomba(60 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 6, true));
            LvlList[0].EnemyList0.Add(new Goomba(67 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 3, true));
            LvlList[0].EnemyList0.Add(new Goomba(70 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 5, true));
            LvlList[0].EnemyList0.Add(new Goomba(73 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 7, true));
        }

        public static void BuildLevel2()
        {
            LvlList[1] = new Level();
            LvlList[1].PlayerPos = new Vector2(2 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale - 131);
            LvlList[1].End.X = 1000 * LevelManager.BlockScale;

            for (int i = 0; i < 1050; i++)
            {
                LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(i * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale), true));

                if (i == 5)
                    i = 14;

                if (i == 18)
                    i = 30;

                if (i == 35)
                    i = 57;

                if (i == 64)
                    i = 130;
            }

            // StartRamp
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(5 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), Direction.Up, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(5 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), Direction.Right, 35));

            // Spin
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(15 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), Direction.Right, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(18 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), Direction.Left, 35));

            // Ramp 2
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(34 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 3), Direction.Up, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(34 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), Direction.Right, 50));

            //EnemyPlatform
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(48 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), true));

            //PushIntoDeath
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(56 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 9), Direction.Left, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(56 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 10), Direction.Left, 35));

            //Tube
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(63 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), Direction.Up, 50));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(63 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), Direction.Right, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(63 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), Direction.Right, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(64 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), Direction.Right, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(64 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), Direction.Right, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(65 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), Direction.Right, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(65 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), Direction.Right, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(66 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), Direction.Right, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(66 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), Direction.Right, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(67 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), Direction.Right, 50));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(67 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), Direction.Right, 50));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(68 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), Direction.Right, 100));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(68 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), Direction.Right, 200));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(69 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), Direction.Up, 35));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(69 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), Direction.Up, 35));

            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(64 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(64 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 3), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(64 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(64 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));

            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 8), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 9), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 10), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 11), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 12), true));

            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(65 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(66 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(67 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(68 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(69 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));

            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 8), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(63 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 8), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(64 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 8), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(65 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 8), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(66 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 8), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(67 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 8), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(68 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 8), true));

            //ZeroGravitiy
            for (int i = 0; i < 12; i++)
            {
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((87 + i) * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), Direction.Up, 1, 1f));
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((87 + i) * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), Direction.Up, 1, 1f));
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((87 + i) * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), Direction.Up, 1, 1f));
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((87 + i) * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 7), Direction.Up, 1, 1f));
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((87 + i) * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 8), Direction.Up, 1, 1f));
            }
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(100 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), true));

            for (int i = 0; i < 12; i++)
            {
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((101 + i) * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), Direction.Up, 3));
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((101 + i) * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), Direction.Down, 1));
            }
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(113 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), Direction.Right, 999999));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(115 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 3), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(115 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(115 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(115 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), true));

            //Stopping Power
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(500 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 2), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(500 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 3), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(500 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 4), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(500 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 5), true));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(500 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 6), true));
        }
        public static void SpawnLevel2Enemys()
        {
            LvlList[1].EnemyList0.Add(new Goomba(17 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 2, true));
            LvlList[1].EnemyList0.Add(new Goomba(48 * LevelManager.BlockScale, (int)Values.WindowSize.Y - LevelManager.BlockScale * 5, true));
        }

        public static void BuildLevel3()
        {
            LvlList[2].PlayerPos = Vector2.Zero;
            LvlList[2].End.X = 1000 * LevelManager.BlockScale;
            for (int i = 0; i < 1000; i++)
            {
                LvlList[2].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(i * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale), true));

                if (i == 5)
                    i = 15;
            }

            LvlList[2].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(10 * LevelManager.BlockScale, Values.WindowSize.Y - LevelManager.BlockScale * 8), true));
        }
        public static void SpawnLevel3Enemys()
        {

        }
    }
}
