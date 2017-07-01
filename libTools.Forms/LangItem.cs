using System;
using System.Windows.Forms;

namespace libTools
{
    public partial class LangItem : UserControl
    {
        public new delegate void TextChanged(LangItem sender, string name, string desc);
        public new event TextChanged OnTextChanged;

        private Language.Message mCurrentMessageName = null;
        private Language.Message mCurrentMessageDesc = null;
        private Languages mCurrentLanguage = Languages.English;

        public Language.Message CurrentName
        {
            get { return mCurrentMessageName; }
            set
            {
                mCurrentMessageName = value;
                Reload();
            }
        }
        public Language.Message CurrentDescription
        {
            get { return mCurrentMessageDesc; }
            set
            {
                mCurrentMessageDesc = value;
                Reload();
            }
        }
        public Languages CurrentLanguage
        {
            get { return mCurrentLanguage; }
            set
            {
                mCurrentLanguage = value;
                Reload();
            }
        }

        private void Reload()
        {
            if (CurrentName == null || CurrentDescription == null)
            {
                groupBox.Text = "Error";
                return;
            }
            switch (CurrentLanguage)
            {
                case Languages.English:
                    groupBox.Text = "English";
                    textBoxName.Text = CurrentName.En;
                    textBoxDesc.Text = CurrentDescription.En;
                    break;
                case Languages.Italian:
                    groupBox.Text = "Italian";
                    textBoxName.Text = CurrentName.It;
                    textBoxDesc.Text = CurrentDescription.It;
                    break;
                case Languages.French:
                    groupBox.Text = "French";
                    textBoxName.Text = CurrentName.Fr;
                    textBoxDesc.Text = CurrentDescription.Fr;
                    break;
                case Languages.German:
                    groupBox.Text = "German";
                    textBoxName.Text = CurrentName.De;
                    textBoxDesc.Text = CurrentDescription.De;
                    break;
                case Languages.Spanish:
                    groupBox.Text = "Spanish";
                    textBoxName.Text = CurrentName.Sp;
                    textBoxDesc.Text = CurrentDescription.Sp;
                    break;
            }
        }

        public LangItem()
        {
            InitializeComponent();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            var str = (sender as TextBox).Text;
            switch (CurrentLanguage)
            {
                case libTools.Languages.English:
                    CurrentName.En = str;
                    break;
                case libTools.Languages.Italian:
                    CurrentName.It = str;
                    break;
                case libTools.Languages.French:
                    CurrentName.Fr = str;
                    break;
                case libTools.Languages.German:
                    CurrentName.De = str;
                    break;
                case libTools.Languages.Spanish:
                    CurrentName.Sp = str;
                    break;
            }
            Hey_TextChanged();
        }

        private void textBoxDesc_TextChanged(object sender, EventArgs e)
        {
            var str = (sender as TextBox).Text;
            switch (CurrentLanguage)
            {
                case libTools.Languages.English:
                    CurrentDescription.En = str;
                    break;
                case libTools.Languages.Italian:
                    CurrentDescription.It = str;
                    break;
                case libTools.Languages.French:
                    CurrentDescription.Fr = str;
                    break;
                case libTools.Languages.German:
                    CurrentDescription.De = str;
                    break;
                case libTools.Languages.Spanish:
                    CurrentDescription.Sp = str;
                    break;
            }
            Hey_TextChanged();
        }

        private void Hey_TextChanged()
        {
            string name, desc;
            switch (CurrentLanguage)
            {
                case libTools.Languages.English:
                    name = CurrentName.En;
                    desc = CurrentDescription.En;
                    break;
                case libTools.Languages.Italian:
                    name = CurrentName.It;
                    desc = CurrentDescription.It;
                    break;
                case libTools.Languages.French:
                    name = CurrentName.Fr;
                    desc = CurrentDescription.Fr;
                    break;
                case libTools.Languages.German:
                    name = CurrentName.De;
                    desc = CurrentDescription.De;
                    break;
                case libTools.Languages.Spanish:
                    name = CurrentName.Sp;
                    desc = CurrentDescription.Sp;
                    break;
                default:
                    name = "<error>";
                    desc = "<error>";
                    break;
            }
            if (OnTextChanged != null)
                OnTextChanged(this, name, desc);
        }
    }
}
