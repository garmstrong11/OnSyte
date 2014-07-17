namespace Crypto
{
	using System.IO;
	using System.Threading.Tasks;

	public interface ICryptoProvider
	{
		Task EncryptToHmpAsync(FileInfo sourceFile, string destinationPath);
		Task DecryptToJpgAsync(FileInfo sourceFile, string destinationPath);
	}
}