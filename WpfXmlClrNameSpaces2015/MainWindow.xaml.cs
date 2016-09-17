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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Windows.Markup;
using System.Collections.ObjectModel;

namespace WpfXmlClrNameSpaces2015
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<XmlClrNameSpaceItem> XmlClrNameSpaceItems { get; set; }

        public MainWindow()
        {
            List<XmlClrNameSpaceItem> XmlClrNameSpaceList = AppDomain.CurrentDomain.GetAssemblies().
                SelectMany(assm => assm.GetCustomAttributes(typeof(XmlnsDefinitionAttribute), false).
                    OfType<XmlnsDefinitionAttribute>().
                    Select(xda => new XmlClrNameSpaceItem { assem = assm, xmlNs = xda.XmlNamespace, clrNs = xda.ClrNamespace })).
                ToList<XmlClrNameSpaceItem>();

            XmlClrNameSpaceItems = new ObservableCollection<XmlClrNameSpaceItem>(XmlClrNameSpaceList);

            InitializeComponent();
        }
    }
}
