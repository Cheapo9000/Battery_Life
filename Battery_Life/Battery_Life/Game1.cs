using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Battery_Life
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Splash declarations
        Obj splashScreen;
        int splash = 0;
        float wait = 0;
        float scaleSplash = 1.0f;
        Obj actor;

        //Game declarations
        DisplayFont instructions;
        Robot robot;
        Obj[][] walls = new Obj[100][];
        int maxWalls = 2600;
        Obj[] floors;
        int maxFloors = 2600;
        Battery battery;
        bool breakFail = false;
        Obj[] batteries;
        int maxBatteries = 100;
        Obj[] bigBatteries;
        int maxBigBatteries = 10;
        Obj[] backgrounds;
        int maxBackgrounds = 200;
        Obj[] foregrounds;
        int maxForegrounds = 20;
        public Random rand = new Random();


        SpriteFont breakStreak;
        Vector2 breakStreakPosition;
        int currentStreak;
        int highestStreak;

        //win state variables
        int finishLine = 0;
        Obj plug;
        bool stop = false;

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
            // TODO: Add your initialization logic here
            splashScreen = new Obj(Content.Load<Texture2D>("Sprites\\titleScreen"), new Vector2(160, 20), false, 0);
            wait = 100;
            actor = new Obj(Content.Load<Texture2D>("Sprites\\Robot_standing"), new Vector2(100, 352), false, 0);

#if !XBOX
            string[] winInstructions = new string[1];
            winInstructions[0] = "A = Red    S = Blue    D = Green    F = Yellow";
            instructions = new DisplayFont(Content.Load<SpriteFont>("Segoe UI Mono"), winInstructions, new Vector2(25, 425), Color.Black, 100);
#elif XBOX
            Texture2D[] xboxButtons = new Texture2D[4];
            xboxButtons[0] = Content.Load<Texture2D>("Sprites\\b_button");
            xboxButtons[1] = Content.Load<Texture2D>("Sprites\\x_button");
            xboxButtons[2] = Content.Load<Texture2D>("Sprites\\a_button");
            xboxButtons[3] = Content.Load<Texture2D>("Sprites\\y_button");
            string[] xboxInstructions = new string[4];
            string temp = " = Red";
            xboxInstructions[0] = temp.ToString();
            temp = " = Blue";
            xboxInstructions[1] = temp.ToString();
            temp = " = Green";
            xboxInstructions[2] = temp.ToString();
            temp = " = Yellow";
            xboxInstructions[3] = temp.ToString();
            instructions = new DisplayFont(Content.Load<SpriteFont>("Courier New"), xboxInstructions, new Vector2(25, 425), Color.Black, 100);
