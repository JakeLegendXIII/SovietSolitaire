using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

	private List<Slot> _slots;
	private List<Card> _cards;
	private Deck _deck;

	public EntityManager()
	{
		_slots = new();
		_deck = new();
		InitializeSlots();
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
		foreach (var slot in _slots)
		{
			slot.Draw(spriteBatch);
		}
	}

	public void Update(GameTime gameTime)
	{
		_deck.Update(gameTime);
		foreach (var slot in _slots)
		{
			slot.Update(gameTime);
		}
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
