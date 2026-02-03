using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoTemplate;

// Main game class inheriting from MonoGame's Game class
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics; // Manages graphics device settings
    private SpriteBatch _spriteBatch;        // Used to draw textures

    // Screen and object size constants
    private const int _preferredScreenWidth = 750, _preferredScreenHeight = 450;
    private const int _playAreaEdgeLineWidth = 12, _ballWidthAndHeight = 50;

    private float _ballSpeed = 10;           // Ball movement speed
    private Vector2 _ballPosition,_ballDirection; // Ball position and movement direction

    private Texture2D _backgroundTexture, _ballTexture; // Textures for background and ball

    private Rectangle _playAreaBoundingBox;  // Play area boundaries

    // Constructor: sets up graphics and content directory
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    // Initializes game state and sets up initial positions and directions
    protected override void Initialize()
    {
        // Set window size
        _graphics.PreferredBackBufferWidth = _preferredScreenWidth;
        _graphics.PreferredBackBufferHeight = _preferredScreenHeight;

        // Set initial ball position and speed
        _ballPosition.X = 150;
        _ballPosition.Y = 195;
        _ballSpeed = 200;

        // Set initial ball direction (up and left)
        _ballDirection.X = -1;
        _ballDirection.Y = -1;

        // Define play area boundaries
        _playAreaBoundingBox = new(0, 0, _preferredScreenWidth, _preferredScreenHeight);

        // Apply graphics changes
        _graphics.ApplyChanges();
        base.Initialize();
    }

    // Loads textures for background and ball
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _backgroundTexture = Content.Load<Texture2D>("Court");
        _ballTexture = Content.Load<Texture2D>("Ball");
    }

    // Updates game logic (called every frame)
    protected override void Update(GameTime gameTime)
    {
        // Exit game if Back button or Escape key is pressed
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Move ball based on direction and speed
        _ballPosition += _ballDirection * _ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Ball collision with left/right edges: reverse X direction
        if (_ballPosition.X <= _playAreaBoundingBox.Left ||
            _ballPosition.X + _ballWidthAndHeight >= _playAreaBoundingBox.Right)
        {
            _ballDirection.X *= -1;
        }

        // Ball collision with top/bottom edges: reverse Y direction
        if (_ballPosition.Y <= _playAreaBoundingBox.Top ||
            _ballPosition.Y + _ballWidthAndHeight >= _playAreaBoundingBox.Bottom)
        {
            _ballDirection.Y *= -1;
        }

        base.Update(gameTime);
    }

    // Draws everything to the screen (called every frame)
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue); // Clear screen with background color

        _spriteBatch.Begin(); // Start drawing

        // Draw background court
        _spriteBatch.Draw(_backgroundTexture, _playAreaBoundingBox, Color.White);

        // Create rectangle for ball's position and size
        var ballRect = new Rectangle((int)_ballPosition.X, (int)_ballPosition.Y, _ballWidthAndHeight, _ballWidthAndHeight);

        // Draw ball
        _spriteBatch.Draw(_ballTexture, ballRect, Color.White);

        _spriteBatch.End(); // End drawing

        base.Draw(gameTime); 
    }
}
