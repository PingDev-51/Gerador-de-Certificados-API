using GeradorCertificados.Dominio.Compartilhado;
using GeradorCertificados.Dominio.Compartilhado.Auth;

namespace GeradorCertificados.Dominio.Modulos.Cursos;

public class Curso : EntidadeBase<Curso>, IEntidadeDeUsuario
{
    public Guid UsuarioId { get; set; }

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

    public Curso() { }

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
            erros.Add(new(nameof(Nome), "O campo Nome deve ser preenchido"));

        if (Nome?.Length < 2)
            erros.Add(new(nameof(Nome), "O campo Nome deve conter pelo menos 2 caracteres"));

        if (Nome?.Length > 200)
            erros.Add(new(nameof(Nome), "O campo Nome deve conter no máximo 200 caracteres"));

        if (Descricao?.Length > 500)
            erros.Add(new(nameof(Descricao), "O campo descrição deve conter no máximo 500 caracteres"));

        if (CargaHoraria is null or 0)
            erros.Add(new(nameof(CargaHoraria), "O campo Carga horária deve ser preenchido"));

        return erros;
    }
}