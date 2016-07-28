using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Emotiv;
using AForge.Math;
using System.Threading;
using Accord.Statistics.Analysis;
using Accord.Statistics;
using Accord.Controls;
using Accord.Math;
using Accord.Math.Decompositions;
using Accord.Statistics.Formats;
using Accord.Audio;
using Accord.Audio.Filters;

using SVM;

namespace Capstone
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Random Randy = new Random();
        //int randycounter =0;
        //private float _elapsed_timer = 0.0f;
        //for graphics
        private int h, w;
        SpriteBatch spritrBatch;
        GraphicsDeviceManager graphics;
        Camera cam;
        private UserInterface ui;
        private BoundingSphereRenderer rendy;
        float tim = 0;
        //for the fps of the app and the emotiv
        private int _total_frames = 0;
        private int _total_frames_emotiv = 0;
        private float _elapsed_time = 0.0f;
        private int _fps = 0;
        private int _fpsEmotiv = 0;

        //the variables for the math
        private static int numNodes = 4;
        private static int max = 512;/////the length of the data. greater length means more time to complete full cycle. 
        private static float fpser = 600f;
        private float abc = 1;
        private Class1 classAct;

        private double[,] nodeRay16;

        private List<double>[] nodeRay16buffer = new List<double>[numNodes];
        private double[][] nodeRay16Foyae = new double[numNodes][];
        private int answer = 0;
        private KeyboardState preKey;
        private List<Node[]> _X;
        private List<double> _Y;
        private bool sample = false;
        private bool drawFreq = false;
        private bool gane = false;
        EmoEngine engine = EmoEngine.Instance;
        Node[] no = new Node[max * 4];
        Texture2D blankTexture;
        Texture2D back;
        Texture2D box;
        Texture2D logo;
        ParticleEmitter pSystem;
        SpriteFont taunt;
        Thread dread;

        bool locked = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            dread = new Thread(doShit);
            dread.IsBackground = true;

            spritrBatch = new SpriteBatch(graphics.GraphicsDevice);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            // graphics.IsFullScreen = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / fpser);/////////////the fps
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();
            Window.AllowUserResizing = true;

            IsMouseVisible = true;
            Viewport vp = GraphicsDevice.Viewport;
            cam = new Camera(GraphicsDevice, 10000);
            rendy = new BoundingSphereRenderer(GraphicsDevice, cam.view, cam.projection);
            h = vp.Height;
            w = vp.Width;


            _X = new List<Node[]>();
            _Y = new List<double>();


            nodeRay16 = new double[numNodes, max];
            for (int i = 0; i < numNodes; i++)
            {
                nodeRay16buffer[i] = new List<double>();
                nodeRay16Foyae[i] = new double[max];

                for (int j = 0; j < max; j++)
                {
                    nodeRay16[i, j] = 0;
                    nodeRay16buffer[i].Add(0);
                }
            }

            // engine.Connect();

            Thread.Sleep(2000);



            dread.Start();

            base.Initialize();

        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            SpriteFont font = Content.Load<SpriteFont>("fps");
            taunt = font;
            ui = new UserInterface(font, GraphicsDevice, h, w);
            //engine.DataAcquisitionEnable(0, true);

            blankTexture = new Texture2D(GraphicsDevice, 5, 5, false, SurfaceFormat.Color);

            Color[] color = new Color[25];
            for (int i = 0; i < color.Length; i++)
            {
                if (i > color.Length / 2)
                    color[i] = Color.White;
                else
                    color[i] = Color.Black;

            }
            blankTexture.SetData(color);

            Effect particleEffect = Content.Load<Effect>("ParticleShader");
            pSystem = new ParticleEmitter(GraphicsDevice, particleEffect, Microsoft.Xna.Framework.Vector3.Zero);

            back = Content.Load<Texture2D>("bg");
            box = Content.Load<Texture2D>("box");
            logo = Content.Load<Texture2D>("logo");
            classAct = new Class1(max, Content.Load<Texture2D>("snake"), Content.Load<Texture2D>("apple"));
            GC.Collect();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float timePassed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            _elapsed_time += timePassed;
            tim = _elapsed_time;
            var currentKeyboardState = Keyboard.GetState();
            // _elapsed_timer += timePassed;

            // if (_elapsed_timer >= 10000.0f)
            //  {
            //     randycounter = Randy.Next(1, 5);
            //     _elapsed_timer = 0;
            //  }



            //cam.UpdateCamera(playerPos, playerRotation);
            //AF3, F7, F3, FC5, T7, P7, O1, O2, P8, T8, FC6, F4, F8, AF4
            //    /*


            //fps update


            //yeah yeah...

            // cam.UpdateCamera(currentKeyboardState);
            //exit game
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }



            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                abc += 0.1f;

            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                abc -= 0.1f;

            }


            if (preKey != currentKeyboardState)
            {


                if (currentKeyboardState.IsKeyDown(Keys.M))
                {
                    drawFreq = true;
                    abc = 1f;
                }
                if (currentKeyboardState.IsKeyDown(Keys.N))
                {
                    drawFreq = false;
                    abc = 1f;
                }
                if (currentKeyboardState.IsKeyDown(Keys.G))
                {
                    if (gane)
                        gane = false;
                    else
                        gane = true;
                }
                if (currentKeyboardState.IsKeyDown(Keys.P))
                {
                    for (int i = 0; i < max; i++)
                    {
                        if (nodeRay16Foyae[1].ElementAt(i) == 1)
                        {
                            //Console.WriteLine(i);   
                            Console.WriteLine((64f / max) * i);
                        }
                    }



                }

            }


            preKey = currentKeyboardState;

            base.Update(gameTime);


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            float timm = (float)gameTime.TotalGameTime.TotalSeconds;
            /////////////// _total_frames++;
            Console.WriteLine(tim);

            nodeRay16buffer[0].Add(Math.Sin(2f * Math.PI * 1f * gameTime.TotalGameTime.TotalSeconds));

            //blu

            nodeRay16buffer[1].Add(Math.Sin(2f * Math.PI * 2f * gameTime.TotalGameTime.TotalSeconds));

            //rouge

            nodeRay16buffer[2].Add(Math.Sin(2f * Math.PI * 4f * gameTime.TotalGameTime.TotalSeconds));

            //yell-o

            nodeRay16buffer[3].Add(Math.Sin(2f * Math.PI * 6f * gameTime.TotalGameTime.TotalSeconds));


            //}
            for (int i = 0; i < numNodes; i++)
            {
                if (nodeRay16buffer[i].Count >= max)
                {
                    int xyz = nodeRay16buffer[i].Count - max;
                    for (int k = 0; k < xyz; k++)
                    {
                        nodeRay16buffer[i].RemoveAt(0);
                    }

                }
            }
            locked = false;
            double[][] pro = new double[numNodes][];
            for (int i = 0; i < numNodes; i++)
            {
                pro[i] = new double[max];
                pro[i] = Budder530(nodeRay16buffer[i].ToArray());

            }


            pro = pcaPepcieMax(pro).Transpose();
            for (int i = 0; i < numNodes; i++)
            {
                nodeRay16Foyae[i] = normalData(fftwave(pro[i]), 1, 0);
            }

            GraphicsDevice.Clear(Color.Black);

            _total_frames++;

            if (!gane)
            {
                if (!drawFreq)
                {
                    for (int i = 0; i < numNodes; i++)
                        if (!locked)
                            rendy.RenderLine(normalData(nodeRay16buffer[i].ToArray(), 1, 0), max, abc, i);

                }
                if (drawFreq)
                {

                    spritrBatch.Begin();
                    spritrBatch.DrawString(taunt, "5", new Vector2(480, 700), Color.White);

                    spritrBatch.DrawString(taunt, "60", new Vector2(1105, 700), Color.White);
                    spritrBatch.DrawString(taunt, "FREQUENCY", new Vector2(705, 800), Color.White);
                    spritrBatch.DrawString(taunt, "POWER", new Vector2(100, 400), Color.White);

                    spritrBatch.End();
                    for (int i = 0; i < numNodes; i++)
                        rendy.RenderLine(nodeRay16Foyae[i], max, abc, i);

                    rendy.Rendy(max);
                }
            }
            else
            {


                spritrBatch.Begin();

                spritrBatch.Draw(back, new Rectangle(0, 0, w, h), Color.White);
                spritrBatch.Draw(box, new Rectangle(w / 4, h / 4, w / 2, h / 2), Color.White);

                classAct.Draw(spritrBatch, blankTexture, w, h);


                spritrBatch.DrawString(taunt, "Up", new Vector2(778, 190), Color.White);
                spritrBatch.DrawString(taunt, "Down", new Vector2(755, 670), Color.White);

                spritrBatch.DrawString(taunt, "Left", new Vector2(280, 400), Color.White);

                spritrBatch.DrawString(taunt, "Right", new Vector2(1250, 400), Color.White);

                spritrBatch.DrawString(taunt, "Score: " + (classAct.count - 1), new Vector2(1400, 0), Color.White);

                spritrBatch.Draw(logo, new Rectangle(1320, 725, 300, 200), Color.White);
                spritrBatch.End();

                pSystem.Draw(cam.view, cam.projection, GraphicsDevice, tim);
            }


            // rendy.RendyBlueLine(max,iteraror);

            ui.Draw(_fps, _total_frames_emotiv);

            base.Draw(gameTime);
        }

        void doShit()
        {
            while (true)
            {

                var currentKeyboardState = Keyboard.GetState();
                if (_elapsed_time >= 1000.0f)
                {

                    _fps = _total_frames;
                    _total_frames = 0;
                    _elapsed_time = 0;
                    _total_frames_emotiv = _fpsEmotiv;
                    _fpsEmotiv = 0;

                    // _fpsEmotiv = (int)engine.DataGetSamplingRate(0);
                    //an update that is only needed once a second for both treesngrass and the ui
                    //Dictionary<EdkDll.EE_DataChannel_t, double[]> data = engine.GetData(0);

                    //if (data != null)
                    //{
                    //locked = true;
                    _fpsEmotiv++;
                    //green


                    //classAct.Update(currentKeyboardState, answer, pro[1]);


                    //  */

                    /*
                     for (int i = 0; i < numNodes; i++)
                     {
                             nodeRay16buffer[i].Add(Math.Sin(2f * Math.PI * (i+2) * gameTime.TotalGameTime.TotalSeconds) * 30);
                             if (nodeRay16buffer[i].Count >= max)
                             {
                                 int xyz = nodeRay16buffer[i].Count - max;
                                 for (int k = 0; k < xyz; k++)
                                 {
                                     nodeRay16buffer[i].RemoveAt(0);
                                 }

                            }
                     }
             
                    */

                }

            }
        }
        ////////////HELPER METHODS
        //multiply matrix
        public double[][] MultiplyMatrix(double[][] A, double[,] B)
        {
            int rA = A.Length;
            int cA = A[0].Length;
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            double temp = 0;
            double[][] kHasil = new double[rA][];

            if (cA != rB)
            {
                //Console.WriteLine("matrik can't be multiplied !!");
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    kHasil[i] = new double[cB];
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i][k] * B[k, j];
                        }
                        kHasil[i][j] = temp;
                    }
                }
                return kHasil;
            }
            return kHasil;
        }
        //normalize a number between two points
        public double[] normalData(double[] x, double _max, double _min)
        {
            double dataMax = x[0];
            double dataMin = x[0];

            foreach (double d in x)
            {
                if (d > dataMax)
                    dataMax = d;
                if (d < dataMin)
                    dataMin = d;
            }

            for (int i = 0; i < max; i++)
            {
                x[i] = (_min + (x[i] - dataMin) * (_max - _min)) / (dataMax - dataMin);
                //x[i] -= 1;

            }

            return x;
        }
        //dot product
        public double dotProduct(double[] a, double[] b, int length)
        {
            double runningSum = 0;
            for (int index = 0; index < length; index++)
                runningSum += a[index] * b[index];
            return runningSum;
        }
        //principle component analisys
        private double[][] pcaPepcieMax(double[][] dd8)
        {
            //data adjust
            double[] xFactor = new double[numNodes];

            for (int i = 0; i < numNodes; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    xFactor[i] += dd8[i][j];
                }
            }
            for (int i = 0; i < numNodes; i++)
            {
                xFactor[i] /= max;
            }
            for (int i = 0; i < numNodes; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    dd8[i][j] -= xFactor[i];
                }
            }

            //covariace matrix
            double[,] cMatrix = new double[numNodes, numNodes];

            for (int i = 0; i < numNodes; i++)
            {
                for (int j = 0; j < numNodes; j++)
                {
                    cMatrix[i, j] = cov(dd8[i], dd8[j]);
                }
            }

            var sumdi = new SingularValueDecomposition(cMatrix);

            double[,] evector = sumdi.RightSingularVectors;
            double[,] sector = new double[numNodes, numNodes - 2];

            for (int i = 0; i < numNodes; i++)
                for (int j = 0; j < numNodes - 2; j++)
                {
                    sector[i, j] = evector[i, j + 1]; //remove first and last principle component oui oui
                }


            //  v.Transpose();
            //  /*1st and last removed and reverted
            dd8 = MultiplyMatrix(dd8.Transpose(), sector);

            dd8 = MultiplyMatrix(dd8, sector.Transpose());
            for (int i = 0; i < numNodes; i++)
                for (int j = 0; j < max; j++)
                {
                    dd8[j][i] += xFactor[i];
                }


            return dd8;
            
            //principel components
            //dd8 = MultiplyMatrix(dd8.Transpose(), evector);
   
            //for (int i = 0; i < numNodes; i++)
            //    for (int j = 0; j < max; j++)
            //    {
            //        dd8[j][i] += xFactor[i];
            //    }
            //return dd8;
             
        }
        //the covariance
        public double cov(double[] a, double[] b)
        {
            double[] proxy = new double[a.Length];
            double xFactor = 0;

            for (int i = 0; i < a.Length; i++)
                proxy[i] = a[i] * b[i];

            for (int i = 0; i < a.Length; i++)
                xFactor += proxy[i];

            xFactor /= max - 1;

            return xFactor;
        }
        //the fourier
        public double[] fftwave(double[] dd8)
        {
            // create proxy wave
            double[] v = new double[max * 2];

            // 0 pad it
            for (int i = 0; i < max * 2; i++)
            {
                v[i] = 0;
            }
            // give it the wave data
            for (int i = 0; i < max; i++)
            {
                v[i] = dd8[i];
            }

            //covariance the wave
            double[] l = new double[max];
            double[] x = new double[max * 2];//new main vaue
            for (int i = -max; i < max; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    if (j + i < 0 || j + i > max)
                    {
                        l[j] = 0;

                    }
                    else
                    {
                        l[j] = v[i + j];

                    }

                }
                x[max + i] = dotProduct(l, v, max);
            }

            //turn it into aforge style wave and fft
            //ComplexSignal c12 = new ComplexSignal(1, max, 125);
            // c12.
            AForge.Math.Complex[] c1 = new AForge.Math.Complex[x.Length];
            for (int i = 0; i < c1.Length; i++)
            {
                c1[i] = new AForge.Math.Complex(x[i], 0);

            }
            FourierTransform.FFT(c1, FourierTransform.Direction.Forward);

            //return the wave back to doulbe
            for (int i = 0; i < c1.Length / 2; i++)
            {

                // dd8[i] = Math.Abs(c1[i].Re);

                dd8[i] = Math.Abs(c1[i].Re);
            }

            return dd8;
        }
        //butter filter
        public double[] Budder530(double[] dd8)
        {
            double GAIN = 1.379370774e+00;
            double[] xv, yv;
            xv = new double[5];
            yv = new double[5];

            double[] dd7 = dd8;

            for (int i = 0; i < max; i++)
            {
                xv[0] = xv[1];
                xv[1] = xv[2];
                xv[2] = xv[3];
                xv[3] = xv[4];
                xv[4] = dd8[i] / GAIN;
                yv[0] = yv[1];
                yv[1] = yv[2];
                yv[2] = yv[3];
                yv[3] = yv[4];
                yv[4] = (xv[0] + xv[4]) - 4 * (xv[1] + xv[3]) + 6 * xv[2]
                             + (-0.5255789745 * yv[0]) + (2.4389986574 * yv[1])
                             + (-4.2755090771 * yv[2]) + (3.3594051013 * yv[3]);

                dd7[i] = yv[4];
            }

            return dd7;
        }
        ////////////HELPER METHODS
    }
}
