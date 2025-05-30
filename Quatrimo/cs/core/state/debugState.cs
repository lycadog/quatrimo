﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace Quatrimo
{
    public class debugState : gameState
    {
        public debugState(stateManager manager) : base(manager)
        {
        }
        public override void addState()
        {
            manager.keyUpdate += updateDebugKeys;
            manager.updateD += debugUpdate;
            manager.draw2xRes += drawDebugText;
            manager.state.Add(this);
        }

        public override void removeState()
        {
            manager.keyUpdate -= updateDebugKeys;
            manager.updateD -= debugUpdate;
            manager.draw2xRes -= drawDebugText;
            manager.state.Remove(this);
        }

        public override void setState()
        {
            manager.keyUpdate = updateDebugKeys;
            manager.updateD = debugUpdate;
            manager.draw2xRes = drawDebugText;
            manager.state.Clear();
            manager.state.Add(this);
        }

        protected void updateDebugKeys(GameTime gameTime)
        {
            keybind.updateKeybinds(keybind.debugKeys, gameTime);
        }
        protected void debugUpdate(GameTime gameTime)
        {
            if(manager.encounter.state is midPiecefallState)
            {
                if (keybind.debugKey1.keyDown)
                {
                    manager.encounter.currentPiece.removeFalling();
                    manager.encounter.currentPiece = manager.encounter.nextPiece;
                    manager.encounter.currentPiece.play();

                    manager.encounter.nextPiece = manager.encounter.bag.drawRandomPiece();
                    manager.encounter.board.nextbox.update(manager.encounter.nextPiece);
                }
            }
        }

        protected void drawDebugText(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(content.font, "DEBUG", new Vector2(450, 20), Color.Yellow);

            spriteBatch.DrawString(content.fontSmall, $"{Mouse.GetState().X}, {Mouse.GetState().Y}", new Vector2(Mouse.GetState().X / (Game1.scale * 0.5f) - Game1.frameOffset, Mouse.GetState().Y / (Game1.scale * 0.5f)), Color.CornflowerBlue);
            //spriteBatch.DrawString(Game1.fontSmall, fps.ToString(), new Vector2(20, 460), Color.CornflowerBlue);
            //spriteBatch.DrawString(Game1.fontSmall, $"gamestate: {manager.encounter.stateOld}", new Vector2(20, 460), Color.CornflowerBlue);
            //spriteBatch.DrawString(Game1.fontSmall, $"piecefallTimer {manager.encounter.piecefallTimer}/600", new Vector2(20, 480), Color.LightBlue);
            //spriteBatch.DrawString(Game1.fontSmall, $"placeTimer {manager.encounter.placeTimer}/1000", new Vector2(20, 500), Color.LightBlue);
            //spriteBatch.DrawString(Game1.fontSmall, $"pieceWaitTimer {manager.encounter.pieceWaitTimer}/5000", new Vector2(20, 520), Color.LightBlue);
        }

        
    }
}
