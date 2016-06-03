using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Laser
    {
        //laser animation
        public Animation LaserAnimation;

        //the speed the laser travels
        float laserMoveSpeed = 30f;

        //position of the laser
        public Vector2 Position;

        //the damage the laser deals
        public int Damage;

        //set the laser to active
        public bool Active;

        //laser beams range
        int Range;

        //the width of the laser image
        public int Width
        {
            get { return LaserAnimation.FrameWidth; }
        }

        //the height of the laser image
        public int Height
        {
            get { return LaserAnimation.FrameHeight; }
        }

        //Set base variables for laser
        public void Initalize(Animation animation, Vector2 position)
        {
            LaserAnimation = animation;
            Position = position;
            Active = true;
            Damage = 10;
        }

        //Move the laser across the screen
        public void Update(GameTime gameTime)
        {
            Position.X += laserMoveSpeed;
            LaserAnimation.Position = Position;
            LaserAnimation.Update(gameTime);
        }

        //Draw the laser animation
        public void Draw(SpriteBatch spriteBatch)
        {
            LaserAnimation.Draw(spriteBatch);
        }
    }
}
