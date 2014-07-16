namespace OnSyte.Ui.ViewModels
{
	using System.IO;
	using Caliburn.Micro;

	public class FileItemViewModel : PropertyChangedBase
	{
		private readonly string _path;
		private bool _selected;

		public FileItemViewModel(string path)
		{
			_path = path;
			Selected = true;
		}

		public string FileName
		{
			get { return Path.GetFileName(_path); }
		}

		public string FilePath
		{
			get { return _path; }
		}

		public bool Selected
		{
			get { return _selected; }
			set
			{
				if (value.Equals(_selected)) return;
				_selected = value;
				NotifyOfPropertyChange();
			}
		}
	}
}