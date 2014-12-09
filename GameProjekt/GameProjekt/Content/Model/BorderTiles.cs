using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.Model
{
    class BorderTiles : Tiles
    {
        public BorderTiles(int i, Rectangle newRectangle)
        {
            texture = Content.Load<Texture2D>("Tiles/Tile1");
            this.Rectangle = newRectangle;
        }
    }
}
