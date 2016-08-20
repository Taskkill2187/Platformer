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
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public class JumpBlock : Block
    {
        public Direction Direction;
        public float Strength;
        public float Friction;

        public JumpBlock(Vector2 Pos, Direction Direction, float Strength)
            : base (Assets.JumpBlock, Pos, false)
        {
            this.Rect = new Rectangle((int)Pos.X, (int)Pos.Y, LevelManager.BlockScale, LevelManager.BlockScale);
            this.Direction = Direction;
            this.Strength = Strength;
            this.Vel = Vector2.Zero;
            Friction = 1.3f;
            Collision = false;
        }

        public JumpBlock(Vector2 Pos, Direction Direction, float Strength, float Friction)
            : base(Assets.JumpBlock, Pos, false)
        {
            this.Rect = new Rectangle((int)Pos.X, (int)Pos.Y, LevelManager.BlockScale, LevelManager.BlockScale);
            this.Direction = Direction;
            this.Strength = Strength;
            this.Vel = Vector2.Zero;
            this.Friction = Friction;
        }

        public override void Update()
        {
            if (this.Rect.Intersects(LevelManager.ThisPlayer.Rect))
            {
                switch (Direction)
                {
                    case Direction.Up:
                        LevelManager.ThisPlayer.Vel.Y += -Strength;
                        LevelManager.ThisPlayer.Vel.X /= Friction;
                        break;

                    case Direction.Down:
                        LevelManager.ThisPlayer.Vel.Y += Strength;
                        LevelManager.ThisPlayer.Vel.X /= Friction;
                        break;

                    case Direction.Left:
                        LevelManager.ThisPlayer.Vel.X += -Strength;
                        LevelManager.ThisPlayer.Vel.Y /= Friction;
                        break;

                    case Direction.Right:
                        LevelManager.ThisPlayer.Vel.X += Strength;
                        LevelManager.ThisPlayer.Vel.Y /= Friction;
                        break;
                }
                Assets.JumpBlockSound.Play(1f, 0, 0);
            }

            for (int i = 0; i < LevelManager.CurrentLevel.EnemyList.Count; i++)
            {
                if (this.Rect.Intersects(LevelManager.CurrentLevel.EnemyList[i].Rect))
                {
                    switch (Direction)
                    {
                        case Direction.Up:
                            LevelManager.CurrentLevel.EnemyList[i].Vel.Y += -Strength / 5;
                            LevelManager.CurrentLevel.EnemyList[i].Vel.X /= Friction;
                            break;

                        case Direction.Down:
                            LevelManager.CurrentLevel.EnemyList[i].Vel.Y += Strength / 5;
                            LevelManager.CurrentLevel.EnemyList[i].Vel.X /= Friction;
                            break;

                        case Direction.Left:
                            LevelManager.CurrentLevel.EnemyList[i].Vel.X += -Strength / 5;
                            LevelManager.CurrentLevel.EnemyList[i].Vel.Y /= Friction;
                            break;

                        case Direction.Right:
                            LevelManager.CurrentLevel.EnemyList[i].Vel.X += Strength / 5;
                            LevelManager.CurrentLevel.EnemyList[i].Vel.Y /= Friction;
                            break;
                    }
                }
            }

            for (int i = 0; i < LevelManager.CurrentLevel.BlockList.Count; i++)
            {
                if (LevelManager.CurrentLevel.BlockList[i].GetType() == typeof(FallingBlock) && Rect.Intersects(LevelManager.CurrentLevel.BlockList[i].Rect))
                {
                    switch (Direction)
                    {
                        case Direction.Up:
                            LevelManager.CurrentLevel.BlockList[i].Vel.Y += -Strength / 5;
                            LevelManager.CurrentLevel.BlockList[i].Vel.X /= Friction;
                            break;

                        case Direction.Down:
                            LevelManager.CurrentLevel.BlockList[i].Vel.Y += Strength / 5;
                            LevelManager.CurrentLevel.BlockList[i].Vel.X /= Friction;
                            break;

                        case Direction.Left:
                            LevelManager.CurrentLevel.BlockList[i].Vel.X += -Strength / 5;
                            LevelManager.CurrentLevel.BlockList[i].Vel.Y /= Friction;
                            break;

                        case Direction.Right:
                            LevelManager.CurrentLevel.BlockList[i].Vel.X += Strength / 5;
                            LevelManager.CurrentLevel.BlockList[i].Vel.Y /= Friction;
                            break;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (Direction)
            {
                case Direction.Up:
                    spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)LevelManager.Camera.X, Rect.Y + (int)LevelManager.Camera.Y, Rect.Width, Rect.Height), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;

                case Direction.Down:
                    spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)LevelManager.Camera.X + Rect.Width / 2, Rect.Y + (int)LevelManager.Camera.Y + Rect.Height / 2, Rect.Width, Rect.Height), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, -3.14f, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 0);
                    break;

                case Direction.Left:
                    spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)LevelManager.Camera.X + Rect.Width / 2, Rect.Y + (int)LevelManager.Camera.Y + Rect.Height / 2, Rect.Width, Rect.Height), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, -1.57f, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 0);
                    break;

                case Direction.Right:
                    spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)LevelManager.Camera.X + Rect.Width / 2, Rect.Y + (int)LevelManager.Camera.Y + Rect.Height / 2, Rect.Width, Rect.Height), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 1.57f, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 0);
                    break;
            }
        }
    }
}
