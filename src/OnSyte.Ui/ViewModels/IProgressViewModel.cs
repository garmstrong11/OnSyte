namespace OnSyte.Ui.ViewModels
{
	using System.Collections;
	using System.Threading.Tasks;
	using Infra;

	public interface IProgressViewModel
	{
		int Progress { get; set; }
		string CurrentFilename { get; set; }
		int FileCount { get; }
		void ProcessFilesAsync();
		CryptoMode CryptoMode { get; set; }
		IList Items { get; set; }
		string DestinationPath { get; set; }
	}
}