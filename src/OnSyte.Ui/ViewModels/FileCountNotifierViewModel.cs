namespace OnSyte.Ui.ViewModels
{
	using Caliburn.Micro;

	public class FileCountNotifierViewModel : PropertyChangedBase
	{
		private string _notificationText;

		public FileCountNotifierViewModel(string text)
		{
			NotificationText = text;
		}

		public string NotificationText
		{
			get { return _notificationText; }
			set
			{
				if (value == _notificationText) return;
				_notificationText = value;
				NotifyOfPropertyChange();
			}
		}
	}
}