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
            main.state = this;
            update = tick;
        }

        protected void tick(GameTime gameTime)
        {
            if (piecefallTimer >= 600)
            {

                if (main.currentPiece.collidesFalling(0, 1))
                {
                    if (placeTimer >= 1000)
                    {

                        main.currentPiece.place();

                        pieceScoreState newState = new pieceScoreState(main);
                        newState.startState();

                        return;
                    }
                }
                else
                {
                    main.currentPiece.move(0, 1);
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

        public void parseTurnInput(GameTime gameTime)
        {
            //for movement keys, when key holds: do action once, wait until timeheld, then move often
            if ((data.leftKey.keyDown || data.leftKey.timeHeld > 130) && leftMoveCooldown > 30)
            {
                if (!main.currentPiece.collidesFalling(-1, 0))
                {
                    main.currentPiece.move(-1, 0);
                    leftMoveCooldown = 0;
                }
            }
            else if ((data.rightKey.keyDown || data.rightKey.timeHeld > 130) && rightMoveCooldown > 30)
            {
                if (!main.currentPiece.collidesFalling(1, 0))
                {
                    main.currentPiece.move(1, 0);
                    rightMoveCooldown = 0;
                }
            }

            if (data.downKey.keyDown || data.downKey.timeHeld > 40 && fastfallCooldown > 10)
            {
                piecefallTimer += 600;
                fastfallCooldown = 0;
            }
            else if (data.downKey.keyUp)
            {
                piecefallTimer = 0;
                placeTimer = 0;
            }


            if (data.upKey.keyHeld)
            {
                //piecefallTimer -= gameTime.ElapsedGameTime.TotalMilliseconds * 0.2;
            }

            if (data.leftRotateKey.keyDown)
            {
                if (main.currentPiece.canRotate(-1))
                {
                    main.currentPiece.rotate(-1);
                }
            }
            else if (data.rightRotateKey.keyDown)
            {
                if (main.currentPiece.canRotate(1))
                {
                    main.currentPiece.rotate(1);
                }
            }

            if (data.slamKey.keyDown)
            {
                main.currentPiece.move(0, main.currentPiece.dropOffset);
                main.currentPiece.place();
                //SKIP AHEAD TO SCORE STATE
            }

            if (data.holdKey.keyDown && canHold)
            {
                holdPiece();
            }

            leftMoveCooldown += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            rightMoveCooldown += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            fastfallCooldown += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void holdPiece()
        {
            if (main.heldPiece == null)
            {
                main.heldPiece = main.nextPiece; //put this all in one method later, main.drawPiece(); or something, updates all next boxes
                main.nextPiece = main.bag.getPiece(main.board);
                main.board.nextbox.update(main.nextPiece);
            }

            boardPiece formerlyHeld = main.heldPiece;

            main.heldPiece = main.currentPiece;
            while (main.heldPiece.rotation != 0)
            {
                main.heldPiece.rotate(1);
            }

            main.board.holdbox.update(main.heldPiece);

            main.currentPiece.removeFalling();
            main.currentPiece = formerlyHeld;
            main.currentPiece.play();
            canHold = false;
        }


    }

}