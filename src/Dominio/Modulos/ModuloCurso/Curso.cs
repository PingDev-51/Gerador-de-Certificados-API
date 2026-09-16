using GeradorCertificados.Dominio.Compartilhado;
using GeradorCertificados.Dominio.Compartilhado.Auth;

namespace GeradorCertificados.Dominio.Modulos.ModuloCurso;

public class Curso : EntidadeBase<Curso>, IEntidadeDeUsuario
{
    public Guid UsuarioId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public uint? CargaHoraria { get; set; }
    public DateTime? DataConclusao { get; set; }

    public Curso() { }

    public Curso(string nome, uint cargaHoraria, DateTime dataConclusao, string? descricao = null)
    {
        Nome = nome;
        CargaHoraria = cargaHoraria;
        DataConclusao = dataConclusao;
        Descricao = descricao;
    }

    public override void Atualizar(Curso entidadeAtualizada)
    {
        Nome = entidadeAtualizada.Nome;
        Descricao = entidadeAtualizada.Descricao;
        CargaHoraria = entidadeAtualizada.CargaHoraria;
        DataConclusao = entidadeAtualizada.DataConclusao;
    }

    public override IReadOnlyList<ErroValidacao> Validar()
    {
        List<ErroValidacao> erros = [];


        if (Nome == null)
            erros.Add(new(nameof(Nome), "O Camnpo Nome deve ser preenchido"));

        if (Nome?.Length < 2)
            erros.Add(new(nameof(Nome), "O campo Nome deve conter mais de 2 caracteres"));

        if (Nome?.Length > 200)
            erros.Add(new(nameof(Nome), "O campo Nome deve conter menos de 200 caracteres"));

        if (Descricao == null)
            erros.Add(new(nameof(Descricao), "O Camnpo descrição deve ser preenchido"));

        if (Descricao?.Length < 2)
            erros.Add(new(nameof(Descricao), "O campo descrição deve conter mais de 2 caracteres"));

        if (Descricao?.Length > 500)
            erros.Add(new(nameof(Descricao), "O campo descrição deve conter menos de 500 caracteres"));

        if (CargaHoraria == null)
            erros.Add(new(nameof(CargaHoraria), "O campo Carga horaria deve ser preenchido"));

        if (DataConclusao == null)
            erros.Add(new(nameof(DataConclusao), "O Campo Data de conclusão deve ser preenchido"));

        return erros;
    }
}