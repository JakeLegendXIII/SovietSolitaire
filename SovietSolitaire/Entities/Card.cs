using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Library;

namespace SovietSolitaire.Entities;

internal class Card : IGameEntity
{
	public string Suit { get; init; }
	public string Value { get; init; }

	private Texture2D _texture;

	public Card(string suit, string value, Texture2D texture)
	{
		Suit = suit;
		Value = value;
		_texture = texture;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		
	}

	public void Update(GameTime gameTime)
	{
		
	}
}
