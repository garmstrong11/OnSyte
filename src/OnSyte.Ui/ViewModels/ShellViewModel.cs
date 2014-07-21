namespace OnSyte.Ui.ViewModels
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Caliburn.Micro;
	using Infra;
	using Screen = Caliburn.Micro.Screen;

	public class ShellViewModel : Screen, IShell
	{
		private string _sourcePath;
		private BindableCollection<FileItemViewModel> _files;
		private CryptoMode _cryptoMode;
		private bool _encryptChecked;
		private bool _decryptChecked;
		private string _destinationPath;
		private bool _hasSelection;
		private readonly IProgressViewModel _progressViewModel;
		private readonly IWindowManager _windowManager;

		public ShellViewModel(IProgressViewModel progressViewModel, IWindowManager windowManager)
		{
			_progressViewModel = progressViewModel;
			_windowManager = windowManager;
			Files = new BindableCollection<FileItemViewModel>();
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

		public void HandleFolderDrag(object evtArgs)
		{
			var args = (System.Windows.DragEventArgs) evtArgs;

			var folderPaths = (string[])args.Data.GetData(DataFormats.FileDrop);
			var info =  new FileInfo(folderPaths[0]);

			if (!info.IsDirectory()) return;

			args.Effects = System.Windows.DragDropEffects.Link;
			args.Handled = true;
		}

		public void HandleFolderDrop(ActionExecutionContext ctx)
		{
			var args = (System.Windows.DragEventArgs)ctx.EventArgs;
			var boxName = ctx.Source.Name;
			var folderPaths = (string[])args.Data.GetData(DataFormats.FileDrop);
			var info = new FileInfo(folderPaths[0]);

			if (!info.IsDirectory()) return;

			args.Effects = System.Windows.DragDropEffects.Link;

			if (boxName == "DestinationPath") {
				DestinationPath = folderPaths[0];
			}
			else {
				SourcePath = folderPaths[0];
			}
			args.Handled = true;
		}

		public void PerformCrypto(IList selectedItems)
		{
			_progressViewModel.Items = selectedItems;
			_progressViewModel.CryptoMode = CryptoMode;
			_progressViewModel.DestinationPath = DestinationPath;
			_progressViewModel.Progress = 0;
			_progressViewModel.CurrentFilename = "";

			_windowManager.ShowDialog(_progressViewModel);
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