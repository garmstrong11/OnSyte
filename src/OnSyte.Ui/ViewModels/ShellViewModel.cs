namespace OnSyte.Ui.ViewModels
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Caliburn.Micro;
	using Crypto;
	using Infra;
	using Screen = Caliburn.Micro.Screen;

	public class ShellViewModel : Screen, IShell
	{
		private string _sourcePath;
		private readonly ICryptoProvider _blowfish;
		private BindableCollection<FileItemViewModel> _files;
		private CryptoMode _cryptoMode;
		private bool _encryptChecked;
		private bool _decryptChecked;
		private string _destinationPath;
		private bool _hasSelection;

		public ShellViewModel(ICryptoProvider blowfish)
		{
			Files = new BindableCollection<FileItemViewModel>();
			_blowfish = blowfish;
		}

		public BindableCollection<FileItemViewModel> Files	
		{
			get { return _files; }
			set
			{
				if (Equals(value, _files)) return;
				_files = value;
				NotifyOfPropertyChange();
			}
		}

		public CryptoMode CryptoMode
		{
			get { return _cryptoMode; }
			set
			{
				if (value == _cryptoMode) return;
				_cryptoMode = value;
				NotifyOfPropertyChange();
				UpdateFiles();
			}
		}

		public bool EncryptChecked
		{
			get { return _encryptChecked; }
			set
			{
				if (value.Equals(_encryptChecked)) return;
				_encryptChecked = value;
				NotifyOfPropertyChange();
				CryptoMode = value ? CryptoMode.Encrypt : CryptoMode.Decrypt;
			}
		}

		public bool DecryptChecked
		{
			get { return _decryptChecked; }
			set
			{
				if (value.Equals(_decryptChecked)) return;
				_decryptChecked = value;
				NotifyOfPropertyChange(); 
				CryptoMode = value ? CryptoMode.Decrypt : CryptoMode.Encrypt;
			}
		}

		public void BrowseSourceFolder()
		{
			var browser = new FolderBrowserDialog
				{
				RootFolder = Environment.SpecialFolder.Desktop,
				SelectedPath = @"\\Nasdee\Ryan\HortRoot\Lowes\L36 TagXpress\jpg"
				};

			var folderResult = browser.ShowDialog();
			if (!folderResult.Equals(DialogResult.OK)) return;

			SourcePath = browser.SelectedPath;
			if (string.IsNullOrEmpty(DestinationPath)) {
				DestinationPath = SourcePath;
			}
		}

		public void BrowseDestinationFolder()
		{
			var browser = new FolderBrowserDialog
			{
				RootFolder = Environment.SpecialFolder.Desktop,
				SelectedPath = @"\\Nasdee\Ryan\HortRoot\Lowes\L36 TagXpress\jpg"
			};

			var folderResult = browser.ShowDialog();
			if (!folderResult.Equals(DialogResult.OK)) return;

			DestinationPath = browser.SelectedPath;
		}

		public string SourcePath
		{
			get { return _sourcePath; }
			set
			{
				if (value == _sourcePath) return;
				_sourcePath = value;
				NotifyOfPropertyChange();
				DestinationPath = _sourcePath;
				UpdateFiles();
			}
		}

		public string DestinationPath
		{
			get { return _destinationPath; }
			set
			{
				if (value == _destinationPath) return;
				_destinationPath = value;
				NotifyOfPropertyChange(() => DestinationPath);
			}
		}

		public async Task PerformCrypto(IList selectedItems)
		{
			foreach (var file in selectedItems)
			{
				var info = new FileInfo(((FileItemViewModel)file).FilePath);
				switch (CryptoMode)
				{
					case CryptoMode.Encrypt:
						await _blowfish.EncryptToHmpAsync(info, DestinationPath);
						break;
					case CryptoMode.Decrypt:
						await _blowfish.DecryptToJpgAsync(info, DestinationPath);
						break;
				}
			}
		}

		public bool HasSelection
		{
			get { return _hasSelection; }
			set
			{
				if (value.Equals(_hasSelection)) return;
				_hasSelection = value;
				NotifyOfPropertyChange(() => HasSelection);
			}
		}

		public void MonitorSelection(IList selectedItems)
		{
			HasSelection = selectedItems.Count > 0;
		}

		private void UpdateFiles()
		{
			if (string.IsNullOrEmpty(SourcePath)) return;
			Files.Clear();
			var mask = CryptoMode == CryptoMode.Encrypt ? "*.jpg" : "*.hmp";
			var dir = new DirectoryInfo(SourcePath);

			var viewModels = dir.EnumerateFiles(mask, SearchOption.AllDirectories)
				.Select(v => new FileItemViewModel(v.FullName));
			Files.AddRange(viewModels);
		}

		protected override void OnActivate()
		{
			DisplayName = "OnSyte Crypto";
			EncryptChecked = true;
		}
	}
}