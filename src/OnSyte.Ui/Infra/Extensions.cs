namespace OnSyte.Ui.Infra
{
	using System.IO;

	public static class Extensions
	{
		public static bool IsDirectory(this FileInfo info)
		{
			return (info.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
		}
	}
}