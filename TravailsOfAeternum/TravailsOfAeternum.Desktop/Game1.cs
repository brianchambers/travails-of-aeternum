using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace TravailsOfAeternum.Desktop
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        private OrthographicCamera _camera;
        private Vector2 _cameraPosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            
            _graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            BoxingViewportAdapter viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 600);
            _camera = new OrthographicCamera(viewportAdapter);

            _cameraPosition = _camera.Origin;
            _camera.LookAt(_cameraPosition);
            _camera.ZoomIn(2.0f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _tiledMap = Content.Load<TiledMap>("map-overworld");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _tiledMapRenderer.Update(gameTime);

            MoveCamera(gameTime);
            _camera.LookAt(_cameraPosition);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            BlendState previousBlendState = GraphicsDevice.BlendState;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            _tiledMapRenderer.Draw(_camera.GetViewMatrix());

            GraphicsDevice.BlendState = previousBlendState;

            base.Draw(gameTime);
        }

        private Vector2 GetMovementDirection()
        {
            Vector2 movementDirection = Vector2.Zero;
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Down))
            {
                movementDirection += Vector2.UnitY;
            }

            if (state.IsKeyDown(Keys.Up))
            {
                movementDirection -= Vector2.UnitY;
            }

            if (state.IsKeyDown(Keys.Left))
            {
                movementDirection -= Vector2.UnitX;
            }

            if (state.IsKeyDown(Keys.Right))
            {
                movementDirection += Vector2.UnitX;
            }

            // Normalize to prevent faster diagonal movement
            if (movementDirection != Vector2.Zero)
            {
                movementDirection.Normalize();
            }

            return movementDirection;
        }

        private void MoveCamera(GameTime gameTime)
        {
            float speed = 200f;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movementDirection = GetMovementDirection();

            _cameraPosition += speed * movementDirection * deltaTime;
            _cameraPosition.X = MathHelper.Clamp(_cameraPosition.X, 0, _tiledMap.WidthInPixels - _camera.BoundingRectangle.Width);
            _cameraPosition.Y = MathHelper.Clamp(_cameraPosition.Y, 0, _tiledMap.HeightInPixels - _camera.BoundingRectangle.Height);
            _cameraPosition = new Vector2((float)System.Math.Round(_cameraPosition.X), (float)System.Math.Round(_cameraPosition.Y));
        }
    }
}
