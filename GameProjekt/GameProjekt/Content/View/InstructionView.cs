using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.View
{
    class InstructionView
    {
        public Vector2 position;
        public GraphicsDeviceManager graphics;

        public InstructionView(GraphicsDeviceManager graphics) 
        {
            this.graphics = graphics;
            position = new Vector2(graphics.PreferredBackBufferWidth / 5, graphics.PreferredBackBufferHeight / 3.5f);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, Button btnMainMenu, ContentManager Content) 
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Content.Load<Texture2D>("Tiles/instructionview"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.DrawString(spriteFont, "*Win map by reaching the top! \n*Press W to start! \n*Spin around RED tiles using SPACEBAR! \n*Avoid GREEN tiles by using A or D button \n" +
            "*Jump FENCES using left CTRL button! \n*A or D can be used once every 2 seconds! \n*You can not spin forever!", position, Color.White);
            btnMainMenu.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
