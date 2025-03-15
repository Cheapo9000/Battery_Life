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
    public class DisplayFont
    {
        private SpriteFont font;
        private string[] text;
        private Vector2 position;
        private Color color;
        private float life;
        private Texture2D[] textures;

        /// <summary>
        /// Constructor for the DisplayFont class on Windows.
        /// </summary>
        /// <param name="FontType">The font.</param>
        /// <param name="displayText">The text that will be displayed.</param>
        /// <param name="FontPosition">The position of the text.</param>
        /// <param name="fontColor">The color of the font.</param>
        /// <param name="lifetime">The time that the font will be displayed for. Negative values mean infinite.</param>
        public DisplayFont(SpriteFont FontType, string[] displayText, Vector2 FontPosition, Color fontColor, float lifetime)
        {
            font = FontType;
            text = new string[1];
            text[0] = displayText[0];
            position = FontPosition;
            color = fontColor;
            life = lifetime;
            textures = null;
        }

        /// <summary>
        /// Constructor for the DisplayFont class on Xbox.
        /// </summary>
        /// <param name="FontType">The font.</param>
        /// <param name="displayText">The text that will be displayed.</param>
        /// <param name="FontPosition">The position of the text.</param>
        /// <param name="fontColor">The color of the font.</param>
        /// <param name="lifetime">The time that the font will be displayed for. Negative values mean infinite.</param>
        /// <param name="images">The images for the buttons on an Xbox controller.</param>
        public DisplayFont(SpriteFont FontType, string[] displayText, Vector2 FontPosition, Color fontColor, float lifetime, Texture2D[] images)
        {
            font = FontType;
            text = new string[displayText.Length];
            for (int i = 0; i < displayText.Length; i++)
            {
                text[i] = displayText[i];
            }
            position = FontPosition;
            color = fontColor;
            life = lifetime;
            textures = new Texture2D[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                textures[i] = images[i];
            }
        }

        /// <summary>
        /// Reset the life to the given value.
        /// </summary>
        /// <param name="newLife">The new life of the text. Negative values mean infinite.</param>
        public void resetLife(float newLife)
        {
            life = newLife;
        }

        /// <summary>
        /// Decrement life
        /// </summary>
        public void Update()
        {
            if (life > 0)
            {
                life -= 0.1f;
            }
        }

        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if ((int)life < 0 || (int)life > 0)
            {
                if (textures == null)
                {
                    spriteBatch.DrawString(font, text[0].ToString(), position, color);
                }
                else
                {
                    for (int i = 0; i < textures.Length; i++)
                    {
                        spriteBatch.Draw(textures[i], new Rectangle((int)position.X + 100 * i, (int)position.Y, textures[i].Width, textures[i].Height), Color.White);
                    }
                }
            }
        }
    }
}
