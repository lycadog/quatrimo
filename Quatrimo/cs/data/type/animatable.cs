using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quatrimo;
using System.Collections.Generic;
using System.Diagnostics;

public class animatable
{

    public List<Texture2D> sequence = new List<Texture2D>(); //maybe combine texture and color into a class
    public List<Color> colors = new List<Color>();
    public int frame = 0;
    public bool loops;
    public bool removeSprite;

    public double time = 0;
    public double hangTime;

    public Texture2D oldTexture;
    public Color oldColor;

    public animatable(List<Texture2D> sequence, List<Color> colors, bool loops, double hangTime, bool removeSprite, element element)
    {
        this.sequence = sequence;
        this.colors = colors;
        this.loops = loops;
        this.hangTime = hangTime;
        this.removeSprite = removeSprite;
        time = 0;
        if (!removeSprite)
        {
            oldTexture = element.tex; oldColor = element.color;
        }
        else { oldTexture = Game1.empty; oldColor = Color.White; }
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
        //set the proper texture and color
        element.tex = sequence[frame];
        element.color = colors[frame];

        time += gameTime.ElapsedGameTime.TotalMilliseconds;

              

    }

    public void endSequence(element element)
    {
        element.tex = oldTexture; //this may need to be changed later! 
        element.color = oldColor;
        element.animatable = null;
    }

}
