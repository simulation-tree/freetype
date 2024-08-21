using FreeTypeSharp;
using System;
using static FreeTypeSharp.FT;

namespace FreeType
{
    public unsafe readonly struct GlyphSlot
    {
        private readonly nint address;

        /// <summary>
        /// Horizontal distance from the pen position to the left
        /// of the glyph's image.
        /// </summary>
        public readonly int Left
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)address;
                return face->glyph->bitmap_left;
            }
        }

        /// <summary>
        /// Vertical distance from the pen position to the top of
        /// the glyph's image.
        /// </summary>
        public readonly int Top
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)address;
                return face->glyph->bitmap_top;
            }
        }

        public readonly (int x, int y) Advance
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)address;
                return ((int)face->glyph->advance.x, (int)face->glyph->advance.y);
            }
        }

        public readonly GlyphMetrics Metrics
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)address;
                return new((nint)face->glyph);
            }
        }

        /// <summary>
        /// Original advance width in 16.16 fractional pixels.
        /// </summary>
        public readonly uint LinearHorizontalAdvance
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)address;
                return (uint)face->glyph->linearHoriAdvance;
            }
        }

        /// <summary>
        /// Original vertical advance in 16.16 fractional pixels.
        /// </summary>
        public readonly uint LinearVerticalAdvance
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)address;
                return (uint)face->glyph->linearVertAdvance;
            }
        }

        internal GlyphSlot(nint address)
        {
            this.address = address;
        }

        /// <summary>
        /// Updates the bitmap located inside this glyph slot
        /// to match the internal image.
        /// </summary>
        public readonly Bitmap Render()
        {
            FT_FaceRec_* face = (FT_FaceRec_*)address;
            FT_Glyph_Format_ format = face->glyph->format;
            if (format != FT_Glyph_Format_.FT_GLYPH_FORMAT_BITMAP)
            {
                FT_Error error = FT_Render_Glyph(face->glyph, FT_Render_Mode_.FT_RENDER_MODE_NORMAL);
                if (error != FT_Error.FT_Err_Ok)
                {
                    throw new Exception($"Failed to render glyph: {error}");
                }
            }

            return new((nint)face->glyph);
        }
    }
}