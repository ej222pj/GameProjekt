using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.Model
{
    class Map
    {
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        private List<BorderTiles> borderTiles = new List<BorderTiles>();
        List<string> lines = new List<string>();

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

        public void Generate(int size, string mapFilePath)
        {
            if (collisionTiles != null) 
                collisionTiles = new List<CollisionTiles>();
            if(borderTiles != null)
                borderTiles = new List<BorderTiles>();
            if(lines != null)
                lines = new List<string>();

            StreamReader reader;
            reader = new StreamReader(mapFilePath);
            int[,] data = null;

            if (File.Exists(mapFilePath))
            {
                Dictionary<string, int> counts = GetRowAndColumnCounts(mapFilePath);

                int rowCount = counts["row_count"];
                int columnCount = counts["column_count"];

                data = new int[rowCount, columnCount];

                using (StreamReader sr = File.OpenText(mapFilePath))
                {
                    string s = "";
                    string[] split = null;

                    for (int i = 0; (s = sr.ReadLine()) != null; i++)
                    {
                        split = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 0; j < columnCount; j++)
                        {
                            data[i, j] = int.Parse(split[j]);
                            //int number = data[j, i];
                            if (data[i, j] == 1)
                            {
                                borderTiles.Add(new BorderTiles(1, new Rectangle(j * size, i * size, size, size)));
                            }
                            if (data[i, j] == 2)
                            {
                                collisionTiles.Add(new CollisionTiles(2, new Rectangle(j * size , i * size, size, size)));
                            }
                            if (data[i, j] == 3)
                            {
                                borderTiles.Add(new BorderTiles(3, new Rectangle(j * size, i * size, size, size)));
                            }
                            width = (j + 1) * size;
                            height = (i + 1) * size;
                        }
                    }
                }
            }
            else
            {
                //throw new FileDoesNotExistException("Input file does not exist");
            }
        }

        private Dictionary<string, int> GetRowAndColumnCounts(string inputFilePath)
        {
            int rowCount = 0;
            int columnCount = 0;

            if (File.Exists(inputFilePath))
            {
                using (StreamReader sr = File.OpenText(inputFilePath))
                {
                    string[] split = null;
                    int lineCount = 0;

                    for (string s = sr.ReadLine(); s != null; s = sr.ReadLine())
                    {
                        split = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        if (columnCount == 0)
                        {
                            columnCount = split.Length;
                        }

                        lineCount++;
                    }

                    rowCount = lineCount;
                }

                if (rowCount == 0 || columnCount == 0)
                {
                    //throw new FileEmptyException("No input data");
                }
            }
            else
            {
                //throw new FileDoesNotExistException("Input file does not exist");
            }

            Dictionary<string, int> counts = new Dictionary<string, int>();

            counts.Add("row_count", rowCount);
            counts.Add("column_count", columnCount);

            return counts;
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
