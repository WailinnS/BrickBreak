using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BrickBreak
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Ball ball;
        Paddle paddle;
        List<Brick> bricks = new List<Brick>();
        string[] brickName = new string[3] { "brick1","brick2","brick3" };
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here

            ball = new Ball(Content.Load<Texture2D>("ball"), 
                   new Vector2(GraphicsDevice.Viewport.Bounds.Center.X, 
                   GraphicsDevice.Viewport.Bounds.Center.Y),
                   Color.White,
                   new Vector2(5, 5));

            paddle = new Paddle(Content.Load<Texture2D>("paddle"), 
                     new Vector2(GraphicsDevice.Viewport.Bounds.Right/2, GraphicsDevice.Viewport.Bounds.Bottom), 
                     Color.White, 
                     new Vector2(7, 0));

            //Offsets the paddle position to be sceen on the screen.
            paddle.X -= paddle.Image.Width / 2;
            paddle.Y -= paddle.Image.Height + 10;


            int height = 0;
            for(int i = 0; i < 3; i++)
            {
                int size = 0;
                for (int j = 0; j < 8; j++)
                {
                    bricks.Add(new Brick(Content.Load<Texture2D>(brickName[i]),
                               new Vector2(GraphicsDevice.Viewport.Bounds.Left + size, GraphicsDevice.Viewport.Y + height),
                               Color.White));

                    size = bricks[j].Hitbox.Right;
                }
                height += bricks[i].Hitbox.Bottom;
               
            }



        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            // TODO: Add your update logic here

            ball.Update(GraphicsDevice.Viewport);
            
            paddle.Update(Keyboard.GetState(),GraphicsDevice.Viewport);

            //Check ball and paddle collision
            if(paddle.Hitbox.Intersects(ball.Hitbox))
            {
                Vector2 newDirection = new Vector2(ball.Speed.X, -Math.Abs(ball.Speed.Y));
                ball.Speed = newDirection;
            }

            for (int i = 0; i < bricks.Count; i++)
            {
                if(bricks[i].Hitbox.Intersects(ball.Hitbox))
                {s
                    bricks.RemoveAt(i);
                }
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            ball.Draw(spriteBatch);

            paddle.Draw(spriteBatch);

            for (int i = 0; i < bricks.Count; i++)
            {
                bricks[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
