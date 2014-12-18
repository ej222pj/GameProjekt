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
        private Vector2 position;
        private Vector2 velocity;
        Vector2 rotatePosition;
        Vector2 rotationDirection;
        private Rectangle rectangle;
        KeyboardState oldState;
        DragLine dragLine;
        Map map;

        private bool hasStarted = false;
        private bool playerShootLine = false;
        private bool isRotating = false;
        private bool posistionForRotationDirection = false;
        private bool beforeFirstRotation = false;
        private bool clockvise = false;
        private int tileSize;
        private float currentAngle = 0f;
        private float angleStep = 0.05f;
        private float distanceBetweenPlayerAndRoatateCenter;
        float speed = 3.0f;

        public bool ShootLine 
        {
            get { return playerShootLine; }
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

        public void Load(ContentManager Content, Map map) 
        {
            texture = Content.Load<Texture2D>("Tiles/Player");
            this.map = map;

            position = new Vector2(map.Width / 2, map.Height - tileSize * 2);
        }

        public void Update(GameTime gameTime, Vector2 center) 
        {
            if (isRotating)
            {
                Vector2 rotationVector = ReleaseRotation(center, position);

                if (clockvise)
                {
                    velocity = rotationVector;
                }
                else
                {
                    velocity = rotationVector * -1;
                }
                
                rectangle = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
                isRotating = false;
            }
            else if (!isRotating)
            {
                if (playerShootLine)
                {
                    rotatePosition = Rotate(position, center);
                    rectangle = new Rectangle((int)rotatePosition.X, (int)rotatePosition.Y, tileSize, tileSize);
                    position = new Vector2(rotatePosition.X, rotatePosition.Y);
                    isRotating = true;
                }
                else
                {
                    position += velocity;
                    rectangle = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
                }
            }
            if (hasStarted && !beforeFirstRotation)
            {
                velocity.Y = -speed;
            }
            
            Input(gameTime, position, center);                
        }

        public Vector2 MovmentDirection()
        {
            Vector2 up = new Vector2(0, -1);
            Matrix rotMatrix = Matrix.CreateRotationZ(currentAngle);
            Vector2 movmentDirection = Vector2.Transform(up, rotMatrix);
            Console.WriteLine(movmentDirection);
            return movmentDirection;
        }

        private void Input(GameTime gametime, Vector2 playerPosition, Vector2 center)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) && !hasStarted) 
            {
                position.Y -= 0.8f;
                hasStarted = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                ResetGame();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                MovmentDirection();
            }

            //Kode for holding down space
            KeyboardState newState = Keyboard.GetState();
            // Is the SPACE key down?
            if (newState.IsKeyDown(Keys.Space) && hasStarted == true)
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    beforeFirstRotation = true;
                    if (center.X < playerPosition.X)
                    {
                        posistionForRotationDirection = true;
                    }
                    else if (center.X > playerPosition.X)
                    {
                        posistionForRotationDirection = false;
                    }
                    isRotating = false;
                    playerShootLine = !playerShootLine;
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
            if (!playerShootLine) 
            {
                dragLine.IsConnected = false; 
            }
        }

        public Vector2 Rotate(Vector2 currentPos, Vector2 centre)
        {
            //float a = MathHelper.ToDegrees(currentAngle);
            //float XDistance = currentPos.X - centre.X;
            //float YDistance = currentPos.Y - centre.Y;
            //distanceBetweenPlayerAndRoatateCenter = (float)Math.Sqrt(XDistance * XDistance + YDistance * YDistance);
            //return new Vector2((float)(distanceBetweenPlayerAndRoatateCenter * Math.Cos(a)),
            //    (float)(distanceBetweenPlayerAndRoatateCenter * Math.Sin(a))) + centre;

            if (posistionForRotationDirection)
            {
                currentAngle -= angleStep;
                clockvise = true;
            }
            else
            {
                currentAngle += angleStep;
                clockvise = false;
            }

            float XDistance = currentPos.X - centre.X;
            float YDistance = currentPos.Y - centre.Y;
            distanceBetweenPlayerAndRoatateCenter = (float)Math.Sqrt(XDistance * XDistance + YDistance * YDistance);

            float xDifference = (float)Math.Cos(currentAngle);
            float yDifference = (float)Math.Sin(currentAngle);
            rotationDirection = new Vector2(xDifference, yDifference);

            Vector2 newPosition = centre + rotationDirection * distanceBetweenPlayerAndRoatateCenter;
            return newPosition;
        }

        public Vector2 ReleaseRotation(Vector2 circle_center, Vector2 playerReleasePoint)
        {
            //Find the tangent
            Vector2 dir = playerReleasePoint - circle_center;
            Vector2 tangent = new Vector2(dir.Y, -dir.X);
            tangent.Normalize();

            return tangent * speed;
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
        

        public void ResetGame() 
        {
            position = new Vector2(map.Width / 2, map.Height - tileSize * 2);
            rotatePosition = position;
            velocity.Y = 0.0f;
            velocity.X = 0.0f;
            hasStarted = false;
            playerShootLine = false;
            beforeFirstRotation = false;
            currentAngle = 0;
            isRotating = false;
            posistionForRotationDirection = false;
            
        }

        public void Draw(SpriteBatch spriteBatch) 
        {                                        
            Vector2 origin = new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2);
            Rectangle size = new Rectangle(0, 0, texture.Width, texture.Height);
            spriteBatch.Draw(texture, rectangle, size, Color.White, currentAngle, origin, SpriteEffects.None, 0);
       }        
    }
}
