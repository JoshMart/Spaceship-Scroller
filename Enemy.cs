using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    abstract class Enemy
    {
        //Position of the enemy ship relative to the top left corner of the screen
        public Vector2 Position;

        //The state of the enemy ship
        public bool Active;

        //Hit points of the enemy, if == 0 the enemy dies
        public int Health;

        //The amount of damage the enemy inflicts on the player
        public int Damage;

        //The amount of score the enemy will give to the player
        public int Value;

        //The speed at which the enemy moves
        public float enemyMoveSpeed;

        //Width of the enemy ship
        public int Width;

        //Height of the enemy ship
        public int Height;

        //Method to update the enemy
        public abstract void UpdateEnemy(GameTime gametime);

        //Method to draw enemy
        public abstract void DrawEnemy(SpriteBatch spritebatch);

     }
}
