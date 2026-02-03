using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoTemplate;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private const int _preferredScreenWidth = 750, _preferredScreenHeight = 450;
    private const int _playAreaEdgeLineWidth = 12, _ballWidthAndHeight = 50;

    private float _ballSpeed = 10;
    private Vector2 _ballPosition,_ballDirection;

    private Texture2D _backgroundTexture, _ballTexture; 

    private Rectangle _playAreaBoundingBox;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _preferredScreenWidth;
        _graphics.PreferredBackBufferHeight = _preferredScreenHeight;

        _ballPosition.X = 150;
        _ballPosition.Y = 195;
        _ballSpeed = 200;

        _ballDirection.X = -1;
        _ballDirection.Y = -1;

        _playAreaBoundingBox = new(0, 0, _preferredScreenWidth, _preferredScreenHeight);
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _backgroundTexture = Content.Load<Texture2D>("Court");
        _ballTexture = Content.Load<Texture2D>("Ball");
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _ballPosition += _ballDirection * _ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Ball collision with left/right edges
        if (_ballPosition.X <= _playAreaBoundingBox.Left ||
            _ballPosition.X + _ballWidthAndHeight >= _playAreaBoundingBox.Right)
        {
            _ballDirection.X *= -1;
        }

        // Ball collision with top/bottom edges
        if (_ballPosition.Y <= _playAreaBoundingBox.Top ||
            _ballPosition.Y + _ballWidthAndHeight >= _playAreaBoundingBox.Bottom)
        {
            _ballDirection.Y *= -1;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(_backgroundTexture, _playAreaBoundingBox, Color.White);

        var ballRect = new Rectangle((int)_ballPosition.X, (int)_ballPosition.Y, _ballWidthAndHeight, _ballWidthAndHeight);

        _spriteBatch.Draw(_ballTexture, ballRect, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime); 
    }
}
