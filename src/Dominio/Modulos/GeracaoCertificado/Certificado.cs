using GeradorCertificados.Dominio.Compartilhado;

namespace GeradorCertificados.Dominio.Modulos.GeracaoCertificado;

public class Certificado : EntidadeBase<Certificado>
{
    public Guid SolicitacaoId { get; private set; }
    public string NomeAluno { get; private set; } = string.Empty;

    public string NomeCurso { get; private set; } = string.Empty;

    public uint CargaHoraria { get; private set; }

    public DateTime DataConclusao { get; private set; }

    public string? CaminhoArquivo { get; private set; }

    public DateTime? DataGeracao { get; private set; }

    public StatusCertificado Status { get; private set; }

    public Certificado()
    {

    }

    public Certificado(
        Guid solicitacaoId,
        string nomeAluno,
        string nomeCurso,
        uint cargaHoraria,
        DateTime dataConclusao)
    {
        NomeAluno = nomeAluno;
        NomeCurso = nomeCurso;
        CargaHoraria = cargaHoraria;
        DataConclusao = dataConclusao;
        Status = StatusCertificado.Pendente;
    }

    public void MarcarComoGerado(string caminhoArquivo)
    {
        CaminhoArquivo = caminhoArquivo;
        DataGeracao = DateTime.UtcNow;
        Status = StatusCertificado.Gerado;
    }

    public void MarcarComoFalha()
    {
        Status = StatusCertificado.Falha;
    }

    public override void Atualizar(Certificado entidadeAtualizada)
    {
        NomeAluno = entidadeAtualizada.NomeAluno;
        NomeCurso = entidadeAtualizada.NomeCurso;
        CargaHoraria = entidadeAtualizada.CargaHoraria;
        DataConclusao = entidadeAtualizada.DataConclusao;
    }

    public override IReadOnlyList<ErroValidacao> Validar()
    {
        List<ErroValidacao> erros = [];

        if (string.IsNullOrWhiteSpace(NomeAluno))
            erros.Add(new(nameof(NomeAluno), "O nome do aluno deve ser preenchido"));

        if (NomeAluno.Length > 200)
            erros.Add(new(nameof(NomeAluno), "O nome do aluno deve possuir no máximo 200 caracteres"));

        if (string.IsNullOrWhiteSpace(NomeCurso))
            erros.Add(new(nameof(NomeCurso), "O nome do curso deve ser preenchido"));

        return erros;
    }
}
