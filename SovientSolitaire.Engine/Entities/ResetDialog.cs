using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Engine.Input;
using SovietSolitaire.Engine.Library;
using System;

namespace SovietSolitaire.Engine.Entities;

public class ResetDialog : IGameEntity
{
	private const string Title = "Reset game?";
	private const string Message = "Start a new shuffled hand?";
	private const string ConfirmLabel = "Reset";
	private const string CancelLabel = "Cancel";
	private const int Width = 600;
	private const int Height = 240;
	private const int Padding = 28;
	private const int ButtonGap = 24;

	private readonly Texture2D _texture;
	private readonly Texture2D _buttonTexture;
	private readonly SpriteFont _font;
	private readonly Vector2 _titlePosition;
	private readonly Vector2 _messagePosition;
	private readonly Vector2 _confirmLabelPosition;
	private readonly Vector2 _cancelLabelPosition;
	private bool _isConfirmHovered;
	private bool _isCancelHovered;
	private bool _isConfirmPressed;
	private bool _isCancelPressed;

	public bool IsOpen { get; private set; }
	public Rectangle Bounds { get; }
	public Rectangle ConfirmButtonBounds { get; }
	public Rectangle CancelButtonBounds { get; }

	public ResetDialog(Point viewportSize)
	{
		_texture = AssetManager.ConfirmationBanner;
		_buttonTexture = AssetManager.ButtonUI;
		_font = AssetManager.FarawayFont;

		int width = Math.Min(Width, viewportSize.X - Padding * 2);
		int height = Math.Min(Height, viewportSize.Y - Padding * 2);
		Bounds = new Rectangle(
			(viewportSize.X - width) / 2,
			(viewportSize.Y - height) / 2,
			width,
			height);

		Vector2 titleSize = _font.MeasureString(Title);
		Vector2 messageSize = _font.MeasureString(Message);
		_titlePosition = new Vector2(Bounds.Center.X - titleSize.X / 2, Bounds.Top + Padding);
		_messagePosition = new Vector2(Bounds.Center.X - messageSize.X / 2,
			_titlePosition.Y + titleSize.Y + 20);

		int buttonsWidth = _buttonTexture.Width * 2 + ButtonGap;
		int buttonY = Bounds.Bottom - Padding - _buttonTexture.Height;
		int confirmX = Bounds.Center.X - buttonsWidth / 2;
		ConfirmButtonBounds = new Rectangle(
			confirmX, buttonY, _buttonTexture.Width, _buttonTexture.Height);
		CancelButtonBounds = new Rectangle(
			ConfirmButtonBounds.Right + ButtonGap, buttonY, _buttonTexture.Width, _buttonTexture.Height);

		_confirmLabelPosition = ConfirmButtonBounds.Center.ToVector2()
			- _font.MeasureString(ConfirmLabel) / 2;
		_cancelLabelPosition = CancelButtonBounds.Center.ToVector2()
			- _font.MeasureString(CancelLabel) / 2;
	}

	public void Show()
	{
		IsOpen = true;
		ResetInputState();
	}

	public void Close()
	{
		IsOpen = false;
		ResetInputState();
	}

	public bool Update()
	{
		if (!IsOpen)
			return false;

		Point mousePosition = InputManager.GetTransformedMousePosition(0, 0).ToPoint();
		_isConfirmHovered = InputManager.IsMouseInViewport && ConfirmButtonBounds.Contains(mousePosition);
		_isCancelHovered = InputManager.IsMouseInViewport && CancelButtonBounds.Contains(mousePosition);

		if (InputManager.IsLeftMouseButtonDown())
		{
			_isConfirmPressed = _isConfirmHovered;
			_isCancelPressed = _isCancelHovered;
		}

		if (InputManager.IsLeftMouseButtonHeld())
			return false;

		bool confirmed = _isConfirmPressed && _isConfirmHovered;
		bool cancelled = _isCancelPressed && _isCancelHovered;
		_isConfirmPressed = false;
		_isCancelPressed = false;

		if (confirmed || cancelled)
			Close();

		return confirmed;
	}

	public void Update(GameTime gameTime) { }

	public void Draw(SpriteBatch spriteBatch)
	{
		if (!IsOpen)
			return;

		spriteBatch.Draw(_texture, Bounds, Color.White);
		spriteBatch.DrawString(_font, Title, _titlePosition, Color.White);
		spriteBatch.DrawString(_font, Message, _messagePosition, Color.White);
		DrawButton(spriteBatch, ConfirmButtonBounds, ConfirmLabel, _confirmLabelPosition,
			_isConfirmHovered, _isConfirmPressed);
		DrawButton(spriteBatch, CancelButtonBounds, CancelLabel, _cancelLabelPosition,
			_isCancelHovered, _isCancelPressed);
	}

	private void DrawButton(SpriteBatch spriteBatch, Rectangle bounds, string label,
		Vector2 labelPosition, bool isHovered, bool isPressed)
	{
		Color tint = isHovered ? (isPressed ? Color.Gray : Color.LightGray) : Color.White;
		spriteBatch.Draw(_buttonTexture, bounds, tint);
		spriteBatch.DrawString(_font, label, labelPosition, Color.White);
	}

	private void ResetInputState()
	{
		_isConfirmHovered = false;
		_isCancelHovered = false;
		_isConfirmPressed = false;
		_isCancelPressed = false;
	}
}
