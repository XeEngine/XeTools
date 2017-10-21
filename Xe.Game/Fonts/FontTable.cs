namespace Xe.Game.Fonts
{
    public class FontTable
    {
        /// <summary>
        /// Nome della texture da caricare.
        /// </summary>
        public string Texture { get; set; }

        /// <summary>
        /// Qual è il primo carattere della tabella.
        /// </summary>
        public int CharStart { get; set; }

        /// <summary>
        /// Numero di caratteri presenti nella tabella.
        /// </summary>
        public int CharCount { get; set; }

        /// <summary>
        /// Numero di caratteri per riga.
        /// </summary>
        public int CharPerRow { get; set; }

        /// <summary>
        /// Carattere di default usato nel caso non si trova quello voluto.
        /// </summary>
        public int CharDefault { get; set; }

        /// <summary>
        /// Y coordinate for first character
        /// </summary>
        public int YOffset { get; set; }

        /// <summary>
        /// Pagina UTF-16
        /// </summary>
        public int CharSet { get; set; }

        /// <summary>
        /// Posizione degli spazi usati dai caratteri.
        /// Se 0, lo spazio di default viene usato.
        /// </summary>
        /// 
        public int SpaceOffset;

        /// <summary>
        /// Larghezza per ogni carattere.
        /// </summary>
        public byte[] Spaces = new byte[0x100];
    }
}
