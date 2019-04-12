using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace Codename___Slash.GameStateManagement
{
    public class WalkwayState : GameState
    {

        private bool loadingComplete;
        
        public override void Enter(Game1 game)
        {
            loadingComplete = false;


        }

        public override void Exit(Game1 game)
        {
            base.Exit(game);
        }

        public override GameState Update(Game1 game, float deltaTime, ref InputHandler inputHandler)
        {
            return base.Update(game, deltaTime, ref inputHandler);
        }


        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            base.Draw(deltaTime, spriteBatch);
        }


        protected override void InitialiseKeyBindings()
        {
            base.InitialiseKeyBindings();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }


        private void LoadNextLevel()
        {



        }


    }
}
