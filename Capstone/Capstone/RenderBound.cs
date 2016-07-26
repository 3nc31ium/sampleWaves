/// <summary>
/// Provides a set of methods for rendering BoundingSpheres.
/// </summary>
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;


namespace Capstone
{

    public class BoundingSphereRenderer
    {
        private VertexBuffer vertBuffer;

        private VertexBuffer linusBuffer;

        private GraphicsDevice graphicsDevice;

        private Vector3 starting = new Vector3(-510, 0, -1000);

        static BasicEffect effect;

        public BoundingSphereRenderer(GraphicsDevice _graphicsDevice,
    Matrix view,
    Matrix projection)
    {
        graphicsDevice = _graphicsDevice;
        effect = new BasicEffect(_graphicsDevice);
        effect.LightingEnabled = false;
        effect.VertexColorEnabled = true;
        effect.World = Matrix.Identity;
        effect.View = view;
        effect.Projection = projection;
    }

        public void RenderLine(double[] power, int max, float amp,int g)
        {

            VertexPositionColor[] v;
            v = new VertexPositionColor[max];
            Color c = Color.White;
            if (g == 0)
                c = Color.Green;//af3
            if (g == 1)
            {
                c = Color.Blue;//af4;
            }
             if (g == 2)
            {
                c = Color.Red;//f3;
            }
            if (g == 3)
            {
                c = Color.Yellow;//f4;
            }

            for (int i = 0; i < v.Length; i++)
            {

                v[i] = new VertexPositionColor(starting + new Vector3(i * (1024/max), (g*200-300)+(float)power[i]*100*amp, 0), c);
            }
            vertBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), v.Length, BufferUsage.WriteOnly);
            vertBuffer.SetData(v); 
            graphicsDevice.SetVertexBuffer(vertBuffer);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //render each circle individually
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      0,
                      v.Length - 1);
            }

        }


        public void Rendy( int max)
        {

            
            VertexPositionColor[] linus;

            float maxy = max / (128 / 2);
            linus = new VertexPositionColor[24*4];//ammount of lines to draw
            Color c = new Color(1f, 1f, 1f, 0f);
            for (int z = 0; z < 24*4; z += 8)
            {
                //5 is distance between each line is hertz
                linus[z] = new VertexPositionColor(starting + new Vector3(((z / 8 + 1)*5 * maxy) * (1024 / max), -90, 0), c);
                linus[z + 1] = new VertexPositionColor(starting + new Vector3(((z / 8 + 1)*5 * maxy) * (1024 / max), -110, 0), c);

                linus[z+2] = new VertexPositionColor(starting + new Vector3(((z / 8 + 1) * 5 * maxy) * (1024 / max), 90, 0), c);
                linus[z + 3] = new VertexPositionColor(starting + new Vector3(((z / 8 + 1) * 5 * maxy) * (1024 / max), 110, 0), c);

                linus[z+4] = new VertexPositionColor(starting + new Vector3(((z / 8 + 1) * 5 * maxy) * (1024 / max), -290, 0), c);
                linus[z + 5] = new VertexPositionColor(starting + new Vector3(((z / 8 + 1) * 5 * maxy) * (1024 / max), -310, 0), c);

                linus[z+6] = new VertexPositionColor(starting + new Vector3(((z / 8 + 1) * 5 * maxy) * (1024 / max), 290, 0), c);
                linus[z + 7] = new VertexPositionColor(starting + new Vector3(((z / 8 + 1) * 5 * maxy) * (1024 / max), 310, 0), c);

            }

            linusBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), linus.Length, BufferUsage.WriteOnly);
            linusBuffer.SetData(linus);
            graphicsDevice.SetVertexBuffer(linusBuffer);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                //render each circle individually
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineList,
                      0,
                      linus.Length - 1);
            }


           

        }

        public void RendyBlueLine(int max, int iter)
        {

             VertexPositionColor[] linus;
            linus = new VertexPositionColor[2];
            linus[0] = new VertexPositionColor(starting + new Vector3(iter * (1024 / max), -2500, 0), Color.Blue);
            linus[1] = new VertexPositionColor(starting + new Vector3(iter * (1024 / max), 2500, 0), Color.Blue);
            linusBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), linus.Length, BufferUsage.WriteOnly);
            linusBuffer.SetData(linus);
            graphicsDevice.SetVertexBuffer(linusBuffer);
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();

                 //render each circle individually
                 graphicsDevice.DrawPrimitives(
                       PrimitiveType.LineStrip,
                       0,
                       linus.Length-1);
             }

            }




        
    }
}