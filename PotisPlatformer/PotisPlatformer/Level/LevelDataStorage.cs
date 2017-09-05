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
            LvlList[0].PlayerPos = new Vector2(5 * Level.BlockScale, 3 * Level.BlockScale - 131);
            LvlList[0].End.X = 80 * Level.BlockScale;
            LvlList[0].End.Y = Values.WindowSize.Y - Level.BlockScale * 7 - Level.EndHeight;

            for (int i = 1; i < 1000; i++)
            {
                LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(i * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale), true, LvlList[0]));

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
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(4 * Level.BlockScale, 6 * Level.BlockScale), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(5 * Level.BlockScale, 6 * Level.BlockScale), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(6 * Level.BlockScale, 6 * Level.BlockScale), true, LvlList[0]));

            // Stairs
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(19 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(20 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(20 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 3), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(21 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(21 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 3), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(21 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), true, LvlList[0]));

            //FakeBlocks
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(0 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale), false, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(33 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale), false, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(34 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale), false, LvlList[0]));

            //BluSpyPlatform
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(40 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(41 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(42 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), true, LvlList[0]));

            //EnemyPlatform
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(44 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), false, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(45 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), false, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(46 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), false, LvlList[0]));

            //EnemyPlatform
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(48 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), false, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(49 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), false, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(50 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), false, LvlList[0]));

            //EnemyPlatforms
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(67 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), false, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(70 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), false, LvlList[0]));
            LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(73 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), false, LvlList[0]));

            //EndPlatform
            for (int i = 0; i < 15; i++ )
            {
                LvlList[0].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2((78 + i) * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), true, LvlList[0]));
            }
        }
        public static void SpawnLevel1Enemys()
        {
            //FallingPlatform
            LvlList[0].BlockList0.Add(new FallingBlock(new Vector2(59 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new FallingBlock(new Vector2(60 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[0]));
            LvlList[0].BlockList0.Add(new FallingBlock(new Vector2(61 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[0]));

            //Enemys
            LvlList[0].EnemyList0.Add(new Goomba(1 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 2, true, LvlList[0]));
            LvlList[0].EnemyList0.Add(new Goomba(17 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 2, true, LvlList[0]));
            LvlList[0].EnemyList0.Add(new Goomba(33 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 2, true, LvlList[0]));
            LvlList[0].EnemyList0.Add(new Goomba(36 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 2, true, LvlList[0]));
            LvlList[0].EnemyList0.Add(new Goomba(45 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 6, true, LvlList[0]));
            LvlList[0].EnemyList0.Add(new Goomba(49 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 5, true, LvlList[0]));
            LvlList[0].EnemyList0.Add(new Goomba(60 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 6, true, LvlList[0]));
            LvlList[0].EnemyList0.Add(new Goomba(67 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 3, true, LvlList[0]));
            LvlList[0].EnemyList0.Add(new Goomba(70 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 5, true, LvlList[0]));
            LvlList[0].EnemyList0.Add(new Goomba(73 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 7, true, LvlList[0]));
        }

        public static void BuildLevel2()
        {
            LvlList[1] = new Level();
            LvlList[1].PlayerPos = new Vector2(2 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale - 131);
            LvlList[1].End.X = 1000 * Level.BlockScale;

            for (int i = 0; i < 1050; i++)
            {
                LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(i * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale), true, LvlList[1]));

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
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(5 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), Direction.Up, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(5 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), Direction.Right, 35, LvlList[1]));

            // Spin
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(15 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), Direction.Right, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(18 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), Direction.Left, 35, LvlList[1]));

            // Ramp 2
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(34 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 3), Direction.Up, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(34 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), Direction.Right, 50, LvlList[1]));

            //EnemyPlatform
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(48 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), true, LvlList[1]));

            //PushIntoDeath
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(56 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 9), Direction.Left, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(56 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 10), Direction.Left, 35, LvlList[1]));

            //Tube
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(63 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), Direction.Up, 50, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(63 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), Direction.Right, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(63 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), Direction.Right, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(64 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), Direction.Right, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(64 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), Direction.Right, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(65 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), Direction.Right, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(65 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), Direction.Right, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(66 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), Direction.Right, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(66 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), Direction.Right, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(67 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), Direction.Right, 50, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(67 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), Direction.Right, 50, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(68 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), Direction.Right, 100, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(68 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), Direction.Right, 200, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(69 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), Direction.Up, 35, LvlList[1]));
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(69 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), Direction.Up, 35, LvlList[1]));

            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(64 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(64 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 3), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(64 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(64 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[1]));

            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 8), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 9), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 10), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 11), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 12), true, LvlList[1]));

            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(65 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(66 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(67 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(68 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(69 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[1]));

            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(62 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 8), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(63 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 8), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(64 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 8), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(65 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 8), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(66 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 8), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(67 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 8), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(68 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 8), true, LvlList[1]));

            //ZeroGravitiy
            for (int i = 0; i < 12; i++)
            {
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((87 + i) * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), Direction.Up, 1, 1f, LvlList[1]));
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((87 + i) * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), Direction.Up, 1, 1f, LvlList[1]));
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((87 + i) * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), Direction.Up, 1, 1f, LvlList[1]));
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((87 + i) * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 7), Direction.Up, 1, 1f, LvlList[1]));
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((87 + i) * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 8), Direction.Up, 1, 1f, LvlList[1]));
            }
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(100 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), true, LvlList[1]));

            for (int i = 0; i < 12; i++)
            {
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((101 + i) * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), Direction.Up, 3, LvlList[1]));
                LvlList[1].BlockList0.Add(new JumpBlock(new Vector2((101 + i) * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), Direction.Down, 1, LvlList[1]));
            }
            LvlList[1].BlockList0.Add(new JumpBlock(new Vector2(113 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), Direction.Right, 999999, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(115 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 3), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(115 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(115 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(115 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), true, LvlList[1]));

            //Stopping Power
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(500 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 2), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(500 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 3), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(500 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 4), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(500 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 5), true, LvlList[1]));
            LvlList[1].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(500 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 6), true, LvlList[1]));
        }
        public static void SpawnLevel2Enemys()
        {
            LvlList[1].EnemyList0.Add(new Goomba(17 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 2, true, LvlList[0]));
            LvlList[1].EnemyList0.Add(new Goomba(48 * Level.BlockScale, (int)Values.WindowSize.Y - Level.BlockScale * 5, true, LvlList[0]));
        }

        public static void BuildLevel3()
        {
            LvlList[2].PlayerPos = Vector2.Zero;
            LvlList[2].End.X = 1000 * Level.BlockScale;
            for (int i = 0; i < 1000; i++)
            {
                LvlList[2].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(i * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale), true, LvlList[2]));

                if (i == 5)
                    i = 15;
            }

            LvlList[2].BlockList0.Add(new Block(Assets.BlockGrass, new Vector2(10 * Level.BlockScale, Values.WindowSize.Y - Level.BlockScale * 8), true, LvlList[2]));
        }
        public static void SpawnLevel3Enemys()
        {

        }
    }
}
