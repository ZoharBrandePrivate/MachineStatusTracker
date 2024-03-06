using System;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MachineStatusTracker.ViewModels;

namespace MachineStatusTracker.Converters
{
    internal class StatusToImagePathConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage bitmapImage = null;
            MachineOperationalStatus status;
            if (value == null)
            {
                status = MachineOperationalStatus.Unavailable;
            }
            else
            {
                status = (MachineOperationalStatus)value;
            }
            if (status == null)
            {
                status = MachineOperationalStatus.Unavailable;
            }
            string imageResourceName = "StatusImage" + status.ToString();
            FrameworkElement fe = new FrameworkElement();

            try
            {
                bitmapImage = (BitmapImage)fe.TryFindResource(imageResourceName);
            }
            catch (Exception)
            {
                bitmapImage = null;
            }
            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IValueConverter Members
    }
}
