using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreak
{
    class Brick : Sprite
    {
        public Brick(Texture2D Image, Vector2 Position,Color Tint )
            :base(Image,Position,Tint)
        {
        }
        
    }
}
