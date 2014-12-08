using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content
{
    class BorderTiles : Tiles
    {
        public BorderTiles(int i, Rectangle newRectangle)
        {
            texture = Content.Load<Texture2D>("Tiles/Tile" + 1);
            this.Rectangle = newRectangle;
        }
    }
}
