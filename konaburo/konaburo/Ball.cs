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
    /* ボール */
    class Ball : Obj_Base
    {
        public Vector2 speed = new Vector2(2.0f, 2.0f);

        public Ball()
        {
            size.X = 16.0f;
            size.Y = 16.0f;
        }

        public void Move()
        {
            pos.X += speed.X;
            pos.Y += speed.Y;
        }
    }
}
