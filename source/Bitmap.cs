using FreeTypeSharp;
using System;

namespace FreeType
{
    public unsafe readonly struct Bitmap
    {
        private readonly nint address;

        public readonly ReadOnlySpan<byte> Buffer
        {
            get
            {
                FT_GlyphSlotRec_* glyph = (FT_GlyphSlotRec_*)address;
                return new(glyph->bitmap.buffer, (int)(glyph->bitmap.width * glyph->bitmap.rows));
            }
        }

        public readonly (uint x, uint y) Size
        {
            get
            {
                FT_GlyphSlotRec_* glyph = (FT_GlyphSlotRec_*)address;
                return (glyph->bitmap.width, glyph->bitmap.rows);
            }
        }

        public readonly int Pitch
        {
            get
            {
                FT_GlyphSlotRec_* glyph = (FT_GlyphSlotRec_*)address;
                return glyph->bitmap.pitch;
            }
        }

        internal Bitmap(nint address)
        {
            this.address = address;
        }
    }
}