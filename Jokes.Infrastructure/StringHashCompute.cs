using System.Text;

namespace Jokes.Infrastructure;

internal static class StringHashCompute
{
    internal static string ComputeHashChecksumFromText(string text)
    {
        string checksum;

        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            checksum = BitConverter.ToString(
                md5.ComputeHash(Encoding.UTF8.GetBytes(text))
            ).Replace("-", string.Empty);
        }

        return checksum;
    }
}