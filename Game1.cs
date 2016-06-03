using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //PLAYER VARIABLES//
        //Represents the player
        Player player;
        //Keyboard states ued to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        //Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;
        //Mouse states to track mouse button presses
        MouseState currentMouseState;
        MouseState previousMouseState;
        //Movement speed for the player
        float playerMovementSpeed;
        //Laser texture
        Texture2D laserTexture;
        //How fast the player can fire the laser
        TimeSpan laserSpawnTime;
        TimeSpan previousLaserSpawnTime;
        //Laser list
        List<Laser> laserBeams;
        //GAME ENVIRONMENT VARIABLES//
        //Image used to display the static background
        Texture2D mainBackground;
        Rectangle rectBackground;
        float scale = 1f;
        //Parallaxing objects
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;
        //Texture for explosion animatino
        Texture2D explosionTexture;
        //Laser sound and Instance
        private SoundEffect laserSound;
        private SoundEffectInstance laserSoundInstance;
        //Explosion sound
        private SoundEffect explosionSound;
        private SoundEffectInstance explosionSoundInstance;
        //Game music
        private Song gameMusic;
        //Player lives texture
        public Texture2D playerLives;
        //Number to determine when to spawn an upgrade
        int upgradeLimit;
        //Array of life texture positions
        Vector2[] lifePositions;
        //Sprite font
        SpriteFont gameFont;
        //Array of gameOver vectors
        Vector2[] stringVec;
        //String to display "Game Over"
        string gameover;
        //Displays player score, how many shots fired, amount of enemies killed, and computed accuracy upon game over
        string score;
        string shotsFired;
        string enemiesKilled;
        string accuracy;
        //Position of the game over text
        Vector2 gOver;
        //Governs when the start screen and game over screen should be displayed
        bool startScreen, endScreen;
        //Position of start screen text
        Vector2 startStringVec;
        //Displays game welcome message
        string startString;
        //ENEMY VARIABLES//
        //Enemies
        Texture2D animatedTexture;
        //enemy 2 texture
        Texture2D staticTexture;
        //List<Enemy> animatedEnemies;
        //List<Enemy> staticEnemies;
        List<Enemy> allEnemies;
        //Enemies objects
        AnimatedEnemy animatedEnemy;
        StaticEnemy staticEnemy;
        //The reate at which enemies appear
        TimeSpan animatedSpawnTime;
        TimeSpan previousSpawnTime;
        TimeSpan staticSpawnTime;
        TimeSpan previousStaticSpawnTime;
        //A random number generator
        Random random;
        //Explosions list
        List<Explosion> explosions;  
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            player = new Player();
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();
            animatedEnemy = new AnimatedEnemy();
            staticEnemy = new StaticEnemy();
            //Initalize random number generator
            random = new Random();
            //Enable FreeDrag
            TouchPanel.EnabledGestures = GestureType.FreeDrag;
            //Initalize the enemy list
            allEnemies = new List<Enemy>();
            //init laser
            laserBeams = new List<Laser>();
            //init collection of explosions
            explosions = new List<Explosion>();
            stringVec = new Vector2[5];
            //Array of life texture positions
            Vector2[] lifePositions = new Vector2[player.lives];
            //Set player move speed
            playerMovementSpeed = 8.0f;
            //Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;
            //Used to determine how fast enemy respawns
            animatedSpawnTime = TimeSpan.FromSeconds(1.0f);
            staticSpawnTime = TimeSpan.FromSeconds(5.0f);
            //Variables to calculate player rate of fire
            const float SECONDS_IN_MINUTE = 60f;
            const float RATE_OF_FIRE = 200f;
            laserSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / RATE_OF_FIRE);
            previousLaserSpawnTime = TimeSpan.Zero;
            //Start screen displayed upon game launch
            startScreen = true;
            //Game over displayed when endScreen == true
            endScreen = false;
            upgradeLimit = 8;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //For static sprites: player.Initialize(Content.Load<Texture2D>("Graphics\\player"), playerPosition);
            //Load player lives texture
            playerLives = Content.Load<Texture2D>("Graphics/player");
            //Animation
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("Graphics/shipAnimation");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerAnimation, playerPosition, playerLives);
            //Load the parallaxing background
            bgLayer1.Initalize(Content, "Graphics/bgLayer1", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -1);
            bgLayer2.Initalize(Content, "Graphics/bgLayer2", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -2);
            mainBackground = Content.Load<Texture2D>("Graphics/mainbackground");
            //load enemy texture
            animatedTexture = Content.Load<Texture2D>("Graphics/mineAnimation");
            //load player laser
            laserTexture = Content.Load<Texture2D>("Graphics/laser");
            //load explosions sheet
            explosionTexture = Content.Load<Texture2D>("Graphics/explosion");
            //Load the laserSound effect and creat the instance
            laserSound = Content.Load<SoundEffect>("Sound/laserFire");
            laserSoundInstance = laserSound.CreateInstance();
            //Load Explosion sound effect and create instance
            explosionSound = Content.Load<SoundEffect>("Sound/explosion");
            explosionSoundInstance = explosionSound.CreateInstance();
            //Load game music
            gameMusic = Content.Load<Song>("Sound/gameMusic");
            MediaPlayer.Play(gameMusic);
            //Load sprite font
            gameFont = Content.Load<SpriteFont>("GameFont");
            //Load enemy 2 textures
            staticTexture = Content.Load<Texture2D>("Graphics/1457411160_alienblaster");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            laserSoundInstance.Dispose();
            explosionSoundInstance.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Game will exit at anytime if escape or back button is pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //Display game over screen if player is out of lives
            if (player.lives <= 0)
                endScreen = true;

            //Update the parallaxing background  
            bgLayer1.Update(gameTime);
            bgLayer2.Update(gameTime);

            //Start screen instructions. Displays upon launch of the game
            if (startScreen == true)
            {
                startStringVec = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Center.X / 3, GraphicsDevice.Viewport.TitleSafeArea.Center.Y / 2);
                startString = "Press ENTER to \n\nfire up the engines.\n\nPress ESC to exit";

                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                    startScreen = false;
            }
            //Game over screen and logic to calculate placement and final variables
            if (endScreen == true)
            {
                double acc;
                gameover = "Game Over";
                score = "Score: " + player.score;
                shotsFired = "Lasers Fired: " + player.shotsFired;
                enemiesKilled = "Enemies Killed: " + player.shipsDestroyed;
                if (player.shotsFired == 0)
                    acc = 0;
                else
                    acc = ((double)player.shipsDestroyed / player.shotsFired) * 100;
                accuracy = "Accuracy: " + (int)acc + " % ";
                gOver = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Center.X / 2, GraphicsDevice.Viewport.TitleSafeArea.Center.Y / 2);
                //Figure out the positions do display the strings
                for (int i = 0; i < 5; i++)
                {
                    stringVec[i] = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Center.X / 2, (GraphicsDevice.Viewport.TitleSafeArea.Center.Y + 40 * i));
                }
            }

            if (endScreen == false && startScreen == false)
            {
                //Save the previous state of the keyboard and game pad so we can determine single key/button presses
                previousGamePadState = currentGamePadState;
                previousKeyboardState = currentKeyboardState;
                previousMouseState = currentMouseState;
                //Read the current state of the keyboard and gamepad and store it
                currentKeyboardState = Keyboard.GetState();
                currentGamePadState = GamePad.GetState(PlayerIndex.One);
                currentMouseState = Mouse.GetState();
                //Update the player
                UpdatePlayer(gameTime);
                base.Update(gameTime);
                //Update the enemies
                UpdateEnemies(gameTime);
                //Update the collision
                UpdateCollision();
                //Update laserbeams
                UpdateLaserBeams(gameTime);
                //Update explosions
                UpdateExplosions(gameTime);
                //Update lives
                UpdateLives();
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);
            //Windows 8 touch gestures for MonoGame
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                if (gesture.GestureType == GestureType.FreeDrag)
                {
                    player.Position += gesture.Delta;
                }
            }
            //Get mouse state and capture button type and respond to button press
            Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 posDelta = mousePosition - player.Position;
                posDelta.Normalize();
                posDelta = posDelta * playerMovementSpeed;
                player.Position = player.Position + posDelta;
            }
            //Get Thumbstick Controls
            player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMovementSpeed;
            player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMovementSpeed;
            //Use Keyboard/Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                player.Position.X -= playerMovementSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                player.Position.X += playerMovementSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) || currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                player.Position.Y -= playerMovementSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) || currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                player.Position.Y += playerMovementSpeed;
            }
            //Make sure the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, player.Width * player.PlayerAnimation.scale / 2, GraphicsDevice.Viewport.Width - player.Width * player.PlayerAnimation.scale / 2);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, player.Height * player.PlayerAnimation.scale / 2, GraphicsDevice.Viewport.Height - player.Height * player.PlayerAnimation.scale / 2);
            //Fires laser
            if (currentKeyboardState.IsKeyDown(Keys.Space) || currentGamePadState.Buttons.X == ButtonState.Pressed)
            {
                FireLaser(gameTime);
            }
        }

        private void AddEnemy(string _type)
        {                    
            //Randomly generate the position of the enemy
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + animatedTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));

            //Initalize the animatino with the correct animatino information    
            if (_type == "animated")
            {
                AnimatedEnemy tempAnimated = new AnimatedEnemy();
                //create the animation object
                Animation enemyAnimation = new Animation();
                //Initalize the enemy
                enemyAnimation.Initialize(animatedTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);
                //Create an enemy
                tempAnimated.InitalizeEnemy(enemyAnimation, position);
                //Add the enemy to the active enemies list
                //animatedEnemies.Add(tempAnimated);
                allEnemies.Add(tempAnimated);
            }
            else
            {
                StaticEnemy tempStatic = new StaticEnemy();
                //Create an enemy
                tempStatic.InitalizeEnemy(position, staticTexture);
                //Add the enemy to the active enemies list
                //staticEnemies.Add(tempStatic);
                allEnemies.Add(tempStatic);
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            string staticEnemy = "static";
            string animatedEnemy = "animated";
              
            //Spawn a new enemy every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > animatedSpawnTime)
            {
                //if time to spawn secondary enemy
                if (gameTime.TotalGameTime - previousStaticSpawnTime > staticSpawnTime)
                {
                    AddEnemy(staticEnemy);
                    previousStaticSpawnTime = gameTime.TotalGameTime;
                    previousSpawnTime = gameTime.TotalGameTime;
                }
                else //Add standard enemy
                {
                    AddEnemy(animatedEnemy);
                    previousSpawnTime = gameTime.TotalGameTime;
                }                

            }
            //Update the Enemies
            for(int i = allEnemies.Count -1; i >= 0; i--)
            {
                allEnemies[i].UpdateEnemy(gameTime);
                if(allEnemies[i].Active == false)
                {
                    allEnemies.RemoveAt(i);
                }
            }
        }

        protected void UpdateCollision()
        {

            // we are going to use the rectangle's built in intersection
            // methods.

            Rectangle playerRectangle;
            Rectangle enemyRectangle;
            Rectangle laserRectangle;

            // create the rectangle for the player
            playerRectangle = new Rectangle(
                (int)player.Position.X,
                (int)player.Position.Y,
                player.Width,
                player.Height);

            // detect collisions between the player and all enemies.
            for (var i = 0; i < allEnemies.Count; i++)
            {
                    //create a new rectangle according to dimensions the object at allEnemies[i]
                    enemyRectangle = new Rectangle(
                   (int)allEnemies[i].Position.X,
                   (int)allEnemies[i].Position.Y,
                   allEnemies[i].Width,
                   allEnemies[i].Height);
                // determine if the player and the enemy intersect.
                if (playerRectangle.Intersects(enemyRectangle))
                {
                    // kill off the enemy
                    allEnemies[i].Health = 0;

                    // Show the explosion where the enemy was...
                    AddExplosion(allEnemies[i].Position);

                    // deal damge to the player
                    player.Health -= allEnemies[i].Damage;

                    // if the player has no health destroy it.
                    if (player.Health <= 0)
                    {
                            player.lives--;
                        if(player.lives > 0)
                            player.Health = 100;
                        else if(player.lives <= 0)
                        {
                            player.Health = 0;
                        }                        
                    }
                }

                for (var l = 0; l < laserBeams.Count; l++)
                {
                    // create a rectangle for this laserbeam
                    laserRectangle = new Rectangle(
                        (int)laserBeams[l].Position.X,
                        (int)laserBeams[l].Position.Y,
                        laserBeams[l].Width,
                        laserBeams[l].Height);

                    // test the bounds of the laser and enemy
                    if (laserRectangle.Intersects(enemyRectangle))
                    {
                        //deal damage to the enemy
                        allEnemies[i].Health -= laserBeams[l].Damage;
                        
                        // kill off the laserbeam
                        laserBeams[l].Active = false;

                        // check to see if an upgrade spawns
                        if(random.Next(1,11) <= upgradeLimit)
                        {

                        }

                        //kill enemy if health == 0
                        if (allEnemies[i].Health == 0)
                        {
                            // Show the explosion where the enemy was...
                            AddExplosion(allEnemies[i].Position);

                            // kill off the enemy
                            allEnemies[i].Health = 0;

                            //Increase player score
                            player.score += allEnemies[i].Value;

                            //Increment shipsDestroyed
                            player.shipsDestroyed++;
                        }
                    }
                }
            }
        }
        
        protected void FireLaser(GameTime gameTime)
        {
            //govern rate of fire for lasers
            if(gameTime.TotalGameTime - previousLaserSpawnTime > laserSpawnTime)
            {
                previousLaserSpawnTime = gameTime.TotalGameTime;
                //Add the laser to the list
                AddLaser();
                //Play the laser sound
                laserSoundInstance.Play();
                //Increment lasers fired for player
                player.shotsFired++;
            }
        }

        protected void AddLaser()
        {
            Animation laserAnimation = new Animation();
            //init the laser animation
            laserAnimation.Initialize(laserTexture, player.Position, 46, 16, 1, 30, Color.White, 1f, true);

            Laser laser = new Laser();
            //Get the starting point of the laser
            var laserPosition = player.Position;

            //init the laser
            laser.Initalize(laserAnimation, laserPosition);
            laserBeams.Add(laser);
        }

        public void UpdateLaserBeams(GameTime gameTime)
        {
            for (var i = 0; i < laserBeams.Count; i++)
            {
                laserBeams[i].Update(gameTime);
                //Remove the beam when it is deactive or end of screen
                if (!laserBeams[i].Active || laserBeams[i].Position.X > GraphicsDevice.Viewport.Width)
                {
                    laserBeams.RemoveAt(i);
                }
            }
         }

        //Create an explosion animation and generate the sound
        protected void AddExplosion(Vector2 Position)
        {
            Animation explosionAnimation = new Animation();

            explosionAnimation.Initialize(explosionTexture, Position, 134, 134, 12, 30, Color.White, 1.0f, true);

            Explosion explosion = new Explosion();
            explosion.Initialize(explosionAnimation, Position);

            explosions.Add(explosion);

            explosionSound.Play();
        }

        //Check to see if there are any explosion animations to remove
        private void UpdateExplosions(GameTime gameTime)
        {   
            for(var e = 0; e < explosions.Count; e++)
            {
                explosions[e].Update(gameTime);
                if(!explosions[e].Active)
                {
                    explosions.Remove(explosions[e]);
                }
            }
        }

        //Check and update player lives and draw them
        protected void UpdateLives()
        {
            Vector2 livesPosition = new Vector2();
            lifePositions = new Vector2[player.lives];
            for (int i = 0; i < player.lives; i++)
            {
                livesPosition = new Vector2((playerLives.Width * i) / 3, 0);
                lifePositions[i] = livesPosition;
            }
        }

        //Generate string and draw the score
        protected void DrawScore(SpriteBatch spritebatch)
        {
            string scoreString = "Score: " + player.score;
            Vector2 scoreVector = new Vector2(0, GraphicsDevice.Viewport.TitleSafeArea.Height - 32);
            spriteBatch.DrawString(gameFont, scoreString, scoreVector, Color.PaleVioletRed);
        }

        //Generate string and draw the health
        protected void DrawHealth(SpriteBatch spritebatch)
        {
            string health = "Health: " + player.Health;
            Vector2 healthVector = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Height - 32);
            spriteBatch.DrawString(gameFont, health, healthVector, Color.PaleVioletRed);
        }  

        //Method for drawing a string to the screen
        protected void DrawString(SpriteBatch spritebatch, Vector2 vector, string _string)
        {
            spriteBatch.DrawString(gameFont, _string, vector, Color.Orange);
        }
                    
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Start drawing
            spriteBatch.Begin();
            //Draw the main background texture
            rectBackground = GraphicsDevice.Viewport.TitleSafeArea;
            spriteBatch.Draw(mainBackground, rectBackground, Color.White);
            //Draw the moving background
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            //Draw the beginning start screen
            if(startScreen == true)
                DrawString(spriteBatch, startStringVec, startString);

            //Draw the game over screen
            if (endScreen == true)
            {

                DrawString(spriteBatch, gOver, gameover);
                DrawString(spriteBatch, stringVec[0], score);
                DrawString(spriteBatch, stringVec[1], shotsFired);
                DrawString(spriteBatch, stringVec[2], enemiesKilled);
                DrawString(spriteBatch, stringVec[3], accuracy);
            }

            //Normal game runtime
            else if (endScreen == false && startScreen == false)
            {
                //Draw lives
                for (int i = 0; i < player.lives; i++)
                {
                    player.DrawLives(spriteBatch, lifePositions[i]);
                }
                //Draw score          
                DrawScore(spriteBatch);
                //Draw health
                DrawHealth(spriteBatch);
                //Draw the lasers
                foreach (var l in laserBeams)
                {
                    l.Draw(spriteBatch);
                }
                //Draw the enemies
                   for (int i = 0; i < allEnemies.Count; i++)
                {
                    allEnemies[i].DrawEnemy(spriteBatch);
                }
                //Draw the player
                player.Draw(spriteBatch);
                //draw explosions
                foreach (var e in explosions)
                {
                    e.Draw(spriteBatch);
                }
            }
            //Stop drawing
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
