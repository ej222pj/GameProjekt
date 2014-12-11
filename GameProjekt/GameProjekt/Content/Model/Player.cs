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
        private bool isRotating = false;
        private bool rotationDirection = false;
        private int tileSize;
        private float currentAngle = 0;
        private float angleStep = 0.105f;
        private float playerAngle = 0;

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

        public void Update(GameTime gameTime, Vector2 playerPosition, Vector2 center) 
        {
            position += velocity;
            if (shootLine)
            {
                Vector2 rotate = Rotate(playerPosition, center);
                
                position = new Vector2(rotate.X, rotate.Y);
                rectangle = new Rectangle((int)rotate.X, (int)rotate.Y, tileSize, tileSize);
                isRotating = true;
            }
            else
            {
                rectangle = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
                isRotating = false;
            }
            Input(gameTime, playerPosition, center);

            if (isRotating) 
            {
                velocity.Y = 0f;
            }
            else if (hasStarted) 
            {
                velocity.Y = -2.0f;
            }
           
        }

        private void Input(GameTime gametime, Vector2 playerPosition, Vector2 center)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D) && !isRotating) 
            {
                velocity.X = 0;//(float)gametime.ElapsedGameTime.TotalMilliseconds / 15;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A) && !isRotating)
            {
                velocity.X = 0; //-(float)gametime.ElapsedGameTime.TotalMilliseconds / 15;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) && !hasStarted) 
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
                    if (center.X < playerPosition.X)
                    {
                        rotationDirection = true;
                    }
                    else if (center.X > playerPosition.X)
                    {
                        rotationDirection = false;
                    }

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
            if (rectangle.TouchTopOf(newRectangle) 
                || rectangle.TouchLeftOf(newRectangle) 
                || rectangle.TouchRightOf(newRectangle) 
                || rectangle.TouchBottomOf(newRectangle)) 
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

        public Vector2 Rotate(Vector2 currentPos, Vector2 centre)
        {
            if(rotationDirection)
            {
                currentAngle -= angleStep;
                playerAngle -= angleStep;
            }
            else
            {
                currentAngle += angleStep;
                playerAngle += angleStep;
            }

            float XDistance = currentPos.X - centre.X;
            float YDistance = currentPos.Y - centre.Y;         
            float distancee = (float)Math.Sqrt(XDistance * XDistance + YDistance * YDistance);

            float yDifference = (float)Math.Sin(currentAngle);
            float xDifference = (float)Math.Cos(currentAngle);
            Vector2 direction = new Vector2(xDifference, yDifference);
            Vector2 newPosition = centre + direction * distancee;
            Console.WriteLine(currentPos);
            return newPosition;   
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

            Color color = new Color(0, 0, 0, 0);

            Vector2 origin = new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2);
            //spriteBatch.Draw(texture, rectangle, Color.White);
            spriteBatch.Draw(texture, rectangle, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, playerAngle, origin, SpriteEffects.None, 0);
            //spriteBatch.Draw(smokeTexture, camera.scaleParticle(position.X, position.Y, smokeSize), new Rectangle(0, 0, smokeTexture.Bounds.Width, smokeTexture.Bounds.Height), color, rotation, origin, SpriteEffects.None, 0);
        }

        
    }
}
