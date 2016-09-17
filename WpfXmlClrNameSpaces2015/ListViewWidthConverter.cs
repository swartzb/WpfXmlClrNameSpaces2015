using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfXmlClrNameSpaces2015
{
    [ValueConversion(typeof(ListView), typeof(double))]
    class ListViewWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListView lv = value as ListView;
            GridView gv = lv.View as GridView;
            double width = 0.0;
            foreach (GridViewColumn gvc in gv.Columns)
            {
                width += gvc.ActualWidth;
            }
            return width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
