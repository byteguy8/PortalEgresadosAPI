using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace PortalEgresadosAPI;

[ApiController]
[Route("[controller]")]

public class UsuarioController : Controller
{
    [HttpPost]
    public IResult Login(LoginDTO login)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawEmail = context
                .Contactos
                .Include(e => e.Participante)
                .ThenInclude(p => p.Usuario)
                .ThenInclude(u => u.Rol)
                .FirstOrDefault(e => e.Nombre == login.email)
            ?? throw Utils.APIError(
                0,
                "No existe informacion para el email suministrado",
                StatusCodes.Status400BadRequest
            );

            var rawParticipante = rawEmail.Participante;
            var rawUsuario = rawParticipante.Usuario;

            var salt = rawUsuario.Salt
            ?? throw Utils.APIError(
                0,
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

            var byteSalt = Encoding.UTF8.GetBytes(salt);
            var bytePassword = Utils.HashPasswordWithSalt(login.password, byteSalt);
            var hexPassword = Convert.ToHexString(bytePassword);

            if (hexPassword != rawUsuario.Password)
            {
                throw Utils.APIError(
                    0,
                    "Email o password incorrectos",
                    StatusCodes.Status400BadRequest
                );
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var byteKey = Encoding.UTF8.GetBytes(Utils.key);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim("email", login.email),
                    new Claim("rol", rawUsuario.Rol.Nombre),
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(byteKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDes);

            return Results.Ok(tokenHandler.WriteToken(token));
        }
        catch (Exception ex)
        {
            return Utils.HandleError(ex);
        }
        finally
        {
            context?.Dispose();
        }
    }
}