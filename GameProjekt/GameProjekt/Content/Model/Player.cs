using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.Model
{
    class Player
    {
        private Texture2D texture;
        private Vector2 position = new Vector2(352, 1536);
        private Vector2 velocity;
        private Rectangle rectangle;

        private bool hasStarted = false;
        private bool shootLine = false;

        public bool ShootLine 
        {
            get { return shootLine; }
        }

        public Vector2 Position 
        {
            get { return position; }
        }

        public void Load(ContentManager Content) 
        {
            texture = Content.Load<Texture2D>("Tiles/Player");
        }

        public void Update(GameTime gameTime) 
        {
            position += velocity;
            rectangle = new Rectangle((int)position.X, (int)position.Y, 32, 32);

            Input(gameTime);

            if (hasStarted) 
            {
                velocity.Y = -2.0f;
            }
           
        }

        private void Input(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D)) 
            {
                velocity.X = (float)gametime.ElapsedGameTime.TotalMilliseconds / 15;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                velocity.X = -(float)gametime.ElapsedGameTime.TotalMilliseconds / 15;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W)) 
            {
                position.Y -= 0.3f;
                hasStarted = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && hasStarted == true)
            {
                shootLine = true;
            }                               
        }

        public void Collision(Rectangle newRectangle) 
        {
            if (rectangle.TouchTopOf(newRectangle) || rectangle.TouchLeftOf(newRectangle) || rectangle.TouchRightOf(newRectangle) || rectangle.TouchBottomOf(newRectangle)) 
            {
                ResetGame();    
            }
        }

        public void BorderCollision(Rectangle newRectangle)
        {
            if (rectangle.TouchTopOf(newRectangle))//Längst ner på skärmen
            {
                rectangle.Y = newRectangle.Y - rectangle.Height;
                velocity.Y = 0.0f;
                hasStarted = false;
            }
            if (rectangle.TouchLeftOf(newRectangle))//Höger sidan av skärmen
            {   //Ska egentligen döda spelaren
                position.X = newRectangle.X - rectangle.Width - 5;
                velocity.X = velocity.X * -1.0f;
            }
            if (rectangle.TouchRightOf(newRectangle))//Vänster sida av skärmen
            {   //Ska egentligen döda spelaren
                position.X = newRectangle.X + newRectangle.Width + 5;
                velocity.X = velocity.X * -1.0f;
            }
            if (rectangle.TouchBottomOf(newRectangle))//Längst upp på skärmen
            {   //Ska vinna banan
                ResetGame();
            }
        }

        public void ResetGame() 
        {
            position = new Vector2(352, 1536);
            velocity.Y = 0.0f;
            velocity.X = 0.0f;
            hasStarted = false;
            shootLine = false;
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        
    }
}
