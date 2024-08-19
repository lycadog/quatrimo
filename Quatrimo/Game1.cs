using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Quatrimo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        main main;

        Texture2D corey;
        Texture2D bgTop;
        Texture2D bgBot;

        public static readonly Vector2I baseRes = new Vector2I(352, 198);
        public Game1()
        {
            Window.AllowUserResizing = true;
            
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 352;
            graphics.PreferredBackBufferHeight = 198;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            main = new main( new bag(data.freakyBag));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            corey = Content.Load<Texture2D>("block/heavy_full");
            bgTop = Content.Load<Texture2D>("bgTop");   
            bgBot = Content.Load<Texture2D>("bgBottom");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            main.coreGameLoop(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            DisplayMode display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(bgTop, new Rectangle(0, 0, 352, 198), new Color(new Vector3(0.0f, 0.08f, 0.1f)));
            spriteBatch.Draw(bgBot, new Rectangle(0, 0, 352, 198), new Color(new Vector3(0.08f, 0.12f, 0.2f)));


            spriteBatch.Draw(corey, new Rectangle(100,100,100,100), Color.Coral);

            spriteBatch.End();
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
