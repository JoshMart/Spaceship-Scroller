using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1;

namespace Shooter
{
    class Player
    {
        //Animation representing the player
        public Animation PlayerAnimation;

        //Position of Player relative to the upper left side of the screen
        public Vector2 Position;

        //Lives texture
        public Texture2D livesTexture;

        //State of the player
        public bool Active;

        //Amount of hitpoints the player has
        public int Health;

        //Player score
        public int score;

        //Number of lasers fired
        public int shotsFired;

        //Number of ships desstroyed
        public int shipsDestroyed;

        //Get the width of the player ship
        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        //Get the height of the player ship
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        //Players lives
        public int lives;

        //Initializer to create a Player object
        public void Initialize(Animation animation, Vector2 position, Texture2D _livesTexture)
        {
            PlayerAnimation = animation;
            //Set starting position of the player around the middle of the screen and to the back
            Position = position;
            //Set player to be active
            Active = true;
            //Set Player health
            Health = 100;
            //Set player lives
            lives = 1;
            //Set inital score
            score = 0;
            //Texture for lives display
            livesTexture = _livesTexture;
            //Set # of lasers fired
            shotsFired = 0;
            //Set # of ships destroyed
            shipsDestroyed = 0;
        }

        //Update the player animation
        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
            
        }

        //Draws an animated image
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }

        //Draws the lives onto the screen
        public void DrawLives(SpriteBatch spriteBatch, Vector2 livesPosition)
        {
            spriteBatch.Draw(livesTexture, livesPosition, Color.White);
        }
        
        /*Draws sprite by queing Texture for rendering (Draws a static image)
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }*/

    }
}
