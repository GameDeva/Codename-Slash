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

        private MouseState mouseState;

        protected List<Texture2D> cursorTextureList;
        protected Texture2D currentCursorTexture;
        protected Vector2 cursorPos;


        public UI(ContentManager content)
        {
            cursorTextureList = new List<Texture2D>();

            Content = content;
        }

        public virtual void Update()
        {
            mouseState = Mouse.GetState();
            cursorPos = new Vector2(mouseState.X, mouseState.Y);
            
        }
        public virtual void LoadContent()
        {
            cursorTextureList.Add(Content.Load<Texture2D>("UI/Cursors/mouse_pointer"));
            currentCursorTexture = cursorTextureList[0];
        }

        public virtual void UnloadContent()
        {
            Content.Unload();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentCursorTexture, cursorPos, Color.White);
        }

    }
}
