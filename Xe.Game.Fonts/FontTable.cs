using System;
using System.IO;

namespace Xe.Game.Fonts
{
    public class Table : IDisposable
    {
        private string filename;
        private Bitmap texture;

        /// <summary>
        /// Nome della texture da caricare.
        /// </summary>
        public string Filename
        {
            get
            {
                return filename;
            }
            set
            {
                filename = value;
                if (File.Exists(filename))
                    texture = new Bitmap(filename);
                else
                    texture = null;
            }
        }
        /// <summary>
        /// Texture annessa alla tabella
        /// </summary>
        [JsonIgnore]
        public Bitmap Texture { get { return texture; } }
        /// <summary>
        /// Qual è il primo carattere della tabella.
        /// </summary>
        public int CharStart;
        /// <summary>
        /// Numero di caratteri presenti nella tabella.
        /// </summary>
        public int CharCount;
        /// <summary>
        /// Numero di caratteri per riga.
        /// </summary>
        public int CharPerRow;
        /// <summary>
        /// Carattere di default usato nel caso non si trova quello voluto.
        /// </summary>
        public int CharDefault;
        /// <summary>
        /// Y coordinate for first character
        /// </summary>
        public int YOffset;
        /// <summary>
        /// Pagina UTF-16
        /// </summary>
        public int CharSet;
        /// <summary>
        /// Posizione degli spazi usati dai caratteri.
        /// Se 0, lo spazio di default viene usato.
        /// </summary>
        [JsonIgnore]
        public int SpaceOffset;
        /// <summary>
        /// Larghezza per ogni carattere.
        /// </summary>
        public byte[] Spaces = new byte[0x100];

        public void Dispose()
        {
            if (texture != null) texture.Dispose();
        }
    }
}
