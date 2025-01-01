using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class movingParticle : drawable
    {
        public sprite sprite;
        public double timer = 0;
        public double lifetime = 0;
        bool timed = true;

        protected Vector2 floatpos;
        protected Vector2 velocity;
        protected Vector2 acceleration;

        public movingParticle(short state, sprite sprite, Vector2 floatpos, Vector2 velocity, Vector2 acceleration, double lifetime = 0, bool timed = false) : base(state)
        {
            this.sprite = sprite;
            this.floatpos = floatpos;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.lifetime = lifetime;
            this.timed = timed;
        }

        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime, Action<List<drawable>> listEditQueue)
        {
            throw new ArgumentException("movingParticle state does not accept basic drawstate 0, please set a different state.");
        }

        public override void setState(short state)
        {   //new states for different particle behavior
            switch (state)
            {
                case 1:
                    draw = noDraw;
                    stale = false;
                    break;
                case 2:
                    draw = staleState;
                    stale = true;
                    break;
                case 3:
                    draw = basicMovement;
                    break;
                case 4: draw = randomAcceleration;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("movingParticle state expects a number between 1 and 5. Number provided: " + state);
            }
        }

        protected void particleTick(SpriteBatch spriteBatch, GameTime gameTime, Action<List<drawable>> list)
        {
            if (timed && timer > lifetime)
            {
                setState(2);
                return;
            }
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            sprite.draw(spriteBatch, gameTime, list);
        }

        protected void updatePos()
        {
            sprite.worldPos = new Vector2I((int)floatpos.X, (int)floatpos.Y);
        }

        protected void basicMovement(SpriteBatch spriteBatch, GameTime gameTime, Action<List<drawable>> list)
        {
            particleTick(spriteBatch, gameTime, list);
            floatpos += new Vector2((float)(velocity.X * gameTime.ElapsedGameTime.TotalSeconds), (float)(velocity.Y * gameTime.ElapsedGameTime.TotalSeconds));
            velocity += new Vector2((float)(acceleration.X * gameTime.ElapsedGameTime.TotalSeconds), (float)(acceleration.Y * gameTime.ElapsedGameTime.TotalSeconds));
            updatePos();
        }
        
        protected void randomAcceleration(SpriteBatch spriteBatch, GameTime gameTime, Action<List<drawable>> list)
        {
            particleTick(spriteBatch, gameTime, list);
            floatpos += velocity;
            //acceleration += new Vector2(util.rand.NextSingle()-0.5f * 2, util.rand.NextSingle()-0.5f * 2);
            velocity += new Vector2((util.rand.NextSingle() - 0.5f) * 2, (util.rand.NextSingle() - 0.5f) * 2);
            updatePos();
        }
        
    }
}