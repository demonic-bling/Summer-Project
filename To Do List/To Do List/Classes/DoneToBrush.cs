using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace To_Do_List
{
    class DoneToBrush : IValueConverter
    {

        Color color = (Color) App.Current.Resources["SystemColorControlAccentColor"];
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool val = (bool) value;

            switch(val)
            {
                case true:
                    return new SolidColorBrush(Colors.White);
                case false:
                    return new SolidColorBrush(color);
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
