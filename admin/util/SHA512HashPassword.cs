using System.Security.Cryptography;

namespace webapi.util;

public class Sha512HashPassword : HashPasswordBase
{
    private const int HashByteLengthConst = 64;


    [Obsolete]
    protected override byte[] ComputeHash(byte[] buffer)
    {
        SHA512 service;
        try
        {
            service = new SHA512CryptoServiceProvider();
        }
        catch (PlatformNotSupportedException)
        {
            service = new SHA512Managed();
        }

        return service.ComputeHash(buffer);
    }

    protected override int GetHashByteLength()
    {
        return HashByteLengthConst;
    }
}