using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.View
{
    class MouseStateView
    {
        float x;
        float y;
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        Camera camera;
        private SpriteBatch spriteBatch;

        public MouseStateView(Camera Camera, GraphicsDevice graphicsDevice)
        {
            camera = Camera;
            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public bool IsButtonPressed() 
        {
            bool mouseIsClicked = false;
            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {     
                mouseIsClicked = true;
            }

            previousMouseState = currentMouseState;

            return mouseIsClicked;
        }

        //public Vector2 GetMousePos()
        //{
        //    currentMouseState = Mouse.GetState();

        //    x = currentMouseState.X;
        //    y = currentMouseState.Y;

        //    Vector2 mouseModelPos = camera.mousePosToModelPos(x, y);

        //    return mouseModelPos;
        //}
    }
}
