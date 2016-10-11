using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace sarahmarinoff_assignment7
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //game objects
        GameObject maze;
        Player player;
        Enemy[] enemies;
        GameObject startDoor;
        GameObject endDoor;

        //Camera 
        Vector3 cameraPosition = new Vector3(-475.0f, 600.0f, -476.0f);
        Vector3 cameraLookAt = new Vector3(-475.0f, 0.0f, -475.0f);
        Matrix cameraProjectionMatrix;
        Matrix cameraViewMatrix;

        bool endGame = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Model maze_model = Content.Load<Model>("collision");
            maze = new GameObject(new Vector3(0, 0, 0), maze_model);

            Model player_model = Content.Load<Model>("player");
            player = new Player(new Vector3(-475, 0, -475), player_model);
            player.setSpeed(4.0f);

            Model door = Content.Load<Model>("door");
            startDoor = new GameObject(new Vector3(-450, 0, -295), door);
            endDoor = new GameObject(new Vector3(450, 0, 295), door);

            enemies = new Enemy[3];

            enemies[0] = new Enemy(new Vector3(225, 0, 225), player_model); //patrol door
            enemies[0].setState(enemyState.Patrol);
            enemies[0].setSpeed(0.03f);
            enemies[0].initNodeSet(27, 34, 33);

            enemies[1] = new Enemy(new Vector3(-75, 0, -75), player_model); //wander
            enemies[1].setState(enemyState.Wander);
            enemies[1].setSpeed(0.03f);
            enemies[1].initNodeSet(19, 19, 18);

            enemies[2] = new Enemy(new Vector3(75, 0, 450), player_model); //chase/idle
            enemies[2].setState(enemyState.Idle);
            enemies[2].setSpeed(0.03f);

            enemies[0].initNodes();

            UpdateCamera();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {

#if !XBOX
            KeyboardState keyboardState = Keyboard.GetState();

            //end program
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (!endGame) //do game only if you haven't lost the game
            {
                Vector3 movement = new Vector3(0, 0, 0);

                //move player
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    movement += -1 * player.zSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    movement += player.zSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    movement += player.xSpeed;
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    movement += -1 * player.xSpeed;
                }

                //check player collisions
                bool collision = player.checkStaticCollisions(player.getPosition() + movement);

                //check door collisions
                if (startDoor.getVisible())
                {
                    if ((-325 <= player.getPosition().Z + movement.Z) && (-275 > player.getPosition().Z + movement.Z) && (-525 <= player.getPosition().X + movement.X) && (-375 > player.getPosition().X + movement.X))
                        collision = true;

                    if( (player.getPosition().Z > -350) && (keyboardState.IsKeyDown(Keys.E)) )
                        startDoor.setVisible(false);
                }
                else if(endDoor.getVisible())
                {
                    if ((275 <= player.getPosition().Z + movement.Z) && (325 > player.getPosition().Z + movement.Z) && (375 <= player.getPosition().X + movement.X) && (525 > player.getPosition().X + movement.X))
                        collision = true;

                    if ((player.getPosition().Z > 250) && (player.getPosition().X > 350) && (keyboardState.IsKeyDown(Keys.E)))
                        endDoor.setVisible(false);
                }

                if(!collision)
                {
                    player.updatePosition(movement);
                    UpdateCamera();
                }

                //check if enemy collision
                bool death = false;

                foreach (Enemy enemy in enemies)
                {
                    if ((Math.Abs(player.getPosition().X - enemy.getPosition().X) < 15) && (Math.Abs(player.getPosition().Z - enemy.getPosition().Z) < 15))
                    {
                        death = true;
                        break;
                    }
                }

                if (death)
                {
                    player.setPosition(new Vector3(-475, 0, -475));
                    UpdateCamera();
                }

            }

#endif

            foreach (Enemy enemy in enemies)
            {
                enemy.update(player.getPosition());
            }

            base.Update(gameTime);
        }

        protected void UpdateCamera()
        {
            cameraLookAt = player.getPosition();
            cameraPosition = new Vector3(cameraLookAt.X, cameraPosition.Y, cameraLookAt.Z + 1);

            cameraViewMatrix = Matrix.CreateLookAt(
                cameraPosition,
                cameraLookAt,
                Vector3.Up);

            cameraProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                (float)graphics.GraphicsDevice.Viewport.Width /
                (float)graphics.GraphicsDevice.Viewport.Height,
                1.0f,
                10000.0f);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            maze.Draw(cameraProjectionMatrix, cameraViewMatrix);
            player.Draw(cameraProjectionMatrix, cameraViewMatrix);
            startDoor.Draw(cameraProjectionMatrix, cameraViewMatrix);
            endDoor.Draw(cameraProjectionMatrix, cameraViewMatrix);

            for (int i = 0; i < 3; i++)
                enemies[i].Draw(cameraProjectionMatrix, cameraViewMatrix);

            base.Draw(gameTime);
        }
    }
}
