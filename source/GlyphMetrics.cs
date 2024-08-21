using FreeTypeSharp;

namespace FreeType
{
    public unsafe readonly struct GlyphMetrics
    {
        private readonly nint address;

        /// <summary>
        /// Dimensions of the glyph image's bounding box.
        /// </summary>
        public readonly (uint x, uint y) Size
        {
            get
            {
                FT_GlyphSlotRec_* glyph = (FT_GlyphSlotRec_*)address;
                return ((uint)glyph->metrics.width, (uint)glyph->metrics.height);
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
        /// Amount of distance to move the cursor to the right for the next glyph,
        /// for horizontal layouts.
        /// </summary>
        public readonly int HorizontalAdvance
        {
            get
            {
                FT_GlyphSlotRec_* glyph = (FT_GlyphSlotRec_*)address;
                return (int)glyph->metrics.horiAdvance;
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
        /// when using a vertical layout.
        /// </summary>
        public readonly int VerticalAdvance
        {
            get
            {
                FT_GlyphSlotRec_* glyph = (FT_GlyphSlotRec_*)address;
                return (int)glyph->metrics.vertAdvance;
            }
        }

        internal GlyphMetrics(nint address)
        {
            this.address = address;
        }
    }
}