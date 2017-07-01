using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe;

namespace libTools.Language
{
    public class Message : UniqueObject, IDeepCloneable
    {
        public static readonly Message Empty = new Message();

        public string En, It, Fr, De, Sp;

        public string Text
        {
            get
            {
                string str;
                switch (Lang.CurrentLanguage)
                {
                    case Languages.English:
                        str = En;
                        break;
                    case Languages.Italian:
                        str = It;
                        break;
                    case Languages.French:
                        str = Fr;
                        break;
                    case Languages.German:
                        str = De;
                        break;
                    case Languages.Spanish:
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
                switch (Lang.CurrentLanguage)
                {
                    case Languages.English:
                        En = value;
                        break;
                    case Languages.Italian:
                        It = value;
                        break;
                    case Languages.French:
                        Fr = value;
                        break;
                    case Languages.German:
                        De = value;
                        break;
                    case Languages.Spanish:
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
            switch (Lang.CurrentLanguage)
            {
                case Languages.English: if (En != null) return En; break;
                case Languages.Italian: if (It != null) return It; break;
                case Languages.French: if (Fr != null) return Fr; break;
                case Languages.German: if (De != null) return De; break;
                case Languages.Spanish: if (Sp != null) return Sp; break;
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
