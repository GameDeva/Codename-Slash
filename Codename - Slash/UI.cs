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
    public class UI
    {
        public ContentManager Content { get; private set; }

        private List<Texture2D> cursorTextureList;
        private Texture2D currentCursorTexture;
        private Vector2 cursorPos;
        private MouseState mouseState;
        
        private SpriteFont hudFont;
        private Texture2D templateSquare;
        
        private Hero hero;

        // Weapon Display
        private Texture2D weaponIconUI;
        private int ammoRemaining;
        private int ammoInMag;


        public UI(ContentManager content)
        {
            cursorTextureList = new List<Texture2D>();

            this.Content = content;
        }

        public UI(ContentManager content, ref Hero hero) : this(content)
        {
            this.hero = hero;
            hero.WeaponHandler.OnWeaponSwap += OnWeaponSwap;
        }

        public void LoadContent(ContentManager content)
        {
            // Load fonts
            hudFont = content.Load<SpriteFont>("UI/Fonts/Hud");

            // Health bar
            templateSquare = content.Load<Texture2D>("UI/white");

            // Set up cursor and textures
            CursorSetup();
        }


        // Load all cursor textures 
        // Specify currently used texture
        private void CursorSetup()
        {
            cursorTextureList.Add(Content.Load<Texture2D>("UI/Cursors/8crosshair"));
            cursorTextureList.Add(Content.Load<Texture2D>("UI/Cursors/8crosshair2"));
            currentCursorTexture = cursorTextureList[0];

        }

        public void Update()
        {
            mouseState = Mouse.GetState();
            cursorPos = new Vector2(mouseState.X, mouseState.Y);
            // Console.WriteLine("x: "+ cursorPos.X + "Y: " + cursorPos.Y);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Begin();
            
            DrawFont(spriteBatch);

            spriteBatch.Draw(currentCursorTexture, cursorPos, Color.White);

            // spriteBatch.End();
        }



        private void DrawFont(SpriteBatch spriteBatch)
        {
            Rectangle titleSafeArea = new Rectangle(10, 0, 100, 100);
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
                                         titleSafeArea.Y + titleSafeArea.Height / 2.0f);

            string text = "Mani";
            string weaponName = "[WepName]";
            
            spriteBatch.DrawString(hudFont, text, hudLocation, Color.Black);
            spriteBatch.DrawString(hudFont, weaponName + " : " + ammoInMag + " / " + ammoRemaining, hudLocation + Vector2.UnitX * 120, Color.Black);


            Rectangle healthRectangle = new Rectangle((int)hudLocation.X, 40, 50, 200);
            Rectangle shieldRectangle = new Rectangle(70, 40, 20, 200);
            Rectangle weaponIconRectangle = new Rectangle(150, 50, 100, 40);


            spriteBatch.Draw(templateSquare, healthRectangle, Color.Red);
            spriteBatch.Draw(templateSquare, shieldRectangle, Color.Blue);
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
            ammoRemaining = newWeapon.CurrentAmmoCarry;
            ammoInMag = newWeapon.CurrentMagHold;
        }


    }
}
