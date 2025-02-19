
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Quatrimo
{
    public class encounterState : gameState
    {
        //rework later and figure out what this delegate does - maybe compress this down onto the encounter,
        //adding the pause behavior to the update method - making this more of a wrapper for encounter
        //actually no, since pause behavior needs to suspend certain update states it should be handled by statemanager
        protected Action<GameTime> updates;

        public encounterState(stateManager manager) : base(manager)
        {
        }

        public void pause()
        {
            updates = manager.updateD;
            manager.updateD = pausedUpdate;

            manager.paused = true;
            manager.encounter.board.sprites.add(manager.encounter.board.pauseText );
        }

        public void unpause()
        {
            manager.updateD = updates;

            manager.paused = false;
            manager.encounter.board.sprites.remove(manager.encounter.board.pauseText);
        }

        public override void setState()
        {
            manager.keyUpdate = updateBoardKeys;
            manager.updateD = encounterUpdate;
            manager.drawBaseRes = drawBoardScene;
            manager.draw2xRes = drawBoardText;

            manager.state.Clear();
            manager.state.Add(this);
        }

        public override void addState()
        {
            manager.keyUpdate += updateBoardKeys;
            manager.updateD += encounterUpdate;
            manager.drawBaseRes += drawBoardScene;
            manager.draw2xRes += drawBoardText;

            manager.state.Add(this);
        }

        public override void removeState()
        {
            manager.keyUpdate -= updateBoardKeys;
            manager.updateD -= encounterUpdate;
            manager.drawBaseRes -= drawBoardScene;
            manager.draw2xRes -= drawBoardText;

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
            spriteBatch.Draw(content.bg, new Rectangle(0, 0, Game1.baseRes.x, Game1.baseRes.y), null, new Color(new Vector3(0.02f, 0.0f, 0.01f)), 0, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(content.bg, new Rectangle(0, 0, Game1.baseRes.x, Game1.baseRes.y), null, new Color(new Vector3(0.01f, 0.00f, 0.16f)), 0, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

            manager.encounter.board.sprites.drawBaseRes(spriteBatch, gameTime);
        }

        protected void drawBoardText(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(content.fontSmall, "SCORE: " + manager.encounter.totalScore.ToString(), new Vector2(680, 300), Color.White);
            spriteBatch.DrawString(content.fontSmall, "LVL: " + manager.encounter.level.ToString(), new Vector2(680, 320), Color.White);
            spriteBatch.DrawString(content.fontSmall, "X: " + manager.encounter.levelTimes.ToString(), new Vector2(680, 340), Color.White);
            spriteBatch.DrawString(content.fontSmall, "LVL UP IN: " + (manager.encounter.rowsRequired - manager.encounter.rowsCleared).ToString() + " ROWS", new Vector2(680, 360), Color.White);
        }
    }
}
