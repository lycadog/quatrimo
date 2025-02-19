using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class pieceCard
    {   //need new system for creating the sprite display
        //change this off of a sprite to its own special class
        public boardPiece piece;

        spriteOld cardBG;
        spriteOld cardBorder;

        regSprite pieceTypeBox;
        regSprite abilityBox;
        regSprite keybindNumber;

        blockSprite[] blockDisplay; 

        static readonly Texture2DRegion[] numbers = [content.number1, content.number2, content.number3, content.number4, content.number5, content.number6, content.number7, content.number8, content.number9, content.number0];

        static readonly Vector2I originPos = new Vector2I(100, 30); //topleft pixel of hand box
        static int yOffset; //how much to offset every card to center them, calculate on hand update

        static readonly int indexYOffset = 40; //how much to offset the y position based on index
        int index;

        public pieceCard(boardPiece piece, int index, int handSize)
        {
            this.piece = piece;
            cardBG = new spriteOld(content.pieceCardBG, Color.White, .911f)
            {
                origin = Vector2I.zero,
                size = new Vector2I(70, 40)
            };

            cardBorder = new spriteOld(content.pieceCardOutline, piece.color, .915f)
            {
                origin = Vector2I.zero,
                size = new Vector2I(70, 40)
            };

            pieceTypeBox = new(content.dropCrosshair, .912f);
            abilityBox = new(content.dropCorners, .912f);
            keybindNumber = new(content.empty, .912f);

            blockDisplay = piece.getSprites();

            update(index, handSize);
        }

        public void play(encounter encounter)
        {
            encounter.currentPiece = piece;
            piece.play();
        }

        public void update(int index, int handSize)
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

            foreach(var bSprite in blockDisplay)
            {
                bSprite.sprite.worldPos = worldPos * 2;
                bSprite.sprite.worldPos = new Vector2I(
                    bSprite.sprite.worldPos.x + bSprite.block.localpos.x * 10,
                    bSprite.sprite.worldPos.y + bSprite.block.localpos.y * 10); //questionable but i think it works
            }

            //add support for pieceTags later
        }

        //draw every sprite
        protected override void drawState(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.drawState(spriteBatch, gameTime);
        }
    }
}