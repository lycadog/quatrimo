using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class util
{
    public static Random rand = new Random();
    public static int randRange(int min, int max)
    {
        return rand.Next(min, max);
    }

}
