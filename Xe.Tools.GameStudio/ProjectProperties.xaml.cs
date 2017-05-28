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
			_shortName.Text = project.Shortname;
		}

		private void Name_TextChanged(object sender, TextChangedEventArgs e)
		{
			_project.Name = (sender as TextBox).Text;
		}
	}
}
