using GameProjekt.Content.Model;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.View
{
    class DrawMap
    {
        public void Draw(SpriteBatch spriteBatch, List<CollisionTiles> collisionTiles, List<BorderTiles> borderTiles)
        {
            foreach (CollisionTiles tile in collisionTiles)
            {
                tile.Draw(spriteBatch);
            }
            foreach (BorderTiles tile in borderTiles)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
