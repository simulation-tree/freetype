using FreeTypeSharp;

namespace FreeType
{
    public unsafe readonly struct GlyphMetrics
    {
        private readonly nint address;

        /// <summary>
        /// Dimensions of the glyph image's bounding box.
        /// </summary>
        public readonly (int x, int y) Size
        {
            get
            {
                FT_GlyphSlotRec_* glyph = (FT_GlyphSlotRec_*)address;
                return ((int)glyph->metrics.width, (int)glyph->metrics.height);
            }
        }

        /// <summary>
        /// Distance away from the baseline cursor position, for horizontal layouts.
        /// </summary>
        public readonly (int x, int y) HorizontalBearing
        {
            get
            {
                FT_GlyphSlotRec_* glyph = (FT_GlyphSlotRec_*)address;
                return ((int)glyph->metrics.horiBearingX, (int)glyph->metrics.horiBearingY);
            }
        }

        /// <summary>
        /// Distances away from the baseline cursor position, for vertical layouts.
        /// </summary>
        public readonly (int x, int y) VerticalBearing
        {
            get
            {
                FT_GlyphSlotRec_* glyph = (FT_GlyphSlotRec_*)address;
                return ((int)glyph->metrics.vertBearingX, (int)glyph->metrics.vertBearingY);
            }
        }

        /// <summary>
        /// Amount of distance to increment the cursor position
        /// depending on the direction of the text layout.
        /// </summary>
        public readonly (int x, int y) Advance
        {
            get
            {
                FT_GlyphSlotRec_* glyph = (FT_GlyphSlotRec_*)address;
                return ((int)glyph->metrics.horiAdvance, (int)glyph->metrics.vertAdvance);
            }
        }

        internal GlyphMetrics(nint address)
        {
            this.address = address;
        }
    }
}