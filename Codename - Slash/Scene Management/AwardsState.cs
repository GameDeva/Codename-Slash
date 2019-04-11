﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Codename___Slash
{
    public class AwardsState : GameState
    {
        private List<UIElement> uIElements;
        private List<Button> buttons;
        private Button buttonOnHoover;
        private MouseState mouseState;

        private SpriteFont hudFont;
        private ContentManager content;

        private UI ui;

        public AwardsData AwardsData { get; private set; }

        public override void Enter(Game1 game)
        {
            content = game.Content;

            ui = new UI(game.Content);
            uIElements = new List<UIElement>();
            buttons = new List<Button>();
            
            base.Enter(game);
        }

        public override void Exit(Game1 game)
        {
            base.Exit(game);
        }

        protected override void LoadContent()
        {
            // Load fonts
            hudFont = content.Load<SpriteFont>("UI/Fonts/Hud");
            // TODO: Change all button mappings toa appropriate states
            // Load and add all the UI elements
            uIElements.Add(new UIElement(stateContent.Load<Texture2D>("UI/AwardsBig"), new Rectangle((Game1.SCREENWIDTH / 2) - (700 / 2), 100, 700, 199)));
            //uIElements.Add(new UIElement(stateContent.Load<Texture2D>("UI/1"), new Rectangle((Game1.SCREENWIDTH / 3) - (82 / 2), 350, 82, 85)));
            //uIElements.Add(new UIElement(stateContent.Load<Texture2D>("UI/2"), new Rectangle((Game1.SCREENWIDTH / 3) - (97 / 2), 450, 97, 85)));
            //uIElements.Add(new UIElement(stateContent.Load<Texture2D>("UI/3"), new Rectangle((Game1.SCREENWIDTH / 3) - (95 / 2), 550, 95, 88)));
            buttons.Add(new Button(stateContent.Load<Texture2D>("UI/Back"), new Rectangle((Game1.SCREENWIDTH / 2) - (250 / 2), 850, 250, 96), MenuState));
            
            ui.LoadContent();

            base.LoadContent();
        }

        protected override void InitialiseKeyBindings()
        {
            if (commandManager != null)
            {
                commandManager.AddMouseBinding(MouseButton.LEFT, OnSelect);
            }

            base.InitialiseKeyBindings();
        }

        private void OnSelect(eButtonState arg1, Vector2 arg2)
        {
            // Attach current button
            if (arg1 == eButtonState.DOWN && buttonOnHoover.onHoover)
            {
                buttonOnHoover.onHoover = true;
                buttonOnHoover.onPressDown = true;
            }

            // Button has been selected
            if (arg1 == eButtonState.UP && buttonOnHoover.onHoover)
            {
                buttonOnHoover.onPressDown = false;
                buttonOnHoover.onSelect = true;

            }
            else if (arg1 == eButtonState.UP && !buttonOnHoover.onHoover)
            {
                buttonOnHoover.onHoover = false;
            }
        }

        public override GameState Update(Game1 game, float deltaTime, ref InputHandler inputHandler)
        {

            ui.Update();

            // Get mouse point
            mouseState = Mouse.GetState();
            Point mousePosition = new Point(mouseState.X, mouseState.Y);

            buttonOnHoover.onHoover = false;
            // Check if hovering over any UIElement
            foreach (Button button in buttons)
            {
                // Or is mouse over button
                if (button.destRect.Contains(mousePosition))
                {
                    buttonOnHoover = button;
                    buttonOnHoover.onHoover = true;
                    break;
                }
            }


            commandManager.Update();
            // If button has been selected
            if (buttonOnHoover.onSelect)
            {
                buttonOnHoover.onHoover = false;
                buttonOnHoover.onPressDown = false;
                buttonOnHoover.onSelect = false;
                return buttonOnHoover.stateToReturn;
            }
            // Exmaple return statement:::: return GameState.optionState;

            base.Update(game, deltaTime, ref inputHandler);
            return null;
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw ui elements
            foreach (UIElement element in uIElements)
            {
                spriteBatch.Draw(element.texture, element.destRect, null, Color.White, 0.0f, new Vector2(1), SpriteEffects.None, 1.0f);
            }

            // Draw the hover rect
            if (buttonOnHoover.onHoover)
                Game1.DrawRect(spriteBatch, buttonOnHoover.destRect);

            // Draw buttons
            foreach (Button element in buttons)
            {
                spriteBatch.Draw(element.texture, element.destRect, null, Color.White, 0.0f, new Vector2(1), SpriteEffects.None, 1.0f);
            }

            if(AwardsData.scores.Count > 0)
            {
                for(int i = 0; i < 10 && i < AwardsData.scores.Count; i++)
                {
                    spriteBatch.DrawString(hudFont, AwardsData.scores[i].ToString(), new Vector2(Game1.SCREENWIDTH / 2 - 300, 400 + (i * 40)), Color.White);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(hudFont, string.Format("{0}. ", i+1), new Vector2(Game1.SCREENWIDTH / 2 - 350, 400 + (i * 40)), Color.White);
            }
            ui.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(deltaTime, spriteBatch);
        }

        public void UpdateAwardsFileWithNewScore(int newScore)
        {
            AwardsData.scores.Add(newScore);
            AwardsData.scores.Sort();

            Loader.ToXmlFile(AwardsData, "AwardsFile.xml");
        }

        public void LoadAwardsFile()
        {
            if (File.Exists("AwardsFile.xml"))
            {
                AwardsData a = new AwardsData();
                Loader.ReadXML("AwardsFile.xml", ref a);
                a.scores.Sort();
                AwardsData = a;
            }
            AwardsData = new AwardsData();
        }

    }
}
