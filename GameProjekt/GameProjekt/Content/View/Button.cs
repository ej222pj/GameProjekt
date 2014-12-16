using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.View
{
    class Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        bool down;
        public bool isClicked;

        Color colour = new Color(255, 255, 255, 255);

        public Vector2 size;

        public Button(Texture2D newTexture, GraphicsDevice graphics) 
        {
            texture = newTexture;

            size = new Vector2(200, 100);
        }

        public void Update(MouseState mouse) 
        {
            rectangle = new Rectangle((int)position.X / 2, (int)position.Y / 2, (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle)) 
            {
                if (colour.A == 255) down = false;
                if (colour.A == 0) down = true;
                if(down) colour.A += 3; else colour.A -= 3;
                if(mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else if(colour.A < 255)
            {
                colour.A += 3;
                isClicked = false;
            }
        }

        public void setPosition(Vector2 NewPosition) 
        {
            position = NewPosition;
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(texture, rectangle, colour);
        }
    }
}
