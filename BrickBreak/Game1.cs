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
        //int extraBallNum;
        int extraLifeNum;

        //This is old an dumb...
        //string[] brickName = new string[3] { "brick1","brick2","brick3" };

        //new way to change the tint of the bricks.
        Color[] colors = { Color.Green, Color.Orange, Color.CornflowerBlue };


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
            for (int i = 0; i < 3; i++)
            {
                int size = 0;
                for (int j = 0; j < 8; j++)
                {
                    bricks.Add(new Brick(Content.Load<Texture2D>("brickTile"),
                               new Vector2(GraphicsDevice.Viewport.Bounds.Left + size, GraphicsDevice.Viewport.Y + height),
                               colors[i]));

                    //bricks.Add(new Brick(Content.Load<Texture2D>("brickTile"),
                    //           new Vector2(GraphicsDevice.Viewport.Bounds.Left + size, GraphicsDevice.Viewport.Y + height + 300),
                    //           Color.White));

                    size = bricks[j].Hitbox.Right;
                }
                height += bricks[i].Hitbox.Bottom;
            }

            //TO DO: power ups
            //extraBallNum = randomNum.Next(bricks.Count );
            extraLifeNum = randomNum.Next(bricks.Count);
            //bricks[extraBallNum] = new PowerUpBrick(bricks[extraBallNum].Image, bricks[extraBallNum].Position, bricks[extraBallNum].Tint,randomNum.Next(2));
            bricks[extraLifeNum] = new PowerUpBrick(bricks[extraLifeNum].Image, bricks[extraLifeNum].Position, bricks[extraLifeNum].Tint, randomNum.Next(2));
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
                //check for regualr bricks then power up bricks


                for (int i = 0; i < bricks.Count; i++)
                {
                    if (bricks[i].Hitbox.Intersects(ball.Hitbox))
                    {
                        ball.Speed = new Vector2(ball.Speed.X, Math.Abs(ball.Speed.Y));

                        if (bricks[i] is PowerUpBrick powerUp)
                        {
                            switch (powerUp.powerType)
                            {
                                case PowerType.extraLife:
                                    life++;
                                    break;
                                case PowerType.extraBall:
                                    //In future Update...
                                    break;
                                default:
                                    break;
                            }

                        }

                        bricks.RemoveAt(i);
                        score++;
                    }

                }


                if (bricks.Count < 1 || life <= 0)
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
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                score = 0;
                life = 3;
                startOfGame = true;
                gameOver = false;
                bricks.Clear(); //clears the list
                makeBricks();
                ball.Setup(GraphicsDevice.Viewport);
            }


        }

        //if the ball goes past bottom of the scrren
        private void outOfBounds()
        {
            //reset the ball position and take away life
            life--;
            ball.Setup(GraphicsDevice.Viewport);
            if (life >= 1)
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

            //debug
            //spriteBatch.DrawString(font, $"Count: {bricks.Count}",new Vector2( GraphicsDevice.Viewport.Width -200, GraphicsDevice.Viewport.Height - 100), Color.White);
            ball.Draw(spriteBatch);

            paddle.Draw(spriteBatch);

            for (int i = 0; i < bricks.Count; i++)
            {
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
