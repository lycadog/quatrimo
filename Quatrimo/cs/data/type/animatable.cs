using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo;
using System.Collections.Generic;
using System.Diagnostics;

public class animatable
{
    //this class sucks dick
    public List<Texture2D> sequence = new List<Texture2D>(); //maybe combine texture and color into a class
    public List<Color> colors = new List<Color>();
    public int frame = 0;
    public bool loops;
    public bool overwrite;

    public double time = 0;
    public double hangTime;
    public animatable(List<Texture2D> sequence, List<Color> colors, bool loops, double hangTime, bool overwrite, element element)
    {
        this.sequence = sequence;
        this.colors = colors;
        this.loops = loops;
        this.hangTime = hangTime;
        this.overwrite = overwrite;
        time = 0;
    }

    //ISSUE: on low frame rates piece falling animation hangs too long!
    public void update(element element, GameTime gameTime)
    {

        if (time > hangTime) //if enough time has passed, advance the sequence
        {
            frame++;
            if (frame >= sequence.Count - 1)
            {
                if (loops) { frame = 0; } else { endSequence(element); }
                return;
            }
            time = 0;
        }

        time += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (!overwrite && element.tex != Game1.empty) { return; }

        //set the proper texture and color
        element.tex = sequence[frame];
        element.color = colors[frame];


              

    }

    public void endSequence(element element)
    {
        element.animatable = null;
        element.tex = Game1.empty; //this may need to be changed later! 
        element.color = Color.White;
    }

}
