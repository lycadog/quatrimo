
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Quatrimo
{
    public class encounterState : gameState
    {

        public encounterState(stateManager manager) : base(manager)
        {
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
            keybind.updateKeybinds(data.boardKeys, gameTime);
        }

        protected void encounterUpdate(GameTime gameTime)
        {
            manager.main.coreGameLoop(gameTime);

            if (data.pauseKey.keyDown)
            {
                manager.paused = true;
                manager.main.board.sprites.Add(manager.main.board.pauseText);
                //updateDel.setTemporaryState(encounterPaused);
            }

            if (data.restartKey.keyDown)
            {
                manager.main = new main(manager.bag);
            }

            if (data.toggleDebugKey.keyDown)
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

        protected void drawBoardScene(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Game1.bg, new Rectangle(0, 0, Game1.baseRes.x, Game1.baseRes.y), null, new Color(new Vector3(0.02f, 0.0f, 0.01f)), 0, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(Game1.bg, new Rectangle(0, 0, Game1.baseRes.x, Game1.baseRes.y), null, new Color(new Vector3(0.01f, 0.00f, 0.16f)), 0, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

            manager.main.board.draw(spriteBatch, gameTime);
        }

        protected void drawBoardText(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(Game1.fontSmall, "SCORE: " + manager.main.totalScore.ToString(), new Vector2(680, 300), Color.White);
            spriteBatch.DrawString(Game1.fontSmall, "LVL: " + manager.main.level.ToString(), new Vector2(680, 320), Color.White);
            spriteBatch.DrawString(Game1.fontSmall, "X: " + manager.main.levelTimes.ToString(), new Vector2(680, 340), Color.White);
            spriteBatch.DrawString(Game1.fontSmall, "LVL UP IN: " + (manager.main.rowsRequired - manager.main.rowsCleared).ToString() + " ROWS", new Vector2(680, 360), Color.White);
        }
    }
}
