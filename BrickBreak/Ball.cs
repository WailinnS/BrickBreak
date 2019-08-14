using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreak
{
    class Ball : Sprite
    {
        public Vector2 Speed { get; set; }

        public Ball(Texture2D texture, Vector2 vector, Color color, Vector2 speed)
            : base(texture, vector, color)
        {
            Speed = speed;
        }

        public void Update(Viewport screen)
        {
            //bouncing ball around the screen.
            Position += Speed;

            if (Hitbox.Left < screen.Bounds.Left)
            {
                Vector2 newSpeed = new Vector2(Math.Abs(Speed.X), Speed.Y);
                Speed = newSpeed;
            }
            else if (Hitbox.Right > screen.Bounds.Right)
            {
                Vector2 newSpeed = new Vector2(-Math.Abs(Speed.X), Speed.Y);
                Speed = newSpeed;
            }

            if (Hitbox.Top < screen.Bounds.Top)
            {
                Vector2 newSpeed = new Vector2(Speed.X, Math.Abs(Speed.Y));
                Speed = newSpeed;
            }
            //else if (Hitbox.Bottom > screen.Bounds.Bottom)
            //{
            //    Vector2 newSpeed = new Vector2(Speed.X, -Math.Abs(Speed.Y));
            //    Speed = newSpeed;

            //}
        }
    }
}
