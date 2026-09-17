using FluentResults;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado.Util;
using GeradorCertificados.Aplicacao.Modulos.Usuarios.Util;
using GeradorCertificados.Dominio.Compartilhado;
using GeradorCertificados.Dominio.Compartilhado.Auth;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;

public sealed record CadastrarCertificadoCommand(
    string Aluno,
    string NomeCurso,
    uint CargaHoraria,
    DateTime DataConclusao,
    string? CaminhoArquivo,
    DateTime? DataGeracao,
    StatusCertificado Status,
    string Email,
    string Senha
) : IRequest<Result<Guid>>;

public sealed class CadastrarCertificadoCommandHandler(
    IRepositorioCertificados repositorioCertificados,
    IGerenciadorDeIdentidade gerenciadorDeIdentidade
) : IRequestHandler<CadastrarCertificadoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CadastrarCertificadoCommand command,
    CancellationToken cancellationToken = default
    )
    {
        var certificado = new Certificado(
           Guid.CreateVersion7(),
           command.Aluno,
           command.NomeCurso,
           command.CargaHoraria,
           command.DataConclusao
        );

        var erros = certificado.Validar();

        if (erros.Count > 0)
            return Result.Fail(ErrosDeCertificado.Validacao(erros));

        try
        {
            UsuarioDto usuario = await gerenciadorDeIdentidade.CadastrarAsync(
                certificado.Id,
                command.Email,
                command.Senha
            );

            await repositorioCertificados.CadastrarAsync(certificado, cancellationToken);

            return Result.Ok(usuario.Id);
        }
        catch (ValidacaoDeIdentidadeException ex)
        {
            return Result.Fail(ErrosDeUsuario.ValidacaoDeIdentidade(ex.Campo, ex.Message));
        }
        catch (ConflitoDeIdentidadeException ex)
        {
            return Result.Fail(ErrosDeUsuario.ConflitoDeIdentidade(ex.Message));
        }
        catch (ConflitoDePersistenciaException)
        {
            await gerenciadorDeIdentidade.ExcluirAsync(certificado.Id);

            return Result.Fail(ErrosDeUsuario.CadastroDuplicado());
        }

    }
}

