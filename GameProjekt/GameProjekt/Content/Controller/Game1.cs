using GameProjekt.Content.Model;
using GameProjekt.Content.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameProjekt.Content.Controller
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        enum GameState
        {
            MainMenu,
            Options,
            Playing,
            WinLevelScreen,
            QuitPause,
        }
        GameState CurrentGameState = GameState.MainMenu;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Button btnPlay;
        Button btnQuit;
        Button btnNextLevel;
        Button btnMainMenu;
        int i = 0;
        Map map;
        Player player;
        DragLine dragLine;
        Camera camera;
        float closestDistance = float.MaxValue;
        private int tileSize = 32;
        Vector2 closestTile;
        Vector2 connectedTile;
        float distance;
        bool changeLevel = false;

        Texture2D dragTexture;
        Texture2D background;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 768;
            graphics.PreferredBackBufferHeight = 668;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
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
            map = new Map();
            dragLine = new DragLine();
            player = new Player(tileSize, dragLine);

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
            btnPlay = new Button(Content.Load<Texture2D>("Tiles/start"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 1.5f));

            btnQuit = new Button(Content.Load<Texture2D>("Tiles/quit"), graphics.GraphicsDevice);
            btnQuit.setPosition(new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 1.1f));

            btnNextLevel = new Button(Content.Load<Texture2D>("Tiles/nextlevel"), graphics.GraphicsDevice);
            btnNextLevel.setPosition(new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 1.5f));

            btnMainMenu = new Button(Content.Load<Texture2D>("Tiles/mainmenu"), graphics.GraphicsDevice);
            btnMainMenu.setPosition(new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 1.1f));

            dragTexture = Content.Load<Texture2D>("Tiles/pixel");
            background = Content.Load<Texture2D>("Tiles/Background");

            // TODO: use this.Content to load your game content here
            Tiles.Content = Content;

            camera = new Camera(GraphicsDevice.Viewport);

            //int level = player.GetSelectLevel().GetHashCode();
            //string mapFilePath = "./Content/Maps/Map{0}.txt";
            //string filePath = string.Format(mapFilePath, level + 1);
            //map.Generate(tileSize, filePath);

            player.Load(Content, map);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void generateMap() 
        {
            string mapFilePath = "./Content/Maps/Map{0}.txt";
            string filePath = string.Format(mapFilePath, player.GetSelectLevel());
            map.Generate(tileSize, filePath); 
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || Keyboard.GetState().IsKeyDown(Keys.Q))
                CurrentGameState = GameState.QuitPause;

            MouseState mouse = Mouse.GetState();

            // TODO: Add your update logic here
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlay.isClicked == true || Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        CurrentGameState = GameState.Playing;
                    }
                    btnPlay.Update(mouse);
                    changeLevel = true;
                    break;

                case GameState.WinLevelScreen:
                    if (btnNextLevel.isClicked == true || Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        CurrentGameState = GameState.Playing;
                    }
                    btnNextLevel.Update(mouse);
                    btnMainMenu.Update(mouse);
                    changeLevel = true;
                    break;

                case GameState.Playing:
                    if (changeLevel || player.HitTopOfMap)
                    {
                        generateMap();
                        changeLevel = false;
                        player.HitTopOfMap = false;
                        player.ResetGame();
                    }
                    
                    if (dragLine.IsConnected)
                    {
                        player.IsConnected = true;
                    }
                    btnPlay.isClicked = false;
                    player.Update(gameTime, closestTile, connectedTile);
                    dragLine.Update(player.Position);
                    foreach (CollisionTiles tile in map.CollisionTiles)
                    {
                        player.Collision(tile.Rectangle);
                    }

                    foreach (BorderTiles tile in map.BorderTiles)
                    {
                        
                        player.BorderCollision(tile.Rectangle);
                        if (player.HitTopOfMap)
                        {
                            player.ResetGame();
                            CurrentGameState = GameState.WinLevelScreen;
                            break;
                        }
                        camera.Update(player.Position, map.Width, map.Height);
                    }
                    
                    break;

                case GameState.QuitPause:
                    if (btnQuit.isClicked == true)
                    {
                        Exit();
                    }
                    if (btnPlay.isClicked == true || Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        CurrentGameState = GameState.Playing;
                    }
                    btnPlay.Update(mouse);
                    btnQuit.Update(mouse);
                    break;
            }         
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Tiles/Tile1"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    spriteBatch.End();
                    break;

                case GameState.WinLevelScreen:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Tiles/LevelComplete"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    btnNextLevel.Draw(spriteBatch);
                    btnMainMenu.Draw(spriteBatch);
                    spriteBatch.End();
                    break;

                case GameState.Playing:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
                    map.Draw(spriteBatch);
                    player.Draw(spriteBatch);

                    
                    if (dragLine.IsConnected) 
                    {
                        dragLine.DrawLine(spriteBatch, dragTexture, connectedTile);
                    }
                    //else if (player.ShootLine)
                    //{
                        Vector2? closest = null;
                        //Denna koden kommer ifrån: http://stackoverflow.com/questions/6920238/xna-find-nearest-vector-from-player skriven av User: Cameron 
                        foreach (CollisionTiles position in map.CollisionTiles)
                        {
                            Vector2 tilePosision = new Vector2(position.Rectangle.X + (tileSize / 2), position.Rectangle.Y + (tileSize / 2));
                            //distance = Vector2.DistanceSquared(tilePosision, player.Position);
                            distance = (float)Math.Sqrt(Math.Pow(player.Position.X - tilePosision.X, 2) + Math.Pow(player.Position.Y - tilePosision.Y, 2));
                            if (!closest.HasValue || distance < closestDistance)
                            {
                                
                                closest = tilePosision;
                                closestDistance = distance;
                                closestTile = closest.Value;
                                if (player.ShootLine && !dragLine.IsConnected)
                                {
                                    connectedTile = closest.Value;
                                }
                            }
                        }// closest.Value now contains the closest vector to the player
                        if (player.ShootLine && !dragLine.IsConnected)
                        {
                            dragLine.DrawLine(spriteBatch, dragTexture, closest.Value);
                        }
                        //dragLine.DrawLine(spriteBatch, dragTexture, closest.Value);
                    //}
                    spriteBatch.End();
                    break;

                case GameState.QuitPause:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Tiles/pausescreen"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnQuit.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }
            base.Draw(gameTime);
        }
    }
}             