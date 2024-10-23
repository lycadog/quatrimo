﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

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
            manager.main.board.sprites.Add( manager.main.board.pauseText );
        }

        public void unpause()
        {
            manager.updateD = updates;

            manager.paused = false;
            manager.main.board.sprites.Remove(manager.main.board.pauseText);
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
            Debug.WriteLine(manager.main.state.ToString());
            manager.main.coreGameLoop(gameTime);

            if (data.pauseKey.keyDown)
            {
                pause();
            }

            if (data.restartKey.keyDown)
            {
                manager.main = new encounter(manager.bag);
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

        protected void pausedUpdate(GameTime gameTime)
        {
            if (data.pauseKey.keyDown)
            {
                unpause();
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
