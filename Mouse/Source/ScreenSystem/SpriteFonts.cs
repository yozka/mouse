using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mouse.ScreenSystem
{
    public class ASpriteFonts
    {
        public SpriteFont detailsFont;
        public SpriteFont diagnosticSpriteFont;
        public SpriteFont frameRateCounterFont;
        public SpriteFont gameSpriteFont;
        public SpriteFont menuSpriteFont;

        public ASpriteFonts(ContentManager content)
        {
            diagnosticSpriteFont = content.Load<SpriteFont>("Fonts/menuSpriteFont");
            menuSpriteFont = content.Load<SpriteFont>("Fonts/menuSpriteFont");
            frameRateCounterFont = content.Load<SpriteFont>("Fonts/menuSpriteFont");
            gameSpriteFont  = content.Load<SpriteFont>("Fonts/menuSpriteFont");
            detailsFont     = content.Load<SpriteFont>("Fonts/menuSpriteFont");
        }
    }
}