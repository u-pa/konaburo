using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace konaburo
{
    /* ブロックの破片 */
    public class Block_Particle : Obj_Base
    {
        public Vector2 speed = new Vector2(0, 0.0f);
        public Vector2 accel = new Vector2(0, 0.01f);

        public Block_Particle()
        {
            size.X = 1.0f;
            size.Y = 1.0f;
        }

        public void Move()
        {
            pos.X += speed.X;
            pos.Y += speed.Y;
            speed.X += accel.X;
            speed.Y += accel.Y;
        }
    }
}
