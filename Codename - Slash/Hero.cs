﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace Codename___Slash
{
    public class Hero
    {
        public Vector2 Position { get { return position; } }
        public Vector2 Velocity { get { return velocity; } }

        private Vector2 position;
        private Vector2 velocity;

        private Vector2 prevPosition;

        Texture2D testTex;

        // Animations
        private Animation idle;
        private Animation diagonalUpRight;
        private Animation diagonalDownRight;
        private Animation up;
        private Animation sideRight;
        private Animation down;

        private Animator animator;
        private SpriteEffects flip = SpriteEffects.None;

        private Vector2 movement;
        private float maxMoveSpeed = 1000f;
        private float moveAcc = 5000f;
        private float friction = 0.85f;
        private float dashAcc;
        private bool shouldDash;

        public Hero(Vector2 position, ContentManager content)
        {
            animator = new Animator();

            LoadContent(content);
            Reset(position);

        }

        public void Reset(Vector2 position)
        {
            this.position = position;
            animator.AttachAnimation(idle);


        }

        public void Update(GameTime gameTime)
        {
            
            //// TODO: Get rid of this please. 
            //if (Keyboard.GetState().IsKeyDown(Keys.W))
            //{
            //    MoveUp();
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.S))
            //{
            //    MoveDown();
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.A))
            //{
            //    MoveLeft();
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.D))
            //{
            //    MoveRight();
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.Space))
            //{

            //}




            ApplyMovement(gameTime);
            AttachAnimation();

            // Reset movement



        }

        public void LoadContent(ContentManager content)
        {
            testTex = content.Load<Texture2D>("Sprites/Hero/2");
            idle = new Animation(content.Load<Texture2D>("Sprites/Hero/2"), 1, 0.1f, true); // Just one frame
            diagonalDownRight = new Animation(content.Load<Texture2D>("Sprites/Hero/2_diagdown"), 4, 0.1f, true); 
            diagonalUpRight = new Animation(content.Load<Texture2D>("Sprites/Hero/2_diagup"), 4, 0.1f, true); 
            up = new Animation(content.Load<Texture2D>("Sprites/Hero/2_north"), 4, 0.1f, true);  
            down = new Animation(content.Load<Texture2D>("Sprites/Hero/2_south2"), 4, 0.1f, true);
            sideRight = new Animation(content.Load<Texture2D>("Sprites/Hero/2_side"), 4, 0.1f, true);
        }
        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(testTex, new Vector2(0, 0), Color.White);





            // Flip the sprite to face the way we are moving.
            if (velocity.X < 0)
                flip = SpriteEffects.FlipHorizontally;
            else
            {
                flip = SpriteEffects.None;
            }

            // Draw that sprite.
            animator.Draw(gameTime, spriteBatch, Position, flip);
        }

        private void ResetMovement()
        {
            // Reset movement
            movement.X = 0.0f;
            movement.Y = 0.0f;
        }

        private void ApplyMovement(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            velocity += movement * moveAcc * elapsed;
            // if(movement == Vector2.Zero)
            velocity *= friction;

            if (!shouldDash)
            {
            } else
            {
                // Do the dash 
                
            }

            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(Velocity.X, -maxMoveSpeed, maxMoveSpeed);
            velocity.Y = MathHelper.Clamp(Velocity.Y, -maxMoveSpeed, maxMoveSpeed);

            prevPosition = position;
            position += Velocity * elapsed;
            // Console.WriteLine("Position X: " + position.X);
            position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
            movement = Vector2.Zero;
        }

        private void AttachAnimation()
        {
            // 
            if(prevPosition == position)
            {
                // Console.WriteLine("Playing Idle");
                animator.AttachAnimation(idle);
            } 
            // Both sideways movement
            else if (Math.Abs(velocity.X) > 0 && velocity.Y == 0)
            {
                Console.WriteLine("MoveRight");
                animator.AttachAnimation(sideRight);
            }
            // Upwards movement
            else if(velocity.Y > 0 && velocity.X == 0)
            {
                animator.AttachAnimation(up);
                Console.WriteLine("MoveUp");
            }
            // Downward movement
            else if (velocity.Y < 0 && velocity.X == 0)
            {
                animator.AttachAnimation(down);
                Console.WriteLine("MoveDown");
            }
            // Diagonal upward movement either left or right
            else if(velocity.Y > 0 && Math.Abs(velocity.X) > 0)
            {
                animator.AttachAnimation(diagonalUpRight);
            }
            // Diagonal downward movement either left or right
            else if (velocity.Y < 0 && Math.Abs(velocity.X) > 0)
            {
                animator.AttachAnimation(diagonalDownRight);
            }
        }

        #region Hero Commands

        public void MoveRight()
        {
            movement.X = 1.0f;
        }

        public void MoveLeft()
        {
            movement.X = -1.0f;
        }

        public void MoveDown()
        {
            movement.Y = 1.0f;
        }

        public void MoveUp()
        {
            movement.Y = -1.0f;
        }

        public void Dash()
        {
            shouldDash = true;
        }

        #endregion


    }
}
