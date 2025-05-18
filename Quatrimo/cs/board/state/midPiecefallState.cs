using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class midPiecefallState : boardState
    {
        double piecefallTimer = -600; //set timers to negative to give more reaction time when a piece is first placed
        double placeTimer = -600;

        int leftMoveCooldown = 0;
        int rightMoveCooldown = 0;
        int fastfallCooldown = 0;

        bool canHold = true;

        public midPiecefallState(encounter main) : base(main)
        {
        }

        public override void startState()
        {
            encounter.state = this;
            update = tick;
        }

        protected void tick(GameTime gameTime)
        {
            if (piecefallTimer >= 600)
            {

                if (encounter.fallingPiece.collidesFalling(0, 1))
                {
                    if (placeTimer >= 1000)
                    {
                        placePiece();
                        return;
                    }
                }
                else
                {
                    encounter.fallingPiece.move(0, 1);
                    piecefallTimer = 0;
                    placeTimer = 0;
                    return;
                }
            }

            parseTurnInput(gameTime);

            //increment timers
            piecefallTimer += gameTime.ElapsedGameTime.Milliseconds;
            placeTimer += gameTime.ElapsedGameTime.Milliseconds;
        }

        void placePiece()
        {
            encounter.fallingPiece.place();
            encounter.updatedRows = [true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true];
            processBoardUpdatesState newState = new processBoardUpdatesState(encounter, encounter.currentPiece);
            newState.startState();
        }

        public void parseTurnInput(GameTime gameTime)
        {
            if (keybind.slamKey.keyDown)
            {
                encounter.fallingPiece.move(0, encounter.currentPiece.dropOffset);
                placePiece();
                return;
            }

            if (keybind.holdKey.keyDown && canHold)
            {
                holdPiece();
                return;
            }

            if (keybind.pieceAbilityKey.keyDown)
            {
                encounter.fallingPiece.useAbility();
                return;
            }

            //for movement keys, when key holds: do action once, wait until timeheld, then move rapidly
            if ((keybind.leftKey.keyDown || keybind.leftKey.timeHeld > 140) && leftMoveCooldown > 30)
            {
                if (!encounter.fallingPiece.collidesFalling(-1, 0))
                {
                    encounter.fallingPiece.move(-1, 0);
                    leftMoveCooldown = 0;
                }
            }
            else if ((keybind.rightKey.keyDown || keybind.rightKey.timeHeld > 140) && rightMoveCooldown > 30)
            {
                if (!encounter.fallingPiece.collidesFalling(1, 0))
                {
                    encounter.fallingPiece.move(1, 0);
                    rightMoveCooldown = 0;
                }
            }

            if (keybind.downKey.keyDown || keybind.downKey.timeHeld > 50 && fastfallCooldown > 10)
            {
                piecefallTimer += 600;
                fastfallCooldown = 0;
            }
            else if (keybind.downKey.keyUp)
            {
                piecefallTimer = -100;
                placeTimer = -100;
            }


            if (keybind.upKey.keyHeld)
            {
                //piecefallTimer -= gameTime.ElapsedGameTime.TotalMilliseconds * 0.2;
            }

            if (keybind.leftRotateKey.keyDown)
            {
                if (encounter.fallingPiece.canRotate(-1))
                {
                    encounter.fallingPiece.rotate(-1);
                }
            }
            else if (keybind.rightRotateKey.keyDown)
            {
                if (encounter.fallingPiece.canRotate(1))
                {
                    encounter.fallingPiece.rotate(1);
                }
            }

            leftMoveCooldown += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            rightMoveCooldown += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            fastfallCooldown += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void holdPiece()
        {
            if(encounter.bag.hold == null)
            {
                //wtf
            }


            if (encounter.bag.hold == null)
            {
                
                encounter.heldPiece = encounter.nextPiece; //put this all in one method later, main.drawPiece(); or something, updates all next boxes
                encounter.nextPiece = encounter.bag.drawRandomPiece();
                encounter.board.nextbox.update(encounter.nextPiece);
            }

            boardPiece formerlyHeld = encounter.heldPiece;

            encounter.heldPiece = encounter.currentPiece;
            while (encounter.heldPiece.rotation != 0)
            {
                encounter.heldPiece.rotate(1);
            }

            encounter.board.holdbox.update(encounter.heldPiece);

            encounter.fallingPiece.removeFalling();
            encounter.currentPiece = formerlyHeld;
            encounter.currentPiece.play();
            canHold = false;
        }


    }

}