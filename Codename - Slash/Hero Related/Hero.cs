using System;
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
    public class Hero : IDamageable
    {
        // Hero stats
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public Vector2 Position { get { return position; } }
        public Vector2 Velocity { get { return velocity; } }

        private Vector2 position;
        private Vector2 velocity;
        private Vector2 prevVelocity;

        private Vector2 prevPosition;
        
        // Animations
        private Animation idle;
        private Animation diagonalUpRight;
        private Animation diagonalDownRight;
        private Animation up;
        private Animation sideRight;
        private Animation down;
        
        public Animator Animator { get; private set; }
        private SpriteEffects heroSpriteEffects = SpriteEffects.None;

        // Player Spawned sprites
        public WeaponHandler WeaponHandler { get; private set; } 

        private Vector2 movement;
        private float maxMoveSpeed = 300f;
        private float moveAcc = 6000f;
        private float friction = 0.8f;
        private float dashAcc;
        private bool shouldDash;

        public Action OnDamage;
        public Action OnDeath;

        public Hero(Vector2 position, ContentManager content)
        {
            Animator = new Animator();
            WeaponHandler = new WeaponHandler();

            prevVelocity = new Vector2(0, 0);
            velocity = new Vector2(0, 0);

        }

        public void Reset(Vector2 position)
        {
            this.position = position;
            Animator.AttachAnimation(idle);


        }

        public void Update(GameTime gameTime)
        {
            WeaponHandler.Update(position, gameTime);
            
            ApplyMovement(gameTime);
            AttachAnimation();
            ResetMovement();
            

        }

        public void LoadContent(ContentManager content)
        {
            // 
            WeaponHandler.LoadContent(content);

            idle = new Animation(content.Load<Texture2D>("Sprites/Hero/2"), 1, 0.1f, true); // Just one frame
            diagonalDownRight = new Animation(content.Load<Texture2D>("Sprites/Hero/2_diagdown"), 4, 0.1f, true); 
            diagonalUpRight = new Animation(content.Load<Texture2D>("Sprites/Hero/2_diagup"), 4, 0.1f, true); 
            up = new Animation(content.Load<Texture2D>("Sprites/Hero/2_north"), 4, 0.1f, true);  
            down = new Animation(content.Load<Texture2D>("Sprites/Hero/2_south2"), 4, 0.1f, true);
            sideRight = new Animation(content.Load<Texture2D>("Sprites/Hero/2_side"), 4, 0.1f, true);

            Reset(position);


        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way we are moving.
            if (velocity.X < 0)
                heroSpriteEffects = SpriteEffects.FlipHorizontally;
            else
            {
                heroSpriteEffects = SpriteEffects.None;
            }

            // Draw that sprite.
            Animator.Draw(gameTime, spriteBatch, Position, heroSpriteEffects);

            // Draw Weapon Related things
            WeaponHandler.Draw(spriteBatch, position);
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
            // movement.Normalize();
            

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
            position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            // prevVelocity = velocity;
        }

        private void AttachAnimation()
        {
            // When player comes to a stop
            if(prevPosition == position)
            {
                Animator.AttachAnimation(idle);
            } 
            // Both sideways movement
            else if (movement.X != 0 && movement.Y == 0)
            {
                Animator.AttachAnimation(sideRight);
            }
            // Upwards movement
            else if(movement.Y < 0 && movement.X == 0)
            {
                Animator.AttachAnimation(up);
            }
            // Downward movement
            else if (movement.Y > 0 && movement.X == 0)
            {
                Animator.AttachAnimation(down);
            }
            // Diagonal upward movement either left or right
            else if (movement.Y < 0 && movement.X > 0)
            {
                Animator.AttachAnimation(diagonalUpRight);
            }
            // Diagonal downward movement either left or right
            else if (movement.Y > 0 && movement.X > 0)
            {
                Animator.AttachAnimation(diagonalDownRight);
            }
        }





        #region Hero Commands

        public void MoveRight(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.PRESSED)
            {
                movement.X = 1.0f;
            }
        }

        public void MoveLeft(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.PRESSED)
            {
                movement.X = -1.0f;
            }
        }

        public void MoveDown(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.PRESSED)
            {
                movement.Y = 1.0f;
            }
        }

        public void MoveUp(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                movement.Y = -1.0f;
            }
        }

        public void Dash(eButtonState buttonState, Vector2 amount)
        {
            shouldDash = true;
        }

        public void ShootWeapon(eButtonState buttonState, Vector2 amount)
        {
            if(buttonState == eButtonState.DOWN)//  || buttonState == eButtonState.PRESSED
                if (WeaponHandler != null)
                WeaponHandler.ShootEquippedWeapon();
        }

        public void PreviousWeapon(eButtonState buttonState, Vector2 amount)
        {
            WeaponHandler.PreviousWeapon();
        }

        public void NextWeapon(eButtonState buttonState, Vector2 amount)
        {
            WeaponHandler.NextWeapon();
        }

        #endregion
        
        public void TakeDamage(int damagePoints)
        {
            CurrentHealth -= damagePoints;
            OnDamage?.Invoke();

            // Health below 0, then player is dead
            // if()    

        }

        public void TakeDamage(int damagePoints, Vector2 direction) 
        {
            // Apply effect in direction of hit

            TakeDamage(damagePoints);
        }
        
    }
}
