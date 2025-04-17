using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class pieceCard : sprite
    {
        public boardPiece piece;

        //card position is calculated using update, this is called after the hand is updated so when a piece is placed
        //or on draw

        //DEPTH: base 0.911f
        //may need way to move a sprite over time with a set speed and duration

        //positions: pieceType box: 94, 6 || ability box: 114, 6 || tags 1, 2, and 4: [96, 50], [108, 50], [96, 62]
        //keybind pos: 102, 26

        //base sprite is the bg
        sprite cardBorder;

        drawObject pieceTypeBox; //create method on piece to provide pieceType texture
        drawObject abilityBox;   //another method on piece for its ability (default to none)
        //also new method on piece for when the abilitybox needs to be updated? or integrate this to the ability method in some way
        regSprite keybindNumber; //calculate when hand is moved around

        int index;

        static readonly Texture2DRegion[] numbers = [content.number1, content.number2, content.number3, content.number4, content.number5, content.number6, content.number7, content.number8, content.number9, content.number0];

        //get pos for the different sprites
        //figure out how to determine the position of the base card
        //should be calculated based on index? offset per index decreased if hand is too big, also calculate and update depth for all elements
        //so the one hovered is on top, otherwise the first card will be shown over the next and so on

        //afterwards: update the rendertarget to line up with our single 2x rendertarget plan
        public pieceCard(boardPiece piece) : base(new Vector2I(10, 3), content.pieceCardBG, Color.White, .911f)
        {
            cardBorder = new(content.pieceCardOutline, piece.color, 0.915f);
            drawObject[] sprites = piece.getGFX();
            for(int i = 0; i < sprites.Length; i++)
            {
                sprites[i].size = new Vector2I(10, 10);
                sprites[i].localPos = new Vector2I(20 + piece.blocks[i].localpos.x * 10, 20 + piece.blocks[i].localpos.x * 10);
                sprites[i].setParent(this);
            }
            pieceTypeBox = piece.getPieceIcon();
            { localPos = new Vector2I(94, 6); }
            abilityBox = piece.getAbilityIcon();
            { localPos = new Vector2I(114, 6); }

            keybindNumber = new(this, content.empty, Color.White, 0.912f);
        }

        public void play(encounter encounter)
        {
            encounter.currentPiece = piece;
            piece.play(this);
        }

        public void update(int index, int handSize)
        {
            this.index = index;
            if(index > 10)
            {
                keybindNumber.tex = content.empty;
                return;
            }
            keybindNumber.tex = numbers[index];
            //UPDATE POS here
        }

        public void hover()
        {
            //display a preview of where the piece will be placed
        }
    }
}