using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Input;
using SovietSolitaire.Library;
using System;
using System.Collections.Generic;

namespace SovietSolitaire.Entities;

public class EntityManager : IGameEntity
{
	private const int ScreenWidth = 1280;
	private const int ScreenHeight = 800;
	private const int SlotCount = 9;
	private const int SideMargin = 20;
	private const int SlotPadding = 10;
	private const int DeckCount = 37; // First card is blank can be used for back of deck or flipped cards for now
	private const float DealDuration = 0.18f;

	private List<Slot> _slots;
	private List<Card> _cards;
	private Deck _deck;
	private Card[] _dealOrder;
	private int _nextDealIndex;
	private float _dealElapsed;
	private List<Card> _draggedCards;
	private Slot _dragSource;
	private Point _dragOffset;

	private bool IsDealing => _nextDealIndex < _dealOrder.Length;

	public EntityManager()
	{
		_slots = new();
		_deck = new();
		InitializeSlots();
		StartDeal();
	}

	private void InitializeSlots()
	{
		int availableWidth = ScreenWidth - (2 * SideMargin);
		int totalPadding = SlotPadding * (SlotCount - 1);
		int cardWidth = (availableWidth - totalPadding) / SlotCount;
		
		int startX = SideMargin;
		int slotY = (ScreenHeight / 2) - 100;

		for (int i = 0; i < SlotCount; i++)
		{
			int slotX = startX + (i * (cardWidth + SlotPadding));
			_slots.Add(new Slot(new Point(slotX, slotY), cardWidth));
		}

		CreateCardDeck();
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		_deck.Draw(spriteBatch);
		if (IsDealing && _nextDealIndex + 1 < _dealOrder.Length)
			_dealOrder[_nextDealIndex + 1].Draw(spriteBatch);

		foreach (var slot in _slots)
		{
			slot.Draw(spriteBatch);
		}

		if (IsDealing)
			_dealOrder[_nextDealIndex].Draw(spriteBatch);

		if (_draggedCards is not null)
		{
			foreach (var card in _draggedCards)
			{
				card.Draw(spriteBatch);
			}
		}
	}

	public void Update(GameTime gameTime)
	{
		_deck.Update(gameTime);
		foreach (var slot in _slots)
		{
			slot.Update(gameTime);
		}

		if (IsDealing)
		{
			UpdateDeal(gameTime);
			return;
		}

		UpdateDrag();
	}

	private void StartDeal()
	{
		_dealOrder = _cards.GetRange(1, _cards.Count - 1).ToArray();
		Random.Shared.Shuffle(_dealOrder);
		_nextDealIndex = 0;
		_dealElapsed = 0;

		foreach (var card in _dealOrder)
		{
			card.Bounds = _deck.Bounds;
		}
	}

	private void UpdateDeal(GameTime gameTime)
	{
		_dealElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
		while (IsDealing && _dealElapsed >= DealDuration)
		{
			_slots[_nextDealIndex % SlotCount].AddCard(_dealOrder[_nextDealIndex]);
			_nextDealIndex++;
			_dealElapsed -= DealDuration;
		}

		if (!IsDealing)
			return;

		Rectangle start = _deck.Bounds;
		Rectangle target = _slots[_nextDealIndex % SlotCount].GetNextCardBounds();
		float progress = _dealElapsed / DealDuration;
		progress = progress * progress * (3 - 2 * progress);
		_dealOrder[_nextDealIndex].Bounds = new Rectangle(
			(int)MathHelper.Lerp(start.X, target.X, progress),
			(int)MathHelper.Lerp(start.Y, target.Y, progress),
			(int)MathHelper.Lerp(start.Width, target.Width, progress),
			(int)MathHelper.Lerp(start.Height, target.Height, progress));
	}

	private void UpdateDrag()
	{
		Point mousePosition = InputManager.GetTransformedMousePosition(0, 0).ToPoint();
		if (_draggedCards is not null)
		{
			if (!InputManager.IsLeftMouseButtonHeld())
			{
				FinishDrag(mousePosition);
				return;
			}

			Point movement = mousePosition - _dragOffset - _draggedCards[0].Bounds.Location;
			foreach (var card in _draggedCards)
			{
				card.Bounds = new Rectangle(card.Bounds.Location + movement, card.Bounds.Size);
			}
			return;
		}

		if (!InputManager.IsMouseInViewport || !InputManager.IsLeftMouseButtonDown())
			return;

		for (int i = _slots.Count - 1; i >= 0; i--)
		{
			Slot slot = _slots[i];
			List<Card> run = slot.TakeRunAt(mousePosition);
			if (run is not null)
			{
				_dragSource = slot;
				_draggedCards = run;
				_dragOffset = mousePosition - _draggedCards[0].Bounds.Location;
				break;
			}
		}
	}

	private void FinishDrag(Point mousePosition)
	{
		Slot destination = _dragSource;
		if (InputManager.IsMouseInViewport)
		{
			foreach (var slot in _slots)
			{
				if (slot.DropBounds.Contains(mousePosition)
					&& (slot == _dragSource || _draggedCards[0].CanPlaceOn(slot.BottomCard)))
				{
					destination = slot;
					break;
				}
			}
		}

		foreach (var card in _draggedCards)
		{
			destination.AddCard(card);
		}
		_draggedCards = null;
		_dragSource = null;
	}

	private void CreateCardDeck()
	{
		Texture2D texture = AssetManager.Cards
			?? throw new InvalidOperationException(
				"Load card assets before creating the deck.");

		int cardWidth = texture.Width / DeckCount;
		int cardHeight = texture.Height;

		string[] suits = ["Hearts", "Diamonds", "Spades", "Clubs"];
		string[] values = ["6", "7", "8", "9", "10", "Ace", "Jack", "Queen", "King"];

		_cards = new List<Card>(DeckCount)
		{
			new Card(
				string.Empty,
				"Blank",
				new Rectangle(0, 0, cardWidth, cardHeight))
		};

		int atlasIndex = 1;

		foreach (string value in values)
		{
			foreach (string suit in suits)
			{
				Rectangle sourceRectangle = new Rectangle(
					atlasIndex * cardWidth,
					0,
					cardWidth,
					cardHeight);

				_cards.Add(new Card(suit, value, sourceRectangle));
				atlasIndex++;
			}
		}
	}
}
