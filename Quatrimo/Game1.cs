using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.ViewportAdapters;
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
        private RenderTarget2D sceneTarget;
        private RenderTarget2D textTarget;

        main main;

        static float scale = 2;
        static float tScale = 1;
        static readonly Vector2I baseRes = new Vector2I(450, 253);
        static readonly Vector2I textRes = new Vector2I(720, 404);
        static Vector2I res = new Vector2I(720, 404);
        static int frameOffset = 0;

        public static bool isPaused;

        public static Texture2DAtlas atlas;

        public static Texture2DRegion empty;
        public static Texture2DRegion full;
        public static Texture2DRegion full75;
        public static Texture2DRegion full50;
        public static Texture2DRegion full25;
        public static Texture2DRegion dropmark;

        public static Texture2DRegion borderUL;
        public static Texture2DRegion borderUR;
        public static Texture2DRegion borderDL;
        public static Texture2DRegion borderDR;
        public static Texture2DRegion borderU;
        public static Texture2DRegion borderD;
        public static Texture2DRegion borderL;
        public static Texture2DRegion borderR;
        public static Texture2DRegion nameQ; public static Texture2DRegion nameU; public static Texture2DRegion nameA;
        public static Texture2DRegion nameT; public static Texture2DRegion nameR; public static Texture2DRegion nameI;
        public static Texture2DRegion nameM; public static Texture2DRegion nameO;

        public static Texture2DRegion box; public static Texture2DRegion boxdetail; public static Texture2DRegion boxdetail2;
        public static Texture2DRegion boxdetail3; public static Texture2DRegion boxsolid;
        public static Texture2DRegion round; public static Texture2DRegion rounddetail; public static Texture2DRegion rounddetail2;
        public static Texture2DRegion rounddetail3; public static Texture2DRegion roundsolid;
        public static Texture2DRegion circle; public static Texture2DRegion circledetail; public static Texture2DRegion circledetail2;
        public static Texture2DRegion circledetail3; public static Texture2DRegion circlesolid;

        public static Texture2DRegion block;
        public static Texture2DRegion block_full;
        public static Texture2DRegion block_fuller;
        public static Texture2DRegion heavy;
        public static Texture2DRegion heavy_full;
        public static Texture2DRegion heavy_fuller;
        public static Texture2DRegion alloy1;
        public static Texture2DRegion alloy2;

        public static Texture2D none;
        public static Texture2D solid;
        public static Texture2D nextBox;
        public static Texture2D holdBox;
        public static Texture2D pausedtext;
        public static Texture2D corey;

        Texture2D bg;

        public static SpriteFont font;

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
            data.dataInit();
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

            font = Content.Load<SpriteFont>("fonts/ToshibaSat");

            Texture2D atlasTex = Content.Load<Texture2D>("png/atlas");
            atlas = Texture2DAtlas.Create("atlas", atlasTex, 10, 10);


            empty = atlas[0];
            full = atlas[12];
            full75 = atlas[13];
            full50 = atlas[14];
            full25 = atlas[15];
            dropmark = atlas[9];

            borderUL = atlas[1];
            borderUR = atlas[3];
            borderDL = atlas[6];
            borderDR = atlas[8];
            borderU = atlas[2];
            borderD = atlas[7];
            borderL = atlas[4];
            borderR = atlas[5];

            box = atlas[36];
            boxdetail = atlas[37];
            boxdetail2 = atlas[38];
            boxdetail3 = atlas[39];
            boxsolid = atlas[40];
            round = atlas[48];
            rounddetail = atlas[49];
            rounddetail2 = atlas[50];
            rounddetail3 = atlas[51];
            roundsolid = atlas[52];
            circle = atlas[60];
            circledetail = atlas[61];
            circledetail2 = atlas[62];
            circledetail3 = atlas[63];
            circlesolid = atlas[64];

            none = Content.Load<Texture2D>("empty");
            solid = Content.Load<Texture2D>("full");

            corey = Content.Load<Texture2D>("png/Corey");
            nextBox = Content.Load<Texture2D>("ui/nextbox");
            holdBox = Content.Load<Texture2D>("ui/holdbox");
            pausedtext = Content.Load<Texture2D>("ui/paused");

            bg = Content.Load<Texture2D>("png/bgTop");

            nameQ = atlas[24];
            nameU = atlas[25];
            nameA = atlas[26];
            nameT = atlas[27];
            nameR = atlas[28];
            nameI = atlas[29];
            nameM = atlas[30];
            nameO = atlas[31];

            data.dataInitContent();
            main = new main(new bag(data.bag2));
            animFrame frame1 = new animFrame(new element(full, Color.White, new Vector2I(10, 5)), 200);
            animFrame frame2 = new animFrame(new element(full75, Color.White, new Vector2I(10, 5)), 200);
            animFrame frame3 = new animFrame(new element(full50, Color.White, new Vector2I(10, 5)), 200);
            animFrame frame4 = new animFrame(new element(full25, Color.White, new Vector2I(10, 5)), 200);


            animSprite anim = new animSprite([frame1, frame2, frame3, frame4], true);
            main.board.sprites.Add(anim);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            keybind.updateKeybinds(data.keys, gameTime);

            if (data.pauseKey.keyDown)
            {
                if (isPaused) { isPaused = false; main.board.sprites.Remove(main.board.pauseText); }
                else { isPaused = true; main.board.sprites.Add(main.board.pauseText); }
            }
            if (isPaused) { gameTime.ElapsedGameTime = new TimeSpan(0); }
            else { main.coreGameLoop(gameTime); }
            

            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }
        //GraphicsDevice.Clear(new Color(new Vector3(0.0f, 0.02f, 0.06f)));

        protected override void Draw(GameTime gameTime)
        {
            if (isPaused) { gameTime.ElapsedGameTime = new TimeSpan(0); }

            DisplayMode display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            GraphicsDevice.SetRenderTarget(sceneTarget);
            GraphicsDevice.Clear(new Color(new Vector3(0.0f, 0.02f, 0.06f)));

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
                
            spriteBatch.Draw(bg, new Rectangle(0, 0, baseRes.x, baseRes.y), null, new Color(new Vector3(0.01f, 0.00f, 0.02f)), 0, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.Draw(bg, new Rectangle(0, 0, baseRes.x, baseRes.y), null, new Color(new Vector3(0.02f, 0.01f, 0.12f)), 0, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
            //spriteBatch.Draw(bg, new Rectangle(0, 0, baseRes.x, baseRes.y), null, new Color(new Vector3(0.02f, 1.0f, 0.12f)), 0, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

            main.board.draw(spriteBatch, gameTime);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            
            GraphicsDevice.SetRenderTarget(textTarget);
            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.DrawString(font, "SCORE: " + main.totalScore.ToString(), new Vector2(480, 300), Color.White);
            spriteBatch.DrawString(font, "LVL: " + main.level.ToString(), new Vector2(480, 320), Color.White);
            spriteBatch.DrawString(font, "X: " + main.levelTimes.ToString(), new Vector2(480, 340), Color.White);
            spriteBatch.DrawString(font, "LVL UP IN: " + (main.rowsRequired - main.rowsCleared).ToString() + " ROWS", new Vector2(480, 360), Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(sceneTarget, new Rectangle(frameOffset, 0, res.x, res.y), Color.White);
            spriteBatch.Draw(textTarget, new Rectangle(frameOffset, 0, res.x, res.y), Color.White);
            spriteBatch.End();
            

            // TODO: Add your drawing code here

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
            tScale = scale / 2;
            
            Debug.WriteLine(scale + ",  OFFSET:" + frameOffset);

            //need code to get the resolution scaling
            //recalculate scaling
            //graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            //graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            //graphics.ApplyChanges();
            Debug.WriteLine("IM GAY");
        }

    }


}
