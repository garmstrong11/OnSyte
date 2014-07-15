namespace Crypto
{
	using System.IO;
	using System.Threading.Tasks;

	public interface ICryptoProvider
	{
		Task EncryptToHmpAsync(FileInfo fileInfo);
		Task DecryptToJpgAsync(FileInfo fileInfo);
	}
}