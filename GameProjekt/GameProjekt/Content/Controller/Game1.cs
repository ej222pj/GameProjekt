﻿using GameProjekt.Content.Model;
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
            QuitPause,
        }
        GameState CurrentGameState = GameState.MainMenu;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Button btnPlay;
        Button btnQuit;

        Map map;
        Player player;
        DragLine dragLine;
        Camera camera;
        float closestDistance = float.MaxValue;
        private int tileSize = 32;
        Vector2 closestTile;
        float distance;
        bool paused = false;
        string mapFilePath = "./Content/Maps/Map1.txt";

        Texture2D dragTexture;

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
            btnPlay = new Button(Content.Load<Texture2D>("Tiles/Tile2"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2));

            btnQuit = new Button(Content.Load<Texture2D>("Tiles/Tile2"), graphics.GraphicsDevice);
            btnQuit.setPosition(new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + 300));

            dragTexture = Content.Load<Texture2D>("Tiles/pixel");

            // TODO: use this.Content to load your game content here
            Tiles.Content = Content;

            camera = new Camera(GraphicsDevice.Viewport);

            map.Generate(tileSize, mapFilePath);

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
                        if (btnPlay.isClicked == true)
                        {
                            CurrentGameState = GameState.Playing;
                        }
                        btnPlay.Update(mouse);
                        break;

                    case GameState.Playing:
                        btnPlay.isClicked = false;
                        player.Update(gameTime, closestTile);
                        dragLine.Update(player.Position);
                        foreach (CollisionTiles tile in map.CollisionTiles)
                        {
                            player.Collision(tile.Rectangle);
                        }

                        foreach (BorderTiles tile in map.BorderTiles)
                        {
                            player.BorderCollision(tile.Rectangle);
                            camera.Update(player.Position, map.Width, map.Height);
                        }
                        break;

                    case GameState.QuitPause:
                        if (btnQuit.isClicked == true)
                        {
                            Exit();
                        }
                        if (btnPlay.isClicked == true)
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
            GraphicsDevice.Clear(Color.Gray);
            // TODO: Add your drawing code here
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Tiles/Tile1"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    spriteBatch.End();
                    break;

                case GameState.Playing:
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
                    map.Draw(spriteBatch);
                    player.Draw(spriteBatch);

                    if (dragLine.IsConnected) 
                    {
                        dragLine.DrawLine(spriteBatch, dragTexture, closestTile);
                    }
                    else if (player.ShootLine)
                    {
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
                            }
                        }// closest.Value now contains the closest vector to the player
                        dragLine.DrawLine(spriteBatch, dragTexture, closest.Value);
                    }
                    spriteBatch.End();
                    break;

                case GameState.QuitPause:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("Tiles/Player"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnQuit.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }
            base.Draw(gameTime);
        }
    }
}             