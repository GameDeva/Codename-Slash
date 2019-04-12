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
    // Struct to represent a texture to be displayed on screen as a UI element
    public struct UIElement
    {
        public Texture2D texture; // Texture to display
        public Rectangle destRect; // Destination rectangle to be displayed at

        // Constructor to initialise and assign values
        public UIElement(Texture2D texture, Rectangle destRect)
        {
            this.texture = texture;
            this.destRect = destRect;
        }
    }
    
    // Struct to represent a UI button on screen
    public struct Button
    {
        public Texture2D texture; // Texture to represent button display
        public Rectangle destRect; // Destination rectangle to be displayed at
        public bool onHoover; // should be set true if mouse hoovers destRect
        public bool onPressDown; // should be set true if mouse click on destRect
        public bool onSelect; // should be set true if mouse click and release over destRect
        public GameState stateToReturn; // GameState that will be transitioned to on click
        public Action SelectMethod; // Delegate method to be called on click, default set to null 

        // Constructor to initialise and assign values, with SelectMethod action set to null by default 
        public Button(Texture2D texture, Rectangle destRect, GameState stateToReturn, Action SelectMethod = null)
        {
            this.texture = texture;
            this.destRect = destRect;
            onHoover = false;
            onPressDown = false;
            onSelect = false;
            this.stateToReturn = stateToReturn;
            this.SelectMethod = SelectMethod;
        }
    }
    
    // UI base class 
    public class UI
    {
        // Reference to a contentManager
        public ContentManager Content { get; private set; }

        private MouseState mouseState; // Mouse state of current session

        protected List<Texture2D> cursorTextureList; // List of cursor textures to be used
        // Current cursor texture and position
        protected Texture2D currentCursorTexture;
        protected Vector2 cursorPos;

        // 
        public UI(ContentManager content)
        {
            cursorTextureList = new List<Texture2D>();
            Content = content;
        }

        // Update mouseState and set current cursor position
        public virtual void Update()
        {
            mouseState = Mouse.GetState();
            cursorPos = new Vector2(mouseState.X, mouseState.Y);
        }

        // Loads all cursor textures to be used in scene
        public virtual void LoadContent()
        {
            cursorTextureList.Add(Content.Load<Texture2D>("UI/Cursors/mouse_pointer"));
            ChangeCursorTexture(0);
        }

        // Updates cursor texture based on index
        public void ChangeCursorTexture(int index)
        {
            currentCursorTexture = cursorTextureList[index];
        }

        // Draws current Cursor texture at current cursor position
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentCursorTexture, cursorPos, Color.White);
        }
    }



}
