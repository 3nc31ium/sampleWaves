using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Capstone
{
    class ParticleEmitter
    {
        Particle[] particles;
        VertexBuffer vBuffer;
        IndexBuffer iBuffer;
        int[] indicies;
        ParticleVertex[] verticies;

        Effect effect;

        Vector3 particleStart;


        public ParticleEmitter(
           GraphicsDevice device,Effect particleEffect, Vector3 start)
        {
           //type 1 = snow ; type 2 = rain
            int maxParticles = 4;
            particles = new Particle[maxParticles];
            verticies = new ParticleVertex[maxParticles * 4];
            indicies = new int[maxParticles * 6];

           
            for (int i = 0; i < maxParticles; i++)
            {

                
                particleStart = start;

                if (i == 0)
                {
                    particles[i] = new Particle(particleStart + new Vector3(-700, -65, 0), i);
                    verticies[i * 4] = new ParticleVertex(particles[i].verticies[0]);
                    verticies[i * 4 + 1] = new ParticleVertex(particles[i].verticies[1]);
                    verticies[i * 4 + 2] = new ParticleVertex(particles[i].verticies[2]);
                    verticies[i * 4 + 3] = new ParticleVertex(particles[i].verticies[3]);

                    indicies[i * 6] = (i * 4);
                    indicies[i * 6 + 1] = (i * 4 + 1);
                    indicies[i * 6 + 2] = (i * 4 + 3);
                    indicies[i * 6 + 3] = (i * 4 + 2);
                    indicies[i * 6 + 4] = (i * 4 + 3);
                    indicies[i * 6 + 5] = (i * 4 + 1);
                }
                if (i == 1)
                {
                    particles[i] = new Particle(particleStart + new Vector3(700, -65, 0), i);
                    verticies[i * 4] = new ParticleVertex(particles[i].verticies[0]);
                    verticies[i * 4 + 1] = new ParticleVertex(particles[i].verticies[1]);
                    verticies[i * 4 + 2] = new ParticleVertex(particles[i].verticies[2]);
                    verticies[i * 4 + 3] = new ParticleVertex(particles[i].verticies[3]);

                    indicies[i * 6] = (i * 4);
                    indicies[i * 6 + 1] = (i * 4 + 1);
                    indicies[i * 6 + 2] = (i * 4 + 3);
                    indicies[i * 6 + 3] = (i * 4 + 2);
                    indicies[i * 6 + 4] = (i * 4 + 3);
                    indicies[i * 6 + 5] = (i * 4 + 1);
                }
                if (i == 2)
                {
                    particles[i] = new Particle(particleStart + new Vector3(0, 265, 0), i);
                    verticies[i * 4] = new ParticleVertex(particles[i].verticies[0]);
                    verticies[i * 4 + 1] = new ParticleVertex(particles[i].verticies[1]);
                    verticies[i * 4 + 2] = new ParticleVertex(particles[i].verticies[2]);
                    verticies[i * 4 + 3] = new ParticleVertex(particles[i].verticies[3]);

                    indicies[i * 6] = (i * 4);
                    indicies[i * 6 + 1] = (i * 4 + 1);
                    indicies[i * 6 + 2] = (i * 4 + 3);
                    indicies[i * 6 + 3] = (i * 4 + 2);
                    indicies[i * 6 + 4] = (i * 4 + 3);
                    indicies[i * 6 + 5] = (i * 4 + 1);
                }
                if (i == 3)
                {
                    particles[i] = new Particle(particleStart + new Vector3(0, -445, 0), i);
                    verticies[i * 4] = new ParticleVertex(particles[i].verticies[0]);
                    verticies[i * 4 + 1] = new ParticleVertex(particles[i].verticies[1]);
                    verticies[i * 4 + 2] = new ParticleVertex(particles[i].verticies[2]);
                    verticies[i * 4 + 3] = new ParticleVertex(particles[i].verticies[3]);

                    indicies[i * 6] = (i * 4);
                    indicies[i * 6 + 1] = (i * 4 + 1);
                    indicies[i * 6 + 2] = (i * 4 + 3);
                    indicies[i * 6 + 3] = (i * 4 + 2);
                    indicies[i * 6 + 4] = (i * 4 + 3);
                    indicies[i * 6 + 5] = (i * 4 + 1);
                }
            }
            
            vBuffer = new VertexBuffer(device,new VertexDeclaration(ParticleVertex.VertexElements), verticies.Length, BufferUsage.WriteOnly);
            iBuffer = new IndexBuffer(device, typeof(int), indicies.Length, BufferUsage.WriteOnly);
            vBuffer.SetData(verticies);
            iBuffer.SetData(indicies);

            effect = particleEffect;

    
        }

        public void Draw(Matrix view, Matrix projection, GraphicsDevice device,float gameTime)
        {
            vBuffer.SetData(verticies);
            device.SetVertexBuffer(vBuffer);
            device.Indices = iBuffer;
            Vector2 center;
            center.X = 800;
            center.Y = 450;

            Matrix Projection = Matrix.CreateOrthographic(center.X * 2, center.Y * 2, 1, 1000);
            //effect.Parameters["World"].SetValue(world * Matrix.CreateTranslation(playerPos));
            effect.CurrentTechnique = effect.Techniques["Textured"];
            effect.Parameters["World"].SetValue(Matrix.Identity);
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["time"].SetValue(gameTime);
            effect.Parameters["size"].SetValue(180);


            device.DepthStencilState = DepthStencilState.DepthRead;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, verticies.Length, 0, indicies.Length / 3);
            }
            device.DepthStencilState = DepthStencilState.Default;
        }


    }
}
