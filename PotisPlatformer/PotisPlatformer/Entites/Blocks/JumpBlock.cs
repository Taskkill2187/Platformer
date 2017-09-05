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

        public JumpBlock(Vector2 Pos, Direction Direction, float Strength, Level Parent)
            : base (Assets.JumpBlock, Pos, false, Parent)
        {
            this.Rect = new Rectangle((int)Pos.X, (int)Pos.Y, Level.BlockScale, Level.BlockScale);
            this.Direction = Direction;
            this.Strength = Strength;
            this.Vel = Vector2.Zero;
            Friction = 1.3f;
            Collision = false;
        }

        public JumpBlock(Vector2 Pos, Direction Direction, float Strength, float Friction, Level Parent)
            : base(Assets.JumpBlock, Pos, false, Parent)
        {
            this.Rect = new Rectangle((int)Pos.X, (int)Pos.Y, Level.BlockScale, Level.BlockScale);
            this.Direction = Direction;
            this.Strength = Strength;
            this.Vel = Vector2.Zero;
            this.Friction = Friction;
        }

        public override void Update()
        {
            if (this.Rect.Intersects(Parent.ThisPlayer.Rect))
            {
                switch (Direction)
                {
                    case Direction.Up:
                        Parent.ThisPlayer.Vel.Y += -Strength;
                        Parent.ThisPlayer.Vel.X /= Friction;
                        break;

                    case Direction.Down:
                        Parent.ThisPlayer.Vel.Y += Strength;
                        Parent.ThisPlayer.Vel.X /= Friction;
                        break;

                    case Direction.Left:
                        Parent.ThisPlayer.Vel.X += -Strength;
                        Parent.ThisPlayer.Vel.Y /= Friction;
                        break;

                    case Direction.Right:
                        Parent.ThisPlayer.Vel.X += Strength;
                        Parent.ThisPlayer.Vel.Y /= Friction;
                        break;
                }
                Assets.JumpBlockSound.Play(1f, 0, 0);
            }

            for (int i = 0; i < Parent.EnemyList.Count; i++)
            {
                if (this.Rect.Intersects(Parent.EnemyList[i].Rect))
                {
                    switch (Direction)
                    {
                        case Direction.Up:
                            Parent.EnemyList[i].Vel.Y += -Strength / 5;
                            Parent.EnemyList[i].Vel.X /= Friction;
                            break;

                        case Direction.Down:
                            Parent.EnemyList[i].Vel.Y += Strength / 5;
                            Parent.EnemyList[i].Vel.X /= Friction;
                            break;

                        case Direction.Left:
                            Parent.EnemyList[i].Vel.X += -Strength / 5;
                            Parent.EnemyList[i].Vel.Y /= Friction;
                            break;

                        case Direction.Right:
                            Parent.EnemyList[i].Vel.X += Strength / 5;
                            Parent.EnemyList[i].Vel.Y /= Friction;
                            break;
                    }
                }
            }

            for (int i = 0; i < Parent.BlockList.Count; i++)
            {
                if (Parent.BlockList[i].GetType() == typeof(FallingBlock) && Rect.Intersects(Parent.BlockList[i].Rect))
                {
                    switch (Direction)
                    {
                        case Direction.Up:
                            Parent.BlockList[i].Vel.Y += -Strength / 5;
                            Parent.BlockList[i].Vel.X /= Friction;
                            break;

                        case Direction.Down:
                            Parent.BlockList[i].Vel.Y += Strength / 5;
                            Parent.BlockList[i].Vel.X /= Friction;
                            break;

                        case Direction.Left:
                            Parent.BlockList[i].Vel.X += -Strength / 5;
                            Parent.BlockList[i].Vel.Y /= Friction;
                            break;

                        case Direction.Right:
                            Parent.BlockList[i].Vel.X += Strength / 5;
                            Parent.BlockList[i].Vel.Y /= Friction;
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
                    spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)Parent.Camera.X, Rect.Y + (int)Parent.Camera.Y, Rect.Width, Rect.Height), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    break;

                case Direction.Down:
                    spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)Parent.Camera.X + Rect.Width / 2, Rect.Y + (int)Parent.Camera.Y + Rect.Height / 2, Rect.Width, Rect.Height), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, -3.14f, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 0);
                    break;

                case Direction.Left:
                    spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)Parent.Camera.X + Rect.Width / 2, Rect.Y + (int)Parent.Camera.Y + Rect.Height / 2, Rect.Width, Rect.Height), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, -1.57f, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 0);
                    break;

                case Direction.Right:
                    spriteBatch.Draw(this.Texture, new Rectangle(Rect.X + (int)Parent.Camera.X + Rect.Width / 2, Rect.Y + (int)Parent.Camera.Y + Rect.Height / 2, Rect.Width, Rect.Height), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 1.57f, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 0);
                    break;
            }
        }
    }
}