#endif

            //robot = new Robot(Content.Load<Texture2D>("Sprites\\Robot16"), Content.Load<Texture2D>("Sprites\\shield"));
            //robot = new Robot(Content.Load<Texture2D>("Sprites\\Robot16"), Content.Load<Texture2D>("Sprites\\shield"), 1, 36, 67);
            //create walls
            highestStreak = 0;

            Texture2D[] backgroundTextures = new Texture2D[15];
            backgroundTextures[0] = Content.Load<Texture2D>("Sprites\\wall_back");
            backgroundTextures[1] = Content.Load<Texture2D>("Sprites\\wall_back2");
            backgroundTextures[2] = Content.Load<Texture2D>("Sprites\\wall_back3");
            backgroundTextures[3] = Content.Load<Texture2D>("Sprites\\wall_back4");
            backgroundTextures[4] = Content.Load<Texture2D>("Sprites\\wall_back5");
            backgroundTextures[5] = Content.Load<Texture2D>("Sprites\\wall_back6");
            backgroundTextures[6] = Content.Load<Texture2D>("Sprites\\wall_back7");
            backgroundTextures[7] = Content.Load<Texture2D>("Sprites\\wall_back8");
            backgroundTextures[8] = Content.Load<Texture2D>("Sprites\\wall_back9");
            backgroundTextures[9] = Content.Load<Texture2D>("Sprites\\wall_back10");
            backgroundTextures[10] = Content.Load<Texture2D>("Sprites\\wall_back11");
            backgroundTextures[11] = Content.Load<Texture2D>("Sprites\\wall_back12");
            backgroundTextures[12] = Content.Load<Texture2D>("Sprites\\column");
            backgroundTextures[13] = Content.Load<Texture2D>("Sprites\\column2");
            backgroundTextures[14] = Content.Load<Texture2D>("Sprites\\column3");

            backgrounds = new Obj[maxBackgrounds];
            Vector2 tempPos = new Vector2(0, 0);
            for (int i = 0; i < maxBackgrounds; i++)
            {
                if (i % 2 == 1)
                {
                    backgrounds[i] = new Obj(backgroundTextures[rand.Next(0, 11)], tempPos, false, 0);
                    tempPos.X += 478;
                }
                else
                {
                    backgrounds[i] = new Obj(backgroundTextures[rand.Next(12, 14)], tempPos, false, 0);
                    tempPos.X += 139;
                }
            }
            for (int i = 0; i < 100; i++)
            {
                walls[i] = new Obj[26];
            }

            int color;
            for (int i = 0; i < maxWalls / 26; i++)
            {
                color = rand.Next(1, 5);
                for (int j = 0; j < 26; j++)
                {
                    walls[i][j] = new Obj(Content.Load<Texture2D>("Sprites\\walltest"), new Vector2(600 + 450 * i, 394 - 22 * j), false, color);
                }
            }


            floors = new Obj[maxFloors];
            for (int i = 0; i < maxFloors; i++)
            {
                floors[i] = new Obj(Content.Load<Texture2D>("Sprites\\floor_block"), new Vector2(124 * i, 416), true, 0);
            }

            battery = new Battery(Content.Load<Texture2D>("Sprites\\lifeBar"), Content.Load<Texture2D>("Sprites\\lifeBlock"));
            InitBatteries();

            plug = new Obj(Content.Load<Texture2D>("Sprites\\plug large"), new Vector2(30000, 150), false, 0);

            base.Initialize();
        }

        protected void InitBatteries()
        {
            batteries = new Obj[maxBatteries];
            bigBatteries = new Obj[maxBigBatteries];
            Texture2D initText = Content.Load<Texture2D>("Sprites\\smallBattery");
            Texture2D initTextBig = Content.Load<Texture2D>("Sprites\\bigBattery");
            batteries[0] = new Obj(initText, new Vector2(3000, 400), false, 0);
            batteries[1] = new Obj(initText, new Vector2(5000, 400), false, 0);
            batteries[2] = new Obj(initText, new Vector2(8000, 400), false, 0);
            batteries[3] = new Obj(initText, new Vector2(13000, 400), false, 0);
            batteries[4] = new Obj(initText, new Vector2(18000, 400), false, 0);

            for (int i = 5; i < maxBatteries; i++)
            {
                batteries[i] = new Obj(initText, new Vector2(-100, -100), false, 0);
                batteries[i].alive = false;
            }

            bigBatteries[0] = new Obj(initTextBig, new Vector2(25000, 400), false, 0);

            for (int i = 1; i < maxBigBatteries; i++)
            {
                bigBatteries[i] = new Obj(initTextBig, new Vector2(-100, -100), false, 0);
                bigBatteries[i].alive = false;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            robot = new Robot(Content.Load<Texture2D>("Sprites\\Robot16"), Content.Load<Texture2D>("Sprites\\shield"), 1, 36, 67);
            robot.Position = new Vector2(100, 352);
            breakStreak = Content.Load<SpriteFont>("Courier New");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            
            //title screen
            if (splash == 0)
            {
                if (wait <= 0)
                {
                    wait = 100;
                    splash = 1;
                    splashScreen = new Obj(Content.Load<Texture2D>("Sprites\\Bomb"), splashScreen.position, false, 0);
                }
                else
                {
                    wait--;
                }
            } 
            //the bomb planting on robot
            else if (splash == 1)// || splash == 2)
            {
                if (splash == 1)
                {
                    splashScreen.position.Y += 3.55f;
                    splashScreen.position.X -= 0.5f;
                    scaleSplash -= 0.01f;
                    if (scaleSplash <= 0)
                    {
                        splash = 2;
                        scaleSplash = 1f;
                    }
                }
            }
            //start of game
            else if (splash == 2 || (splash == 3))
            {
                //Run if haven't reached end / Congratulation screen
                if (splash != 3)
                {

                    instructions.Update();
                    //running animation
                    robot.Animate(gameTime);

                    battery.Update();

                    for (int i = 0; i < 100; i++)
                    {
                        if (robot.Update(walls[i], breakFail) && !breakFail)
                        {
                            breakFail = true;
                            break;
                        }
                        else if (breakFail)
                        {
                            breakFail = false;
                        }
                    }
                    for (int i = 0; i < backgrounds.Length; i++)
                    {
                        backgrounds[i].Update(robot.isColliding(), breakFail, stop);
                    }
                    for (int i = 0; i < maxBatteries; i++)
                    {
                        batteries[i].Update(robot.isColliding(), breakFail, stop);
                    }
                    for (int i = 0; i < maxBigBatteries; i++)
                    {
                        bigBatteries[i].Update(robot.isColliding(), breakFail, stop);
                    }
                    robot.Update(floors, breakFail);
                    robot.Update(batteries, battery, 1);
                    robot.Update(bigBatteries, battery, 5);
                    //int trueCount = 0;
                    if (breakFail)
                    {
                        int i = 0;
                    }
                    for (int j = 0; j < walls.Length; j++)
                    {
                        for (int i = 0; i < walls[j].Length; i++)
                        {
                            walls[j][i].Update(robot.isColliding(), robot.speed, robot.color, breakFail, rand, stop);
                        }
                    }
                    //if (trueCount > 0)
                    //{
                    //    breakFail = true;
                    //}
                    //else
                    //{
                    //    breakFail = false;
                    //}
                    for (int i = 0; i < floors.Length; i++)
                    {
                        floors[i].Update(robot.isColliding(), robot.speed, robot.color, breakFail, rand, stop);
                    }

                    if (battery.life <= 0)
                    {
                        splash = 4;
                    }

                    plug.Update(robot.isColliding(), breakFail, stop);
                    //if (plug.position.X <= 500)
                    //{
                    //    breakFail = true;
                    //}
                    if (plug.position.X <= 100)
                    {
                        stop = true;
                        if (wait <= 0)
                        {
                            splash = 3;
                            wait = 100;
                            splashScreen = new Obj(Content.Load<Texture2D>("Sprites\\win"), splashScreen.position, false, 0);
                        }
                        else
                        {
                            wait--;
                        }
                    }   
                   
                }
                //Reached plug / Win condition / Congratulation screen
                else
                {
                    if (wait <= 0)
                    {
                        //splash = 5; //reset game? or end? (we haven't decided yet)
                        wait = 100;
                        splashScreen = new Obj(Content.Load<Texture2D>("Sprites\\win"), splashScreen.position, false, 0);
                    }
                    else
                    {
                        wait--;
                    }
                }
                base.Update(gameTime);
            }
            //Death / Game Over Screen
            else if (splash == 4)
            {
                splashScreen = new Obj(Content.Load<Texture2D>("Sprites\\lose"), splashScreen.position, false, 0);
            }
            
        }

        public void Streak()
        {                
            if (robot.streakCount > highestStreak)
            {
                highestStreak = robot.streakCount;
            }
        }

        public void drawStreak()
        {
            Streak();
            //sprite fonts
            // Find the center of the string
            Vector2 FontOrigin = breakStreak.MeasureString(robot.streakCount.ToString()) / 2;
            // Draw the string
            spriteBatch.DrawString(breakStreak, "Current Streak: " + (robot.streakCount).ToString(), new Vector2(20, 20), Color.Black,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(breakStreak, "Highest Streak: " + highestStreak.ToString(), new Vector2(20, 40), Color.Black,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            if (splash == 4)
            {
                spriteBatch.DrawString(breakStreak, "Game Over", new Vector2(20, 60), Color.Black,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (splash != 4)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
            }

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (splash == 0)
            {
                //spriteBatch.Draw(splashScreen.texture, new Rectangle(0, 0, splashScreen.texture.Width, splashScreen.texture.Height), Color.White);
                spriteBatch.Draw(splashScreen.texture, splashScreen.position, new Rectangle(0, 0, splashScreen.texture.Width, splashScreen.texture.Height), Color.White, 0f, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
            }
            else if (splash == 1 || (splash == 2 || splash == 3))
            {
                for (int i = 0; i < backgrounds.Length; i++)
                {
                    if (backgrounds[i].draw)
                    {
                        //spriteBatch.Draw(backgrounds[i].texture, new Rectangle((int)backgrounds[i].position.X, (int)backgrounds[i].position.Y, backgrounds[i].texture.Width, backgrounds[i].texture.Height), Color.White);
                        spriteBatch.Draw(backgrounds[i].texture, backgrounds[i].position, new Rectangle((int)0, (int)0, (int)(backgrounds[i].texture.Width), (int)(backgrounds[i].texture.Height)), Color.White, 0f, Vector2.Zero, .93f, SpriteEffects.None, 0f);
                    }
                }

                drawStreak();
                plug.Draw(spriteBatch);

                for (int i = 0; i < batteries.Length; i++)
                {
                    if (batteries[i].draw)
                    {
                        spriteBatch.Draw(batteries[i].texture, new Rectangle((int)batteries[i].position.X, (int)batteries[i].position.Y, batteries[i].texture.Width, batteries[i].texture.Height), Color.White);
                    }
                }
                for (int i = 0; i < bigBatteries.Length; i++)
                {
                    if (bigBatteries[i].draw)
                    {
                        spriteBatch.Draw(bigBatteries[i].texture, new Rectangle((int)bigBatteries[i].position.X, (int)bigBatteries[i].position.Y, bigBatteries[i].texture.Width, bigBatteries[i].texture.Height), Color.White);
                    }
                }
                for (int j = 0; j < 100; j++)
                {
                    for (int i = 0; i < 26; i++)
                    {
                        walls[j][i].Draw(spriteBatch);
                    }
                }
                for (int i = 0; i < maxFloors; i++)
                {
                    floors[i].Draw(spriteBatch);
                }
                battery.Draw(spriteBatch);
                robot.Draw(spriteBatch);

#if !XBOX
                instructions.Draw(spriteBatch);
#elif XBOX

#endif

                if (splash == 1)
                {
                    spriteBatch.Draw(actor.texture, new Rectangle(100, 352, actor.texture.Width, actor.texture.Height), Color.White);
                    spriteBatch.Draw(splashScreen.texture, splashScreen.position, new Rectangle(0, 0, splashScreen.texture.Width, splashScreen.texture.Height), Color.White, 0f, new Vector2(0, 0), scaleSplash, SpriteEffects.None, 0);
                }
                else if (splash == 3)
                {
                    //spriteBatch.Draw(actor.texture, new Rectangle(100, 352, actor.texture.Width, actor.texture.Height), Color.White);
                    spriteBatch.Draw(splashScreen.texture, splashScreen.position, new Rectangle(0, 0, splashScreen.texture.Width, splashScreen.texture.Height), Color.White, 0f, new Vector2(0, 180), scaleSplash, SpriteEffects.None, 0);
                }

            }
            //Display Game Over / Death Screen
            else if (splash == 4)
            {
                spriteBatch.Draw(splashScreen.texture, splashScreen.position, new Rectangle(0, 0, splashScreen.texture.Width, splashScreen.texture.Height), Color.White, 0f, new Vector2(35, 380), scaleSplash, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
