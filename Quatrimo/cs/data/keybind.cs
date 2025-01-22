using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Quatrimo
{
    public class keybind
    {
        public Keys key1 { get; set; }
        public Keys key2 { get; set; }

        /// <summary>
        /// If they key has just been pressed this frame
        /// </summary>
        public bool keyDown = false;

        /// <summary>
        /// If the key has just been released this frame
        /// </summary>
        public bool keyUp = false;

        /// <summary>
        /// If the key is pressed this frame
        /// </summary>
        public bool keyHeld = false;
        public double timeHeld = 0;

        

        public keybind(Keys key1, Keys key2)
        {
            this.key1 = key1;
            this.key2 = key2;
        }
        public static void updateMouse()
        {
            //update mouse position adjusted for smaller renderTarget resolution
            //only run on moving mouse, run from Game1
            //store data as static here in keybind
            //mouseUpdated bool
        }

        public void updateKey(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(key1) || Keyboard.GetState().IsKeyDown(key2))
            {
                if (keyHeld) //if key is being pressed and has been held since the last frame: unflag keyDown
                {
                    keyDown = false;
                    timeHeld += gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else { keyDown = true; keyHeld = true; } //if key has just been pressed: flag keyDown and keyHeld
            }
            else if (keyHeld) { keyUp = true; keyHeld = false; keyDown = false; } //if key has just stopped being held
            else { keyUp = false; timeHeld = 0; } //if key is not being held and has not been held
        }

        public static void updateKeybinds(keybind[] keys, GameTime gameTime)
        {
            foreach (keybind keybind in keys)
            {
                keybind.updateKey(gameTime);
            }
        }

        public static keybind leftKey = new(Keys.Left, Keys.A);
        public static keybind rightKey = new(Keys.Right, Keys.D);
        public static keybind upKey = new(Keys.Up, Keys.W);
        public static keybind downKey = new(Keys.Down, Keys.S);
        public static keybind slamKey = new(Keys.Space, Keys.None);
        public static keybind leftRotateKey = new(Keys.Q, Keys.None);
        public static keybind rightRotateKey = new(Keys.E, Keys.None);
        public static keybind holdKey = new(Keys.F, Keys.None);
        public static keybind pieceAbilityKey = new(Keys.C, Keys.None);
        public static keybind restartKey = new(Keys.R, Keys.None);
        public static keybind pauseKey = new(Keys.Escape, Keys.None);
        public static keybind toggleDebugKey = new(Keys.OemTilde, Keys.None);

        public static keybind debugMode1 = new keybind(Keys.OemOpenBrackets, Keys.None);
        public static keybind debugMode2 = new keybind(Keys.OemCloseBrackets, Keys.None);
        public static keybind debugMode3 = new keybind(Keys.OemBackslash, Keys.None);

        public static keybind debugKey1 = new keybind(Keys.F1, Keys.None);
        public static keybind debugKey2 = new keybind(Keys.F2, Keys.None);
        public static keybind debugKey3 = new keybind(Keys.F3, Keys.None);
        public static keybind debugKey4 = new keybind(Keys.F4, Keys.None);
        public static keybind debugKey5 = new keybind(Keys.F5, Keys.None);

        public static keybind[] boardKeys = [leftKey, rightKey, upKey, downKey, slamKey, leftRotateKey, rightRotateKey, holdKey, pieceAbilityKey, restartKey, pauseKey, toggleDebugKey];
        public static keybind[] debugKeys = [debugMode1, debugMode2, debugMode3, debugKey1, debugKey2, debugKey3, debugKey4, debugKey5];


    }
}