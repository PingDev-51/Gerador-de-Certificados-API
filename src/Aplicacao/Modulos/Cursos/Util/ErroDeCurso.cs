using System;
using GeradorCertificados.Dominio.Compartilhado;
using FluentResults;

namespace GeradorCertificados.Aplicacao.Modulos.Cursos.Util;

public static class ErrosDeCurso
{
    public static Error Validacao(IReadOnlyList<ErroValidacao> erros)
    {
        return new Error("O curso possui erros de validação.").WithMetadata("erros", erros);
    }
}