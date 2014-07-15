namespace OnSyte.Tests.Integration
{
	using System.IO;
	using Crypto;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class BlowfishTests
	{
		private const string testDir = @"\\Nasdee\Ryan\HortRoot\Lowes\L36 TagXpress\jpg\BFN 76232";
		private const string stringKey = @"©jÚD<ÒâáG»ÀÚ¤©Z/L~hyÿð}ÃøjÓÍÛXŽ:jí*ò.QIè®¨¹Y£Â!&±6ÎðÎsþ¶";
		
		
		[Test]
		public async void CanRoundTrip()
		{
			const string testJpgName = "413250_BACK_test.jpg";
			const string testHmpName = "413250_BACK_test.hmp";
			const string origJpgName = "413250_BACK.jpg";

			var testOrigFile = new FileInfo(Path.Combine(testDir, origJpgName));
			var fileToEncrypt = testOrigFile.CopyTo(Path.Combine(testDir, testJpgName));
			var blo = new Blowfish(stringKey);

			await blo.EncryptToHmpAsync(fileToEncrypt);
			await blo.DecryptToJpgAsync(new FileInfo(Path.Combine(testDir, testHmpName)));

			var actualBytes = File.ReadAllBytes(Path.Combine(testDir, testJpgName));
			var expectedBytes = File.ReadAllBytes(Path.Combine(testDir, origJpgName));

			actualBytes.Should().Equal(expectedBytes);
		}

		[Test]
		public async void CanDecryptOldFile()
		{
			const string origDir = @"\\Nasdee\Ryan\HortRoot\Lowes\L36 TagXpress\jpg\McCorkle 72971";
			const string origHmpFileName = "88007_BACK.hmp";
			const string origJpgFileName = "88007_BACK.jpg";

			var blo = new Blowfish(stringKey);

			var origJpgFile = new FileInfo(Path.Combine(origDir, origJpgFileName));
			var origHmpFile = new FileInfo(Path.Combine(origDir, origHmpFileName));

			var testHmpFile = origHmpFile.CopyTo(Path.Combine(testDir, origHmpFileName));

			await blo.DecryptToJpgAsync(testHmpFile);

			//var actualBytes = File.ReadAllBytes();
		}
	}
}