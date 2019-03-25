using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace Codename___Slash
{
    public struct UIElement
    {
        public Texture2D texture;
        public Rectangle destRect;


        public UIElement(Texture2D texture, Rectangle destRect)
        {
            this.texture = texture;
            this.destRect = destRect;
        }

    }


    public class MenuState : GameState
    {
        UIElement title;
        UIElement spaceToBegin;
        UIElement highscores;
        UIElement quit;

        public override void Enter(Game1 game)
        {
            base.Enter(game);
        }

        public override void Exit(Game1 game)
        {
            base.Exit(game);
        }

        protected override void LoadContent()
        {
            title = new UIElement(stateContent.Load<Texture2D>("UI/Title"), new Rectangle((Game1.SCREENWIDTH / 2) - (700 / 2), 100, 700, 199));
            spaceToBegin = new UIElement(stateContent.Load<Texture2D>("UI/SpaceToBegin"), new Rectangle((Game1.SCREENWIDTH / 2) - (1227 / 2), 450, 1227, 133));
            highscores = new UIElement(stateContent.Load<Texture2D>("UI/Highscores"), new Rectangle((Game1.SCREENWIDTH / 2) - (747 / 2), 650, 747, 107));
            quit = new UIElement(stateContent.Load<Texture2D>("UI/Quit"), new Rectangle((Game1.SCREENWIDTH / 2) - (291 / 2), 800, 291, 119));


            base.LoadContent();
        }

        public override GameState Update(Game1 game, ref GameTime gameTime, ref InputHandler inputHandler)
        {
            // Console.WriteLine("In menu state");

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                return GameplayState;

            // Exmaple return statement:::: return GameState.optionState;

            base.Update(game, ref gameTime, ref inputHandler);
            return null;
        }

        public override void Draw(ref GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(title.texture, title.destRect, null, Color.White, 0.0f, new Vector2(1), SpriteEffects.None, 1.0f);
            spriteBatch.Draw(spaceToBegin.texture, spaceToBegin.destRect, null, Color.White, 0.0f, new Vector2(1), SpriteEffects.None, 1.0f);
            spriteBatch.Draw(highscores.texture, highscores.destRect, null, Color.White, 0.0f, new Vector2(1), SpriteEffects.None, 1.0f);
            spriteBatch.Draw(quit.texture, quit.destRect, null, Color.White, 0.0f, new Vector2(1), SpriteEffects.None, 1.0f);
            spriteBatch.End();
            base.Draw(ref gameTime, spriteBatch);
        }
        

    }
}
