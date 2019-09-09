using System;
using System.Collections.Generic;
using System.IO;
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
using Database;

namespace StatusBoard
{
    /// <summary>
    /// Interaction logic for StatusPage.xaml
    /// </summary>
    public partial class StatusPageControl : UserControl
    {
        public static DependencyProperty StatusPageTitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(StatusPageControl));

        public static DependencyProperty StatusPageDescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(StatusPageControl));

        public static DependencyProperty StatusPageBackgroundImageProperty =
            DependencyProperty.Register("BackgroundImage", typeof(string), typeof(StatusPageControl));

        public static DependencyProperty StatusPageEquipmentGroupsProperty =
            DependencyProperty.Register("EquipmentGroups", typeof(ICollection<StatusPageGrouping>), typeof(StatusPageControl));

        public static DependencyProperty StatusPageIsDisplayedProperty =
            DependencyProperty.Register("IsDisplayed", typeof(bool), typeof(StatusPageControl));

        public string Title
        {
            get { return (string)GetValue(StatusPageTitleProperty); }
            set { SetValue(StatusPageTitleProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(StatusPageDescriptionProperty); }
            set { SetValue(StatusPageDescriptionProperty, value); }
        }

        public string BackgroundImage
        {
            get { return (string)GetValue(StatusPageBackgroundImageProperty); }
            set
            {
                SetValue(StatusPageBackgroundImageProperty, value);

                if (!string.IsNullOrEmpty(value))
                    if (File.Exists(value))
                        backgroundImage.Source = new BitmapImage(new Uri(value));
            }
        }

        internal void Refresh(ICollection<StatusPageGrouping> statusPageGrouping)
        {
            EquipmentGroups = statusPageGrouping;
        }

        public ICollection<StatusPageGrouping> EquipmentGroups
        {
            get { return (ICollection<StatusPageGrouping>)GetValue(StatusPageEquipmentGroupsProperty); }
            set { SetValue(StatusPageEquipmentGroupsProperty, value); }
        }

        public bool IsDisplayed
        {
            get { return (bool) GetValue(StatusPageIsDisplayedProperty); }
            set { SetValue(StatusPageIsDisplayedProperty, value); }
        }


        public StatusPageControl()
        {
            InitializeComponent();
        }

        public readonly int StatusPageId;
        public StatusPageControl(StatusPage statusPage)
        {
            InitializeComponent();

            StatusPageId = statusPage.Id;
            Title = statusPage.Title;
            Description = statusPage.Description;
            BackgroundImage = statusPage.BackgroundImage;
            EquipmentGroups = statusPage.StatusPageGroupings;
        }
    }
}
