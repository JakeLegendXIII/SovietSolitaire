using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Library;

namespace SovietSolitaire.Entities;

internal class Card : IGameEntity
{
	public string Suit { get; init; }
	public string Value { get; init; }

	private Texture2D _texture;
	private Rectangle _cardPositionOnAtlas;

	public Card(string suit, string value, Rectangle cardPosition)
	{
		Suit = suit;
		Value = value;
		_texture = AssetManager.Cards;
		_cardPositionOnAtlas = cardPosition;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		
	}

	public void Update(GameTime gameTime)
	{
		
	}
}
