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
        Vector2 rotate;
        Vector2 direction;
        private Rectangle rectangle;
        KeyboardState oldState;
        DragLine dragLine;

        private bool hasStarted = false;
        private bool shootLine = false;
        private bool isRotating = false;
        private bool rotationDirection = false;
        private bool beforeFirstRotation = false;
        private int tileSize;
        private float currentAngle = 0;
        private float angleStep = 0.05f;
        private float playerAngle = 0;
        private float distance;
        Vector2 tangent1;
        Vector2 tangent2;

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

        public void Update(GameTime gameTime, Vector2 center) 
        {
            position += velocity;
            if (shootLine)
            {
                rotate = Rotate(position, center);
                
                position = new Vector2(rotate.X, rotate.Y);
                rectangle = new Rectangle((int)rotate.X, (int)rotate.Y, tileSize, tileSize);
                isRotating = true;
            }
            else
            {
                rotate = new Vector2(position.X, position.Y);
                rectangle = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
                isRotating = false;
            }
            Input(gameTime, position, center);

            if (isRotating) 
            {
                velocity.Y = 0f;
            }
            else if (hasStarted && !beforeFirstRotation) 
            {
                velocity.Y = -0.7f;
            }
            else if (hasStarted && beforeFirstRotation)
            {
                if (center.X < position.X)
                {
                    velocity = tangent2;
                }
                else if (center.X > position.X)
                {
                    velocity = tangent1;
                }
                 
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
                    beforeFirstRotation = true;
                    if (center.X < playerPosition.X)
                    {
                        rotationDirection = true;
                    }
                    else if (center.X > playerPosition.X)
                    {
                        rotationDirection = false;
                    }
                    ReleaseRotation(center, distance, rotate);
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

        public void ReleaseRotation(Vector2 circle_center, float circle_radius, Vector2 point)
        {
            //http://www.gamedev.net/topic/499818-tangents-to-a-circle-through-a-point/ skrivet av user: Swiftcoder
            //def find_tangents_through_point():
            //#find the direction from the point to the center of the circle
            Vector2 dir = point - circle_center;
            //#extract the length and angle
            float len = dir.Length();
            float angle = (float)Math.Atan2(dir.Y, dir.X);

            //# derive the length of the tangent using pythagoras
            float tangent_len = (float)Math.Sqrt(len * 2 - circle_radius * 2);
            //# and the angle using trigonometry
            float tangent_angle = (float)Math.Asin(circle_radius / len);

            //# there are 2 tangents, one either side
            float pos = angle + tangent_angle;
            float neg = angle - tangent_angle;

            //#return the direction vector of each tanget (the starting point was passed in)
            tangent1 = new Vector2((float)Math.Cos(pos), (float)Math.Sin(pos));
            tangent2 = new Vector2((float)Math.Cos(neg), (float)Math.Sin(neg));
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
            distance = (float)Math.Sqrt(XDistance * XDistance + YDistance * YDistance);

            float yDifference = (float)Math.Sin(currentAngle);
            float xDifference = (float)Math.Cos(currentAngle);
            direction = new Vector2(xDifference, yDifference);
            Vector2 newPosition = centre + direction * distance;
            Console.WriteLine(direction);
            return newPosition;   
        }

        public void ResetGame() 
        {
            position = new Vector2(352, 1536);
            rotate = position;
            velocity.Y = 0.0f;
            velocity.X = 0.0f;
            hasStarted = false;
            shootLine = false;
            beforeFirstRotation = false;
            playerAngle = 0;
            currentAngle = 0;
            tangent1 = new Vector2(0,0);
            tangent2 = new Vector2(0, 0);
            isRotating = false;
            rotationDirection = false;
            
        }

        public void Draw(SpriteBatch spriteBatch) 
        {

            Color color = new Color(0, 0, 0, 0);

            Vector2 origin = new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2);
            spriteBatch.Draw(texture, rectangle, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, playerAngle, origin, SpriteEffects.None, 0);
       }

        
    }
}
