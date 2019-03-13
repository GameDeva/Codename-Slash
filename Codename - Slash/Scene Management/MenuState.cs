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
    public class MenuState : GameState
    {
        public override void Enter(Game1 game)
        {
            base.Enter(game);
        }

        public override void Exit(Game1 game)
        {
            base.Exit(game);
        }

        public override void LoadContent(ContentManager content) { }


        public override GameState Update(Game1 game, ref GameTime gameTime, ref InputHandler inputHandler)
        {
            Console.WriteLine("In menu state");

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


            base.Draw(ref gameTime, spriteBatch);
        }
    }
}
