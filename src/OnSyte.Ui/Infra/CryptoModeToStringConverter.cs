namespace OnSyte.Ui.Infra
{
	using System;
	using System.Globalization;
	using System.Windows.Data;

	public class CryptoModeToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var mode = (CryptoMode)value;
			//if (mode == null) return "Unknown";
			return mode == CryptoMode.Decrypt ? "Decrypt Files" : "Encrypt Files";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}