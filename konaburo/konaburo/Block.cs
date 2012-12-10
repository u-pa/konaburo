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
    /* ブロック */
    class Block : Obj_Base
    {
        public Block()
        {
            size.X = 32.0f;
            size.Y = size.X / 4;
        }
        
    }
}
