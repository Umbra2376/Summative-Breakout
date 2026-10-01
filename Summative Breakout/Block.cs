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

        public Block(Texture2D texture, Rectangle rect, Color color)
        {
            _texture = texture;
            _rect = rect;
            _color = color;
        }
        public Rectangle Rect
        {
            get { return _rect; }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rect, _color);
        }
    }
}
