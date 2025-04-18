﻿
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Quatrimo
{
    public class animHandler
    {
        //fix up this nightmare
        encounter encounter;
        public delegate void animDelegate(GameTime gameTime);

        public animDelegate animState;
        public List<drawableOld> animations = [];

        double timer = 0;
        short num = 0;
        byte counter1 = 0; byte counter2 = 0; byte counter3 = 0; byte counter4 = 0;
        Vector2I vector2 = Vector2I.zero;

        Vector3[] scorePositions;
        //stores 3 positions: the X value of the left most scored block, the X value of the right most scored block,
        //and the y value - this stores where the animation should start for each y value

        Vector2[] scoreCompletion;
        List<Vector2I> processedVectors = [];

        public animHandler(encounter main)
        {
            this.encounter = main;
            animState = none;
        }

        /// <summary>
        /// State before score animation processes to set up everything
        /// </summary>
        public void scoreAnimStart(GameTime gameTime)
        {
            /*timer = 0;
            
            List<block> scoredPieceBlocks = new List<block>();
            foreach(block block in main.currentPiece.blocks) //process through all the scored piece's blocks
            {
                if (main.scorableRows.Contains((byte)block.boardpos.y)) //save blocks that have scored
                {
                    scoredPieceBlocks.Add(block);
                }
                else
                {
                    element sprite1 = new element(Game1.boxsolid, Color.White, element.boardPos2ElementPos(block.boardpos), 0.85f);
                    animFrame frame1 = new animFrame(sprite1, 200);
                    animSprite anim = new animSprite([frame1]);

                    main.board.sprites.Add(anim);
                }

            }

            scoreCompletion = new Vector2[main.scorableRows.Count];
            //used in a later animation step


            scorePositions = new Vector3[main.scorableRows.Count];

            for (int v = 0; v < main.scorableRows.Count; v++)
            {
                scoreCompletion[v] = new Vector2(0, 0);

                scorePositions[v] = new Vector3(30, -30, 50); //these will be overwritten,
                //initialize them with these values so they will be overwritten by min/max operations
            }            

            for (int i = 0; i < main.scorableRows.Count; i++)
            {
                foreach(block block in scoredPieceBlocks)
                {
                    if(block.boardpos.y == main.scorableRows[i]) //if the block is in the current row we are checking
                    {
                        scorePositions[i] = new Vector3(Math.Min(scorePositions[i].X, block.boardpos.x), 
                            Math.Max(scorePositions[i].Y, block.boardpos.x), main.scorableRows[i]);
                    }

                    main.board.sprites.Add(getDecayingAnim(block.boardpos));
                    block.removeFromBoard(block);
                }
            }


            
            animState = score;
            
            */
        }


        /// <summary>
        /// Process through all scoring animations, spreading out from the scored piece
        /// </summary>
        /// <param name="gameTime"></param>
        public void score(GameTime gameTime)
        {
            /*
            if(timer >= 25)
            {

                for(int i = 0; i < scorePositions.Length; i++)
                {
                    if(scorePositions[i].X > 0)
                    {
                        scorePositions[i].X--;
                        Vector2I vector = new Vector2I((int)scorePositions[i].X, (int)scorePositions[i].Z);
                        if (!processedVectors.Contains(vector))
                        {
                            decayBlock(main.board.blocks[vector.x, vector.y]);
                            processedVectors.Add(vector);
                        }

                    }
                    else { scoreCompletion[i].X = 1; }

                    if (scorePositions[i].Y < main.board.dimensions.x - 1)
                    {
                        scorePositions[i].Y++;
                        Vector2I vector = new Vector2I((int)scorePositions[i].Y, (int)scorePositions[i].Z);
                        if (!processedVectors.Contains(vector))
                        {
                            decayBlock(main.board.blocks[vector.x, vector.y]);
                            processedVectors.Add(vector);
                        }
                    }else { scoreCompletion[i].Y = 1; }
                }

                //IF ALL OF SCORE COMPLETION IS THE VALUE 1, THEN PROGRESS TO SCOREWAITFORFINISH
                byte counter = 0; //this sucks find a better way later
                foreach (var var in scoreCompletion)
                {
                    if(var.X == 0 | var.Y == 0) { break; }
                    counter++;
                }
                if(counter == scoreCompletion.Length)
                {
                    animState = waitForAnimations;
                    processedVectors.Clear();
                }
                timer = 0;
            }

            timer += gameTime.ElapsedGameTime.TotalMilliseconds;*/
        }

        //IF ALL ANIMATIONS ARE DONE, PROGRESS TO END STATE AND CLEAR ANIMATIONS
        public void waitForAnimations(GameTime gameTime)
        {
            bool completed = true;
            foreach(var anim in animations)
            {
                if(!anim.stale) { completed = false; }
            }

            if (completed)
            {
                animState = end;
            }
        }

        public void highlightPlacedPiece(GameTime gameTime) //change later to briefly flash the newly placed piece white
        {
            foreach(var block in encounter.currentPiece.blocks)
            {
                regSprite sprite1 = new regSprite(content.boxsolid, Color.White, 0.85f);

                animFrame frame1 = new animFrame(sprite1, 200);
                animSprite anim = new animSprite(encounter.board.boardRoot, [frame1], block.boardpos);

            }
            timer = 100;
            animState = timedWait;
        }

        public void timedWait(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if(timer <= 0)
            {
                animState = none;
            }

        }

        public void end(GameTime gameTime) { animations.Clear(); animState = none; }
        public void none(GameTime gameTime) { }


        public static animSprite getDecayingAnim(Vector2I boardpos)
        {
            //rework this then go to block method and rework that
            regSprite sprite1 = new regSprite(content.full, Color.White, boardpos, 0.86f);
            regSprite sprite2 = new regSprite(content.full75, Color.White, boardpos, 0.86f);
            regSprite sprite3 = new regSprite(content.full50, Color.White, boardpos, 0.86f);
            regSprite sprite4 = new regSprite(content.full25, Color.White, boardpos, 0.86f);

            animFrame frame1 = new animFrame(sprite1, 100);
            animFrame frame2 = new animFrame(sprite2, 50);
            animFrame frame3 = new animFrame(sprite3, 50);
            animFrame frame4 = new animFrame(sprite4, 50);

            animFrame[] sequence = [frame1, frame2, frame3, frame4];

            animSprite anim = new animSprite(sequence);
            //animatons.Add(anim);
            return anim;
        }

    }
}
