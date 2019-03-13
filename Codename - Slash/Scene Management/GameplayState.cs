using System;
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
        ObjectPool<Bullet> bulletPool;

        private Hero hero; // Hero instance for this gameplay session
        private Camera camera; // Camera instance for this gameplay session
        // private MapGenerator map;

        private UI ui; // UI instance for this gameplay session

        // Initialise the hero on the enter state 
        public override void Enter(Game1 game)
        {
            stateContent = new ContentManager(game.Services, "Content"); // Should be based on level, maybe later object types or areas
            
            // Create the appropriate map TODO : change to level state logic 
            // map = new MapGenerator(game.Services);
            
            // TODO : Based on serialization, saved hero could have several bits of data already stored i.e. weapons held, points scored 
            hero = new Hero(new Vector2(800, 500), stateContent);
            ui = new UI(stateContent, ref hero);
            
            camera = new Camera();

            bulletPool = new ObjectPool<Bullet>(10);
            
            base.Enter(game);
            
        }

        public override void LoadContent(ContentManager content) 
        {
            hero.LoadContent(stateContent);
            ui.LoadContent(stateContent);
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
            camera.Follow(hero);
            ui.Update();

            base.Update(game, ref gameTime, ref inputHandler);
            return null;
        }

        public override void Draw(ref GameTime gameTime, SpriteBatch spriteBatch)
        {
            // transformMatrix: camera.Transform [add as parameter]
            spriteBatch.Begin();

            hero.Draw(gameTime, spriteBatch);
            //spriteBatch.End();

            
            //spriteBatch.Begin();
            ui.Draw(spriteBatch);

            base.Draw(ref gameTime, spriteBatch);

            spriteBatch.End();
        }

        public void Dispose()
        {
            stateContent.Unload();
        }

    }
}
