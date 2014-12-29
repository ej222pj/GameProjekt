﻿using Microsoft.Xna.Framework;
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
        enum SelectLevel
        {
            firstLevel,
            secondLevel,
            thirdLevel,
        }

        private Vector2 position;
        private Vector2 velocity;
        Vector2 rotatePosition;
        Vector2 oldPosition;
        private Rectangle rectangle;
        KeyboardState oldState;
        Map map;

        private bool hasStarted;
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
        private float angleStep = -0.04f;
        float speedY = 4f;
        float speedX = 0.1f;
        SelectLevel selectLevel;

        public int GetSelectLevel() 
        {
            return selectLevel.GetHashCode() + 1;
        }

        public bool ShootLine
        {
            get { return playerShootLine; }
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

        public Player(int tileSize)
        {
            selectLevel = SelectLevel.firstLevel;
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
                Console.WriteLine(connectedTile);
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
            //If line not shot. Its not connected
            if (!playerShootLine)
            {
            }
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
            {
                ResetGame();
            }
            if (rectangle.TouchRightOf(newRectangle))//Vänster sida av skärmen
            {
                ResetGame();
            }
            if (rectangle.TouchBottomOf(newRectangle))//Längst upp på skärmen
            {   //Ska vinna banan
                if (selectLevel == SelectLevel.firstLevel && !HitTopOfMap) 
                {
                    HitTopOfMap = true;
                    selectLevel = SelectLevel.secondLevel;
                }
                else if (selectLevel == SelectLevel.secondLevel && !HitTopOfMap)
                {
                    HitTopOfMap = true;
                    selectLevel = SelectLevel.thirdLevel;
                }
                else if (selectLevel == SelectLevel.thirdLevel && !HitTopOfMap)
                {
                    HitTopOfMap = true;
                    //Man vann
                }
                
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
            angleStep = -0.04f;
            speedY = 4f;
            speedX = 0.1f;
        }
    }
}
