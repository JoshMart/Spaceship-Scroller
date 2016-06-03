using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1;

namespace Game1
{
    class Explosion
    {
        public Animation explosionAnimation;
        public Vector2 Position;
        public bool Active;
        int timeToLive;
        public int Width

        {
            get { return explosionAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return explosionAnimation.FrameWidth; }
        }

        //Set the base variables
        public void Initialize(Animation animation, Vector2 position)
        {
            explosionAnimation = animation;
            Position = position;
            Active = true;
            timeToLive = 30;
        }

        //Decrement timeToLive until 0, then set active to false
        public void Update(GameTime gameTime)
        {
            explosionAnimation.Update(gameTime);
            timeToLive -= 1;
            if(timeToLive <= 0)
            {
                this.Active = false;
            }
        }

        //Draw the explosion
        public void Draw(SpriteBatch spriteBatch)
        {
            explosionAnimation.Draw(spriteBatch);
        }
    }
}
