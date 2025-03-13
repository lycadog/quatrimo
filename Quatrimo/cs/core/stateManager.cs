
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

namespace Quatrimo
{
    public class stateManager
    {
        Game1 game;

        public List<gameState> state = [];
        public Stack<List<gameState>> stateStack = new();

        public Action<GameTime> keyUpdate;
        public Action<GameTime> mouseUpdated;
        public Action<GameTime> updateD;
        public Action<GraphicsDeviceManager, SpriteBatch, GameTime> drawBaseRes;
        public Action<GraphicsDeviceManager, SpriteBatch, GameTime> draw2xRes;
        public Action<GraphicsDeviceManager, SpriteBatch, GameTime> drawRaw;

        encounter encounter;
        runData runData;
        bool paused = false;
        bool debugMode = false;

        public static drawObject baseParent = new drawObject(baseParent);
        public static drawObject hiddenParent = new drawObject(hiddenParent);

        static Vector2I staleMousePos;

        public stateManager(Game1 game)
        {
            this.game = game;
        }

        public void update(GameTime gameTime)
        {
            Vector2I mousepos = new Vector2I(Mouse.GetState().X, Mouse.GetState().Y);

            if (mousepos != staleMousePos)
            {
                mouseUpdated?.Invoke(gameTime);
            }
            staleMousePos = mousepos;

            keyUpdate(gameTime);
            updateD(gameTime);
        }
        
        public void draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (paused) { gameTime.ElapsedGameTime = new System.TimeSpan(0); }

            game.GraphicsDevice.SetRenderTarget(game.sceneTarget);
            game.GraphicsDevice.Clear(new Color(new Vector3(0.0f, 0.02f, 0.06f)));

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            drawBaseRes.Invoke(graphics, spriteBatch, gameTime); // ==== ==== RENDER SCENE ==== ==== //////////

            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(game.textTarget);
            game.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            draw2xRes.Invoke(graphics, spriteBatch, gameTime); // ==== ==== RENDER TEXT ==== ==== //////////

            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // ==== DRAW BOTH RENDERTARGETS
            spriteBatch.Draw(game.sceneTarget, new Rectangle(Game1.frameOffset, 0, Game1.res.x, Game1.res.y), Color.White);
            spriteBatch.Draw(game.textTarget, new Rectangle(Game1.frameOffset, 0, Game1.res.x, Game1.res.y), Color.White);

            drawRaw.Invoke(graphics, spriteBatch, gameTime);
            spriteBatch.End();
        }

        public void removeState<T>() where T : gameState
        {
            foreach (var state in state)
            {
                if (state is T)
                {
                    state.removeState();
                    break;
                }
            }
        }

        public void startEncounter()
        {
            bag bag = data.bag1.createBag(); //fill out later
            encounter = new encounter(bag);
            encounterState newState = new encounterState(this);

            newState.addState();

            debugMode = true;
            new debugState(this).addState();
        }
        

    }
}
