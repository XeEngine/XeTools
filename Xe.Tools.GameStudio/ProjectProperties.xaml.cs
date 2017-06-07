using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Xe.Tools.GameStudio
{
	/// <summary>
	/// Interaction logic for ProjectProperties.xaml
	/// </summary>
	public partial class ProjectProperties : Window
	{
		private Project _project;

		public ProjectProperties(Project project)
		{
			InitializeComponent();
			_project = project;

			_name.Text = project.Name;
			_shortName.Text = project.ShortName;
            _company.Text = project.Company;
            _producer.Text = project.Producer;
            _copyright.Text = project.Copyright;
            _year.Text = project.Year.ToString();
            _versionMajor.Text = project.Version.Major.ToString();
            _versionMinor.Text = project.Version.Minor.ToString();
            _versionRevision.Text = project.Version.Revision.ToString();
            _versionInfo.Text = project.Version.Info.ToString();
        }

		private void Name_TextChanged(object sender, TextChangedEventArgs e)
		{
			_project.Name = (sender as TextBox).Text;
		}
		private void ShortName_TextChanged(object sender, TextChangedEventArgs e)
		{
			_project.ShortName = (sender as TextBox).Text;
		}
		private void Company_TextChanged(object sender, TextChangedEventArgs e)
		{
			_project.Company = (sender as TextBox).Text;
		}
		private void Producer_TextChanged(object sender, TextChangedEventArgs e)
		{
			_project.Producer = (sender as TextBox).Text;
		}
		private void Copyright_TextChanged(object sender, TextChangedEventArgs e)
		{
			_project.Copyright = (sender as TextBox).Text;
		}
		private void Year_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (int.TryParse((sender as TextBox).Text, out int result))
				_project.Year = result;
		}

		private void VersionMajor_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (int.TryParse((sender as TextBox).Text, out int result))
				_project.Version.Major = result;
		}
		private void VersionMinor_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (int.TryParse((sender as TextBox).Text, out int result))
				_project.Version.Minor = result;
		}
		private void VersionRevision_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (int.TryParse((sender as TextBox).Text, out int result))
				_project.Version.Revision = result;
		}
		private void VersionInfo_TextChanged(object sender, TextChangedEventArgs e)
		{
			_project.Version.Info = (sender as TextBox).Text;
		}

		private void TextBoxNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Utility.Helper.Wpf_NumberValidationTextBox(sender, e);
		}
	}
}
