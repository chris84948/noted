using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotedUI.UI
{
    /// <summary>
    /// Interaction logic for FormatMenu.xaml
    /// </summary>
    public partial class FormatMenu : UserControl
    {
        // TODO this is just a placeholder for now. Likely I'll be passing a textbox
        // But I may want to change it, so for now it's just an object
        private object _commandParameter;

        public static readonly DependencyProperty Header1CommandProperty = 
            DependencyProperty.Register("Header1Command", 
                                        typeof(ICommand), 
                                        typeof(FormatMenu),
                                        new PropertyMetadata((ICommand)null));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand Header1Command
        {
            get { return (ICommand)GetValue(Header1CommandProperty); }
            set { SetValue(Header1CommandProperty, value); }
        }



        public FormatMenu()
        {
            InitializeComponent();
        }

        private void Header1_Click(object sender, RoutedEventArgs e)
        {
            Header1Command?.Execute(_commandParameter);
        }

        private void Header2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Header3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Header4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Header5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Header6_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Bold_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Strikethrough_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Quote_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Code_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BulletPoint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void List_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Link_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
