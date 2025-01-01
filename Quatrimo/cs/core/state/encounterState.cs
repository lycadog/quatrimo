
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quatrimo
{
    public class encounterState : gameState
    {
        protected stateManager.gameDelegate updates;

        public encounterState(stateManager manager) : base(manager)
        {
        }

        public void pause()
        {
            updates = manager.updateD;
            manager.updateD = pausedUpdate;

            manager.paused = true;
            manager.encounter.board.sprites.Add(manager.encounter.board.pauseText );
        }

        public void unpause()
        {
            manager.updateD = updates;

            manager.paused = false;
            manager.encounter.board.sprites.Remove(manager.encounter.board.pauseText);
        }

        public override void setState()
        {
            manager.keyUpdateD = updateBoardKeys;
            manager.updateD = encounterUpdate;
            manager.drawScene = drawBoardScene;
            manager.drawText = drawBoardText;

            manager.state.Clear();
            manager.state.Add(this);
        }

        public override void addState()
        {
            manager.keyUpdateD += updateBoardKeys;
            manager.updateD += encounterUpdate;
            manager.drawScene += drawBoardScene;
            manager.drawText += drawBoardText;

            manager.state.Add(this);
        }

        public override void removeState()
        {
            manager.keyUpdateD -= updateBoardKeys;
            manager.updateD -= encounterUpdate;
            manager.drawScene -= drawBoardScene;
            manager.drawText -= drawBoardText;

            manager.state.Remove(this);
        }

        protected void updateBoardKeys(GameTime gameTime)
        {
            keybind.updateKeybinds(keybind.boardKeys, gameTime);
        }

        protected void encounterUpdate(GameTime gameTime)
        {
            manager.encounter.update(gameTime);

            if (keybind.pauseKey.keyDown)
            {
                pause();
            }

            if (keybind.restartKey.keyDown)
            {
                manager.encounter = new encounter(manager.encounter.bag);
            }

            if (keybind.toggleDebugKey.keyDown)
            {
                if (!manager.debugMode){
                    manager.debugMode = true; 
                    new debugState(manager).addState();
                }
                
                else { 
                    manager.debugMode = false;
                    manager.removeState<debugState>();
                }
            }
        }

        protected void pausedUpdate(GameTime gameTime)
        {
            if (keybind.pauseKey.keyDown)
            {
                unpause();
            }
        }

        protected void drawBoardScene(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texs.bg, new Rectangle(0, 0, Game1.baseRes.x, Game1.baseRes.y), null, new Color(new Vector3(0.02f, 0.0f, 0.01f)), 0, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(texs.bg, new Rectangle(0, 0, Game1.baseRes.x, Game1.baseRes.y), null, new Color(new Vector3(0.01f, 0.00f, 0.16f)), 0, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

            manager.encounter.board.draw(spriteBatch, gameTime);
        }

        protected void drawBoardText(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(texs.fontSmall, "SCORE: " + manager.encounter.totalScore.ToString(), new Vector2(680, 300), Color.White);
            spriteBatch.DrawString(texs.fontSmall, "LVL: " + manager.encounter.level.ToString(), new Vector2(680, 320), Color.White);
            spriteBatch.DrawString(texs.fontSmall, "X: " + manager.encounter.levelTimes.ToString(), new Vector2(680, 340), Color.White);
            spriteBatch.DrawString(texs.fontSmall, "LVL UP IN: " + (manager.encounter.rowsRequired - manager.encounter.rowsCleared).ToString() + " ROWS", new Vector2(680, 360), Color.White);
        }
    }
}
