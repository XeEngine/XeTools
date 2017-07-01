namespace libTools.Forms
{
    public class ComboBoxEx : System.Windows.Forms.ComboBox
    {
        public void DataSourceRefresh()
        {
            if (DataSource == null) return;
            var index = SelectedIndex;
            var data = DataSource;
            DataSource = null;
            DataSource = data;
            SelectedIndex = index;
        }
    }
}
