namespace OnSyte.Ui.ViewModels
{
	using System;
	using System.IO;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Caliburn.Micro;
	using Crypto;
	using Infra;
	using Screen = Caliburn.Micro.Screen;

	public class ShellViewModel : Screen, IShell
	{
		private string _sourcePath;
		private ICryptoProvider _blowfish;
		private BindableCollection<FileInfo> _files;
		private CryptoMode _cryptoMode;
		private bool _encryptChecked;
		private bool _decryptChecked;

		public ShellViewModel(ICryptoProvider blowfish)
		{
			Files = new BindableCollection<FileInfo>();
			_blowfish = blowfish;
		}

		public BindableCollection<FileInfo> Files	
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

		public void BrowseFolderAsync()
		{
			var browser = new FolderBrowserDialog
				{
				RootFolder = Environment.SpecialFolder.Desktop,
				SelectedPath = @"\\Nasdee\Ryan\HortRoot\Lowes\L36 TagXpress\jpg"
				};

			var folderResult = browser.ShowDialog();
			if (!folderResult.Equals(DialogResult.OK)) return;

			//var dir = new DirectoryInfo(browser.SelectedPath);
			SourcePath = browser.SelectedPath;
			UpdateFiles();
		}

		public string SourcePath
		{
			get { return _sourcePath; }
			set
			{
				if (value == _sourcePath) return;
				_sourcePath = value;
				NotifyOfPropertyChange();
			}
		}

		public async Task PerformCrypto()
		{
			var count = Files.Count - 1;

			for (var i = count; i >= 0; i--) {
				switch (CryptoMode) {
					case CryptoMode.Encrypt:
						await _blowfish.EncryptToHmpAsync(Files[i]);
						break;
					case CryptoMode.Decrypt:
						await _blowfish.DecryptToJpgAsync(Files[i]);
						break;
				}

				Files.Remove(Files[i]);
				//Files = new BindableCollection<FileInfo>(filesToHandle);
			}
		}

		private void UpdateFiles()
		{
			if (string.IsNullOrEmpty(SourcePath)) return;
			Files.Clear();
			var mask = CryptoMode == CryptoMode.Encrypt ? "*.jpg" : "*.hmp";
			var dir = new DirectoryInfo(SourcePath);

			Files.AddRange(dir.EnumerateFiles(mask, SearchOption.AllDirectories));
		}

		protected override void OnActivate()
		{
			DisplayName = "OnSyte Crypto";
			EncryptChecked = true;
		}
	}
}