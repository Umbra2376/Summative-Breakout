using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Particle_Generator;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace Summative_Breakout
{
    public enum Screen
    {
        Title, Game, Win, Lose
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D title, blockT, paddleT, ballT, lose, paddleFire, brokenBlock;
        Rectangle window, paddleR, ballR;
        Vector2 ballS = new Vector2(5, -5);
        SpriteFont blockFont;
        SoundEffect paddleSound;
        SoundEffectInstance paddleInstance;
        Song titleSong, mainSong, loseSong;
        float titleO, resetTime, blockTime;
        double titleTime;
        KeyboardState keyboardState;
        ParticleEngine paddleEngine, blockEngine;
        List<Block> blocks =  new List<Block>();
        Paddle paddle;
        Ball ball;
        
        private Screen screen;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screen = Screen.Title;
            window = new Rectangle(0, 0, 845, 600);
            paddleR = new Rectangle(325, 550, 100, 20);
            ballR = new Rectangle(355, 520, 30, 30);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();
            base.Initialize();
            paddle = new Paddle(paddleT, paddleR, window);
            ball = new Ball(ballT, ballR, ballS, window);
            paddleEngine = new ParticleEngine(paddleFire, new Vector2(paddle.Rect.Center.X, paddle.Rect.Center.Y), false, false);
            blockEngine = new ParticleEngine(brokenBlock, new Vector2(3, 3), false, true);
            blockEngine.TTL = 30;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            title = Content.Load<Texture2D>("blockBreakerTitle");
            paddleT = Content.Load<Texture2D>("paddle");
            blockFont = Content.Load<SpriteFont>("blockFont");
            blockT = Content.Load<Texture2D>("tile");
            lose = Content.Load<Texture2D>("loseScreen");
            CreateBlocks();
            paddleFire = (Content.Load<Texture2D>("paddleFire"));
            brokenBlock = (Content.Load<Texture2D>("brokenBlock"));
            ballT = Content.Load<Texture2D>("ballBreakout");
            paddleSound = Content.Load<SoundEffect>("Flamethrower");
            paddleInstance = paddleSound.CreateInstance();
            paddleInstance.IsLooped = true;
            titleSong = Content.Load<Song>("titleSong");
            mainSong = Content.Load<Song>("mainblockTheme");
            loseSong = Content.Load<Song>("loseSong");
            MediaPlayer.Volume = 0.6f;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (screen == Screen.Title)
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(titleSong);
                }
                keyboardState = Keyboard.GetState();
                titleTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (titleTime >= 0.5)
                {
                    titleO = titleO == 1 ? 0 : 1;
                    titleTime = 0;
                }
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    titleTime = 0;
                    screen = Screen.Game;
                    MediaPlayer.Stop();
                }
            }
            if (screen == Screen.Game)
            {
                blockTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (MediaPlayer.State == MediaState.Stopped)                
                    MediaPlayer.Play(mainSong);
                keyboardState = Keyboard.GetState();
                paddleEngine.Enabled = false;
                foreach (Block block in blocks)
                {
                    if (block.IsAlive && ball.Rect.Intersects(block.Rect))
                    {
                        block.Hit();
                        blockTime = 0;
                        blockEngine.Enabled = true;
                        blockEngine.EmitterLocation = new Vector2(block.Rect.Center.X, block.Rect.Center.Y);
                        blockEngine.EmitterDirection = Vector2.Zero;
                        if (blockTime >= 0.3)
                            blockEngine.Enabled = false;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    paddle.MoveLeft();
                    paddleInstance.Play();
                    paddleEngine.Enabled = true;
                    paddleEngine.EmitterLocation = new Vector2(paddle.Rect.Right, paddle.Rect.Center.Y);
                    paddleEngine.EmitterDirection = new Vector2(3, 0);
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    paddle.MoveRight();
                    paddleInstance.Play();
                    paddleEngine.Enabled = true;
                    paddleEngine.EmitterLocation = new Vector2(paddle.Rect.Left, paddle.Rect.Center.Y);
                    paddleEngine.EmitterDirection = new Vector2(-3, 0);
                }
                else
                    paddleInstance.Stop();
                ball.Update(paddle, blocks);
                paddleEngine.Update();
                blockEngine.Update();
                if (ball.Rect.Top >= window.Bottom)
                {
                    screen = Screen.Lose;
                    MediaPlayer.Stop();
                    paddleInstance.Stop(true);
                }
            if (screen == Screen.Lose)
            {
                resetTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (MediaPlayer.State == MediaState.Stopped)
                   MediaPlayer.Play(loseSong);
            }
        }
            base.Update(gameTime);
    }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (screen == Screen.Title)
            {
                _spriteBatch.Draw(title, window, Color.White);
                _spriteBatch.DrawString(blockFont, "Press ENTER", new Vector2(70, 500), Color.White * titleO);
            }
            else if (screen == Screen.Game)
            {
                foreach (Block block in blocks)
                {
                    block.Draw(_spriteBatch);
                }
                paddleEngine.Draw(_spriteBatch);
                blockEngine.Draw(_spriteBatch);
                paddle.Draw(_spriteBatch);
                ball.Draw(_spriteBatch);
            }
            else if (screen == Screen.Lose)
            {
                _spriteBatch.Draw(lose, window, Color.White);
            }
                _spriteBatch.End();
            base.Draw(gameTime);
        }
        private void CreateBlocks()
        {
            int rows = 8;
            int columns = 13;
            int brickWidth = 60;
            int brickHeight = 25;
            int spacing = 10;
            int startX = 5;
            int startY = 5;
            for (int row = 0; row < rows; row++)
            {
                Color[] colors =
                {
                    Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Cyan, Color.Blue, Color.Purple, Color.Pink
                };
                Color blockColor = colors[row];
                for (int column = 0; column < columns; column++)
                {
                    int x = startX + column * (brickWidth + spacing);
                    int y = startY + row * (brickHeight + spacing);
                    Rectangle blockR = new Rectangle(x, y, brickWidth, brickHeight);
                    Block block = new Block(blockT, blockR, blockColor);
                    blocks.Add(block);
                }
            }
        }
    }
}
