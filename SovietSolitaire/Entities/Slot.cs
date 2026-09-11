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

	public List<Card> TakeRunAt(Point position)
	{
		for (int i = _cards.Count - 1; i >= 0; i--)
		{
			if (!_cards[i].Bounds.Contains(position))
				continue;

			for (int j = i + 1; j < _cards.Count; j++)
			{
				if (!_cards[j].CanPlaceOn(_cards[j - 1]))
					return null;
			}

			List<Card> run = _cards.GetRange(i, _cards.Count - i);
			_cards.RemoveRange(i, run.Count);
			return run;
		}

		return null;
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
