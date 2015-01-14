using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.View
{
    class Camera
    {
        private float scale;
        private Matrix transform;
        private Vector2 center;
        private Viewport viewport;
        public Matrix Transform 
        {
            get { return transform;  }
        }

        public float Scale
        {
            get { return scale; }
        }           

        public Camera(Viewport newViewport) 
        {
            viewport = newViewport;
            int scaleX = (viewport.Width);
            int scaleY = (viewport.Height);

            scale = scaleX;
            if (scaleY < scaleX)
            {
                scale = scaleY;
            }
        }

        public Rectangle scaleParticle(float xPos, float yPos, float splitterSize)
        {
            int vSize = (int)(splitterSize * scale);

            int vX = (int)xPos;//(int)(xPos * scale);
            int vY = (int)yPos;//(int)(yPos * scale);

            return new Rectangle(vX, vY, vSize, vSize);
        }

        public void Update(Vector2 position, int xOffset, int yOffset)
        {
            center.X = viewport.Width / 2;

            if (position.Y < viewport.Height / 2)
            {
                center.Y = viewport.Height / 2;
            }
            else if (position.Y > yOffset - (viewport.Height / 2))
            {
                center.Y = yOffset - (viewport.Height / 2);
            }
            else
            {
                center.Y = position.Y;
            }

            transform = Matrix.CreateTranslation(new Vector3(0, -center.Y + (viewport.Height / 2), 0));
        }
    }
}
