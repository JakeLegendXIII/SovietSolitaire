using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Input;
using SovietSolitaire.Library;

namespace SovietSolitaire.Entities;

public class RulesButton : IGameEntity
{
    private const string Label = "Rules";

    private readonly Texture2D _texture;
    private readonly SpriteFont _font;
    private readonly Vector2 _textPosition;
    private bool _isHovered;
    private bool _isPressed;

    public Rectangle Bounds { get; }

    public RulesButton(Point position)
    {
        _texture = AssetManager.ButtonUI;
        _font = AssetManager.ArmadaFont;
        Bounds = new Rectangle(position.X, position.Y, _texture.Width, _texture.Height);
        _textPosition = Bounds.Center.ToVector2() - _font.MeasureString(Label) / 2;
    }

    public void Update(GameTime gameTime) { }

    public bool Update()
    {
        Point mousePosition = InputManager.GetTransformedMousePosition(0, 0).ToPoint();
        _isHovered = InputManager.IsMouseInViewport && Bounds.Contains(mousePosition);

        if (InputManager.IsLeftMouseButtonDown())
            _isPressed = _isHovered;

        if (!InputManager.IsLeftMouseButtonHeld())
        {
            bool clicked = _isPressed && _isHovered;
            _isPressed = false;
            return clicked;
        }

        return false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Color tint = _isHovered ? (_isPressed ? Color.Gray : Color.LightGray) : Color.White;
        spriteBatch.Draw(_texture, Bounds, tint);
        spriteBatch.DrawString(_font, Label, _textPosition, Color.White);
    }
}
