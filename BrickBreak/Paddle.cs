using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreak
{
    class Paddle:Sprite
    {
        public Vector2 Speed { get; set; }

        private bool moveLeft = false;
        private bool moveRight = false;

        public Paddle(Texture2D Image, Vector2 Position, Color Tint,Vector2 Speed)
            : base(Image, Position, Tint)
        {
            this.Speed = Speed;
        }

        public void Update(KeyboardState ks,Viewport sceen)
        {
            //moveing the paddle
            if(ks.IsKeyDown(Keys.Left))
            {
                moveLeft = true;
            }
            else if(ks.IsKeyUp(Keys.Left))
            {
                moveLeft = false;
            }

            if(ks.IsKeyDown(Keys.Right))
            {
                moveRight = true;
            }
            else if(ks.IsKeyUp(Keys.Right))
            {
                moveRight = false;
            }


            if(moveLeft && Position.X > sceen.Bounds.Left)
            {
                Position -= Speed;
            }
            if(moveRight && Position.X + Image.Width < sceen.Bounds.Right)
            {
                Position += Speed;
            }
           
        }

    }
}
