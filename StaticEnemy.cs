using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class StaticEnemy
        : Enemy
    {
        //2D Texture for non-animated enemy
        public Texture2D EnemyTexure;
        
        public void InitalizeEnemy(Vector2 position, Texture2D texture)
        {
            //Load enemy2 texture
            EnemyTexure = texture;
            //Set Width
            Width = texture.Width;
            //Set Height
            Height = texture.Height;
            //Set position of the enemy
            Position = position;
            //We initalize the enemy to be active so it will update in the game
            Active = true;
            //Set the health of the enemy
            Health = 20;
            //Set the amount of damage the enemy can do
            Damage = 20;
            //Set how fast the enemy moves
            enemyMoveSpeed = 6f;
            //Set the score value of the enemy
            Value = 500;
        }

        //Static Enemy implementation of DrawEnemy
        override public void DrawEnemy(SpriteBatch spritebatch)
        {
            spritebatch.Draw(EnemyTexure, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        //Static Enemy implementation of UpdateEnemy
        override public void UpdateEnemy(GameTime gametime)
        {
            //The enemy always moves to the left so derement its X position
            Position.X -= enemyMoveSpeed;
            //If the enemy is past the screen or its health reaches 0, deactive it
            if (Position.X < -EnemyTexure.Width || Health <= 0)
            {
                //Setting active flage to false, the object will be removed from the active game list
                Active = false;
            }
        }
    }
}
