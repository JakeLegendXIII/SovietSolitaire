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
	private const int CardWidth = 71;
	private const int SlotPadding = 50;	

	private List<Slot> _slots;
	private Deck _deck;

	public EntityManager()
	{
		_slots = new();
		_deck = new();
		InitializeSlots();
	}

	private void InitializeSlots()
	{
		int totalSlotWidth = (CardWidth * SlotCount) + (SlotPadding * (SlotCount - 1));
		int startX = (ScreenWidth - totalSlotWidth) / 2;
		int slotY = ScreenHeight / 2;

		for (int i = 0; i < SlotCount; i++)
		{
			int slotX = startX + (i * (CardWidth + SlotPadding));
			_slots.Add(new Slot(new Point(slotX, slotY)));
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
