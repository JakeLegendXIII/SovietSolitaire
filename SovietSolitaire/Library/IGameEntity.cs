using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SovietSolitaire.Library;

public interface IGameEntity
{
	void Update(GameTime gameTime);
	void Draw(SpriteBatch spriteBatch);
}
