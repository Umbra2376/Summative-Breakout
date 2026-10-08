using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Summative_Breakout
{
    public class Ball
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _speed;
        private Rectangle _window;
        private SoundEffect _soundEffect;
        private Random _generator;

        public Ball(Texture2D texture, Rectangle location, Vector2 speed, Rectangle window, SoundEffect sound)
        {
            _texture = texture;
            _location = location;
            _speed = speed;
            _window = window;
            _soundEffect = sound;
            _generator = new Random();
        }

        public Rectangle Rect
        {
            get { return _location; }
        }

        public Block Update(Paddle paddle, List<Block> blocks)
        {
            _location.X += (int)_speed.X;
            _location.Y += (int)_speed.Y;

            if (_location.Left <= _window.Left)
            {
                _location.X = _window.Left;
                _speed.X *= -1;
                _soundEffect.Play();
            }

            if (_location.Right >= _window.Right)
            {
                _location.X = _window.Right - _location.Width;
                _speed.X *= -1;
                _soundEffect.Play();
            }

            if (_location.Top <= _window.Top)
            {
                _location.Y = _window.Top;
                _speed.Y *= -1;
                _soundEffect.Play();
            }

            if (_location.Intersects(paddle.Rect))
            {
                if (_speed.Y > 0 && _location.Bottom <= paddle.Rect.Top + 10)
                {
                    _location.Y = paddle.Rect.Top - _location.Height;
                    _speed.Y *= -1;

                    float paddleCenter = paddle.Rect.Center.X;
                    float ballCenter = _location.Center.X;

                    if (ballCenter < paddleCenter)
                    {
                        _speed.X = (float)(_generator.NextDouble() * -10 + 1);
                    }
                    else
                    {
                        _speed.X = (float)(_generator.NextDouble() * 10 - 1);
                    }

                    _soundEffect.Play();
                }
                else if (_location.Right > paddle.Rect.Left && _location.Left < paddle.Rect.Right)
                {
                    _speed.Y = 5;
                    _speed.X *= -10;
                }
            }
            for (int i = 0; i < blocks.Count; i++)
            {
                Block block = blocks[i];

                if (block.IsAlive && _location.Intersects(block.Rect))
                {
                    block.Hit();
                    Rectangle intersection = Rectangle.Intersect(_location, block.Rect);

                    if (intersection.Width < intersection.Height)
                        _speed.X *= -1;
                    else
                        _speed.Y *= -1;

                    blocks.RemoveAt(i);

                    return block;
                }
            }
            return null;
        }
        public void Reset(Rectangle location, Vector2 speed)
        {
            _location = location;
            _speed = speed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}
