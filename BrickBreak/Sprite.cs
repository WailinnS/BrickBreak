using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreak
{
    class Sprite
    {
        public Texture2D Image { get; set; }
        public Vector2 Position { get; set; }
        public Color Tint { get; set; }
        public float X
        {
            get => Position.X;
            set
            {
                Position = new Vector2(value, Position.Y);
            }
        }
        public float Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position = new Vector2(Position.X, value);
            }
        }


        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            }
        }


        public Sprite(Texture2D image, Vector2 position, Color tint)
        {
            Image = image;
            Position = position;
            Tint = tint;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, Tint);
        }
    }
}
