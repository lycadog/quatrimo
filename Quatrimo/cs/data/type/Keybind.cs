using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

public class keybind
{

    public Keys key1 { get; set; }
    public Keys key2 { get; set; } 

    public bool keyDown = false; //key is just pressed
    public bool keyUp = false; //key is just released
    public bool keyHeld = false; //key is held

    public keybind(Keys key1, Keys key2)
    {
        this.key1 = key1;
        this.key2 = key2;
    }

    public void updateKey()
    {
        if (Keyboard.GetState().IsKeyDown(key1) || Keyboard.GetState().IsKeyDown(key2))
        {
            if(keyHeld) //if key is being pressed and has been held since the last frame: unflag keyDown
            {
                keyDown = false;
            }
            else { keyDown = true; keyHeld = true; } //if key has just been pressed: flag keyDown and keyHeld
        }
        else if (keyHeld) { keyUp = true; keyHeld = false; keyDown = false; } //if key has just stopped being held
        else { keyUp = false; } //if key is not being held and has not been held
    }

    public static void updateKeybinds(keybind[] keys)
    {
        foreach(keybind keybind in keys)
        {
            keybind.updateKey();
        }
    }

}