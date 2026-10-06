using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Summative_Breakout
{
    public class Paddle
    {
        private Rectangle _rect;
        private Texture2D _texture;
        private int _speed;
        private Rectangle _window;

        public Paddle(Texture2D texture, Rectangle rect, Rectangle window)
        {
            _texture = texture;
            _rect = rect;
            _window = window;
            _speed = 8;
        }
        public Rectangle Rect
        {
            get { return _rect; }
        }
        public void Reset(Rectangle location)
        {
            _rect = location;
        }
        public void MoveLeft()
        {
            _rect.X -= _speed;
            if (_rect.Left < _window.Left)
                _rect.X = _window.Left;
        }
        public void MoveRight()
        {
            _rect.X += _speed;
            if (_rect.Right > _window.Right)
                _rect.X = _window.Right - _rect.Width;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rect, Color.White);
        }
    }
}
