using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameProjekt.Content.Model
{
    class Player
    {
        

        private Vector2 position;
        private Vector2 velocity;
        Vector2 rotatePosition;
        Vector2 oldPosition;
        private Rectangle rectangle;
        KeyboardState oldState;
        Map map;
        Level level;

        private bool hasStarted;
        private bool gameIsWon;
        private bool playerShootLine;
        private bool isRotating;
        private bool rightDirectionMovment;
        private bool beforeFirstRotation;
        private bool upMovement;
        private bool RightOfCenter;
        private bool LeftOfCenter;
        private bool overCenter;
        private bool underCenter;
        private bool isConnected;
        private int tileSize;
        private bool hitTopOfMap;
        private bool playerIsDead;
        private bool jumpActivated;
        private float angleStep;
        float speedY;
        float speedX;
        float timeBeforeUseOfTurn;
        float timeInTheAir;
        float timePassed;
        float jumpTimePassed;

        public float TimePassed 
        {
            get { return timePassed; }
        }

        public bool ShootLine
        {
            get { return playerShootLine; }
        }

        public bool GameIsWon 
        {
            get { return gameIsWon; }
            set { gameIsWon = value; }
        }

        public bool GetPlayerIsDead 
        {
            get { return playerIsDead; }
        }

        public bool JumpActivated 
        {
            get { return jumpActivated; }
        }

        public bool HitTopOfMap
        {
            get { return hitTopOfMap; }
            set { hitTopOfMap = value; }
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

        public Player(int tileSize, Level level)
        {
            this.level = level;
            this.tileSize = tileSize;
        }

        public void Load(Map map)
        {
            this.map = map;
        }

        public void Update(GameTime gameTime, Vector2 clostestTile, Vector2 connectedTile)
        {
            position += velocity;
            Input(gameTime, position, clostestTile);
            if (isRotating)
            {
                rotatePosition = Rotate(position, connectedTile);
                if (playerShootLine)
                {
                    velocity = ReleaseRotation(connectedTile, rotatePosition);
                }
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
                    oldPosition = position; //Behövs för att se vilket håll han åker
                } 
            }
            rectangle = new Rectangle((int)position.X - tileSize / 2, (int)position.Y - tileSize / 2, tileSize, tileSize);
        }

        private void Input(GameTime gametime, Vector2 playerPosition, Vector2 center)
        {
            timePassed += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (timePassed > timeBeforeUseOfTurn)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A) && hasStarted && !GetPlayerIsDead && !isRotating)
                {
                    position.X -= 100.0f;
                    timePassed = 0;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D) && hasStarted && !GetPlayerIsDead && !isRotating)
                {
                    position.X += 100.0f;
                    timePassed = 0;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R) && !GetPlayerIsDead)
            {
                ResetGame();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && !GetPlayerIsDead && !isRotating)
            {
                jumpActivated = true;
            }

            if (jumpActivated) 
            {
                jumpTimePassed += (float)gametime.ElapsedGameTime.TotalSeconds;  
            }
            if(jumpTimePassed > timeInTheAir)
            {
                jumpActivated = false;
                jumpTimePassed = 0;
            } 

            if (Keyboard.GetState().IsKeyDown(Keys.W) && !hasStarted)
            {
                
                hasStarted = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
            }
            //Kode for holding down space
            KeyboardState newState = Keyboard.GetState();
            // Is the SPACE key down?
            if (newState.IsKeyDown(Keys.Space) && hasStarted == true && !GetPlayerIsDead && !jumpActivated)
            {
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    if (playerPosition.X < center.X)//Vänster om Center 
                    {
                        LeftOfCenter = true;
                        RightOfCenter = false;
                    }
                    if (playerPosition.X > center.X) //Höger om center
                    {
                        LeftOfCenter = false;
                        RightOfCenter = true;
                    }
                    if (playerPosition.Y > center.Y) //Under Center
                    {
                        underCenter = true;
                        overCenter = false;
                    }
                    if (playerPosition.Y < center.Y)//Över center
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
        }
        public Vector2 Rotate(Vector2 currentPos, Vector2 centre)
        {       
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
            return new Vector2(0, 0);
        }

        public Vector2 ReleaseRotation(Vector2 circle_center, Vector2 playerReleasePoint)
        {
            //Find the tangent
            Vector2 dir = playerReleasePoint - circle_center;
            Vector2 tangent = new Vector2(dir.Y, -dir.X);
            tangent.Normalize();

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
            RectangleCollide(newRectangle);
        }

        public void KillTileCollision(Rectangle newRectangle)
        {
            RectangleCollide(newRectangle);
        }

        public void FenceTileCollision(Rectangle newRectangle)
        {
            if (!jumpActivated)
            {
                RectangleCollide(newRectangle);
            }
        }

        public void RectangleCollide(Rectangle newRectangle)
        {
            if (rectangle.TouchTopOf(newRectangle)
                  || rectangle.TouchLeftOf(newRectangle)
                  || rectangle.TouchRightOf(newRectangle)
                  || rectangle.TouchBottomOf(newRectangle))
            {
                PlayerIsDead();
            }
        }

        private void PlayerIsDead()
        {
            playerIsDead = true;
            velocity.Y = 0.0f;
            velocity.X = 0.0f;
            isRotating = false;
        }

        public void WinTileCollision(Rectangle newRectangle) 
        {
            if ((rectangle.TouchTopOf(newRectangle)) 
                || rectangle.TouchLeftOf(newRectangle) 
                || rectangle.TouchRightOf(newRectangle) 
                || (rectangle.TouchBottomOf (newRectangle)))
            {   //Ska vinna banan
                if ((level.GetSelectedLevel() == SelectLevel.Tutorial && !HitTopOfMap) 
                    || (level.GetSelectedLevel() == SelectLevel.FirstLevel && !HitTopOfMap) 
                    || (level.GetSelectedLevel() == SelectLevel.SecondLevel && !HitTopOfMap))
                {
                    level.ChangeMap();
                }
                else if (level.GetSelectedLevel() == SelectLevel.ThirdLevel && !HitTopOfMap)
                {
                    GameIsWon = true;
                }
                HitTopOfMap = true;
            }
        }

        public void BorderCollision(Rectangle newRectangle)
        {
            if ((rectangle.TouchTopOf(newRectangle) && hasStarted) 
                || rectangle.TouchLeftOf(newRectangle) 
                || rectangle.TouchRightOf(newRectangle) 
                || (rectangle.TouchBottomOf (newRectangle)))
            {
                PlayerIsDead();
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
            isRotating = false;
            rightDirectionMovment = true;
            beforeFirstRotation = false;
            upMovement = true;
            RightOfCenter = false;
            LeftOfCenter = false;
            overCenter = false;
            underCenter = false;
            isConnected = false;
            hitTopOfMap = false;
            gameIsWon = false;
            playerIsDead = false;
            timeBeforeUseOfTurn = 2f;
            timeInTheAir = 0.6f;
            timePassed = 0;
            angleStep = -0.02f;
            speedY = 3f;
            speedX = 0.02f;
            jumpTimePassed = 0;
            jumpActivated = false;

            if (level.GetSelectedLevel() == SelectLevel.FirstLevel)
            {
                angleStep = -0.06f;
                speedY = 6f;
                timeInTheAir = 0.5f;
            }

            if (level.GetSelectedLevel() == SelectLevel.SecondLevel) 
            {
                angleStep = -0.07f;
                speedY = 7f;
                timeInTheAir = 0.4f;
            }

            if (level.GetSelectedLevel() == SelectLevel.ThirdLevel)
            {
                angleStep = -0.08f;
                speedY = 8f;
                timeInTheAir = 0.3f;
            }

        }
    }
}
