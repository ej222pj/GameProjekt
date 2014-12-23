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
        Vector2 oldPosition;
        private Rectangle rectangle;
        KeyboardState oldState;
        DragLine dragLine;
        Map map;

        private bool hasStarted = false;
        private bool playerShootLine = false;
        private bool isRotating = false;
        private bool rightDirectionMovment = true;
        private bool beforeFirstRotation = false;
        private bool upMovement = true;
        private bool RightOfCenter = false;
        private bool LeftOfCenter = false;
        private bool overCenter = false;
        private bool underCenter = false;
        private bool isConnected = false;
        private int tileSize;
        private float currentAngle = 0f;
        private float angleStep = -0.04f;
        float speedY = 4f;
        float speedX = 0.1f;

        public bool ShootLine
        {
            get { return playerShootLine; }
        }
        public bool HasStarted
        {
            get { return hasStarted; }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; }
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
            texture = Content.Load<Texture2D>("Tiles/boll");
            this.map = map;

            position = new Vector2(map.Width / 2, map.Height - tileSize * 2);
        }

        public void Update(GameTime gameTime, Vector2 center)
        {
            position += velocity;
            Input(gameTime, position, center);
            if (isRotating)
            {
                rotatePosition = Rotate(position, center);
                if (playerShootLine)
                {
                    velocity = ReleaseRotation(center, rotatePosition);
                }
                //rectangle = new Rectangle((int)rotatePosition.X, (int)rotatePosition.Y, tileSize, tileSize);
                position = rotatePosition;
            }
            else if (!isRotating)
            {
                if (hasStarted && !beforeFirstRotation)
                {
                    velocity.Y = -speedY;
                    velocity.X = speedX;
                }
                if (playerShootLine)
                {
                    beforeFirstRotation = true;
                    isRotating = true;
                }
                else
                {
                    //rectangle = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
                    oldPosition = position;
                } 
            }
            rectangle = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
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
            }
            //Kode for holding down space
            KeyboardState newState = Keyboard.GetState();
            // Is the SPACE key down?
            if (newState.IsKeyDown(Keys.Space) && hasStarted == true)
            {
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    if (oldPosition.X < center.X)//Vänster om Center 
                    {
                        LeftOfCenter = true;
                        RightOfCenter = false;
                    }
                    if (oldPosition.X > center.X) //Höger om center
                    {
                        LeftOfCenter = false;
                        RightOfCenter = true;
                    }
                    if (oldPosition.Y > center.Y) //Under Center
                    {
                        underCenter = true;
                        overCenter = false;
                    }
                    if (oldPosition.Y < center.Y)//Över center
                    {
                        underCenter = false;
                        overCenter = true; 
                    }

                    if (oldPosition.X > playerPosition.X)//Om user är på väg åt Vänster 
                    {
                        rightDirectionMovment = false;

                    }
                    if (oldPosition.X < playerPosition.X)//Om user är på väg åt Höger
                    {
                        rightDirectionMovment = true;
                    }
                    //Console.WriteLine(oldPosition); 
                    //Console.WriteLine(playerPosition);
                    if (oldPosition.Y < playerPosition.Y)//ÅKer ner
                    {
                        upMovement = false;
                    }
                    if (oldPosition.Y > playerPosition.Y)//Åker up
                    {
                        upMovement = true;
                    }
                    isRotating = false;
                    playerShootLine = !playerShootLine;
                }    
            }
            else if (oldState.IsKeyDown(Keys.Space) && hasStarted == true)
            {
                // Key was down last update, but not down now, so
                // it has just been released.
                //shootLine = false;
                // If not down last update, key has just been pressed.
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
            float XDistance = currentPos.X - centre.X;
            float YDistance = currentPos.Y - centre.Y;
            float distanceBetweenPlayerAndRoatateCenter = (float)Math.Sqrt(XDistance * XDistance + YDistance * YDistance);
            Console.WriteLine(distanceBetweenPlayerAndRoatateCenter);

            if (overCenter && RightOfCenter && rightDirectionMovment && !upMovement)//Om user är över till höger om centrum, rör sig höger neråt
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep * -1)) + centre;
            }
            if (overCenter && RightOfCenter && rightDirectionMovment && upMovement)//Om user är över till höger om centrum, rör sig höger uppåt
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep)) + centre;
            }
            if (overCenter && RightOfCenter && !rightDirectionMovment && upMovement)
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep)) + centre;
            }
            if (overCenter && RightOfCenter && !rightDirectionMovment && !upMovement)
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep * -1)) + centre;
            }
            if (underCenter && RightOfCenter && rightDirectionMovment && !upMovement)
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep * -1)) + centre;
            }
            if (underCenter && RightOfCenter && rightDirectionMovment && upMovement)//Under, höger, rör sig höger upp
            {
                 return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep)) + centre;
            }
            if (underCenter && RightOfCenter && !rightDirectionMovment && upMovement)//Under, höger, rör sig vänster upp
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep)) + centre;
            }
            if (underCenter && RightOfCenter && !rightDirectionMovment && !upMovement)//Under, höger, rör sig vänster ner
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep * -1)) + centre;
            }
            if (underCenter && LeftOfCenter && !rightDirectionMovment && upMovement)
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep * -1)) + centre;
            }
            if (underCenter && LeftOfCenter && rightDirectionMovment && !upMovement)
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep)) + centre;
            }
            if (underCenter && LeftOfCenter && rightDirectionMovment && upMovement)//Under, vänster, höger upp
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep * -1)) + centre;
            }
            if (underCenter && LeftOfCenter && !rightDirectionMovment && !upMovement)//Under, vänster, höger ner
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep)) + centre;
            }
            if (overCenter && LeftOfCenter && rightDirectionMovment && upMovement)
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep * -1)) + centre;
            }
            if (overCenter && LeftOfCenter && !rightDirectionMovment && !upMovement)
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep)) + centre;
            }
            if (overCenter && LeftOfCenter && !rightDirectionMovment && upMovement)
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep * -1)) + centre;
            }
            if (overCenter && LeftOfCenter && rightDirectionMovment && !upMovement)
            {
                return Vector2.Transform(currentPos - centre, Matrix.CreateRotationZ(angleStep)) + centre;
            }
            Console.WriteLine("Fuck");
            return new Vector2(0, 0);



            //if (posistionForRotationDirection)
            //{
            //    currentAngle -= angleStep;
            //    clockvise = true;
            //}
            //else
            //{
            //    currentAngle += angleStep;
            //    clockvise = false;
            //}

            //float XDistance = currentPos.X - centre.X;
            //float YDistance = currentPos.Y - centre.Y;
            //distanceBetweenPlayerAndRoatateCenter = (float)Math.Sqrt(XDistance * XDistance + YDistance * YDistance);

            //float xDifference = (float)Math.Cos(currentAngle);
            //float yDifference = (float)Math.Sin(currentAngle);
            //rotationDirection = new Vector2(xDifference, yDifference);

            //return centre + (rotationDirection * distanceBetweenPlayerAndRoatateCenter);

        }

        public Vector2 ReleaseRotation(Vector2 circle_center, Vector2 playerReleasePoint)
        {
            //Find the tangent
            Vector2 dir = playerReleasePoint - circle_center;
            Vector2 tangent = new Vector2(dir.Y, -dir.X);
            tangent.Normalize();

           // if (upMovement)
               // return (tangent * speedY);
            //else
            //    return (tangent * speedY) * -1;

                if (overCenter && RightOfCenter && rightDirectionMovment && !upMovement)//Om user är över till höger om centrum, rör sig höger neråt
                {
                    return (tangent * speedY) * -1;
                }
                if (overCenter && RightOfCenter && rightDirectionMovment && upMovement)//Om user är över till höger om centrum, rör sig höger uppåt
                {
                    return (tangent * speedY);
                }
                if (overCenter && RightOfCenter && !rightDirectionMovment && upMovement)
                {
                    return (tangent * speedY);
                }
                if (overCenter && RightOfCenter && !rightDirectionMovment && !upMovement)
                {
                    return (tangent * speedY) * -1;
                }
                if (underCenter && RightOfCenter && rightDirectionMovment && !upMovement)
                {
                    return (tangent * speedY) * -1;
                }
                if (underCenter && RightOfCenter && rightDirectionMovment && upMovement)//Under, höger, rör sig höger upp
                {
                    return (tangent * speedY);
                }
                if (underCenter && RightOfCenter && !rightDirectionMovment && upMovement)//Under, höger, rör sig vänster upp
                {
                    return (tangent * speedY);
                }
                if (underCenter && RightOfCenter && !rightDirectionMovment && !upMovement)//Under, höger, rör sig vänster ner
                {
                    return (tangent * speedY) * -1;
                }
                if (underCenter && LeftOfCenter && !rightDirectionMovment && upMovement)
                {
                    return (tangent * speedY) * -1;
                }
                if (underCenter && LeftOfCenter && rightDirectionMovment && !upMovement)
                {
                    return (tangent * speedY);
                }
                if (underCenter && LeftOfCenter && rightDirectionMovment && upMovement)//Under, vänster, höger upp
                {
                    return (tangent * speedY) * -1;
                }
                if (underCenter && LeftOfCenter && !rightDirectionMovment && !upMovement)//Under, vänster, höger ner
                {
                    return (tangent * speedY);
                }
                if (overCenter && LeftOfCenter && rightDirectionMovment && upMovement)
                {
                    return (tangent * speedY) * -1;
                }
                if (overCenter && LeftOfCenter && !rightDirectionMovment && !upMovement)
                {
                    return (tangent * speedY);
                }
                if (overCenter && LeftOfCenter && !rightDirectionMovment && upMovement)
                {
                    return (tangent * speedY) * -1;
                }
                if (overCenter && LeftOfCenter && rightDirectionMovment && !upMovement)
                {
                    return (tangent * speedY);
                }
                return (tangent * speedY);
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
                //velocity.X = 0.0f;
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
            upMovement = true;
            currentAngle = 0;
            isRotating = false;
            rightDirectionMovment = false;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2);
            Rectangle size = new Rectangle(0, 0, texture.Width, texture.Height);
            spriteBatch.Draw(texture, rectangle, size, Color.White, currentAngle, origin, SpriteEffects.None, 0);
        }
    }
}
