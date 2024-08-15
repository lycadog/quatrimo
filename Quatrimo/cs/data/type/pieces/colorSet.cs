
public class colorSet
{
    public colorSet(string[] colors)
    {
        this.colors = colors;
    }

    public string[] colors;
    public string getRandomColor()
    {
        return colors[ util.rand.Next(colors.Length - 1) ];
    }
}
