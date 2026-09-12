using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Library;
using SovietSolitaire.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SovietSolitaire.Entities;

public class Slot : IGameEntity
{
	private const int CardHeight = 165;
	private const int LineWidth = 2;
	private const int StackOffset = 32;

	private Point _position;
	private int _cardWidth;
	private readonly List<Card> _cards = new();

	public int CardCount => _cards.Count;
	public Card BottomCard => _cards.Count == 0 ? null : _cards[^1];
	public bool IsRoyalSetComplete { get; private set; }
	public bool IsNumberedSetComplete
	{
		get
		{
			if (_cards.Count != 5 || _cards[0].Value != "10" || _cards[^1].Value != "6")
				return false;

			for (int i = 1; i < _cards.Count; i++)
			{
				if (!_cards[i].CanPlaceOn(_cards[i - 1]))
					return false;
			}

			return true;
		}
	}
	public Rectangle Bounds => new Rectangle(_position.X, _position.Y, _cardWidth, CardHeight);
	public Rectangle DropBounds => new Rectangle(_position.X, _position.Y, _cardWidth,
		CardHeight + (_cards.Count > 0 ? (_cards.Count - 1) * StackOffset : 0));

	public Slot(Point position, int cardWidth)
	{
		_position = position;
		_cardWidth = cardWidth;
	}

    public void Draw(SpriteBatch spriteBatch)
    {
        RectangleSprite.DrawRectangle(spriteBatch, Bounds, Color.White, LineWidth);
        foreach (var card in _cards)
        {
            card.Draw(spriteBatch, IsRoyalSetComplete);
        }
    }

    public void Update(GameTime gameTime) { }

    public Rectangle GetNextCardBounds()
	{
		return new Rectangle(_position.X, _position.Y + _cards.Count * StackOffset, _cardWidth, CardHeight);
	}

	public void AddCard(Card card)
	{
		if (IsRoyalSetComplete)
			throw new InvalidOperationException("Cannot add cards to a completed royal set.");

		card.Bounds = GetNextCardBounds();
		_cards.Add(card);
	}

	public void CompleteRoyalSet()
	{
		if (IsRoyalSetComplete || _cards.Count != 4)
			return;

		IsRoyalSetComplete = _cards.All(card => card.Suit == _cards[0].Suit
			&& card.Value is "Ace" or "Jack" or "Queen" or "King")
			&& _cards.Select(card => card.Value).Distinct().Count() == 4;
	}

	public List<Card> TakeRunAt(Point position)
	{
		if (IsRoyalSetComplete)
			return null;

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
}
