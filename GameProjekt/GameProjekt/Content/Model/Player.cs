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
        KeyboardState oldState;
        DragLine dragLine;

        private bool hasStarted = false;
        private bool shootLine = false;
        private int tileSize;

        public bool ShootLine 
        {
            get { return shootLine; }
        }

        public Vector2 Position 
        {
            get { return position; }
        }

        public Player(int tileSize, DragLine dragline) 
        {
            this.tileSize = tileSize;
            dragLine = dragline;
        }

        public void Load(ContentManager Content) 
        {
            texture = Content.Load<Texture2D>("Tiles/Player");
        }

        public void Update(GameTime gameTime) 
        {
            position += velocity;
            rectangle = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);

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
                position.Y -= 0.4f;
                hasStarted = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                ResetGame();
            }

            //Kode for holding down space
            KeyboardState newState = Keyboard.GetState();
            // Is the SPACE key down?
            if (newState.IsKeyDown(Keys.Space) && hasStarted == true)
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    shootLine = !shootLine;
                }
            }
            else if (oldState.IsKeyDown(Keys.Space))
            {
                // Key was down last update, but not down now, so
                // it has just been released.
                //shootLine = false;
            }
            // Update saved state.
            oldState = newState;
            //If line not shot. Its not connected
            if (!shootLine) 
            {
                dragLine.IsConnected = false; 
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
