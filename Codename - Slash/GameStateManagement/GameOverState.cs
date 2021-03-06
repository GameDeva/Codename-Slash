﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Codename___Slash.UIRelated;

namespace Codename___Slash.GameStateManagement
{
    // Game Over State of the game 
    public class GameOverState : GameState
    {
        private SpriteFont hudFont;
        private int finalScore;
        public bool win;

        public Action<int> UpdateScores;

        private MenuUI menuUI; // State's menuUI  
        private GameManager gameManager; // Reference to Gamemanager Singleton

        // On entering state, 
        //  Create menuUI with current state's contentManager
        public override void Enter(Game1 game)
        {
            gameManager = GameManager.Instance;
            menuUI = new MenuUI(stateContentManager);

            finalScore = gameManager.CurrentScore;
            // UpdateScores?.Invoke(finalScore);
            
            gameManager.UpdateAwardsFileWithNewScore(finalScore);
            gameManager.DeleteSaveFile(); // Deletes old save

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
            // Load and add all the UI elements
            // Only add if lost
            if (!win)
                menuUI.UIElements.Add(new UIElement(stateContentManager.Load<Texture2D>("UI/GameOver"), new Rectangle((Game1.SCREENWIDTH / 2) - (968 / 2), 100, 968, 198)));
            menuUI.Buttons.Add(new Button(stateContentManager.Load<Texture2D>("UI/Retry"), new Rectangle((Game1.SCREENWIDTH / 2) - (287 /2), 650, 287, 90), GameplayState, gameManager.OnNewGame));
            menuUI.Buttons.Add(new Button(stateContentManager.Load<Texture2D>("UI/BackToMain"), new Rectangle((Game1.SCREENWIDTH / 2) - (424 / 2), 850, 424, 67), MenuState));

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

        }

        // Update State
        public override GameState Update(Game1 game, float deltaTime)
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

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(hudFont, "You Win!", new Vector2(Game1.SCREENWIDTH / 2 - 100, (Game1.SCREENHEIGHT / 2) - 400), Color.White);

            spriteBatch.DrawString(hudFont, "Final Score: " + finalScore.ToString(), new Vector2(Game1.SCREENWIDTH/2 - 100, Game1.SCREENHEIGHT / 2), Color.White);

            menuUI.Draw(spriteBatch);

            spriteBatch.End();
        }

    }
}
