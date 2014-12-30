using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProjekt.Content.View
{
    class SplitterSystem
    {
        private SplitterParticle[] particles;
        private int maxPartical = 100;
        private float time = 0;
        private static float runTime = 1.0f;
        private static float maxSpeed = 200f;
        private Camera camera;
        private Vector2 position;

        public SplitterSystem(Viewport viewPort)
        {
            camera = new Camera(viewPort);

            particles = new SplitterParticle[maxPartical];

            newSystem();
        }
        private void newSystem()
        {
            Random rand = new Random();

            for (int i = 0; i < maxPartical; i++)
            {
                Vector2 direction = new Vector2(((float)rand.NextDouble() - 0.5f), ((float)rand.NextDouble() - 0.5f));
                direction.Normalize();
                direction = direction * ((float)rand.NextDouble() * maxSpeed);

                particles[i] = new SplitterParticle(direction);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D splitterTexture, float timeElapsed, Vector2 position)
        {                    
            time += timeElapsed;

            for (int i = 0; i < maxPartical; i++)
            {
                particles[i].Update(position, timeElapsed);
            }
            if (time < runTime)
            {
                for (int i = 0; i < maxPartical; i++)
                {
                    particles[i].Draw(spriteBatch, splitterTexture, camera);
                }
            }
        }
    }
}
