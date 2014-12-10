using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.Model
{
    class DragLine
    {
        private bool isConnected;
        private Vector2 position;

        public bool IsConnected 
        {
            get { return isConnected; }
            set { isConnected = value; }
        }

        public void Update(Vector2 position)
        {
            this.position = position;
        }

        public void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 end)
        {
            spriteBatch.Draw(texture, position, null, Color.White,
                             (float)Math.Atan2(end.Y - position.Y, end.X - position.X),
                             new Vector2(0f, (float)texture.Height / 2),
                             new Vector2(Vector2.Distance(position, end), 1.0f),
                             SpriteEffects.None, 0f);

            IsConnected = true;
        }



        
    }
}
