﻿using FreeTypeSharp;
using System;
using System.Diagnostics;
using static FreeTypeSharp.FT;

namespace FreeType
{
    public unsafe struct Face : IDisposable
    {
        private nint value;

        public readonly bool IsDisposed => value == default;
        public readonly uint GlyphCout
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)value;
                return (uint)face->num_glyphs;
            }
        }

        /// <summary>
        /// Contains versions of ascender, descender,
        /// height and max advance values that respect the
        /// set character pixel size.
        /// </summary>
        public readonly SizeMetrics SizeMetrics
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)value;
                return new SizeMetrics(face->size->metrics);
            }
        }

        public readonly int Ascender
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)value;
                return (int)face->ascender;
            }
        }

        public readonly int Descender
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)value;
                return (int)face->descender;
            }
        }

        /// <summary>
        /// A set value for the distance between baseline to baseline.
        /// </summary>
        public readonly uint Height
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)value;
                return (uint)face->height;
            }
        }

        public readonly (uint x, uint y) MaxAdvance
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)value;
                return ((uint)face->max_advance_width, (uint)face->max_advance_height);
            }
        }

        public readonly (int xMin, int xMax, int yMin, int yMax) Bounds
        {
            get
            {
                FT_FaceRec_* face = (FT_FaceRec_*)value;
                FT_BBox_ bounds = face->bbox;
                return ((int)bounds.xMin, (int)bounds.xMax, (int)bounds.yMin, (int)bounds.yMax);
            }
        }

        internal Face(nint address)
        {
            value = address;
        }

        [Conditional("DEBUG")]
        private readonly void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(Face));
            }
        }

        public void Dispose()
        {
            ThrowIfDisposed();
            FT_Done_Face((FT_FaceRec_*)value);
            value = default;
        }

        public readonly int CopyFamilyName(Span<char> buffer)
        {
            ThrowIfDisposed();
            FT_FaceRec_* face = (FT_FaceRec_*)value;
            int length = 0;
            byte* source = face->family_name;
            fixed (char* destination = buffer)
            {
                while (length < buffer.Length)
                {
                    byte value = *source;
                    if (value == 0)
                    {
                        break;
                    }

                    destination[length++] = (char)value;
                    source++;
                }
            }

            return length;
        }

        public readonly int CopyStyleName(Span<char> buffer)
        {
            ThrowIfDisposed();
            FT_FaceRec_* face = (FT_FaceRec_*)value;
            int length = 0;
            byte* source = face->style_name;
            fixed (char* destination = buffer)
            {
                while (length < buffer.Length)
                {
                    byte value = *source;
                    if (value == 0)
                    {
                        break;
                    }

                    destination[length++] = (char)value;
                    source++;
                }
            }

            return length;
        }

        public readonly void SetPixelSize(uint width, uint height)
        {
            ThrowIfDisposed();
            FT_FaceRec_* face = (FT_FaceRec_*)value;
            FT_Error error = FT_Set_Pixel_Sizes(face, width, height);
            if (error != FT_Error.FT_Err_Ok)
            {
                throw new Exception($"Failed to set pixel size: {error}");
            }
        }

        public readonly uint GetCharIndex(char charcode)
        {
            ThrowIfDisposed();
            FT_FaceRec_* face = (FT_FaceRec_*)value;
            return FT_Get_Char_Index(face, charcode);
        }

        /// <summary>
        /// Loads the wanted glyph into this font face's slot.
        /// </summary>
        public readonly GlyphSlot LoadGlyph(uint index)
        {
            ThrowIfDisposed();
            FT_Error error = FT_Load_Glyph((FT_FaceRec_*)value, index, FT_LOAD.FT_LOAD_DEFAULT);
            if (error != FT_Error.FT_Err_Ok)
            {
                throw new Exception($"Failed to load glyph: {error}");
            }

            FT_FaceRec_* face = (FT_FaceRec_*)value;
            return new GlyphSlot((nint)face);
        }

        public readonly (int x, int y) GetKerning(char first, char next)
        {
            ThrowIfDisposed();
            FT_Vector_ kerning;
            FT_Kerning_Mode_ mode = FT_Kerning_Mode_.FT_KERNING_DEFAULT;
            FT_Error error = FT_Get_Kerning((FT_FaceRec_*)value, first, next, mode, &kerning);
            if (error != FT_Error.FT_Err_Ok)
            {
                throw new Exception($"Failed to get kerning: {error}");
            }

            return ((int)kerning.x, (int)kerning.y);
        }
    }
}