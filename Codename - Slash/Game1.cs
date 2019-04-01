using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Codename___Slash
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static int SCREENWIDTH = 1920;
        public static int SCREENHEIGHT = 1080;
        
        // Resources for drawing.
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private InputHandler inputHandler;
        private GameState state;

        private Song song;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this); // TODO: Maybe not needed here if this is handling in the state classes? 
            Content.RootDirectory = "Content";
            
            graphics.PreferredBackBufferWidth = SCREENWIDTH;
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;
            // graphics.IsFullScreen = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            inputHandler = new InputHandler();

            // Initalise all states
            GameState.MenuState.InitialiseState(this);
            GameState.GameplayState.InitialiseState(this);
            GameState.OptionsState.InitialiseState(this);

            // Set first state
            state = GameState.MenuState;
            state.Enter(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            

            //Known issue that you get exceptions if you use Media PLayer while connected to your PC
            //See http://social.msdn.microsoft.com/Forums/en/windowsphone7series/thread/c8a243d2-d360-46b1-96bd-62b1ef268c66
            //Which means its impossible to test this from VS.
            //So we have to catch the exception and throw it away
            try
            {
                // MediaPlayer.IsRepeating = true;
                song = Content.Load<Song>("Sound/Music/Everybody-Dies-Instrumental");
                // MediaPlayer.Play(song);
            }
            catch { }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            //Stop playing the music
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // IMPORTANT
            // Scene/Game State handling area
            GameState s = state.Update(this, ref gameTime, ref inputHandler);
            if (s != null)
            {
                state.Exit(this); // Call previous state's exit method 
                state = s;
                state.Enter(this); // Call new state's enter method
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            // spriteBatch.Begin();

            state.Draw(ref gameTime, spriteBatch);

            // spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
