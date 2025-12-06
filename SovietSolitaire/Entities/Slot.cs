using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Library;
using SovietSolitaire.Graphics;

namespace SovietSolitaire.Entities;

internal class Slot : IGameEntity
{
	private const int CardWidth = 71;
	private const int CardHeight = 96;
	private const int LineWidth = 2;

	private Point _position;

	public Slot(Point position)
	{
		_position = position;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		Rectangle slotRectangle = new Rectangle(_position.X, _position.Y, CardWidth, CardHeight);
		RectangleSprite.DrawRectangle(spriteBatch, slotRectangle, Color.White, LineWidth);
	}

	public void Update(GameTime gameTime)
	{
		
	}
}
