using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace Codename___Slash.GameStateManagement
{
    public abstract class GameState 
    {
        // List of states
        // Stored as static auto properties, since the game will only have one game state
        // This saves memory from having to instaniate new states every time
        public static MainMenuState MenuState { get; } = new MainMenuState();
        public static AwardsState AwardsState { get; } = new AwardsState();
        public static ProtocolState ProtocolState { get; } = new ProtocolState();
        public static GameplayState GameplayState { get; } = new GameplayState();
        public static GameOverState GameOverState { get; } = new GameOverState();
        // public static NextStageState NextStageState { get; } = new NextStageState();

        protected CommandManager commandManager;
        protected IServiceProvider services;
        protected ContentManager stateContentManager; // Content manager for each state

        public virtual void InitialiseState(Game1 game)
        {
            // Create new content manager for each state
            stateContentManager = new ContentManager(game.Services, "Content"); 
        }

        // Enter method to be called on entry into particular state
        public virtual void Enter(Game1 game)
        {
            // Load each state's content
            LoadContent();
            // Create new commandManager
            commandManager = new CommandManager();
            // Initalise the keybindings
            InitialiseKeyBindings();
        }

        protected abstract void InitialiseKeyBindings();

        // Exit method to be called on exit out of statey
        public virtual void Exit(Game1 game)
        {
            UnloadContent();
        }

        protected virtual void LoadContent() { }

        protected virtual void UnloadContent()
        {
            // Unloads all content from state
            stateContentManager.Unload();
        }

        // Update method for each scene
        public virtual GameState Update(Game1 game, float deltaTime) { return null; }

        public abstract void Draw(float deltaTime, SpriteBatch spriteBatch);
    }
}
