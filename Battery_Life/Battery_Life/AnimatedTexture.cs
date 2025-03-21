#region File Description
//-----------------------------------------------------------------------------
// AnimatedTexture.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprite
{
    public class AnimatedTexture
    {
        Texture2D myTexture;
        float timer = 100f;           //amount of time to pass before next frame
        float interval = 70f;      //how often to step to the next frame in the animation
        int currentFrame = 0;       //keep track of current frame
        int spriteWidth = 45;              
        int spriteHeight = 64;
        int spriteSpeed = 2;
        Rectangle sourceRect;       //rectangle where the sprite is drawn
        Vector2 position;           
        Vector2 origin;

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
            get { return myTexture; }
            set { myTexture = value; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        //constructor
        public AnimatedTexture(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
        {
            this.myTexture = texture;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
        }

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
    }

}
