using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Library;
using System.Collections.Generic;

namespace SovietSolitaire.Entities;

public class EntityManager : IGameEntity
{
	// TODO : List of Slots that have list of cards
	private List<Slot> _slots;
	private Deck _deck;

	public EntityManager()
	{
		_slots = new();
		_deck = new();
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
