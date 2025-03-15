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
    public class Obj
    {
        public bool alive;
        public bool draw;
        protected bool Collision;
        public Vector2 position;
        public Texture2D texture;
        public float speed;
        protected float increaseSpeed = 0.01f;
        protected bool floor;
        protected bool exploding;
        protected float explodeTimeElapsed;
        //Random rand;
        //red = 1, blue = 2, green = 3, yellow = 4, solid = 0
        public int color;

        protected Obj()
        {
            alive = true;
            draw = false;
            Collision = false;
            position = Vector2.Zero;
            texture = null;
            speed = 1.0f;
            floor = false;
            exploding = false;
            explodeTimeElapsed = 0.0f;
            //rand = new Random();
            color = 0;
        }

        public Obj(Texture2D Texture, Vector2 Pos, bool Floor, int objColor)
        {
            alive = true;
            draw = true;
            Collision = false;
            position = Pos;
            texture = Texture;
            speed = 1.0f;
            floor = Floor;
            exploding = false;
            explodeTimeElapsed = 0.0f;
            //rand = new Random((int)Pos.X);
            color = objColor;
        }

        public bool isFloor()
        {
            return floor;
        }

        public bool isColliding()
        {
            return Collision;
        }

        public void explode()
        {
            exploding = true;
        }

        public bool isExploding()
        {
            return exploding;
        }

        /// <summary>
        /// Updates the Obj class handling movement involving collisions and on collisions.
        /// </summary>
        /// <param name="robotColliding">True if the robot is colliding with a wall.</param>
        /// <param name="breakFail">Used to keep syncronized with other moving objects.</param>
        public void Update(bool robotColliding, bool breakFail, bool stop)
        {
            if (!stop)
            {
                if (!robotColliding || (!breakFail && robotColliding))
                {
                    position.X -= speed;
                    speed += increaseSpeed;
                }
                else
                {
                    speed = 1.0f;
                }
            }
            else
            {
                speed = 1.0f;
            }
        }

        /// <summary>
        /// Updates the Obj class handling movement involving collisions and on collisions.
        /// </summary>
        /// <param name="robotColliding">True if the robot is colliding with a wall.</param>
        /// <param name="collideSpeed">Speed of the robot.</param>
        /// <param name="collideColor">Collide color of the robot.</param>
        /// <param name="breakFail">Always given false to the walls[].update()</param>
        /// <returns>True if the player failed to break through a wall with a shield.</returns>
        public void Update(bool robotColliding, float collideSpeed, int collideColor, bool breakFail, Random rand, bool stop)
        {
            if (alive)
            {
                if (!stop)
                {
                    if (!robotColliding || (!breakFail && robotColliding))
                    {
                        position.X -= speed;
                        speed += increaseSpeed;
                    }
                    else
                    {
                        speed = 1.0f;
                    }
                    if (exploding)
                    {
                        if (explodeTimeElapsed >= 10f)
                        {
                            exploding = false;
                            explodeTimeElapsed = 0.0f;
                            alive = false;
                            draw = false;
                        }
                        else
                        {
                            explodeTimeElapsed += 0.1f;
                            position.X += 2 * collideSpeed * (1 + (float)rand.NextDouble());
                            if (rand.NextDouble() >= .5)
                            {
                                position.Y -= (1 + (float)rand.NextDouble());
                            }
                            else
                            {
                                position.Y += (1 + (float)rand.NextDouble());
                            }
                        }
                    }
                }
                else
                {
                    speed = 1.0f;
                }
            }
            //return breakFail;
        }
        
        //draw environment objects
        public void Draw(SpriteBatch spriteBatch)
        {
            if (draw)
            {
                switch (color)
                {
                    case 0:
                        spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.Red);
                        break;
                    case 2:
                        spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.Blue);
                        break;
                    case 3:
                        spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.Green);
                        break;
                    case 4:
                        spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), Color.Yellow);
                        break;
                }
            }
        }
    }
}
