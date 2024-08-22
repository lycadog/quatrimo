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

    public double time = 0;
    public double hangTime;

    public animatable(List<Texture2D> sequence, List<Color> colors, bool loops, double hangTime)
    {
        this.sequence = sequence;
        this.colors = colors;
        this.loops = loops;
        this.hangTime = hangTime;
        time = 0;
    }

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
        element.tex = Game1.empty; //this may need to be changed later! 
        element.color = Color.White;
        element.animatable = null;
    }

    /*public List<string> sequence = new List<string>(); //the list of characters to display in order
    public int frame; //indicates how far in the sequence the animation is
    public bool loops;
    public Vector2I pos;

    public double time;
    public double hangTime; //time needed before progressing to the next frame

    public board board;

    public animatable(List<string> sequence, bool loops, Vector2I pos, double hangTime, board board)
    {
        this.sequence = sequence;
        this.loops = loops;
        this.pos = pos;
        this.hangTime = hangTime;
        this.board = board;

        frame = 0;
        time = 0;
        advanceSequence();
    }

    public void tick(double deltaTime)
    {
        
        time += deltaTime;
        if(time > hangTime) //if enough time has passed, advance the sequence
        {
            advanceSequence();
            time = 0;
        }
    }

    public void advanceSequence()
    {
        if (frame >= sequence.Count) //if the sequence is over: loop if the animation loops, otherwise end the animation
        {
            if (loops) { frame = 0; } else { endSequence(); return; }
        }
        
        renderable render = new renderable(pos, sequence[frame], 2, false); //create a render object to add to the render queue
        //board.tickRenderQueue.Add(render);

        frame++; //increment frame
    }

    public void endSequence()
    {
        //remove from graphics
        //board.markTickStale(pos, 2);
        //board.staleAnimatables.Add(this);
    }*/
}
