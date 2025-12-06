using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Library;
using SovietSolitaire.Graphics;

namespace SovietSolitaire.Entities;

internal class Slot : IGameEntity
{
	private const int CardHeight = 165;
	private const int LineWidth = 2;

	private Point _position;
	private int _cardWidth;

	public Slot(Point position, int cardWidth)
	{
		_position = position;
		_cardWidth = cardWidth;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		Rectangle slotRectangle = new Rectangle(_position.X, _position.Y, _cardWidth, CardHeight);
		RectangleSprite.DrawRectangle(spriteBatch, slotRectangle, Color.White, LineWidth);
	}

	public void Update(GameTime gameTime)
	{
		
	}
}
