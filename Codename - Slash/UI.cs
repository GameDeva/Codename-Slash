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
        public ContentManager Content { get; }

        private List<Texture2D> cursorTextureList;
        private Texture2D currentCursorTexture;
        private Vector2 cursorPos;
        private MouseState mouseState;

        private ContentManager content;

        private SpriteFont hudFont;
        private Texture2D templateSquare;


        public UI(ContentManager content)
        {
            cursorTextureList = new List<Texture2D>();

            this.content = content;

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
            cursorTextureList.Add(content.Load<Texture2D>("UI/Cursors/8crosshair"));
            cursorTextureList.Add(content.Load<Texture2D>("UI/Cursors/8crosshair2"));
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
            //spriteBatch.Begin();
            
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
            spriteBatch.DrawString(hudFont, weaponName + " : " + , hudLocation + Vector2.UnitX * 120, Color.Black);


            Rectangle healthRectangle = new Rectangle((int)hudLocation.X, 40, 50, 200);
            Rectangle shieldRectangle = new Rectangle(70, 40, 20, 200);

            spriteBatch.Draw(templateSquare, healthRectangle, Color.Red);
            spriteBatch.Draw(templateSquare, shieldRectangle, Color.Blue);
            
        }
        

    }
}
