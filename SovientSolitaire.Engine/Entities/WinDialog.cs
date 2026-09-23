using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Engine.Input;
using SovietSolitaire.Engine.Library;

namespace SovietSolitaire.Engine.Entities;

public class WinDialog : IGameEntity
{
	private const string Title = "You won!";
	private const string ResetLabel = "Reset";
	private const int Width = 600;
	private const int Height = 200;
	private const int Padding = 28;

	private readonly Texture2D _texture;
	private readonly Texture2D _buttonTexture;
	private readonly SpriteFont _font;
	private readonly Vector2 _titlePosition;
	private readonly Vector2 _resetLabelPosition;
	private bool _isHovered;
	private bool _isPressed;

	public bool IsOpen { get; private set; }
	public Rectangle Bounds { get; }
	public Rectangle ResetButtonBounds { get; }

	public WinDialog(Point viewportSize)
	{
		_texture = AssetManager.ConfirmationBanner;
		_buttonTexture = AssetManager.ButtonUI;
		_font = AssetManager.FarawayFont;

		Bounds = new Rectangle((viewportSize.X - Width) / 2, (viewportSize.Y - Height) / 2, Width, Height);
		_titlePosition = new Vector2(Bounds.Center.X - _font.MeasureString(Title).X / 2, Bounds.Top + Padding);
		ResetButtonBounds = new Rectangle(Bounds.Center.X - _buttonTexture.Width / 2,
			Bounds.Bottom - Padding - _buttonTexture.Height, _buttonTexture.Width, _buttonTexture.Height);
		_resetLabelPosition = ResetButtonBounds.Center.ToVector2() - _font.MeasureString(ResetLabel) / 2;
	}

	public void Show()
	{
		IsOpen = true;
		_isHovered = false;
		_isPressed = false;
	}

	public bool Update()
	{
		if (!IsOpen)
			return false;

		Point mousePosition = InputManager.GetTransformedMousePosition(0, 0).ToPoint();
		_isHovered = InputManager.IsMouseInViewport && ResetButtonBounds.Contains(mousePosition);

		if (InputManager.IsLeftMouseButtonDown())
			_isPressed = _isHovered;

		if (!InputManager.IsLeftMouseButtonHeld())
		{
			bool clicked = _isPressed && _isHovered;
			_isPressed = false;
			if (clicked)
				IsOpen = false;
			return clicked;
		}

		return false;
	}

	public void Update(GameTime gameTime) { }

	public void Draw(SpriteBatch spriteBatch)
	{
		if (!IsOpen)
			return;

		spriteBatch.Draw(_texture, Bounds, Color.White);
		spriteBatch.DrawString(_font, Title, _titlePosition, Color.White);
		Color tint = _isHovered ? (_isPressed ? Color.Gray : Color.LightGray) : Color.White;
		spriteBatch.Draw(_buttonTexture, ResetButtonBounds, tint);
		spriteBatch.DrawString(_font, ResetLabel, _resetLabelPosition, Color.White);
	}
}
