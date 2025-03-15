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

namespace Battery_Life
{
    public class Wall : Obj
    {
        public Wall(Texture2D Image, Vector2 Pos) 
        {
            draw = true;
            position = Pos;
            texture = Image;
        }
    }
}
