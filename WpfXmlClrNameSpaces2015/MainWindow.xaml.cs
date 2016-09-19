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
using System.ComponentModel;

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

            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(lstview.ItemsSource);
            dataView.Filter = new Predicate<object>(XmlNamespaceFilter);
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void lv_ColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked =
                  e.OriginalSource as GridViewColumnHeader;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    ListSortDirection direction;
                    string upTriangle = " " + '\u25B2'.ToString();
                    string downTriangle = " " + '\u25BC'.ToString();

                    // Remove triangle from previously sorted header
                    if (_lastHeaderClicked != null)
                    {
                        string lastHeader = _lastHeaderClicked.Column.Header as string;
                        if (lastHeader.Contains(upTriangle))
                        {
                            _lastHeaderClicked.Column.Header = lastHeader.Replace(upTriangle, "");
                        }
                        else if (lastHeader.Contains(downTriangle))
                        {
                            _lastHeaderClicked.Column.Header = lastHeader.Replace(downTriangle, "");
                        }
                    }

                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    switch (header)
                    {
                        case "XML Namespace":
                            Sort("xmlNs", direction);
                            break;
                        case "CLR Namespace":
                            Sort("clrNs", direction);
                            break;
                        case "Assembly":
                            Sort("assemName", direction);
                            break;
                    }

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.Header = header + upTriangle;
                    }
                    else
                    {
                        headerClicked.Column.Header = header + downTriangle;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(lstview.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(lstview.ItemsSource);

            dataView.Refresh();
        }

        bool XmlNamespaceFilter(object obj)
        {
            return (string.IsNullOrEmpty(filterText.Text)) ? true : (obj as XmlClrNameSpaceItem).xmlNs.Contains(filterText.Text);
        }
    }
}
