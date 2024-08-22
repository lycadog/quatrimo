
using Microsoft.Xna.Framework;

public class colorSet
{
    public colorSet(Color[] colors)
    {
        this.colors = colors;
    }

    public Color[] colors;
    public Color getRandomColor()
    {
        return colors[ util.rand.Next(colors.Length - 1) ];
    }
}
