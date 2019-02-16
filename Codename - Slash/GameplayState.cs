using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public class GameplayState : GameState
    {
        private Hero hero;
        private MapGenerator map;

        // Initialise the hero on the enter state 
        public override void Enter(Game1 game)
        {
            stateContent = new ContentManager(game.Services, "content/Gameplay"); // Should be based on level, maybe later object types or areas

            // Create the appropriate map TODO : change to level state logic 
            map = new MapGenerator(1, 2, game.Services);


            // TODO : Based on serialization, saved hero could have several bits of data already stored 
            hero = new Hero();
            base.Enter(game);



        }

        public override void Exit(Game1 game)
        {

            base.Exit(game);
        }

        public override GameState Update(Game1 game, ref InputHandler inputHandler)
        {
            // TODO: Add your update logic here
            Command command = inputHandler.HandleInput();
            if (command != null)
            {
                Console.WriteLine("Something has been pressed");
                command.execute(ref hero);
            }

            base.Update(game, ref inputHandler);
            return null;
        }
    }
}
