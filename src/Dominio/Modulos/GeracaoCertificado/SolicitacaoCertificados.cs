using GeradorCertificados.Dominio.Compartilhado;

namespace GeradorCertificados.Dominio.Modulos.GeracaoCertificado;

public class SolicitacaoCertificados : EntidadeBase<SolicitacaoCertificados>
{
    public Guid CursoId { get; private set; }

    public StatusGeracaoCertificados Status { get; private set; }

    public string? CaminhoZip { get; private set; }

    public DateTime DataSolicitacao { get; private set; }

    public DateTime? DataConclusao { get; private set; }

    public List<Certificado> Certificados { get; private set; } = [];

    public SolicitacaoCertificados()
    {

    }

    public SolicitacaoCertificados(
     Guid cursoId,
     string nomeCurso,
     uint cargaHoraria,
     DateTime dataConclusao,
     List<string> nomesAlunos)
    {
        CursoId = cursoId;
        DataSolicitacao = DateTime.UtcNow;
        Status = StatusGeracaoCertificados.Pendente;

        foreach (var nomeAluno in nomesAlunos)
        {
            Certificados.Add(
                new Certificado(
                    Id,
                    nomeAluno,
                    nomeCurso,
                    cargaHoraria,
                    dataConclusao
                )
            );
        }
    }

    public void IniciarGeracao()
    {
        Status = StatusGeracaoCertificados.GerandoCertificados;
    }

    public void IniciarGeracaoZip()
    {
        Status = StatusGeracaoCertificados.GerandoZip;
    }

    public void MarcarComoConcluida(string caminhoZip)
    {
        CaminhoZip = caminhoZip;
        DataConclusao = DateTime.Now;
        Status = StatusGeracaoCertificados.Concluido;
    }

    public void MarcarComoFalha()
    {
        Status = StatusGeracaoCertificados.Falha;
        DataConclusao = DateTime.Now;
    }

    public bool EstaProcessando()
    {
        return Status == StatusGeracaoCertificados.Pendente ||
               Status == StatusGeracaoCertificados.GerandoCertificados ||
               Status == StatusGeracaoCertificados.GerandoZip;
    }

    public override void Atualizar(SolicitacaoCertificados entidadeAtualizada)
    {
        Status = entidadeAtualizada.Status;
        CaminhoZip = entidadeAtualizada.CaminhoZip;
        DataConclusao = entidadeAtualizada.DataConclusao;
    }

    public override IReadOnlyList<ErroValidacao> Validar()
    {
        List<ErroValidacao> erros = [];

        if (CursoId == Guid.Empty)
            erros.Add(new(nameof(CursoId), "O curso deve ser informado"));

        if (Certificados.Count == 0)
            erros.Add(new(nameof(Certificados),
                "A solicitação deve possuir pelo menos um aluno"));

        return erros;
    }
}
