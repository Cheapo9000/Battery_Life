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
    public class Battery
    {
        public float life;
        public Texture2D texture;
        public Texture2D barTexture;
        public bool lowBattery;

        public Battery(Texture2D Image, Texture2D ImageBar)
        {
            life = 100;
            texture = Image;
            barTexture = ImageBar;
        }

        public void Update() 
        {
            life -= 0.025f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(195, 0, texture.Width, texture.Height), Color.White);
            for (int i = 0; i < (int)life - 1; i++)
            {
                if (life < 15)
                {
                    spriteBatch.Draw(barTexture, new Rectangle(208 + 4 * i, 6, barTexture.Width, barTexture.Height), Color.Red);
                }
                else
                {
                    spriteBatch.Draw(barTexture, new Rectangle(208 + 4 * i, 6, barTexture.Width, barTexture.Height), Color.White);
                }
            }
        }
    }
}
