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

        public UI(ContentManager content)
        {
            cursorTextureList = new List<Texture2D>();

            this.content = content;
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
            spriteBatch.Draw(currentCursorTexture, cursorPos, Color.White);
            // spriteBatch.End();
        }





    }
}
