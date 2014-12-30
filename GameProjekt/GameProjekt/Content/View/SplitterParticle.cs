using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.View
{
    class SplitterParticle
    {
            private Vector2 velocity;
            Vector2 position;
            private Vector2 acceleration;
            private static float splitterSize = 0.01f;
            Vector2 startPos;


            public SplitterParticle( Vector2 Velocity)
            {
                velocity = Velocity;
                acceleration = new Vector2(0, 14.7f);
            }

            public void Update(Vector2 Position, float timeElapsed)
            {
                if (startPos != Position)
                    startPos = Position;
                if (position == new Vector2(0,0))
                    position = Position;


                Vector2 newPos = new Vector2();
                Vector2 newVel = new Vector2();

                newVel.X = velocity.X + timeElapsed * acceleration.X;
                newVel.Y = velocity.Y + timeElapsed * acceleration.Y;

                newPos.X = position.X + timeElapsed * velocity.X;
                newPos.Y = position.Y + timeElapsed * velocity.Y;

                velocity = newVel;
                position = newPos;

            }

            public void Draw(SpriteBatch spriteBatch, Texture2D splitterTexture, Camera camera)
            {
                spriteBatch.Draw(splitterTexture, camera.scaleParticle(position.X - 8, position.Y - 8, splitterSize), Color.Black);
            }
        }
}
