using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash.EnemyStates
{
    public class ShootState : NPCState
    {
        private Texture2D bulletTexture;
        private Vector2 bulletColliderSize;
        private float bulletSpeed;
        private float timeBetweenShots;
        private bool startWithAttack;

        public bool InAttack { get; private set; }
        private float currentTimerToAttack;
        private Vector2 currentAttackPos;

        private Vector2 initialPosition;

        public ShootState(string name, float bulletSpeed, float timeBetweenAttacks, bool startWithAttack, Texture2D bulletTexture)
        {
            Name = name;
            this.bulletSpeed = bulletSpeed;
            this.timeBetweenShots = timeBetweenAttacks;
            this.startWithAttack = startWithAttack;
            this.bulletTexture = bulletTexture;
            bulletColliderSize = new Vector2(60);
        }

        public override void Enter(Enemy owner)
        {
            owner.DrawColor = Color.Red;
            owner.Animator.AttachAnimation(owner.EnemyAnimations.IdleAnimation);

            // 
            InAttack = startWithAttack;
            currentTimerToAttack = 0.0f;
            initialPosition = owner.Position;
            currentAttackPos = EnemyDirector.Instance.GetHeroPosition();
        }

        public override void Execute(Enemy owner, float deltaTime)
        {
            if (!InAttack)
            {
                // Timer for between attacks
                currentTimerToAttack += deltaTime;
                if (currentTimerToAttack >= timeBetweenShots)
                {
                    currentAttackPos = EnemyDirector.Instance.GetHeroPosition();
                    InAttack = true;
                    currentTimerToAttack = 0.0f;
                }

            }
            else
            {
                // Shoot
                PoolManager.Instance.SpawnBullet(new ArgsBullet(true, new Vector2(0.5f), owner.Position, Vector2.Normalize(EnemyDirector.Instance.GetHeroPosition() - owner.Position), bulletTexture, 5, bulletSpeed, bulletColliderSize, 10));
                // Reset timer
                InAttack = false;
            }
        }

        public override void Exit(Enemy owner)
        {

        }
    }
}
