
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace Quatrimo
{
    /*
    public class gameManager
    {
        public Game1 game;
        public main main;

        public bag bag;
        public bool paused = false;
        public bool debugMode = false;


        protected delegate void gameDelegate(GameTime gameTime);
        protected delegate void drawDelegate(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime);

        //protected updateState<gameDelegate> keyDel = new updateState<gameDelegate>(); //keys to update every update call
        //protected updateState<gameDelegate> updateDel = new updateState<gameDelegate>(); //update method to call
        //protected updateState<drawDelegate> sceneDrawDel = new updateState<drawDelegate>(); //scene details to draw using sceneTarget render target
        //protected updateState<drawDelegate> textDrawDel = new updateState<drawDelegate>(); //text to draw using textTarget render target

        public gameManager(Game1 game)
        {
            this.game = game;
        }

        public void update(GameTime gameTime)
        {
            updateDel.state.Invoke(gameTime);
        }

        public void updateKeys(GameTime gameTime)
        {
            keyDel.state.Invoke(gameTime);
        }

        public void draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (paused) { gameTime.ElapsedGameTime = new System.TimeSpan(0); }

            game.GraphicsDevice.SetRenderTarget(game.sceneTarget);
            game.GraphicsDevice.Clear(new Color(new Vector3(0.0f, 0.02f, 0.06f)));

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            sceneDrawDel.state.Invoke(graphics, spriteBatch, gameTime); // ==== ==== RENDER SCENE ==== ==== //////////

            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(game.textTarget);
            game.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            textDrawDel.state.Invoke(graphics, spriteBatch, gameTime); // ==== ==== RENDER TEXT ==== ==== //////////

            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // ==== DRAW BOTH RENDERTARGETS
            spriteBatch.Draw(game.sceneTarget, new Rectangle(Game1.frameOffset, 0, Game1.res.x, Game1.res.y), Color.White);
            spriteBatch.Draw(game.textTarget, new Rectangle(Game1.frameOffset, 0, Game1.res.x, Game1.res.y), Color.White);
            spriteBatch.End();
        }

        

        public void startEncounter()
        {
            keyDel.state = updateBoardKeys;
            updateDel.state = encounterUpdate;
            sceneDrawDel.state = drawBoardScene;
            textDrawDel.state = drawBoardText;

            bag = new bag(data.bag2);
            main = new main(bag);
        }

        // ================== DELEGATES BELOW ==================
        protected void encounterUpdate(GameTime gameTime)
        {
            main.coreGameLoop(gameTime);

            if (data.pauseKey.keyDown)
            {
                paused = true;
                main.board.sprites.Add(main.board.pauseText);
                updateDel.setTemporaryState(encounterPaused);
            }

            if (data.restartKey.keyDown)
            {
                main = new main(bag);
            }

            if(data.toggleDebugKey.keyDown)
            {
                if (!debugMode) 
                { debugMode = true; updateDel.state += debugUpdate; textDrawDel.state += drawDebugText; keyDel.state += updateDebugKeys; }
                else { debugMode = false; updateDel.state -= debugUpdate; textDrawDel.state -= drawDebugText; keyDel.state -= updateDebugKeys; }
            }
        }

        protected void encounterPaused(GameTime gameTime)
        {
            if (data.pauseKey.keyDown)
            {
                paused = false;
                main.board.staleSprites.Add(main.board.pauseText);
                updateDel.endState();
            }
        }

        protected void debugUpdate(GameTime gameTime)
        {
            if (data.debugMode1.keyDown)
            {
                updateDel.setTemporaryState(debugSlowModeUpdate);
                keyDel.setTemporaryState(updateDebugKeys);
            }
        }

        protected void debugSlowModeUpdate(GameTime gameTime)
        {
            paused = true;
            if (data.debugMode1.keyDown)
            {
                updateDel.endState();
                keyDel.endState();
                paused = false;
                return;
            }

            if (data.debugKey1.keyDown)
            {
                paused = false;
                gameTime.ElapsedGameTime = new System.TimeSpan(0, 0, 0, 0, 20);
                keybind.updateKeybinds(data.boardKeys, gameTime);
                encounterUpdate(gameTime);
            }

            if (data.debugKey2.keyDown)
            {
                paused = false;
                gameTime.ElapsedGameTime = new System.TimeSpan(0, 0, 0, 0, 999);
                keybind.updateKeybinds(data.boardKeys, gameTime);
                encounterUpdate(gameTime);
            }
        }

        protected void updateBoardKeys(GameTime gameTime)
        {
            keybind.updateKeybinds(data.boardKeys, gameTime);
        }

        protected void updateDebugKeys(GameTime gameTime)
        {
            keybind.updateKeybinds(data.debugKeys, gameTime);
        }

        protected void drawBoardScene(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Game1.bg, new Rectangle(0, 0, Game1.baseRes.x, Game1.baseRes.y), null, new Color(new Vector3(0.02f, 0.0f, 0.01f)), 0, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(Game1.bg, new Rectangle(0, 0, Game1.baseRes.x, Game1.baseRes.y), null, new Color(new Vector3(0.01f, 0.00f, 0.16f)), 0, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

            main.board.draw(spriteBatch, gameTime);
        }

        protected void drawBoardText(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(Game1.fontSmall, "SCORE: " + main.totalScore.ToString(), new Vector2(680, 300), Color.White);
            spriteBatch.DrawString(Game1.fontSmall, "LVL: " + main.level.ToString(), new Vector2(680, 320), Color.White);
            spriteBatch.DrawString(Game1.fontSmall, "X: " + main.levelTimes.ToString(), new Vector2(680, 340), Color.White);
            spriteBatch.DrawString(Game1.fontSmall, "LVL UP IN: " + (main.rowsRequired - main.rowsCleared).ToString() + " ROWS", new Vector2(680, 360), Color.White);
        }
        
        protected void drawDebugText(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(Game1.font, "DEBUG", new Vector2(450, 20), Color.Yellow);

            spriteBatch.DrawString(Game1.fontSmall, $"gamestate: {main.state}", new Vector2(20, 460), Color.CornflowerBlue);
            spriteBatch.DrawString(Game1.fontSmall, $"piecefallTimer {main.piecefallTimer}/600", new Vector2(20,480), Color.LightBlue);
            spriteBatch.DrawString(Game1.fontSmall, $"placeTimer {main.placeTimer}/1000", new Vector2(20, 500), Color.LightBlue);
            spriteBatch.DrawString(Game1.fontSmall, $"pieceWaitTimer {main.pieceWaitTimer}/5000", new Vector2(20, 520), Color.LightBlue);
        }

    }*/
}
