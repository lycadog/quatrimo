using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public static class texs
    {
        public static Texture2DAtlas atlas;

        public static Texture2DRegion empty;
        public static Texture2DRegion full;
        public static Texture2DRegion full75;
        public static Texture2DRegion full50;
        public static Texture2DRegion full25;
        public static Texture2DRegion dropCorners;
        public static Texture2DRegion dropCrosshair;
        public static Texture2DRegion fullDropMark;

        public static Texture2DRegion borderUL;
        public static Texture2DRegion borderUR;
        public static Texture2DRegion borderDL;
        public static Texture2DRegion borderDR;
        public static Texture2DRegion borderU;
        public static Texture2DRegion borderD;
        public static Texture2DRegion borderL;
        public static Texture2DRegion borderR;
        public static Texture2DRegion nameQ; public static Texture2DRegion nameU; public static Texture2DRegion nameA;
        public static Texture2DRegion nameT; public static Texture2DRegion nameR; public static Texture2DRegion nameI;
        public static Texture2DRegion nameM; public static Texture2DRegion nameO;

        public static Texture2DRegion box; public static Texture2DRegion boxdetail; public static Texture2DRegion boxdetail2;
        public static Texture2DRegion boxdetail3; public static Texture2DRegion boxsolid;
        public static Texture2DRegion round; public static Texture2DRegion rounddetail; public static Texture2DRegion rounddetail2;
        public static Texture2DRegion rounddetail3; public static Texture2DRegion roundsolid;
        public static Texture2DRegion circle; public static Texture2DRegion circledetail; public static Texture2DRegion circledetail2;
        public static Texture2DRegion circledetail3; public static Texture2DRegion circlesolid;

        public static Texture2DRegion shuriken;
        public static Texture2DRegion bomb;
        public static Texture2DRegion cursedclosed;
        public static Texture2DRegion cursedopen;
        public static Texture2DRegion piercing;
        public static Texture2DRegion hologram1;
        public static Texture2DRegion hologram2;

        public static Texture2D none;
        public static Texture2D solid;
        public static Texture2D nextBox;
        public static Texture2D holdBox;
        public static Texture2D pausedtext;
        public static Texture2D corey;

        public static Texture2D bg;

        public static SpriteFont fontSmall;
        public static SpriteFont font;


        public static void loadContent(ContentManager Content)
        {
            fontSmall = Content.Load<SpriteFont>("fonts/ToshibaSatSmall");
            font = Content.Load<SpriteFont>("fonts/ToshibaSat");

            Texture2D atlasTex = Content.Load<Texture2D>("png/atlas");
            atlas = Texture2DAtlas.Create("atlas", atlasTex, 10, 10);


            empty = atlas[0];
            full = atlas[12];
            full75 = atlas[13];
            full50 = atlas[14];
            full25 = atlas[15];
            dropCorners = atlas[9];
            dropCrosshair = atlas[10];
            fullDropMark = atlas[11];

            borderUL = atlas[1];
            borderUR = atlas[3];
            borderDL = atlas[6];
            borderDR = atlas[8];
            borderU = atlas[2];
            borderD = atlas[7];
            borderL = atlas[4];
            borderR = atlas[5];

            box = atlas[36];
            boxdetail = atlas[37];
            boxdetail2 = atlas[38];
            boxdetail3 = atlas[39];
            boxsolid = atlas[40];
            round = atlas[48];
            rounddetail = atlas[49];
            rounddetail2 = atlas[50];
            rounddetail3 = atlas[51];
            roundsolid = atlas[52];
            circle = atlas[60];
            circledetail = atlas[61];
            circledetail2 = atlas[62];
            circledetail3 = atlas[63];
            circlesolid = atlas[64];

            bomb = atlas[123];
            cursedclosed = atlas[132];
            cursedopen = atlas[133];
            piercing = atlas[138];
            hologram1 = atlas[145];
            hologram2 = atlas[146];

            none = Content.Load<Texture2D>("empty");
            solid = Content.Load<Texture2D>("full");

            corey = Content.Load<Texture2D>("png/Corey");
            nextBox = Content.Load<Texture2D>("ui/nextbox");
            holdBox = Content.Load<Texture2D>("ui/holdbox");
            pausedtext = Content.Load<Texture2D>("ui/paused");

            bg = Content.Load<Texture2D>("misc/bg");

            nameQ = atlas[24];
            nameU = atlas[25];
            nameA = atlas[26];
            nameT = atlas[27];
            nameR = atlas[28];
            nameI = atlas[29];
            nameM = atlas[30];
            nameO = atlas[31];
        }
    }
}