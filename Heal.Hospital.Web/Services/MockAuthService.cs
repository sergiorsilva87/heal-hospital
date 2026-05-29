using System.Security.Claims;

namespace Heal.Hospital.Web.Services;

/// <summary>Mock in-memory user store for wireframe authentication.</summary>
public class MockAuthService
{
    public const string MfaCode = "123456";

    private static readonly MockUser[] Users =
    [
        new("receptionist",    "Heal@2026", "Receptionist",        "Ana Costa"),
        new("tecnico.rad",     "Heal@2026", "RadiologyTechnician", "Carlos Mendes"),
        new("gerente",         "Heal@2026", "HospitalManager",     "Marina Silva"),
        new("financeiro",      "Heal@2026", "HospitalFinancial",   "Roberto Lima"),
        new("dr.solicitante",  "Heal@2026", "RequestingPhysician", "Dr. Paulo Ferreira"),
        new("dr.auxiliar",     "Heal@2026", "MedicalAuxiliary",    "Dra. Camila Rocha"),
        new("digitador",       "Heal@2026", "Transcriptionist",    "João Batista"),
    ];

    /// <summary>Returns the user if username + password match; null otherwise.</summary>
    public MockUser? Validate(string username, string password) =>
        Users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)
            && u.Password == password);

    /// <summary>Looks up a user by username (used after MFA validation).</summary>
    public static MockUser? FindByUsername(string username) =>
        Users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

    /// <summary>Builds the ClaimsPrincipal used to sign in.</summary>
    public static ClaimsPrincipal BuildPrincipal(MockUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name,  user.Username),
            new Claim(ClaimTypes.Role,  user.Role),
            new Claim("FullName",       user.FullName),
        };
        var identity = new ClaimsIdentity(claims, "HospitalCookies");
        return new ClaimsPrincipal(identity);
    }
}

public record MockUser(string Username, string Password, string Role, string FullName);
