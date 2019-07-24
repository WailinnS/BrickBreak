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

        private bool _moveleft = false;
        private bool _moveRight = false;

        public Paddle(Texture2D Image, Vector2 Position, Color Tint,Vector2 Speed)
            : base(Image, Position, Tint)
        {
            this.Speed = Speed;
        }

        public void MovePaddle(KeyboardState ks)
        {
            if(ks.IsKeyDown(Keys.Left))
            {
                _moveleft = true;
            }
            if(ks.IsKeyUp(Keys.Left))
            {
                _moveleft = false;
            }

            if(_moveleft)
            {
                Position += Speed;
            }
            else
            {
                Position = Position;
            }
        }

    }
}
