namespace OnSyte.Ui.ViewModels
{
	using System.Collections;
	using System.IO;
	using Caliburn.Micro;
	using Crypto;
	using Infra;

	public class ProgressViewModel : Screen, IProgressViewModel
	{
		private int _progress;
		private string _currentFilename;
		private readonly ICryptoProvider _blowfish;

		public ProgressViewModel(ICryptoProvider blowfish)
		{
			_blowfish = blowfish;
		}

		protected override void OnActivate()
		{
			DisplayName = "Progress";
			ProcessFilesAsync();
		}

		public int Progress
		{
			get { return _progress; }
			set
			{
				if (value == _progress) return;
				_progress = value;
				NotifyOfPropertyChange();
			}
		}

		public string CurrentFilename
		{
			get { return _currentFilename; }
			set
			{
				if (value == _currentFilename) return;
				_currentFilename = value;
				NotifyOfPropertyChange();
			}
		}

		public int FileCount
		{
			get { return Items.Count; }
		}

		public async void ProcessFilesAsync()
		{
			foreach (var file in Items)
			{
				var info = new FileInfo(((FileItemViewModel)file).FilePath);
				CurrentFilename = info.Name;

				switch (CryptoMode)
				{
					case CryptoMode.Encrypt:
						await _blowfish.EncryptToHmpAsync(info, DestinationPath);
						break;
					case CryptoMode.Decrypt:
						await _blowfish.DecryptToJpgAsync(info, DestinationPath);
						break;
				}

				Progress += 1;
			}

			TryClose();
		}

		public CryptoMode CryptoMode { get; set; }
		public IList Items { get; set; }
		public string DestinationPath { get; set; }
	}
}