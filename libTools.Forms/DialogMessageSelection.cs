using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using libTools.Language;

namespace libTools.Forms
{
    public partial class DialogMessageSelection : Form
    {
        private Lang _language;
        private Segment _cursegment;
        private Language.Message _curmessage;
        private bool _isMessageNew;
        private bool _isMessageSelecting;

        public Lang Language
        {
            get { return _language; }
            set
            {
                _language = value;
                if (value == null)
                {
                    buttonOk.Enabled = false;
                    comboBoxSegment.Enabled = false;
                    return;
                }
                else
                {
                    buttonOk.Enabled = true;
                    comboBoxSegment.Enabled = true;
                }
                comboBoxSegment.DataSource = value.Segments;
                if (value.Segments == null || value.Segments.Count < 1)
                {
                    comboBoxSegment.Enabled = false;
                    return;
                }
                else
                    comboBoxSegment.Enabled = true;
            }
        }
        public Guid MessageId
        {
            get
            {
                if (_curmessage != null)
                    return _curmessage.UID;
                return Guid.Empty;
            }
        }

        private bool IsMessageNew
        {
            get { return _isMessageNew; }
            set
            {
                if (_isMessageNew != value)
                {
                    _isMessageNew = value;
                    string text;
                    Color newcolor;
                    if (value)
                    {
                        text = "&Add";
                        newcolor = Color.LightGoldenrodYellow;
                    }
                    else
                    {
                        text = "&Ok";
                        newcolor = Color.Black;
                    }
                    buttonOk.Text = text;
                    radioButtonEnglish.ForeColor = newcolor;
                    radioButtonItalian.ForeColor = newcolor;
                    radioButtonFrench.ForeColor = newcolor;
                    radioButtonDetush.ForeColor = newcolor;
                    radioButtonSpanish.ForeColor = newcolor;
                    radioButtonJapanese.ForeColor = newcolor;
                }
            }
        }
        private Languages CurrentLanguage
        {
            get { return Lang.CurrentLanguage; }
            set
            {
                Lang.CurrentLanguage = value;
                comboBoxMessage.DataSourceRefresh();
            }
        }

        public DialogMessageSelection()
        {
            InitializeComponent();
        }
        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            if (_isMessageSelecting) return;
            IsMessageNew = true;
            switch (Lang.CurrentLanguage)
            {
                case Languages.English:
                    radioButtonEnglish.ForeColor = Color.Black;
                    break;
                case Languages.Italian:
                    radioButtonItalian.ForeColor = Color.Black;
                    break;
                case Languages.French:
                    radioButtonFrench.ForeColor = Color.Black;
                    break;
                case Languages.German:
                    radioButtonDetush.ForeColor = Color.Black;
                    break;
                case Languages.Spanish:
                    radioButtonSpanish.ForeColor = Color.Black;
                    break;
            }
            _curmessage.Text = (sender as TextBox).Text;
        }

        private void comboBoxSegment_TextUpdate(object sender, EventArgs e)
        {
            var text = (sender as ComboBox).Text;
            _cursegment = _language.GetSegment(text);
            if (_cursegment == null)
            {
                var index = (sender as ComboBox).SelectedIndex;
                _cursegment = _language.Segments[index];
            }
            if (_cursegment == null)
            {
                comboBoxMessage.Enabled = false;
                comboBoxMessage.DataSource = null;
            }
            else
            {
                comboBoxMessage.Enabled = true;
                comboBoxMessage.DataSource = _cursegment.Messages;
            }
        }

        private void comboBoxMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            _isMessageSelecting = true;
            var index = (sender as ComboBox).SelectedIndex;
            if (index >= 0)
            {
                IsMessageNew = false;
                _curmessage = _cursegment.Messages[index];
            }
            else {
                IsMessageNew = true;
                _curmessage = libTools.Language.Message.Empty;
            }
            textBoxText.Text = _curmessage.Text;
            _isMessageSelecting = false;
        }

        private void radioButtonEnglish_CheckedChanged(object sender, EventArgs e)
        {
            Lang.CurrentLanguage = Languages.English;
            (sender as ComboBox).ForeColor = Color.Black;
        }
        private void radioButtonItalian_CheckedChanged(object sender, EventArgs e)
        {
            Lang.CurrentLanguage = Languages.Italian;
            (sender as ComboBox).ForeColor = Color.Black;
        }
        private void radioButtonFrench_CheckedChanged(object sender, EventArgs e)
        {
            Lang.CurrentLanguage = Languages.French;
            (sender as ComboBox).ForeColor = Color.Black;
        }
        private void radioButtonDetush_CheckedChanged(object sender, EventArgs e)
        {
            Lang.CurrentLanguage = Languages.German;
            (sender as ComboBox).ForeColor = Color.Black;
        }
        private void radioButtonSpanish_CheckedChanged(object sender, EventArgs e)
        {
            Lang.CurrentLanguage = Languages.Spanish;
            (sender as ComboBox).ForeColor = Color.Black;
        }
        private void radioButtonJapanese_CheckedChanged(object sender, EventArgs e)
        {
            (sender as ComboBox).ForeColor = Color.Black;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            _cursegment.Messages.Add(_curmessage);
            _language.Save();
        }
    }
}
