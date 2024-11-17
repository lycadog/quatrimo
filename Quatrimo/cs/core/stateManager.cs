
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Quatrimo
{
    public class stateManager
    {
        Game1 game;

        public List<gameState> state = [];
        public Stack<List<gameState>> stateStack = new();

        public encounter encounter;

        public bag bag;
        public bool paused = false;
        public bool debugMode = false;

        public delegate void gameDelegate(GameTime gameTime);
        public delegate void drawDelegate(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime);

        public gameDelegate keyUpdateD;
        public gameDelegate updateD;
        public drawDelegate drawScene;
        public drawDelegate drawText;

        public stateManager(Game1 game)
        {
            this.game = game;
            
        }

        public void removeState<T>() where T : gameState
        {
            foreach(var state in state)
            {
                if(state is T)
                {
                    state.removeState();
                    break;
                }
            }
        }

        public void keyUpdate(GameTime gameTime)
        {
            keyUpdateD.Invoke(gameTime);
        }

        public void update(GameTime gameTime)
        {
            updateD.Invoke(gameTime);
        }
        
        public void draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (paused) { gameTime.ElapsedGameTime = new System.TimeSpan(0); }

            game.GraphicsDevice.SetRenderTarget(game.sceneTarget);
            game.GraphicsDevice.Clear(new Color(new Vector3(0.0f, 0.02f, 0.06f)));

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            drawScene.Invoke(graphics, spriteBatch, gameTime); // ==== ==== RENDER SCENE ==== ==== //////////

            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(game.textTarget);
            game.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            drawText.Invoke(graphics, spriteBatch, gameTime); // ==== ==== RENDER TEXT ==== ==== //////////

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
            encounterState newState = new encounterState(this);
            newState.addState();

            bag = new bag(data.debugbag);
            encounter = new encounter(bag);
        }
        

    }
}
