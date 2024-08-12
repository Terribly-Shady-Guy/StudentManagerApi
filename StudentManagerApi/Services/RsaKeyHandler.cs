using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace StudentManagerApi.Services;

public class RsaKeyConfig
{
    public string? Public { get; set; }
    public string? Private { get; set; }

}
public class RsaKeyHandler
{
    private readonly IOptions<RsaKeyConfig> _config;
    private static readonly string _rsaDirectoryPath = Path.Combine(Environment.CurrentDirectory, "..", "rsa");

    public RsaKeyHandler(IOptions<RsaKeyConfig> config)
    {
        _config = config;
    }

    public async Task CreateKey()
    {
        if (Directory.Exists(_rsaDirectoryPath))
        {
            return;
        }
        Directory.CreateDirectory(_rsaDirectoryPath);

        var rsa = RSA.Create();

        string publicKeyXml = rsa.ToXmlString(false);
        string privateKeyXml = rsa.ToXmlString(true);

        await File.WriteAllTextAsync(Path.Combine(_rsaDirectoryPath, _config.Value.Public + ".xml"), publicKeyXml);
        await File.WriteAllTextAsync(Path.Combine(_rsaDirectoryPath, _config.Value.Private + ".xml"), privateKeyXml);
    }

    public async Task<RsaSecurityKey?> GetPublicKey()
    {
        string publicKeyPath = Path.Combine(_rsaDirectoryPath, _config.Value.Public + ".xml");

        if (!File.Exists(publicKeyPath))
        {
            return null;
        }

        string publicKeyXml = await File.ReadAllTextAsync(publicKeyPath);

        var rsa = RSA.Create();
        rsa.FromXmlString(publicKeyXml);

        return new RsaSecurityKey(rsa);
    }

    public async Task<RsaSecurityKey?> GetPrivateKey()
    {
        string privateKeyPath = Path.Combine(_rsaDirectoryPath, _config.Value.Private + ".xml");

        if (!File.Exists(privateKeyPath))
        {
            return null;
        }

        string privateKeyXml = await File.ReadAllTextAsync(privateKeyPath);

        var rsa = RSA.Create();
        rsa.FromXmlString(privateKeyXml);

        return new RsaSecurityKey(rsa);
    }
}
