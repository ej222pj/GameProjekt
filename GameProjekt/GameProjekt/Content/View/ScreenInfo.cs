using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.View
{
    class ScreenInfo
    {
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, Vector2 position, string info)
        {
            spriteBatch.DrawString(spriteFont, info, position, Color.White);
        }
    }
}
