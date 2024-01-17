using AspDigitalMemoSlip.Domain;
using Microsoft.AspNetCore.Identity;
using OtpNet;
using System.Security.Cryptography;

public class AuthenticatorTokenProvider<TUser> : IUserTwoFactorTokenProvider<User>
{

    private readonly UserManager<User> userManager;

    public AuthenticatorTokenProvider(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    /// <summary>
    /// Checks if two-factor authentication is enabled for the user.
    /// </summary>
    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
    {
        return Task.FromResult(user.TwoFactorEnabled);
    }

    /// <summary>
    /// Generates a Time-based One-Time Password (TOTP) for the user.
    /// </summary>
    public async Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
    {
        var totp = await GenerateTOTP(user);
        return await Task.FromResult(totp);
    }

    /// <summary>
    /// Validates the TOTP entered by the user.
    /// </summary>
    public Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
    {
        var isValid = ValidateTOTP(user, token);
        return Task.FromResult(isValid);
    }

    /// <summary>
    /// Generates a TOTP for the user.
    /// </summary>
    private async Task<string> GenerateTOTP(User user)
    {
        byte[] secretKey = new byte[20];
        RandomNumberGenerator.Fill(secretKey);

        string secretKeyBase32 = Base32Encoding.ToString(secretKey);

        user.TwoFactorSecretKey = secretKeyBase32;
        await userManager.UpdateAsync(user);

        var totp = new Totp(secretKey);

        var totpCode = totp.ComputeTotp();

        var issuer = "ASPDigitalMemoSlip";
        //Should make the qr code work for different authenticators:
        //Tested authenticators: Miscrosoft Authenticator, Google Authenticator
        var url = $"otpauth://totp/{user.UserName}?secret={secretKeyBase32}&issuer={issuer}";

        return url;
    }

    /// <summary>
    /// Validates the TOTP entered by the user.
    /// </summary>
    private bool ValidateTOTP(User user, string token)
    {
        byte[] secretKey = Base32Encoding.ToBytes(user.TwoFactorSecretKey);

        var totp = new Totp(secretKey);

        bool isValid = totp.VerifyTotp(token, out long timeStepMatched, new VerificationWindow(2, 2));

        return isValid;
    }
}
