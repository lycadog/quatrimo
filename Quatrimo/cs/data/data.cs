using Microsoft.Xna.Framework;
using System;

namespace Quatrimo
{
    public static class data
    {
        public static keybind[] boardKeys = [leftKey, rightKey, upKey, downKey, slamKey, leftRotateKey, rightRotateKey, holdKey, restartKey, pauseKey, toggleDebugKey];
        public static keybind[] debugKeys = [debugMode1, debugMode2, debugMode3, debugKey1, debugKey2, debugKey3, debugKey4, debugKey5];

        public static keybind leftKey = new(Microsoft.Xna.Framework.Input.Keys.Left, Microsoft.Xna.Framework.Input.Keys.A);
        public static keybind rightKey = new(Microsoft.Xna.Framework.Input.Keys.Right, Microsoft.Xna.Framework.Input.Keys.D);
        public static keybind upKey = new(Microsoft.Xna.Framework.Input.Keys.Up, Microsoft.Xna.Framework.Input.Keys.W);
        public static keybind downKey = new(Microsoft.Xna.Framework.Input.Keys.Down, Microsoft.Xna.Framework.Input.Keys.S);
        public static keybind slamKey = new(Microsoft.Xna.Framework.Input.Keys.Space, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind leftRotateKey = new(Microsoft.Xna.Framework.Input.Keys.Q, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind rightRotateKey = new(Microsoft.Xna.Framework.Input.Keys.E, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind holdKey = new(Microsoft.Xna.Framework.Input.Keys.F, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind restartKey = new(Microsoft.Xna.Framework.Input.Keys.R, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind pauseKey = new(Microsoft.Xna.Framework.Input.Keys.Escape, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind toggleDebugKey = new(Microsoft.Xna.Framework.Input.Keys.OemTilde, Microsoft.Xna.Framework.Input.Keys.None);

        public static keybind debugMode1 = new keybind(Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugMode2 = new keybind(Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugMode3 = new keybind(Microsoft.Xna.Framework.Input.Keys.OemBackslash, Microsoft.Xna.Framework.Input.Keys.None);

        public static keybind debugKey1 = new keybind(Microsoft.Xna.Framework.Input.Keys.F1, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugKey2 = new keybind(Microsoft.Xna.Framework.Input.Keys.F2, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugKey3 = new keybind(Microsoft.Xna.Framework.Input.Keys.F3, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugKey4 = new keybind(Microsoft.Xna.Framework.Input.Keys.F4, Microsoft.Xna.Framework.Input.Keys.None);
        public static keybind debugKey5 = new keybind(Microsoft.Xna.Framework.Input.Keys.F5, Microsoft.Xna.Framework.Input.Keys.None);

        static Color[] allColors = [new Color(new Vector3(1, 0.16f, 0.16f)), new Color(new Vector3(1, .396f, .396f)), new Color(new Vector3(1, .325f, .102f)), new Color(new Vector3(.96f, .561f, .427f)), new Color(new Vector3(1, .561f, .102f)), new Color(new Vector3(.969f, .702f, .427f)), new Color(new Vector3(1, .729f, .102f)), new Color(new Vector3(.969f, .804f, .427f)), new Color(new Vector3(.961f, 1, 10.2f)), new Color(new Vector3(.945f, .969f, .427f)), new Color(new Vector3(.71f, 1, .102f)), new Color(new Vector3(.79f, .969f, .427f)), new Color(new Vector3(.243f, .957f, .169f)), new Color(new Vector3(.463f, .992f, .404f)), new Color(new Vector3(.106f, 1, .376f)), new Color(new Vector3(.404f, .992f, .58f)), new Color(new Vector3(.059f, 1, .714f)), new Color(new Vector3(.4f, .969f, .796f)), new Color(new Vector3(.059f, .965f, 1)), new Color(new Vector3(.4f, .945f, .969f)), new Color(new Vector3(.059f, .765f, 1)), new Color(new Vector3(.4f, .824f, .969f)), new Color(new Vector3(.059f, .518f, 1)), new Color(new Vector3(.4f, .675f, .969f)), new Color(new Vector3(.059f, .341f, 1)), new Color(new Vector3(.4f, .569f, .969f)), new Color(new Vector3(.102f, .059f, 1)), new Color(new Vector3(.427f, .4f, .969f)), new Color(new Vector3(36.5f, .059f, 1)), new Color(new Vector3(.584f, .4f, .969f)), new Color(new Vector3(.573f, .059f, 1)), new Color(new Vector3(.71f, .4f, .969f)), new Color(new Vector3(.647f, .059f, 1)), new Color(new Vector3(.753f, .4f, .969f)), new Color(new Vector3(.773f, .059f, 1)), new Color(new Vector3(.831f, .4f, .969f)), new Color(new Vector3(.945f, .059f, 1)), new Color(new Vector3(.933f, .4f, .969f)), new Color(new Vector3(1, .059f, .855f)), new Color(new Vector3(.969f, .4f, .878f)), new Color(new Vector3(1, .059f, .608f)), new Color(new Vector3(.969f, .4f, .729f)), new Color(new Vector3(1, .059f, .431f)), new Color(new Vector3(.969f, .4f, .624f)), new Color(new Vector3(1, .059f, .31f)), new Color(new Vector3(.969f, .4f, .553f))];

        public static Func<block>[] blocks = [
            () => { return new block(); },          //0
            () => { return new cursedBlock(); },    //1
            () => { return new bombBlock(); }       //2
        ];

        public static Func<encounter, Vector2I, Vector2I, string, boardPiece>[] pieces = [
            (encounter e, Vector2I dim, Vector2I orgn, string name) => { return new boardPiece(e, dim, orgn, name); }//0
        ];

        public static objPool<int> basic = new(0);

        public static pieceShape sLine = new(new bool[,] { { true, true, true, true } }, new Vector2I(4, 1), new Vector2I(1, 0), 4);
        public static pieceShape sSquare = new(new bool[,] { { true, true }, { true, true } }, new Vector2I(2, 2), new Vector2I(1, 1), 4);
        public static pieceShape sTBlock = new(new bool[,] { { true, true, true }, { false, true, false } }, new Vector2I(2, 3), new Vector2I(0, 1), 4);
        public static pieceShape sLeftL = new(new bool[,] { { false, false, true }, { true, true, true } }, new Vector2I(2,3), new Vector2I(0,2), 4);

        public static simplePiece basicLine = new(sLine, basic, 0, "Line", Game1.box, allColors);

        public static void initialize()
        {

        }

        public static void contentInit()
        {

        }
    }
}