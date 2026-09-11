using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Library;
using SovietSolitaire.Graphics;

namespace SovietSolitaire.Entities;

public class Deck : IGameEntity
{
	private const int CardWidth = 190;
	private const int CardHeight = 250;
	private const int CardX = 1057;
	private const int CardY = 10;
	private const int LineWidth = 2;

	public Rectangle Bounds => new Rectangle(CardX, CardY, CardWidth, CardHeight);

	public void Draw(SpriteBatch spriteBatch)
	{
		RectangleSprite.DrawRectangle(spriteBatch, Bounds, Color.White, LineWidth);
	}

	public void Update(GameTime gameTime)
	{
		
	}
}
