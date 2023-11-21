using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace PortalEgresadosAPI;

[ApiController]
[Route("[controller]")]
public class UsuarioController : Controller
{
    [HttpPost("Email")]
    public IResult IniciarSesion([FromBody] LoginDTO login)
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
                .FirstOrDefault(e => e.Nombre == login.Identificacion)
            ?? throw Utils.APIError(
                0,
                "No existe informacion para la combinacion de email y password suministrados",
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

            var byteSalt = Convert.FromHexString(salt);
            var bytePassword = Utils.HashPasswordWithSalt(login.Password, byteSalt);
            var hexPassword = Convert.ToHexString(bytePassword);

            if (hexPassword != rawUsuario.Password)
            {
                throw Utils.APIError(
                    0,
                    "No existe informacion para la combinacion de email y password suministrados",
                    StatusCodes.Status400BadRequest
                );
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var byteKey = Encoding.UTF8.GetBytes(Utils.key);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim("identificacion", login.Identificacion),
                    new Claim("rol", rawUsuario.Rol.Nombre)
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

    [Authorize]
    [HttpPut("Email")]
    public IResult ActualizarPassword([FromBody] CambiarPasswordDTO login)
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
                .FirstOrDefault(e => e.Nombre == login.Identificacion)
            ?? throw Utils.APIError(
                0,
                "No existe informacion para la identificacion suministrada",
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

            var byteSalt = Convert.FromHexString(salt);
            var bytePassword = Utils.HashPasswordWithSalt(login.PasswordVieja, byteSalt);
            var hexPassword = Convert.ToHexString(bytePassword);

            if (hexPassword != rawUsuario.Password)
            {
                throw Utils.APIError(
                    0,
                    "Password anterior incorrecta",
                    StatusCodes.Status400BadRequest
                );
            }

            var newByteSalt = Utils.GenerateSalt();
            var newBytePassword = Utils.HashPasswordWithSalt(login.PasswordNueva, newByteSalt);

            rawUsuario.Salt = Convert.ToHexString(newByteSalt);
            rawUsuario.Password = Convert.ToHexString(newBytePassword);

            context.SaveChanges();

            return Results.Ok(true);
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