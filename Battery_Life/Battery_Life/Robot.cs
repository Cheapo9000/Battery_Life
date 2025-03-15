using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Battery_Life
{
    public class Robot : Obj
    {
        bool onGround;
        //bool shieldActivated;
        //float batteryLife;
        Texture2D shield;
        public int streakCount = 0; //counts successful consecutive column breaks with shield
        public int currStreak;

        ///////////////SPRITE ANIMATION CODE (VARIABLES)////////////////////////////////////////////////////////

        //Texture2D myTexture;
        float timer = 100f;           //amount of time to pass before next frame
        float interval = 70f;      //how often to step to the next frame in the animation
        int currentFrame = 0;       //keep track of current frame
        int spriteWidth = 36;
        int spriteHeight = 67;
        //int spriteSpeed = 2;
        Rectangle sourceRect;       //rectangle where the sprite is drawn
        //Vector2 positionR;
        Vector2 origin;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        //public Robot(Texture2D Image, Texture2D Shield)
        //{
        //    draw = true;
        //    Collision = false;
        //    position = new Vector2(100, 352);
        //    texture = Image;
        //    speed = 1f;
        //    onGround = true;
        //    //shieldActivated = false;
        //    //batteryLife = 100f;
        //    shield = Shield;
        //}

        public Robot(Texture2D image, Texture2D Shield, int currentFrame, int spriteWidth, int spriteHeight)
        {
            draw = true;
            Collision = false;
            position = new Vector2(100, 352);
            texture = image;
            speed = 1f;
            onGround = true;
            //shieldActivated = false;
            //batteryLife = 100f;
            shield = Shield;
            //this.myTexture = image;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
        }

        //properties for getting and setting above variables
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        //constructor
        //public Robot(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
        //{
        //    this.myTexture = texture;
        //    this.currentFrame = currentFrame;
        //    this.spriteWidth = spriteWidth;
        //    this.spriteHeight = spriteHeight;
        //}

        public void Animate(GameTime gameTime)
        {
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
     
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame > 5) //end frame 5 ( total frames - 1 )
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }    
        }
        //////////////////END OF SPRITE ANIMATION CODE/////////////////////////////////////////////////

        public void Update(Obj[] obj, Battery battery, int multiplier)
        {
            int lifeIncrease = 5 * multiplier;
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i].alive)
                {
                    if (obj[i].position.X <= position.X)
                    {
                        obj[i].alive = false;
                        obj[i].draw = false;
                        //obj[i].position.X;
                        if (battery.life + lifeIncrease >= 100)
                        {
                            battery.life = 100;
                        }
                        else
                        {
                            battery.life += lifeIncrease;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ///////////////UPDATE FUNCTION OBJECTS BEING COMPARED//////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="obj"> Objects being compared.</param>
        /// <param name="breakFail"> Fail to break through wall using shield.</param>
        /// <returns>True: if successfully break through walls.</returns>
        public bool Update(Obj[] obj, bool breakFail)
        {
            bool wallBreak = false;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                color = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                color = 2;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                color = 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                color = 4;
            }
            else
            {
                color = 0;
            }

            int collideCount = 0; //keeps count of how many times objects collide
            for (int i = 0; i < obj.Length; i++)
            {
                if (!obj[i].isExploding())
                {
                    //Not shielding
                    if (color == 0)
                    {
                        if (obj[i].position.X <= position.X + (texture.Width / 6) && obj[i].position.X + obj[i].texture.Width / 2 >= position.X + (texture.Width / 6))
                        {
                            if ((obj[i].position.Y > position.Y && obj[i].position.Y < (position.Y + texture.Height)) || (position.Y > obj[i].position.Y && position.Y < obj[i].position.Y + obj[i].texture.Height))
                            {
                                if (!obj[i].isFloor())
                                {
                                    collideCount++;
                                    obj[i].explode();
                                    //Successfully break wall
                                    if (obj[i].color == color)
                                    {
                                        breakFail = false;
                                    }
                                    //Fail to break wall
                                    else
                                    {  
                                        streakCount = 0;
                                        breakFail = true;
                                    }
                                    //obj[i].position.Y -= 100f;
                                }
                            }

                            //if (obj[i].position.Y >= position.Y + texture.Height  && obj[i].position.Y - texture.Height >= position.Y)
                            //if ((position.Y >= obj[i].position.Y - texture.Height) && (position.Y < obj[i].position.Y + 800))
                            if (position.Y > 350)
                            {
                                onGround = true;
                            }
                            else
                            {
                                //if (obj[i].isFloor())
                                //{
                                //    onGround = false;
                                //}
                            }
                        }
                    }
                    //Shielding
                    else
                    {
                        if (obj[i].position.X <= position.X + (texture.Width / 6) + shield.Width && obj[i].position.X + obj[i].texture.Width / 2 >= position.X + (texture.Width / 6) + shield.Width)
                        {
                            if ((obj[i].position.Y > position.Y && obj[i].position.Y < (position.Y + texture.Height)) || (position.Y > obj[i].position.Y && position.Y < obj[i].position.Y + obj[i].texture.Height))
                            {
                                if (!obj[i].isFloor())
                                {
                                    collideCount++;
                                    obj[i].explode();
                                    if (obj[i].color == color)
                                    {
                                        wallBreak = true; //for streak count
                                        breakFail = false;
                                    }
                                    else
                                    {
                                        streakCount = 0;
                                        breakFail = true;
                                    }
                                    //obj[i].position.Y -= 100f;
                                }
                            }

                            //if (obj[i].position.Y >= position.Y + texture.Height  && obj[i].position.Y - texture.Height >= position.Y)
                            //if ((position.Y >= obj[i].position.Y - texture.Height) && (position.Y < obj[i].position.Y + 800))
                            if (position.Y > 350)
                            {
                                onGround = true;
                            }
                            else
                            {
                                //if (obj[i].isFloor())
                                //{
                                //    onGround = false;
                                //}
                            }
                        }
                    }
                    if (collideCount > 0 && !obj[0].isFloor())
                    {
                        Collision = true;
                    }
                    else if (!obj[0].isFloor())
                    {
                        Collision = false;
                    }
                }
            }

            if (!Collision)
            {
                //position.X += speed;
            }
            if (!onGround && obj[0].isFloor())
            {
                //position.Y += speed;
            }

            if (wallBreak == true)
            {
                currStreak = streakCount;
                streakCount++;
            }

            return breakFail;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (draw)
            {
                switch (color)
                {
                    case 0:
                        //spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.White);
                        spriteBatch.Draw(texture, position, sourceRect,
                         Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
                        break;
                    case 1:
                        spriteBatch.Draw(texture, position, sourceRect,
                         Color.Red, 0f, origin, 1.0f, SpriteEffects.None, 0);
                        spriteBatch.Draw(shield, new Rectangle((int)position.X + (texture.Width / 6), (int)position.Y, shield.Width, texture.Height), Color.Red);
                        break;
                    case 2:
                        spriteBatch.Draw(texture, position, sourceRect,
                         Color.Blue, 0f, origin, 1.0f, SpriteEffects.None, 0);
                        spriteBatch.Draw(shield, new Rectangle((int)position.X + (texture.Width / 6), (int)position.Y, shield.Width, texture.Height), Color.Blue);
                        break;
                    case 3:
                        spriteBatch.Draw(texture, position, sourceRect,
                         Color.Green, 0f, origin, 1.0f, SpriteEffects.None, 0);
                        spriteBatch.Draw(shield, new Rectangle((int)position.X + (texture.Width / 6), (int)position.Y, shield.Width, texture.Height), Color.Green);
                        break;
                    case 4:
                        spriteBatch.Draw(texture, position, sourceRect,
                         Color.Yellow, 0f, origin, 1.0f, SpriteEffects.None, 0);
                        spriteBatch.Draw(shield, new Rectangle((int)position.X + (texture.Width / 6), (int)position.Y, shield.Width, texture.Height), Color.Yellow);
                        break;
                }
            }
        }
    }
}
