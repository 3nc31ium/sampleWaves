using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class UserInterface
{

    SpriteFont font;
    SpriteBatch spritrBatch;
    int _h, _w;
    Point p;
    public UserInterface(SpriteFont _font, GraphicsDevice g,int h, int w)
    {
        _h = h;
        _w = w;
   
        font = _font;
        spritrBatch = new SpriteBatch(g);
        p = new Point(w/2-50,h/2-50);
    }



    public void Draw(float _fps, double amp)
    {
        spritrBatch.Begin();

        //draw the fps counter
     //   spritrBatch.DrawString(font, "delt" + d, new Vector2(40, 400), Color.Wheat, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
     //   spritrBatch.DrawString(font, "thet" + t, new Vector2(150, 400), Color.Wheat, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
     //   spritrBatch.DrawString(font, "alph" + a, new Vector2(300, 400), Color.Wheat, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
     //   spritrBatch.DrawString(font, "bet" + b, new Vector2(550, 400), Color.Wheat, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
     //   spritrBatch.DrawString(font, "gamm" +g, new Vector2(750, 400), Color.Wheat, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
        
        spritrBatch.DrawString(font, "FPS: " + _fps, new Vector2(15, 0), Color.White);
        spritrBatch.DrawString(font, "FPS ePacket: " + amp, new Vector2(250, 0), Color.White);
        
 

      //  spritrBatch.DrawString(font, "Red: " + ampp, new Vector2(15, 550), Color.White);

 
        //spritrBatch.DrawString(font, "MED: " + med, new Vector2(600, 500), Color.White);
       

       // spritrBatch.DrawString(font, "5", new Vector2(140, 350), Color.GreenYellow);
       // spritrBatch.DrawString(font, "10", new Vector2(250, 350), Color.GreenYellow);
        //spritrBatch.DrawString(font, "15", new Vector2(375, 350), Color.GreenYellow);
       // spritrBatch.DrawString(font, "20", new Vector2(500, 350), Color.GreenYellow);
       // spritrBatch.DrawString(font, "25", new Vector2(625, 350), Color.GreenYellow);
     
        //[ rawValue * (1.8/4096) ] / 2000
       // spritrBatch.DrawString(font, "Voltage: " + (r*(1.8 / 4096))/2000, new Vector2(250, 450), Color.LawnGreen);
       // spritrBatch.DrawString(font, "Meditation: " + med, new Vector2(15, 500), Color.MediumPurple);
       // spritrBatch.DrawString(font, "Attention: " + att, new Vector2(15, 550), Color.PapayaWhip);


        spritrBatch.End();
    }

}

