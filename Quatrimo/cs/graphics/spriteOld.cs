﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class spriteOld : drawableOld
    {
        public Vector2I size = new Vector2I(10, 10);
        public float depth = 0;
        public float rot = 0;
        public Vector2 origin = new Vector2(5, 5);

        public Texture2D tex = content.none;
        public Color color = Color.White;
        public SpriteEffects effect = SpriteEffects.None;

        static readonly int eSize = 10; //global element size

        public spriteOld(Texture2D tex, Color color, float depth = 0, SpriteEffects effect = SpriteEffects.None)
        {
            this.tex = tex;
            this.color = color;
            this.depth = depth;
            this.effect = effect;
        }

        public spriteOld(Texture2D tex, Color color, Vector2I boardpos, float depth = 0, SpriteEffects effect = SpriteEffects.None)
        {
            this.tex = tex;
            this.color = color;
            boardPos = boardpos;
            this.depth = depth;
            this.effect = effect;
        }

        public spriteOld(Texture2D tex, Vector2I elementPos, Color color, float depth = 0, SpriteEffects effect = SpriteEffects.None)
        {
            this.tex = tex;
            this.color = color;
            this.elementPos = elementPos;
            this.depth = depth;
            this.effect = effect;
        }

        public spriteOld() { setState(0); }


        protected Vector2I _worldPos = Vector2I.zero;
        public Vector2I worldPos { get { return _worldPos; } set { _worldPos = value; updatePos(); } }

        protected Vector2I _elementPos;
        public Vector2I elementPos { get { return _elementPos; } set
            {
                _elementPos = value;
                _worldPos = new Vector2I(elementPos.x * eSize + 5, elementPos.y * eSize + 5);
                _boardpos = new Vector2I(elementPos.x + board.offset.x + 1, elementPos.y + 4);
                _floatPos = worldPos;
            } }

        protected Vector2I _boardpos;
        public Vector2I boardPos { get { return _boardpos; } set
            {
                _boardpos = value;
                _worldPos = new Vector2I((board.offset.x + boardPos.x + 1) * eSize + 5, (boardPos.y - 4) * eSize + 5);
                _elementPos = new Vector2I(board.offset.x + boardPos.x + 1, boardPos.y - 4);
                _floatPos = worldPos;
            } }

        protected Vector2 _floatPos;
        public Vector2 floatPos { get { return _floatPos; } set
            {
                _floatPos = value;
                _worldPos = (Vector2I)floatPos;
                updatePos();
            } }

        /// <summary>
        /// Draws the sprite to the provided spritebatch
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw to</param>
        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(tex, new Rectangle(worldPos.x, worldPos.y, size.x, size.y), null, color, rot, origin, effect, depth);
        }

        void updatePos()
        {
            _elementPos = new Vector2I((int)(worldPos.x / (float)10) - 5, (int)(worldPos.y / (float)10) - 5);
            _boardpos = new Vector2I(_elementPos.x + board.offset.x + 1, _elementPos.y + 4);
            //TEST BOARDPOS CONVERSION LATER - boardpos2elementpos is Vector2I(board.offset.x + bpos.x + 1, bpos.y - 4);
            //the -4 does not make sense. the +4 here is an inversion of that and still makes no sense. double check this later !!!!
            //think i fixed it
        }
    }
}
