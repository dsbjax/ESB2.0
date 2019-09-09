using Database;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace StatusBoard
{
    /// <summary>
    /// Interaction logic for EquipmentStatusIndicator.xaml
    /// </summary>
    public partial class EquipmentStatusIndicator : UserControl
    {
        public static DependencyProperty EquipmentStatusProperty =
            DependencyProperty.Register("EquipmentStatus", typeof(EquipmentStatus), typeof(EquipmentStatusIndicator));

        public static DependencyProperty NomenclatureProperty =
            DependencyProperty.Register("Nomenclature", typeof(string), typeof(EquipmentStatusIndicator));

        public EquipmentStatus EquipmentStatus
        {
            get { return (EquipmentStatus) GetValue(EquipmentStatusProperty); }
            set { SetValue(EquipmentStatusProperty, value); }
        }

        public string Nomenclature
        {
            get { return (string) GetValue(NomenclatureProperty); }
            set { SetValue(NomenclatureProperty, value); }
        }

        public EquipmentStatusIndicator()
        {
            InitializeComponent();
        }
    }

    public class GetDarkColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = Colors.DarkGreen;

            if(value != null)
                switch((EquipmentStatus)value)
                {
                    case EquipmentStatus.Degraded:
                        color = Colors.DarkOrange;
                        break;

                    case EquipmentStatus.Down:
                        color = Colors.DarkRed;
                        break;

                    case EquipmentStatus.OffLine:
                        color = Colors.DarkGray;
                        break;
                }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GetColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = Colors.Green;

            if(value != null)
                switch ((EquipmentStatus)value)
                {
                    case EquipmentStatus.Degraded:
                        color = Colors.Orange;
                        break;

                    case EquipmentStatus.Down:
                        color = Colors.Red;
                        break;

                    case EquipmentStatus.OffLine:
                        color = Colors.Gray;
                        break;
                }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
