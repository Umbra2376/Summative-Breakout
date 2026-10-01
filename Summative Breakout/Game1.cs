using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Particle_Generator;
using System.Collections.Generic;
using System.Diagnostics;

namespace Summative_Breakout
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D title;
        Rectangle window;
        SpriteFont blockFont;
        float titleO;
        double titleTime;
        KeyboardState keyboardState;
        MouseState mouseState;
        ParticleEngine particleEngine;
        List<Texture2D> particles;
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
            window = new Rectangle(0, 0, 845, 600);
            particles = new List<Texture2D>();
            base.Initialize();
            particleEngine = new ParticleEngine(particles, Vector2.Zero, false, true);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            title = Content.Load<Texture2D>("blockBreakerTitle");
            blockFont = Content.Load<SpriteFont>("blockFont");
            particles.Add(Content.Load<Texture2D>("brokenBlock"));
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
                if (titleTime >= 0.5)
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
            if (screen == Screen.Game)
            {
                keyboardState = Keyboard.GetState();
                mouseState = Mouse.GetState();
                particleEngine.EmitterLocation = mouseState.Position.ToVector2();
                particleEngine.Update();
                if (mouseState.LeftButton == ButtonState.Pressed)
                    particleEngine.Enabled = true;
                else
                    particleEngine.Enabled = false;
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
                particleEngine.Draw(_spriteBatch);
            }    
                _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
