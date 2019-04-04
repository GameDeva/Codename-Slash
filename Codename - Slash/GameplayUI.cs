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
    public class GameplayUI : UI
    {
        private SpriteFont hudFont;
        private Texture2D templateSquare;

        Rectangle titleSafeArea;
        Vector2 hudLocation;
        Vector2 center;

        private Rectangle healthRectangle;
        private Rectangle dashRectangle;
        private Rectangle weaponIconRectangle;
        
        private Hero hero;

        // Weapon Display
        private Texture2D weaponIconUI;
        private int ammoRemaining;
        private int ammoInMag;
        private string weaponName;

        // 
        private int healthRemaining;
        private bool completeStageText;
        private int stageNumber;
        public int Score { get; private set; }

        public GameplayUI(ContentManager content) : base(content)
        {

        }

        public void Initialise(SaveData saveData, ref Hero hero)
        {
            this.hero = hero;
            healthRemaining = 100;
            
            titleSafeArea = new Rectangle(10, 32, 100, 100);
            hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
                                         titleSafeArea.Y + titleSafeArea.Height / 2.0f);
            healthRectangle = new Rectangle(titleSafeArea.X, titleSafeArea.Y + 32, 50, healthRemaining * 2);
            dashRectangle = new Rectangle(70, titleSafeArea.Y + 32, 20, 200);
            weaponIconRectangle = new Rectangle(150, titleSafeArea.Y + 32, 100, 40);


            stageNumber = saveData.stageNumber;

            hero.WeaponHandler.OnWeaponSwap += OnWeaponSwap;
            hero.OnDamage += OnTakeDamage;
            StageManager.Instance.OnCompleteStage += CompleteStage;
            StageManager.Instance.OnNewStage += BeginStage;
            EnemyDirector.Instance.ScoreIncrement += IncrementScore;
        }

        private void BeginStage(StageData stageData)
        {
            completeStageText = false;
            stageNumber = stageData.stageNumer;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void LoadContent()
        {
            // Load fonts
            hudFont = Content.Load<SpriteFont>("UI/Fonts/Hud");

            // Health bar
            templateSquare = Content.Load<Texture2D>("UI/white");

            // Set up cursor and textures
            CrosshairSetup();
        }


        // Load all cursor textures 
        // Specify currently used texture
        private void CrosshairSetup()
        {
            cursorTextureList.Add(Content.Load<Texture2D>("UI/Cursors/8crosshair"));
            cursorTextureList.Add(Content.Load<Texture2D>("UI/Cursors/8crosshair2"));
            currentCursorTexture = cursorTextureList[0];

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Begin();
            
            DrawFont(spriteBatch);

            base.Draw(spriteBatch);
            
            // spriteBatch.End();
        }



        private void DrawFont(SpriteBatch spriteBatch)
        {
            string text = "Geezer101";
            // string weaponName = "[WepName]";
            
            // spriteBatch.DrawString(hudFont, text, hudLocation, Color.White);
            spriteBatch.DrawString(hudFont, weaponName + " : " + ammoInMag + " / " + ammoRemaining, hudLocation + Vector2.UnitX * 120, Color.White);

            spriteBatch.DrawString(hudFont, "Stage: " + stageNumber.ToString(), new Vector2(Game1.SCREENWIDTH-200, hudLocation.Y), Color.White);
            spriteBatch.DrawString(hudFont, "Score: " + Score.ToString(), new Vector2(Game1.SCREENWIDTH - 200, hudLocation.Y+32), Color.White);

            //if(completeStageText)
            //    spriteBatch.DrawString(hudFont, "Next one ->", new Vector2(Game1.SCREENWIDTH - 200, hudLocation.Y + 32), Color.White);


            spriteBatch.Draw(templateSquare, healthRectangle, Color.Red);
            // spriteBatch.Draw(templateSquare, dashRectangle, Color.Blue);
            if(weaponIconUI != null)
            {
                spriteBatch.Draw(weaponIconUI, weaponIconRectangle, Color.Black);
            }

        }
        

        private void OnShoot()
        {
            ammoInMag--;
        }

        private void OnReload(int amountToRefill)
        {
            ammoInMag += amountToRefill;
            ammoRemaining -= amountToRefill;
        }
        
        private void OnWeaponSwap(ref Weapon oldWeapon, ref Weapon newWeapon)
        {
            if(oldWeapon != null)
            {
                // Unsubscibe old delegates 
                oldWeapon.OnShootAction -= OnShoot;
                oldWeapon.OnReload -= OnReload;
            }

            // Subscribe new delegates
            newWeapon.OnShootAction += OnShoot;
            newWeapon.OnReload += OnReload;

            // Update UI display values
            weaponIconUI = newWeapon.WeaponIconTexture;
            weaponName = newWeapon.GetType().ToString().Remove(0, 17);
            ammoRemaining = newWeapon.CurrentAmmoCarry;
            ammoInMag = newWeapon.CurrentMagHold;
        }

        private void OnTakeDamage(int damageVal)
        {
            healthRemaining -= damageVal;
            healthRectangle.Height = healthRemaining * 2;
        }

        private void CompleteStage()
        {
            completeStageText = true;
        }

        private void IncrementScore(int increment)
        {
            Score += increment;
        }
    }
}
