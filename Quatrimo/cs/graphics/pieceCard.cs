using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class pieceCard : sprite
    {   //add code for moving and initializing all the sprites properly
        public boardPiece piece;

        sprite cardBorder;

        regSprite pieceTypeBox;
        regSprite abilityBox;
        regSprite keybindNumber;

        static readonly Texture2DRegion[] numbers = [content.number1, content.number2, content.number3, content.number4, content.number5, content.number6, content.number7, content.number8, content.number9, content.number0];

        static readonly Vector2I originPos = new Vector2I(100, 30); //topleft pixel of hand box
        static int yOffset; //how much to offset every card to center them

        static readonly int indexYOffset = 40; //how much to offset the y position based on index
        int index;

        public void play(encounter encounter)
        {
            encounter.currentPiece = piece;
            piece.play();
        }

        public void update(int index)
        {
            this.index = index;
            updateSpritePos();
        }

        public void hover()
        {
            //display a preview of where the piece will be placed
        }

        void updateSpritePos()
        {
            worldPos = new Vector2I(originPos.x, originPos.y + indexYOffset * index + yOffset);
            cardBorder.worldPos = worldPos;

            pieceTypeBox.worldPos = worldPos + new Vector2I(48, 4);
            abilityBox.worldPos = worldPos + new Vector2I(58, 4);

            keybindNumber.worldPos = worldPos + new Vector2I(52, 13);
            if(index < 10)
            {
                keybindNumber.tex = numbers[index];
            }
            else { keybindNumber.tex = content.empty; }

            //add support for pieceTags later
        }

        //draw every sprite
        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime, ref Action<List<drawable>> list)
        {
            base.drawState(spriteBatch, gameTime, ref list);
        }
    }
}