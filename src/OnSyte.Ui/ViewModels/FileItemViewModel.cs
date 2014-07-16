namespace OnSyte.Ui.ViewModels
{
	using System.IO;
	using Caliburn.Micro;

	public class FileItemViewModel : PropertyChangedBase
	{
		private readonly FileInfo _fileInfo;
		private bool _selected;

		public FileItemViewModel(FileInfo fileInfo)
		{
			_fileInfo = fileInfo;
			Selected = true;
		}

		public string FileName
		{
			get { return _fileInfo.Name; }
		}

		public FileInfo FileInfo
		{
			get { return _fileInfo; }
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