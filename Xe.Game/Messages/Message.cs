namespace Xe.Game.Messages
{
    public class Message : UniqueObject, IDeepCloneable
    {
        private static Language CurrentLanguage => MessageContainer.CurrentLanguage;

        public static readonly Message Empty = new Message();
        
        public string En { get; set; }
        public string It { get; set; }
        public string Fr { get; set; }
        public string De { get; set; }
        public string Sp { get; set; }

        public string Text
        {
            get
            {
                string str;
                switch (CurrentLanguage)
                {
                    case Language.English:
                        str = En;
                        break;
                    case Language.Italian:
                        str = It;
                        break;
                    case Language.French:
                        str = Fr;
                        break;
                    case Language.Deutsch:
                        str = De;
                        break;
                    case Language.Spanish:
                        str = Sp;
                        break;
                    default:
                        return "";
                }
                return str ?? "";
            }
            set
            {
                if (value == null)
                    value = "";
                switch (CurrentLanguage)
                {
                    case Language.English:
                        En = value;
                        break;
                    case Language.Italian:
                        It = value;
                        break;
                    case Language.French:
                        Fr = value;
                        break;
                    case Language.Deutsch:
                        De = value;
                        break;
                    case Language.Spanish:
                        Sp = value;
                        break;
                }
            }
        }

        public object DeepClone()
        {
            var item = new Message()
            {
                En = En != null ? En.Clone() as string : null,
                It = It != null ? It.Clone() as string : null,
                Fr = Fr != null ? Fr.Clone() as string : null,
                De = De != null ? De.Clone() as string : null,
                Sp = Sp != null ? Sp.Clone() as string : null
            };
            return item;
        }

        public override string ToString()
        {
            switch (CurrentLanguage)
            {
                case Language.English: if (En != null) return En; break;
                case Language.Italian: if (It != null) return It; break;
                case Language.French: if (Fr != null) return Fr; break;
                case Language.Deutsch: if (De != null) return De; break;
                case Language.Spanish: if (Sp != null) return Sp; break;
            }
            return
                (En != null && En.Length > 0) ? En :
                (It != null && It.Length > 0) ? It :
                (Fr != null && Fr.Length > 0) ? Fr :
                (De != null && De.Length > 0) ? De :
                (Sp != null && Sp.Length > 0) ? Sp :
                "<null>";
        }
    }
}
