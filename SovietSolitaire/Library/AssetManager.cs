using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SovietSolitaire.Library;

public static class AssetManager
{
	public static SpriteFont ArmadaFont { get; private set; }

	public static void Load(ContentManager content)
	{
		ArmadaFont = content.Load<SpriteFont>("Fonts/ArmadaBold16");
	}
}
