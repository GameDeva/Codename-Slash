using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace Codename___Slash.UIRelated
{
    // UI used during the GameplayState as HUD for the player
    public class GameplayUI : UI
    {
        // Font to write with
        private SpriteFont hudFont;
        // Blank texture, can be resized and drawn with differnet colors, for health, energy etc
        private Texture2D templateSquare;

        // Locations of the HUD 
        Rectangle titleSafeArea;
        Vector2 hudLocation;
        Vector2 center;

        // Different UI components
        private Rectangle healthRectangle;
        private Rectangle dashRectangle;
        private Rectangle weaponIconRectangle;

        // Weapon Display
        private Texture2D weaponIconUI;
        private int ammoRemaining;
        private int ammoInMag;
        private string weaponName;

        // 
        private int healthRemaining;
        
        // Constructor that passes the given ContentManager to the base UI class
        public GameplayUI(ContentManager content) : base(content)
        {

        }

        // Initialise
        public void Initialise(Hero hero)
        {
            // Store hero's starting health
            healthRemaining = (int)hero.MaxHealth;
            
            // Setup UI rectangles
            titleSafeArea = new Rectangle(10, 32, 100, 100);
            hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
                                         titleSafeArea.Y + titleSafeArea.Height / 2.0f);
            healthRectangle = new Rectangle(titleSafeArea.X, titleSafeArea.Y + 36, 50, healthRemaining * 2);
            dashRectangle = new Rectangle(70, titleSafeArea.Y + 32, 20, 200);
            weaponIconRectangle = new Rectangle(150, titleSafeArea.Y + 32, 100, 40);

            // Attach relevant events
            hero.WeaponHandler.OnWeaponSwap += OnWeaponSwap;
            hero.OnDamage += OnTakeDamage;

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
            string text = "Hero";
            
            spriteBatch.DrawString(hudFont, text, hudLocation, Color.White);
            spriteBatch.DrawString(hudFont, weaponName + " : " + ammoInMag + " / " + ammoRemaining, hudLocation + Vector2.UnitX * 120, Color.White);

            spriteBatch.DrawString(hudFont, "Stage: " + GameManager.Instance.CurrentStage.ToString(), new Vector2(Game1.SCREENWIDTH-200, hudLocation.Y), Color.White);
            spriteBatch.DrawString(hudFont, "Score: " + GameManager.Instance.CurrentScore.ToString(), new Vector2(Game1.SCREENWIDTH - 200, hudLocation.Y+32), Color.White);

            
            spriteBatch.Draw(templateSquare, healthRectangle, Color.Red);
            // spriteBatch.Draw(templateSquare, dashRectangle, Color.Blue);
            if(weaponIconUI != null)
            {
                spriteBatch.Draw(weaponIconUI, weaponIconRectangle, Color.Black);
            }

        }
        
        // Reduces ammo in mag display
        private void OnShoot()
        {
            ammoInMag--;
        }

        // Updates mag and ammo remaining display
        private void OnReload(int amountToRefill)
        {
            ammoInMag += amountToRefill;
            ammoRemaining -= amountToRefill;
        }
        
        // Swap the events, by unsubscribing from old weapon and subscribing to the new
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

        // Updates hero damage value and size of square
        private void OnTakeDamage(int damageVal)
        {
            healthRemaining -= damageVal;
            healthRectangle.Height = healthRemaining * 2;
        }
    }
}
