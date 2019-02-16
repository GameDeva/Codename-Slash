using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace Misc_Helpers
{
    public class UI
    {
        public ContentManager Content { get; }

        private List<Texture2D> cursorTextureList;
        private Texture2D currentCursorTexture;
        private Vector2 cursorPos;
        private MouseCursor mouse;

        private ContentManager content;

        public UI(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "content/UI");

            // Set up cursor and textures
            CursorSetup();


        }

        public void Update()
        {
            // cursorPos = new Vector2(mouseState.X, mouseState.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(currentCursorTexture, cursorPos, Color.White);
            spriteBatch.End();
        }



        // Load all cursor textures 
        // Specify currently used texture
        private void CursorSetup()
        {
            cursorTextureList.Add(content.Load<Texture2D>("Cursors/8crosshair"));
            currentCursorTexture = cursorTextureList[0];
        }


    }
}
