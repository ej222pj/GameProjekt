using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.Model
{
    class Map
    {
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        private List<BorderTiles> borderTiles = new List<BorderTiles>();

        public List<CollisionTiles> CollisionTiles 
        {
            get { return collisionTiles; }
        }

        public List<BorderTiles> BorderTiles
        {
            get { return borderTiles; }
        }

        private int width, height;
        public int Width 
        {
            get { return width; }
        }
        public int Height 
        {
            get { return height; }
        }

        public void Generate(int[,] map, int size) 
        {
            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];

                    if (number == 1)
                    {
                        borderTiles.Add(new BorderTiles(number, new Rectangle(x * size, y * size, size, size)));
                    }
                    if (number == 2)
                    {
                        collisionTiles.Add(new CollisionTiles(number, new Rectangle(x * size, y * size, size, size)));
                    }
                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }
        }
        public void Draw(SpriteBatch spriteBatch) 
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
