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
        public RenderTarget2D sceneTarget;
        public RenderTarget2D textTarget;

        stateManager stateManager;

        static float scale = 2;
        public static readonly Vector2I baseRes = new Vector2I(480, 270);
        public static readonly Vector2I textRes = new Vector2I(960, 540);
        public static Vector2I res = new Vector2I(960, 540);
        public static int frameOffset = 0;

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

        public static Texture2D bg;

        public static SpriteFont fontSmall;
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

            fontSmall = Content.Load<SpriteFont>("fonts/ToshibaSatSmall");
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

            bg = Content.Load<Texture2D>("misc/bg");

            nameQ = atlas[24];
            nameU = atlas[25];
            nameA = atlas[26];
            nameT = atlas[27];
            nameR = atlas[28];
            nameI = atlas[29];
            nameM = atlas[30];
            nameO = atlas[31];

            data.dataInitContent();
            stateManager = new stateManager(this);
            stateManager.startEncounter();
            regionSprite sprite = new regionSprite();
            sprite.tex = nameQ;
            sprite.color = Color.Magenta;
            sprite.pos = new Vector2I(240, 240);
            sprite.depth = 1f;  

            stateManager.encounter.board.sprites.Add(new movingSprite(sprite, new Vector2(0, -200f), new Vector2(0, 100f)));
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
