using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using DB.Gui;
using DB.Gui.Boxes;
using DB.Audio;

namespace DB.Diver
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DiverGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GuiManager guiManager = new GuiManager();
        SpriteFont font;
        AudioDevice audioDevice;
        AudioMixer audioMixer;
        AudioClip audioClip;

        public DiverGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            audioDevice = new AudioDevice();
            audioMixer = new AudioMixer(audioDevice);
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

            Texture2D white = new Texture2D(GraphicsDevice, 4, 4, 1, TextureUsage.Tiled, SurfaceFormat.Color);
            white.SetData(new uint[] { 0xffffffff, 0xffffffff, 0xffffffff, 0xffffffff,
                                       0xffffffff, 0xffffffff, 0xffffffff, 0xffffffff,
                                       0xffffffff, 0xffffffff, 0xffffffff, 0xffffffff,
                                       0xffffffff, 0xffffffff, 0xffffffff, 0xffffffff });

            font = Content.Load<SpriteFont>("Font");

            guiManager.Top = new Container();
            guiManager.Top.Size = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            guiManager.Top.Font = font;

            BorderBox areaDefault = new BorderBox(white, true, Color.Gray, Color.Black, 2);
            BorderBox areaPressed = new BorderBox(white, true, Color.Black, Color.White, 2);
            BorderBox areaHover = new BorderBox(white, true, Color.LightGray, Color.Black, 2);

            Button b = new Button("Click me!", areaDefault, areaPressed, areaHover);
            b.X = 10; 
            b.Y = 10;
            guiManager.Top.Add(b);
            b.AutoResize();

            audioClip = new AudioClip(Content.RootDirectory + "/" + "clip.wav");

            b.Clicked += new Button.ClickedHandler(PlaySound);

            // TODO: use this.Content to load your game content here
        }

        void PlaySound(Button sender, int x, int y, MouseButton button)
        {
            audioClip.Play(1.0f, 0.5f, 0.2f);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            audioDevice.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            guiManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            guiManager.Draw(graphics.GraphicsDevice, gameTime);

            base.Draw(gameTime);
        }
    }
}
