namespace Xe.Game.Fonts
{
    public class FontCharset
    {
        /// <summary>
        /// Larghezza massima in pixel per ogni singolo carattere.
        /// </summary>
        public int MaximumWidth { get; set; }

        /// <summary>
        /// Altezza massima in pixel per ogni singolo carattere.
        /// </summary>
        public int MaximumHeight { get; set; }

        /// <summary>
        /// Distanza verticale in pixel tra un carattere e l'altro.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Offset verticale in pixel per spostare il carattere.
        /// </summary>
        public int YOffset { get; set; }
    }
}
