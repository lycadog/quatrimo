using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Quatrimo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        main main;

        public static float rScale = 1;
        public static int resX = 352;
        public static int resY = 198;
        public static int xOffset = 0;

        public static Texture2D empty;
        public static Texture2D full;
        public static Texture2D full75;
        public static Texture2D full50;
        public static Texture2D full25;

        public static Texture2D borderUL;
        public static Texture2D borderUR;
        public static Texture2D borderDL;
        public static Texture2D borderDR;
        public static Texture2D borderU;
        public static Texture2D borderD;
        public static Texture2D borderL;
        public static Texture2D borderR;
        public static Texture2D nameQ;
        public static Texture2D nameU;
        public static Texture2D nameA;
        public static Texture2D nameT;
        public static Texture2D nameR;
        public static Texture2D nameI;
        public static Texture2D nameM;
        public static Texture2D nameO;


        public static Texture2D box;
        public static Texture2D box_full;
        public static Texture2D box_solid;
        public static Texture2D circle;
        public static Texture2D circle_full;
        public static Texture2D circle_solid;
        public static Texture2D round;
        public static Texture2D round_full;
        public static Texture2D round_solid;
        public static Texture2D heavy;
        public static Texture2D heavy_full;
        public static Texture2D heavy_fuller;


        public static Texture2D corey;

        Texture2D bgTop;
        Texture2D bgBot;

        public static readonly Vector2I baseRes = new Vector2I(352, 198);
        public Game1()
        {
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 352;
            graphics.PreferredBackBufferHeight = 198;
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            data.dataInit();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            empty = Content.Load<Texture2D>("empty");
            full = Content.Load<Texture2D>("full");
            full75 = Content.Load<Texture2D>("75");
            full50 = Content.Load<Texture2D>("50");
            full25 = Content.Load<Texture2D>("25");

            borderUL = Content.Load<Texture2D>("png/borderUL");
            borderUR = Content.Load<Texture2D>("png/borderUR");
            borderDL = Content.Load<Texture2D>("png/borderDL");
            borderDR = Content.Load<Texture2D>("png/borderDR");
            borderU = Content.Load<Texture2D>("png/borderU");
            borderD = Content.Load<Texture2D>("png/borderD");
            borderL = Content.Load<Texture2D>("png/borderL");
            borderR = Content.Load<Texture2D>("png/borderR");

            box = Content.Load<Texture2D>("block/box");
            box_full = Content.Load<Texture2D>("block/box_full");
            box_full = Content.Load<Texture2D>("block/box_solid");
            circle = Content.Load<Texture2D>("block/circle");
            circle_full = Content.Load<Texture2D>("block/circle_full");
            circle_solid = Content.Load<Texture2D>("block/circle_solid");
            round = Content.Load<Texture2D>("block/round");
            round_full = Content.Load<Texture2D>("block/round_full");
            round_solid = Content.Load<Texture2D>("block/round_solid");
            heavy = Content.Load<Texture2D>("block/heavy");
            heavy_full = Content.Load<Texture2D>("block/heavy_full");
            heavy_fuller = Content.Load<Texture2D>("block/heavy_fuller");

            corey = Content.Load<Texture2D>("png/Corey");

            bgTop = Content.Load<Texture2D>("png/bgTop");
            bgBot = Content.Load<Texture2D>("png/bgBottom");

            nameQ = Content.Load<Texture2D>("misc/nameQ");
            nameU = Content.Load<Texture2D>("misc/nameU");
            nameA = Content.Load<Texture2D>("misc/nameA");
            nameT = Content.Load<Texture2D>("misc/nameT");
            nameR = Content.Load<Texture2D>("misc/nameR");
            nameI = Content.Load<Texture2D>("misc/nameI");
            nameM = Content.Load<Texture2D>("misc/nameM");
            nameO = Content.Load<Texture2D>("misc/nameO");

            data.dataInitContent();
            main = new main(new bag(data.bag1));
            main.board.initializeElements();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keybind.updateKeybinds(data.keys, gameTime);
            main.coreGameLoop(gameTime);

            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            DisplayMode display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            
            GraphicsDevice.Clear(new Color(new Vector3(0.0f, 0.02f, 0.06f)));
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(bgTop, new Rectangle(0 + xOffset, 0, res(352), res(198)), new Color(new Vector3(0.0f, 0.02f, 0.06f)));
            spriteBatch.Draw(bgBot, new Rectangle(0 + xOffset, 0, res(352), res(198)), new Color(new Vector3(0.02f, 0.04f, 0.14f)));
            
            main.board.drawElements(spriteBatch, gameTime);

            spriteBatch.End();
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public static int res(int num) //used to scale graphics by resolution
        {
            return (int)(num * rScale);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {

            //get the game frame which is a 16:9 square
            //get the distance from the edge to center the frame
            //set the frame height equal to the window height
            //calculate the frame width by using the height and applying 16:9 ratio

            //REWORK to add Integer Scaling to get rid of improper pixelling and gridlines
            resY = Window.ClientBounds.Height; //REWORK this later to use the lower dimension to calculate the greater dimension, to account for aspect ratios lower than 16:9
            resX = (int)Math.Round ( resY * 1.77777777778);
            xOffset = Math.Max((Window.ClientBounds.Width - resX)/2, 0);
            rScale = resY / (float)baseRes.y;


            Debug.WriteLine(rScale + ",  OFFSET:" + xOffset);

            //need code to get the resolution scaling
            //recalculate scaling
            //graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            //graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            //graphics.ApplyChanges();
            Debug.WriteLine("IM GAY");
        }

    }


}
