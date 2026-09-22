using System;
using GeradorCertificados.Dominio.Compartilhado;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeradorCertificados.Aplicacao.Modulos.Cursos.Util;

public class ErroDeCurso
{
    public static Error Validacao(IReadOnlyList<ErroValidacao> erros)
    {
        return new Error("O curso possui erros de validação.")
            .WithMetadata("erros", erros);
    }

}
