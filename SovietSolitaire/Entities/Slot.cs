using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Library;
using SovietSolitaire.Graphics;
using System.Collections.Generic;

namespace SovietSolitaire.Entities;

internal class Slot : IGameEntity
{
	private const int CardHeight = 165;
	private const int LineWidth = 2;
	private const int StackOffset = 32;

	private Point _position;
	private int _cardWidth;
	private readonly List<Card> _cards = new();

	public int CardCount => _cards.Count;
	public Card BottomCard => _cards.Count == 0 ? null : _cards[^1];
	public Rectangle Bounds => new Rectangle(_position.X, _position.Y, _cardWidth, CardHeight);
	public Rectangle DropBounds => new Rectangle(_position.X, _position.Y, _cardWidth,
		CardHeight + (_cards.Count > 0 ? (_cards.Count - 1) * StackOffset : 0));

	public Slot(Point position, int cardWidth)
	{
		_position = position;
		_cardWidth = cardWidth;
	}

	public Rectangle GetNextCardBounds()
	{
		return new Rectangle(_position.X, _position.Y + _cards.Count * StackOffset, _cardWidth, CardHeight);
	}

	public void AddCard(Card card)
	{
		card.Bounds = GetNextCardBounds();
		_cards.Add(card);
	}

	public Card TakeBottomCard()
	{
		Card card = BottomCard;
		if (card is not null)
			_cards.RemoveAt(_cards.Count - 1);
		return card;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		RectangleSprite.DrawRectangle(spriteBatch, Bounds, Color.White, LineWidth);
		foreach (var card in _cards)
		{
			card.Draw(spriteBatch);
		}
	}

	public void Update(GameTime gameTime)
	{
		
	}
}
