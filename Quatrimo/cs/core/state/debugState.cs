
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quatrimo
{
    public class debugState : gameState
    {
        public debugState(stateManager manager) : base(manager)
        {
        }

        public override void addState()
        {
            manager.keyUpdateD += updateDebugKeys;
            manager.updateD += debugUpdate;
            manager.drawText += drawDebugText;
            manager.state.Add(this);
        }

        public override void removeState()
        {
            manager.keyUpdateD -= updateDebugKeys;
            manager.updateD -= debugUpdate;
            manager.drawText -= drawDebugText;
            manager.state.Remove(this);
        }

        public override void setState()
        {
            manager.keyUpdateD = updateDebugKeys;
            manager.updateD = debugUpdate;
            manager.drawText = drawDebugText;
            manager.state.Clear();
            manager.state.Add(this);
        }


        protected void updateDebugKeys(GameTime gameTime)
        {
            keybind.updateKeybinds(data.debugKeys, gameTime);
        }
        protected void debugUpdate(GameTime gameTime)
        {
            if (data.debugMode1.keyDown)
            {
                //updateDel.setTemporaryState(debugSlowModeUpdate);
                //keyDel.setTemporaryState(updateDebugKeys);
            }
        }

        protected void drawDebugText(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(Game1.font, "DEBUG", new Vector2(450, 20), Color.Yellow);

            //spriteBatch.DrawString(Game1.fontSmall, $"gamestate: {manager.encounter.stateOld}", new Vector2(20, 460), Color.CornflowerBlue);
            //spriteBatch.DrawString(Game1.fontSmall, $"piecefallTimer {manager.encounter.piecefallTimer}/600", new Vector2(20, 480), Color.LightBlue);
            //spriteBatch.DrawString(Game1.fontSmall, $"placeTimer {manager.encounter.placeTimer}/1000", new Vector2(20, 500), Color.LightBlue);
            //spriteBatch.DrawString(Game1.fontSmall, $"pieceWaitTimer {manager.encounter.pieceWaitTimer}/5000", new Vector2(20, 520), Color.LightBlue);
        }
    }
}
