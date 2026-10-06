using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summative_Breakout
{
    public class Block
    {
        private Rectangle _rect;
        private Texture2D _texture;
        private Color _color;
        private bool _isAlive;

        public Block(Texture2D texture, Rectangle rect, Color color)
        {
            _texture = texture;
            _rect = rect;
            _color = color;
            _isAlive = true;
        }

        public Rectangle Rect
        {
            get { return _rect; }
        }

        public bool IsAlive
        {
            get { return _isAlive; }
        }

        public void Hit()
        {
            _isAlive = false;
        }
        public Color Color
        {
            get { return _color; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isAlive)
            {
                spriteBatch.Draw(_texture, _rect, _color);
            }
        }
    }
}
