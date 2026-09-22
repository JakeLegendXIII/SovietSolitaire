using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SovietSolitaire.Engine.Library;

public static class AssetManager
{
	public static SpriteFont ArmadaFont { get; private set; }
	public static SpriteFont FarawayFont { get; private set; }
	public static Texture2D ClubCards { get; private set; }
	public static Texture2D DiamondCards { get; private set; }
	public static Texture2D HeartCards { get; private set; }
	public static Texture2D SpadeCards { get; private set; }
	public static Texture2D ButtonUI { get; private set; }
	public static Texture2D ConfirmationBanner { get; private set; }

	public static void Load(ContentManager content)
	{
		ArmadaFont = content.Load<SpriteFont>("Fonts/ArmadaBold16");
		FarawayFont = content.Load<SpriteFont>("Fonts/Faraway16");
		ClubCards = content.Load<Texture2D>("Sprites/Club_Cards");
		DiamondCards = content.Load<Texture2D>("Sprites/Diamond_Cards");
		HeartCards = content.Load<Texture2D>("Sprites/Heart_Cards");
		SpadeCards = content.Load<Texture2D>("Sprites/Spade_Cards");
		ButtonUI = content.Load<Texture2D>("UI/MenuButton");
		ConfirmationBanner = content.Load<Texture2D>("UI/ConfirmationBanner");
	}
}
