using System.Security.Cryptography;
using System.Text;

public class Utils
{
    public static string key = "zFx5iAfzYwzVs6mdamU9GQ3amdx+iftwkLD70Abq8kEfI77TEUzXgLClzMnFOZml09nnpKpeYkdYRzKNkK+nLg==";
    
    public static IResult HandleError(Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        Console.Error.WriteLine(ex.ToString());

        if (ex is APIException error)
        {
            return Results.Json(
                data: new ErrorMsg(error.Code, error.Message),
                statusCode: error.Status
            );
        }

        return Results.Json(
            data: new ErrorMsg(0, "Hubo un error al procesar la solicitud. Intentelo de nuevo"),
            statusCode: StatusCodes.Status500InternalServerError
        );
    }

    public static APIException APIError(int code, string message, int status)
    {
        return new APIException(code, status, message);
    }

    public static byte[] GenerateSalt(int size)
    {
        byte[] salt = new byte[size];

        using var rand = RandomNumberGenerator.Create();

        rand.GetBytes(salt);

        return salt;
    }

    public static byte[] HashPasswordWithSalt(string password, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            salt,
            10000,
            HashAlgorithmName.SHA512
        );

        return pbkdf2.GetBytes(32);
    }

    public static byte[] HASH256(string input)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        return SHA256.HashData(inputBytes);
    }
}