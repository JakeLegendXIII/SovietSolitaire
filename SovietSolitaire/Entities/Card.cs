using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Library;

namespace SovietSolitaire.Entities;

public class Card : IGameEntity
{
	public string Suit { get; init; }
	public string Value { get; init; }
	public Rectangle Bounds { get; set; }

	private bool IsRed => Suit is "Hearts" or "Diamonds";
	private bool IsRoyalOrAce => Value is "Ace" or "Jack" or "Queen" or "King";

	private Texture2D _texture;
	private Rectangle _cardPositionOnAtlas;

	public Card(string suit, string value, Rectangle cardPosition)
	{
		Suit = suit;
		Value = value;
		_texture = AssetManager.Cards;
		_cardPositionOnAtlas = cardPosition;
	}

	public bool CanPlaceOn(Card target)
	{
		if (Value == "Blank")
			return false;

		if (target is null)
			return true;

		if (IsRoyalOrAce || target.IsRoyalOrAce)
			return IsRoyalOrAce && target.IsRoyalOrAce && Suit == target.Suit;

		return int.TryParse(Value, out int rank)
			&& int.TryParse(target.Value, out int targetRank)
			&& rank >= 6 && rank <= 10
			&& targetRank >= 6 && targetRank <= 10
			&& rank == targetRank - 1
			&& IsRed != target.IsRed;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		Draw(spriteBatch, false);
	}

	public void Draw(SpriteBatch spriteBatch, bool showBlank)
	{
		Rectangle source = showBlank
			? new Rectangle(0, 0, _cardPositionOnAtlas.Width, _cardPositionOnAtlas.Height)
			: _cardPositionOnAtlas;
		spriteBatch.Draw(_texture, Bounds, source, Color.White);
	}

	public void Update(GameTime gameTime)
	{
		
	}
}
