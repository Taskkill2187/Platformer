using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer.Neural_Network
{
    public abstract class NeuralNetworkEntity
    {
        public abstract void Update();
        public abstract float GetArragementIndex();
        public abstract void Mutate();
        public abstract void Draw(SpriteBatch SB, Vector2 NeuronGrid_Middle, Vector2 NeuronGrid_Size);
    }
}
