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
    public class Obj_Base
    {
        public Vector2 pos = new Vector2(0.0f, 0.0f);
        public Vector2 size = new Vector2(0.0f, 0.0f);
        public bool enable = false;
    }
}
