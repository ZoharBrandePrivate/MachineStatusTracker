using System;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MachineStatusTracker.ViewModels;

namespace MachineStatusTracker.Converters
{
    /// <summary>
    /// Converts the machine operational status to an image path.
    /// </summary>
    internal class StatusToImagePathConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts the machine operational status to an image path.
        /// </summary>
        /// <param name="value">The machine operational status.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The image path corresponding to the machine status.</returns>
        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage bitmapImage = null;
            MachineOperationalStatus status;

            // Default status to Unavailable if value is null
            if (value == null)
            {
                status = MachineOperationalStatus.Unavailable;
            }
            else
            {
                status = (MachineOperationalStatus)value;
            }

            // Set status to Unavailable if still null
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

        /// <summary>
        /// Converts the image path back to machine operational status (not implemented).
        /// </summary>
        /// <param name="value">The image path.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>NotImplementedException.</returns>
        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IValueConverter Members
    }
}
