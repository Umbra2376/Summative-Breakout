using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Summative_Breakout
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D happyBallT, hitBallT, paddleT, blockT, title;
        Rectangle window, paddleR;
        SpriteFont blockFont;
        float titleO;
        double titleTime;
        KeyboardState keyboardState;
        List<Texture2D> blocksT = new List<Texture2D>();
        List<Rectangle> blocksR = new List<Rectangle>();
        Color blockColor;
        enum Screen
        {
            Title, Game, Win, Lose
        }
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
            window = new Rectangle(0, 0, 800, 600);
            paddleR = new Rectangle(340, 500, 80, 20);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            title = Content.Load<Texture2D>("blockBreakerTitle");
            paddleT = Content.Load<Texture2D>("paddle");
            blockFont = Content.Load<SpriteFont>("blockFont");
            blocksT.Add(Content.Load<Texture2D>("tile"));
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (screen == Screen.Title)
            {
                keyboardState = Keyboard.GetState();
                titleTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (titleTime >= 1)
                {
                    titleO = titleO == 1 ? 0 : 1;
                    titleTime = 0;
                }
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    titleTime = 0;
                    screen = Screen.Game;
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
            if (screen == Screen.Game)
            {
                _spriteBatch.Draw(paddleT, paddleR, Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
