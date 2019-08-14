using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrickBreak
{
    //things to look up, color Lerp, and white png files for bricks.
    class PowerUpBrick : Brick
    {
        Color color1;
        Color color2;
        float currentLerpStep = 0;
        float step = .005f;

        public PowerUpBrick(Texture2D image, Vector2 position, Color tint)
            : base(image, position, tint)
        {
            color1 = Color.Lerp(Color.White, tint, 0.5f);
            color2 = Color.Lerp(tint, Color.Black, 0.1f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Tint = Color.Lerp(color1, color2, currentLerpStep);

            currentLerpStep += step;
            if(currentLerpStep >= 1 || currentLerpStep <= 0)
            {
                step *= -1;
            }

            base.Draw(spriteBatch);
        }

    }
}
