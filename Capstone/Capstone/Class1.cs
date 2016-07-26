using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Capstone
{
    class Class1
    {
        static int numx = 16;
        static int numy = 9;

        int[][] gridSmith;

        int x, y, xx, yy;

        List<int[]> snake;
        Random ran = new Random();

        public int count = 1;
        int max;
        Texture2D picSnake;
        Texture2D picApple;
        public Class1(int m, Texture2D sna, Texture2D app)
        {
            max = m;
            picSnake = sna;
            picApple = app;
            snake = new List<int[]>();
            gridSmith = new int[numx][];
            for (int i = 0; i < numx; i++)
            {
                gridSmith[i] = new int[numy];
                for (int j = 0; j < numy; j++)
                    gridSmith[i][j] = 0;
            }

            //starting position
            x = numx / 2;
            y = numy / 2;
            snake.Add(new int[] { x, y });

            //starting object
            xx = 7;
            yy = 2;
   
        }

        public void Update(KeyboardState statr, int answ, double[] sds)
        {
            double avg = sds.Average();
         //   double stand = 0;
          //  for (int i = 0; i < sds.Count(); i++)
          //  {
          //      stand += Math.Pow(sds[i] - avg, 2);
          ////  }
         //   stand /= sds.Count() - 1;
         //   stand = Math.Sqrt(stand);
            //Console.WriteLine(avg);
            if (avg>0.07)
            {
                //Console.WriteLine("mike is pleb");
               
            }
            else
            {
                double a = crossConfirm(sds, 7.5);//up
                double b = crossConfirm(sds, 5); //down
                double c = crossConfirm(sds, 12.75f);//left
                double d = crossConfirm(sds, 10); //right

                double abcd = a;
                if (b > abcd)
                    abcd = b;
                if (c > abcd)
                    abcd = c;
                if (d > abcd)
                    abcd = d;



                if (abcd==a&&abcd>0.9)
                    y--;
                if (abcd == b && abcd > 0.9)
                    y++;
                if (abcd == c && abcd > 0.9)
                    x--;
                if (abcd == d && abcd > 0.9)
                    x++;
            }
           // if (crossConfirm(sds, 15) > 0.9)
           //     y++;

            
          
            if (statr.IsKeyDown(Keys.Up))
                y--;
            if (statr.IsKeyDown(Keys.Down))
                y++;
            if (statr.IsKeyDown(Keys.Left))
                x--;
            if (statr.IsKeyDown(Keys.Right))
                x++;
            if (x >= numx)
                x = 1;
            if (x <= 0)
                x = numx-1;
            if (y >= numy)
                y = 1;
            if (y <= 0)
                y = numy-1;
            if (x == xx && y == yy)
            {
                count++;

                snake.Add(new int[] { x, y });
                xx = ran.Next(1, numx);
                yy = ran.Next(1, numy);
            }
            else
            {
                snake.Add(new int[] { x, y });
            
                    snake.RemoveAt(0);
            }

            for (int i = 0; i < numx; i++)
                for (int j = 0; j < numy; j++)
                    if(xx==i&&yy==j)
                        gridSmith[i][j] = 2;
                    else
                        gridSmith[i][j] = 0;
            foreach(int[] k in snake)
                gridSmith[k[0]][k[1]] = 1;


        //    Console.WriteLine(snake.Count);
        }

        public void Draw(SpriteBatch god, Texture2D t, int w, int h)
        {
            for (int i = 0; i < numx; i++)
                for (int j = 0; j < numy; j++)
                    if (gridSmith[i][j] == 1)
                        god.Draw(picSnake, new Rectangle(w / 4 + i * (w / 2) / numx-20, h / 4 + j * (h / 2) / numy-20, (w / 2) / numx, (h / 2) / numy), Color.White);
                    else if (gridSmith[i][j] == 2)
                        god.Draw(picApple, new Rectangle(w / 4 + i * (w / 2) / numx-20, h / 4 + j * (h / 2) / numy-20, (w / 2) / numx, (h / 2) / numy), Color.White);
        }

        public double crossConfirm(double[] x, double freq)
        {
            double prox = (max / 64) * freq;
            if (prox % 1 != 0)
                prox = Math.Truncate(prox) - 2;
            
            int proxprox = (int)prox;
            double[] bin = new double[5];
           //Console.WriteLine(prox);
         
            for (int i = 0; i < bin.Length; i++)
            {
                bin[i] = x[i + proxprox - 2];
            }

            return bin.Max();
        }

        public bool analyze()
        {

            return true;
        }

    }
}
