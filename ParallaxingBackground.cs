﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class ParallaxingBackground
    {
        //Image representing the parallaxing background
        Texture2D texture;
        //An array of positions of the parallaxing background
        Vector2[] positions;
        //The speed which the background is moving
        int speed;
        //Background variables
        int bgHeight;
        int bgWidth;

        public void Initalize(ContentManager content, String texturePath, int screenWidth, int screenHeight, int speed)
        {
            bgHeight = screenHeight;
            bgWidth = screenWidth;

            //Load the background texture we will be using
            texture = content.Load<Texture2D>(texturePath);
            //Set the speed of the background
            this.speed = speed;
            //If we divide the screen with the texture width we can determine the number of tiles needed
            //Add 1 so we wont have a gap in the tiling
            positions = new Vector2[screenWidth / texture.Width + 1];
            //Set the initial positions of the parallaxing background
            for (int i = 0; i < positions.Length; i++)
            {
                //We need the tiles to be side by side to create a tiling effect 
                positions[i] = new Vector2(i * texture.Width, 0);
            }

        }

        public void Update(GameTime gameTime)
        {
            //Update the positions of the background
            for(int i = 0; i < positions.Length; i++)
            {
                //Update the position of the screen by adding the speed
                positions[i].X += speed;
                //If the speed has the background moving to the left
                if(speed <= 0)
                {
                    //Check the texture is out of view then put that texture at the end of the screen                    
                        if(positions[i].X <= -texture.Width)
                        {
                            positions[i].X = texture.Width * (positions.Length - 1);
                        }                    
                }
                //If the speed has the background moving to the right
                else
                {
                    //Check if the texture is out of view then position it to the start of the screen
                    if(positions[i].X >= texture.Width * (positions.Length -1))
                    {
                        positions[i].X = -texture.Width;
                    }

                }
            }
        }

        //Draw the background
        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < positions.Length; i++)
            {
                Rectangle rectBg = new Rectangle((int)positions[i].X, (int)positions[i].Y, bgWidth, bgHeight);
                spriteBatch.Draw(texture, rectBg, Color.White);
            }
        }
    }
}
