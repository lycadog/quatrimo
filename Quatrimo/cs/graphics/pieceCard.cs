using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Quatrimo
{
    public class pieceCard : sprite
    {
        public boardPiece piece;

        //base sprite is the bg
        sprite cardBorder;

        regSprite pieceTypeBox; //create method on piece to provide pieceType texture
        regSprite abilityBox;   //another method on piece for its ability (default to none)
        //also new method on piece for when the abilitybox needs to be updated? or integrate this to the ability method in some way
        regSprite keybindNumber; //calculate when hand is moved around

        int index;

        static readonly Texture2DRegion[] numbers = [content.number1, content.number2, content.number3, content.number4, content.number5, content.number6, content.number7, content.number8, content.number9, content.number0];

        //get pos for the different sprites
        //figure out how to determine the position of the base card
        //should be calculated based on index? offset per index decreased if hand is too big, also calculate and update depth for all elements
        //so the one hovered is on top, otherwise the first card will be shown over the next and so on

        public pieceCard(boardPiece piece) : base(new Vector2I(10, 3), content.pieceCardBG, Color.White, .911f)
        {
            cardBorder = new(content.pieceCardOutline, piece.color, 0.915f);
        }

        public void play(encounter encounter)
        {
            encounter.currentPiece = piece;
            piece.play();
        }

        public void update(int index, int handSize)
        {
            this.index = index;
        }

        public void hover()
        {
            //display a preview of where the piece will be placed
        }
    }
}