using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Library;
using System.Collections.Generic;

namespace SovietSolitaire.Entities;

public class EntityManager : IGameEntity
{
	private const int ScreenWidth = 1280;
	private const int ScreenHeight = 800;
	private const int SlotCount = 9;
	private const int SideMargin = 20;
	private const int SlotPadding = 10;
	private const int DeckCount = 36;

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
}
