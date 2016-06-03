using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class AnimatedEnemy
        : Enemy
    {
        //Animation representing the enemy
        public Animation EnemyAnimation;

        public void InitalizeEnemy(Animation animation, Vector2 position)
        {
            //Load enemy ship texture
            EnemyAnimation = animation;
            //Set Width
            Width = animation.FrameWidth;
            //Set Height
            Height = animation.FrameHeight;
            //Set position of the enemy
            Position = position;
            //We initalize the enemy to be active so it will update in the game
            Active = true;
            //Set the health of the enemy
            Health = 10;
            //Set the amount of damage the enemy can do
            Damage = 10;
            //Set how fast the enemy moves
            enemyMoveSpeed = 6f;
            //Set the score value of the enemy
            Value = 100;
        }

        //Move the enemy across the screen and if it reaches the edge, set active to false
        override public void UpdateEnemy(GameTime gameTime)
        {
            //The enemy always moves to the left so derement its X position
            Position.X -= enemyMoveSpeed;
            //Update the position of the animation
            EnemyAnimation.Position = Position;
            //Update Animation
            EnemyAnimation.Update(gameTime);
            //If the enemy is past the screen or its health reaches 0, deactive it
            if (Position.X < -Width || Health <= 0)
            {
                //Setting active flage to false, the object will be removed from the active game list
                Active = false;
            }
        }

        //Draws the enemy 
        override public void DrawEnemy(SpriteBatch spriteBatch)
        {
            //Draw the animation
            EnemyAnimation.Draw(spriteBatch);
        }
    }
    }
