using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.View
{
    class PlayerView
    {
        Texture2D texture;
        public void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Tiles/user");
        }

        internal void Draw(SpriteBatch spriteBatch, Vector2 position, int tileSize)
        {
            spriteBatch.Draw(texture,  new Rectangle((int)position.X - tileSize / 2, (int)position.Y - tileSize / 2, tileSize, tileSize ), Color.White);
        }
    }
}
