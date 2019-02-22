﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    public class GameplayState : GameState
    {
        private Hero hero;
        private MapGenerator map;

        private UI ui;

        // Initialise the hero on the enter state 
        public override void Enter(Game1 game)
        {
            stateContent = new ContentManager(game.Services, "Content"); // Should be based on level, maybe later object types or areas
            
            ui = new UI(stateContent);
            

            // Create the appropriate map TODO : change to level state logic 
            // map = new MapGenerator(game.Services);
            
            // TODO : Based on serialization, saved hero could have several bits of data already stored i.e. weapons held, points scored 
            hero = new Hero(new Vector2(0, 0), stateContent);

            base.Enter(game);
            
        }
        
        public override void Exit(Game1 game)
        {
            Dispose();
            base.Exit(game);
        }
        
        public override GameState Update(Game1 game, ref GameTime gameTime, ref InputHandler inputHandler)
        {
            // TODO: Add your update logic here
            List<Command> commands = inputHandler.HandleInput();

            if (commands != null)
            {
                foreach (Command c in commands)
                {
                    c.execute(ref hero);
                }
            }
            
            // Handle State object Updates
            hero.Update(gameTime);
            ui.Update();

            base.Update(game, ref gameTime, ref inputHandler);
            return null;
        }

        public override void Draw(ref GameTime gameTime, SpriteBatch spriteBatch)
        {
            hero.Draw(gameTime, spriteBatch);
            ui.Draw(spriteBatch);

            base.Draw(ref gameTime, spriteBatch);
        }

        public void Dispose()
        {
            stateContent.Unload();
        }

    }
}
