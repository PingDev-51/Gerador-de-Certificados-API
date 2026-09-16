namespace GeradorCertificados.Dominio.Compartilhado.Auth;

public interface IProvedorDeUsuario
{
    Guid? Id { get; }
    string? Email { get; }


}
