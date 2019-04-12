using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Codename___Slash.UIRelated;

namespace Codename___Slash
{
    // Awards State of the game 
    public class AwardsState : GameState
    {
        private SpriteFont hudFont;

        private MenuUI menuUI; // State's menuUI  
        private GameManager gameManager; // Reference to Gamemanager Singleton

        // On entering state, 
        //  Create menuUI with current state's contentManager
        public override void Enter(Game1 game)
        {
            gameManager = GameManager.Instance;
            menuUI = new MenuUI(stateContentManager);
            
            base.Enter(game);
        }

        // On exiting the state
        public override void Exit(Game1 game)
        {
            base.Exit(game);
        }

        protected override void LoadContent()
        {
            // Load fonts
            hudFont = stateContentManager.Load<SpriteFont>("UI/Fonts/Hud");
            // Load and add all the UI elements, buttons
            menuUI.UIElements.Add(new UIElement(stateContentManager.Load<Texture2D>("UI/AwardsBig"), new Rectangle((Game1.SCREENWIDTH / 2) - (700 / 2), 100, 700, 199)));
            menuUI.Buttons.Add(new Button(stateContentManager.Load<Texture2D>("UI/Back"), new Rectangle((Game1.SCREENWIDTH / 2) - (250 / 2), 850, 250, 96), MenuState));
            
            menuUI.LoadContent();

            base.LoadContent();
        }

        // Initialise State's keybindings 
        protected override void InitialiseKeyBindings()
        {
            if (commandManager != null)
            {
                // Attach mouse left click to menuUI's on select, for interactive buttons
                commandManager.AddMouseBinding(MouseButton.LEFT, menuUI.OnSelect);
            }

            base.InitialiseKeyBindings();
        }

        // Update State
        public override GameState Update(Game1 game, float deltaTime, ref InputHandler inputHandler)
        {
            // Update Menu UI
            menuUI.Update();
            // Check which buttons on menuUI is hovered
            menuUI.ButtonsHoverCheck();
            // Update commandManager's input
            commandManager.Update();
            // Return appropriate GameState or null, based on if button selected
            return menuUI.ButtonsSelectCheck();
        }

        // Draw state 
        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            menuUI.Draw(spriteBatch);

            if (gameManager.AwardsData.scores.Count > 0)
            {
                for(int i = 0; i < 10 && i < gameManager.AwardsData.scores.Count; i++)
                {
                    spriteBatch.DrawString(hudFont, gameManager.AwardsData.scores[i].ToString(), new Vector2(Game1.SCREENWIDTH / 2 - 300, 400 + (i * 40)), Color.White);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(hudFont, string.Format("{0}. ", i+1), new Vector2(Game1.SCREENWIDTH / 2 - 350, 400 + (i * 40)), Color.White);
            }
            
            spriteBatch.End();
            base.Draw(deltaTime, spriteBatch);
        }
    }
}
