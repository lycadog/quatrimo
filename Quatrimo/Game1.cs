using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using System;
using System.Diagnostics;
using System.Threading;

namespace Quatrimo
{
    //我的鸡巴
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public RenderTarget2D sceneTarget;
        public RenderTarget2D textTarget;

        stateManager stateManager;

        static float scale = 2;
        public static readonly Vector2I baseRes = new Vector2I(480, 270);
        public static readonly Vector2I textRes = new Vector2I(960, 540);
        public static Vector2I res = new Vector2I(960, 540);
        public static int frameOffset = 0;

        


        public Game1()
        {
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = textRes.x;
            graphics.PreferredBackBufferHeight = textRes.y;
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            data.initialize();
            base.Initialize();

            sceneTarget = new RenderTarget2D(
                GraphicsDevice,
                baseRes.x, baseRes.y,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);
            textTarget = new RenderTarget2D(
                GraphicsDevice,
                textRes.x, textRes.y,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texs.loadContent(Content);
            

            data.contentInit();

            stateManager = new stateManager(this);
            stateManager.startEncounter();
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            stateManager.keyUpdate(gameTime);
            stateManager.update(gameTime);
            
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            DisplayMode display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            stateManager.draw(graphics, spriteBatch, gameTime);
            

            base.Draw(gameTime);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {

            //get the game frame which is a 16:9 square
            //get the distance from the edge to center the frame
            //set the frame height equal to the window height
            //calculate the frame width by using the height and applying 16:9 ratio

            //REWORK to add Integer Scaling to get rid of improper pixelling and gridlines
            res.y = Window.ClientBounds.Height; //REWORK this later to use the lower dimension to calculate the greater dimension, to account for aspect ratios lower than 16:9
            res.x = (int)Math.Round ( res.y * 1.77777777778);
            frameOffset = Math.Max((Window.ClientBounds.Width - res.x)/2, 0);
            scale = res.y / (float)baseRes.y;
            
            Debug.WriteLine("Resolution changed! SCALE: " + scale + ",  OFFSET:" + frameOffset);

            //need code to get the resolution scaling
            //recalculate scaling
            //graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            //graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            //graphics.ApplyChanges();
        }

    }


}
