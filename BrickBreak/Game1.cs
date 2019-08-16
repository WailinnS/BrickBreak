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
        bool startOfGame = true;
        bool gameOver = false;
        //This is old an dumb...
        //string[] brickName = new string[3] { "brick1","brick2","brick3" };

        //new way to change the tint of the bricks.
        Color[] colors = { Color.LightGreen, Color.HotPink, Color.LightBlue };


        SpriteFont font;

        private int score = 0;
        private int life = 3;
        Random randomNum = new Random();
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

            IsMouseVisible = true;
            //Window.AllowUserResizing = true;
            //Window.ClientSizeChanged += (s, e) => Window.Title = $"New size: {Window.ClientBounds.Width} x {Window.ClientBounds.Height}";
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
            font = Content.Load<SpriteFont>("font");

            ball = new Ball(Content.Load<Texture2D>("ball"),
                   new Vector2(GraphicsDevice.Viewport.Bounds.Center.X,
                   GraphicsDevice.Viewport.Bounds.Center.Y - 50),
                   Color.White,
                   new Vector2(0, 0));

            paddle = new Paddle(Content.Load<Texture2D>("paddle"),
                     new Vector2(GraphicsDevice.Viewport.Bounds.Right / 2, GraphicsDevice.Viewport.Bounds.Bottom),
                     Color.White,
                     new Vector2(7, 0));

            //Offsets the paddle position to be sceen on the screen.
            paddle.X -= paddle.Image.Width / 2;
            paddle.Y -= paddle.Image.Height + 10;


            makeBricks();

        }

        private void makeBricks()
        {
            int height = 0;
            for (int i = 0; i < 1; i++)
            {
                int size = 0;
                for (int j = 0; j < 8; j++)
                {
                    bricks.Add(new Brick(Content.Load<Texture2D>("brickTile"),
                               new Vector2(GraphicsDevice.Viewport.Bounds.Left + size, GraphicsDevice.Viewport.Y + height),
                               colors[i]));

                    //bricks.Add(new Brick(Content.Load<Texture2D>(brickName[i]),
                    //           new Vector2(GraphicsDevice.Viewport.Bounds.Left + size, GraphicsDevice.Viewport.Y + height + 300),
                    //           Color.White));

                    size = bricks[j].Hitbox.Right;
                }
                height += bricks[i].Hitbox.Bottom;
            }

            //TO DO: power ups

            //bricks[1] = new PowerUpBrick(bricks[1].Image, bricks[1].Position, bricks[1].Tint);
            //bricks[20] = new PowerUpBrick(bricks[20].Image, bricks[20].Position, bricks[20].Tint);
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
            if (!startOfGame)
            {
              
                ball.Update(GraphicsDevice.Viewport);

                paddle.Update(Keyboard.GetState(), GraphicsDevice.Viewport);

                //Checks if the ball hits the bottom of the screen.
                if (ball.Hitbox.Bottom > GraphicsDevice.Viewport.Bounds.Bottom)
                {
                   outOfBounds();
                }

                //Check ball and paddle collision
                if (paddle.Hitbox.Intersects(ball.Hitbox))
                {
                    Vector2 newDirection = new Vector2(ball.Speed.X, -Math.Abs(ball.Speed.Y));
                    ball.Speed = newDirection;
                }

                //ball hits bricks
               
                    for (int i = 0; i < bricks.Count; i++)
                    {
                        if (bricks[i].Hitbox.Intersects(ball.Hitbox))
                        {
                            if(bricks.Count == 1)
                        {

                        }
                            ball.Speed = new Vector2(ball.Speed.X, Math.Abs(ball.Speed.Y));
                            bricks.RemoveAt(i);
                            score++;
                        }
                    }
                
                //else
                //{
                //    if(bricks[0].Hitbox.Intersects(ball.Hitbox))
                //    {
                //        bricks.Remove(bricks[0]);
                //        score++;
                //    }
                //}

                

                if (bricks.Count < 0 || life < 0)
                {
                    gameOver = true;
                }

            }
            base.Update(gameTime);
        }

        private void waitToStart()
        {

            spriteBatch.DrawString(font,
                                   "Press Space to Start!",
                                   new Vector2(GraphicsDevice.Viewport.Width / 3, GraphicsDevice.Viewport.Height / 2),
                                   Color.White);


            //Press space to start the game
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                startOfGame = false;
                float randomSpeed = randomNum.Next(1, 101);
                if (randomSpeed % 2 == 0)
                {
                    ball.Speed = new Vector2(5, 5);
                }
                else
                {
                    ball.Speed = new Vector2(-5, 5);
                }

            }


        }
        private void endScreen()
        {
            ball.Speed = new Vector2(0, 0);
            spriteBatch.DrawString(font,
                                  $"Game Over! \n" +
                                  "Press r to restart \n" +
                                  $"Your Score: {score}",
                                  new Vector2(GraphicsDevice.Viewport.Width / 3, GraphicsDevice.Viewport.Height / 2),
                                  Color.White);
            if(Keyboard.GetState().IsKeyDown(Keys.R))
            {
                score = 0;
                life = 3;
                startOfGame = true;
                gameOver = false;
                makeBricks();
            }


        }

        //if the ball goes past bottom of the scrren
        private void outOfBounds()
        {
            //reset the ball position and take away life
            life--;
            ball.Setup(GraphicsDevice.Viewport);
            if(life > 0)
            {
                startOfGame = true;
            }
            else
            {
                gameOver = true;
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.DrawString(font, $"Lives: {life}",
                                   new Vector2(50, GraphicsDevice.Viewport.Height - 50),
                                   Color.White);
            spriteBatch.DrawString(font, $"Score: {score}",
                                   new Vector2(GraphicsDevice.Viewport.Width - 150, GraphicsDevice.Viewport.Height - 50),
                                   Color.White);
            spriteBatch.DrawString(font, $"Count: {bricks.Count}",new Vector2( GraphicsDevice.Viewport.Width -200, GraphicsDevice.Viewport.Height - 100), Color.White);
            ball.Draw(spriteBatch);

            paddle.Draw(spriteBatch);

            for (int i = 0; i < bricks.Count; i++)
            {
                if (bricks.Count == 1)
                {

                }
                bricks[i].Draw(spriteBatch);
            }

            if (startOfGame)
            {
                waitToStart();
            }
             if (gameOver)
            {
                endScreen();
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
