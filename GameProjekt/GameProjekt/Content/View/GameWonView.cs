using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.View
{
    class GameWonView
    {
        internal void Draw(SpriteBatch spriteBatch, Button btnMainMenu, ContentManager Content, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Content.Load<Texture2D>("Tiles/WinGame"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            btnMainMenu.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
