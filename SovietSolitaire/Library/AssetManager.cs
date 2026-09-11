using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SovietSolitaire.Library;

public static class AssetManager
{
	public static Texture2D ArmadaFont { get; private set; }
	public static Texture2D Cards {  get; private set; }

	public static void Load(ContentManager content)
	{
		ArmadaFont = content.Load<Texture2D>("Fonts/ArmadaBold16");
		Cards = content.Load<Texture2D>("Sprites/Cards");
	}
}
