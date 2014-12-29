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
        private int width;
        private int height;
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

        public Rectangle scaleParticle(float xPos, float yPos, float size)
        {
            int vSize = (int)(size * scale);

            Vector2 smokeVector = scaleVector(xPos, yPos);

            return new Rectangle((int)smokeVector.X, (int)smokeVector.Y, vSize, vSize);
        }

        public Vector2 scaleVector(float xPos, float yPos)
        {
            float X = (xPos * scale);
            float Y = (yPos * scale);

            return new Vector2(X, Y);
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
