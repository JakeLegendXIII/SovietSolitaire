using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Input;
using SovietSolitaire.Library;
using System;
using System.Text;

namespace SovietSolitaire.Entities;

internal class RulesDialog : IGameEntity
{
    private const string Title = "How to play";
    private const string CloseLabel = "OK";
    private const int Padding = 28;
    private const int Gap = 16;
    private const string RulesText =
        "Goal: Complete four alternating-color 10-to-6 stacks and four same-suit royal/Ace sets. The holding slot and one of the nine play slots must be empty.\n\n" +
        "Deal: The 36 playable cards are shuffled into nine stacks of four.\n\n" +
        "Moving cards: Drag a bottom card, or an exposed card with every card below it. A moved run must already follow the building rules. Any card or valid run may enter an empty slot.\n\n" +
        "Numbered cards: Build down by one, alternating red and black: 10, 9, 8, 7, 6.\n\n" +
        "Royals and Ace: Ace, Jack, Queen and King may be stacked in any order, but must share a suit. Never mix them with numbered cards.\n\n" +
        "Holding slot: The top slot holds one card only, not a run. Once released there, the card must follow normal placement rules when moved back out.\n\n" +
        "Completed sets: A slot with only the four same-suit royals/Ace turns into four blanks and locks. Numbered stacks stay movable until you win.\n\n" +
        "Invalid moves return to their source. Reset starts a newly shuffled hand. Winning stops card moves; Reset lets you play again.";

    private readonly Texture2D _texture;
    private readonly Texture2D _buttonTexture;
    private readonly SpriteFont _font;
    private readonly string _bodyText;
    private readonly float _bodyScale;
    private readonly Vector2 _titlePosition;
    private readonly Vector2 _bodyPosition;
    private readonly Vector2 _closeLabelPosition;
    private bool _isHovered;
    private bool _isPressed;

    public bool IsOpen { get; private set; }
    public Rectangle Bounds { get; }
    public Rectangle CloseButtonBounds { get; }

    public RulesDialog(Point viewportSize)
    {
        _texture = AssetManager.ConfirmationBanner;
        _buttonTexture = AssetManager.ButtonUI;
        _font = AssetManager.FarawayFont;

        int width = Math.Min(960, viewportSize.X - 80);
        _bodyText = WrapText(RulesText, width - Padding * 2);
        Vector2 titleSize = _font.MeasureString(Title);
        Vector2 bodySize = _font.MeasureString(_bodyText);
        float availableBodyHeight = viewportSize.Y - 80 - Padding * 2 - Gap * 2
            - titleSize.Y - _buttonTexture.Height;
        _bodyScale = Math.Min(1f, availableBodyHeight / bodySize.Y);
        int height = (int)Math.Ceiling(bodySize.Y * _bodyScale + titleSize.Y)
            + Padding * 2 + Gap * 2 + _buttonTexture.Height;

        Bounds = new Rectangle((viewportSize.X - width) / 2, (viewportSize.Y - height) / 2, width, height);
        _titlePosition = new Vector2(MathF.Floor(Bounds.Center.X - titleSize.X / 2), Bounds.Top + Padding);
        _bodyPosition = new Vector2(Bounds.Left + Padding, _titlePosition.Y + titleSize.Y + Gap);
        CloseButtonBounds = new Rectangle(Bounds.Center.X - _buttonTexture.Width / 2,
            Bounds.Bottom - Padding - _buttonTexture.Height, _buttonTexture.Width, _buttonTexture.Height);
        _closeLabelPosition = CloseButtonBounds.Center.ToVector2() - _font.MeasureString(CloseLabel) / 2;
    }

    public void Show()
    {
        IsOpen = true;
        _isHovered = false;
        _isPressed = false;
    }

    public void Close()
    {
        IsOpen = false;
        _isHovered = false;
        _isPressed = false;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsOpen)
            return;

        Point mousePosition = InputManager.GetTransformedMousePosition(0, 0).ToPoint();
        _isHovered = InputManager.IsMouseInViewport && CloseButtonBounds.Contains(mousePosition);

        if (InputManager.IsLeftMouseButtonDown())
            _isPressed = _isHovered;

        if (!InputManager.IsLeftMouseButtonHeld())
        {
            bool clicked = _isPressed && _isHovered;
            _isPressed = false;
            if (clicked)
                Close();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsOpen)
            return;

        spriteBatch.Draw(_texture, Bounds, Color.White);
        spriteBatch.DrawString(_font, Title, _titlePosition, Color.White);
        spriteBatch.DrawString(_font, _bodyText, _bodyPosition, Color.White,
            0, Vector2.Zero, _bodyScale, SpriteEffects.None, 0);
        Color tint = _isHovered ? (_isPressed ? Color.Gray : Color.LightGray) : Color.White;
        spriteBatch.Draw(_buttonTexture, CloseButtonBounds, tint);
        spriteBatch.DrawString(_font, CloseLabel, _closeLabelPosition, Color.White);
    }

    private string WrapText(string text, float maxWidth)
    {
        var result = new StringBuilder();
        foreach (string paragraph in text.Split('\n'))
        {
            string line = string.Empty;
            foreach (string word in paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                string candidate = line.Length == 0 ? word : line + " " + word;
                if (line.Length > 0 && _font.MeasureString(candidate).X > maxWidth)
                {
                    result.AppendLine(line);
                    line = word;
                }
                else
                {
                    line = candidate;
                }
            }
            result.AppendLine(line);
        }
        return result.ToString().TrimEnd();
    }
}
