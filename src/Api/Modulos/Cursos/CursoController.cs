using System;
using GeradorCertificados.Aplicacao.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GeradorCertificados.WebApi.Modulos.Cursos;

[ApiController]
[Route("api/Curso")]
public class CursoController : ControllerBase
{

    [HttpGet]
    public ActionResult SelecionarTodos()
    {

        return StatusCode(200);
    }

    // [HttpPost]
    // public ActionResult Cadastrar(CadastrarCursoRequest req)
    // {
    //     var dto = new CadastrarCursoDto(req.Nome, req.Descricao, req.CargaHoraria, req.DataConclusao);

    //     var resultado = servicoCurso.Cadastrar(dto);

    //     if (resultado.IsFailed)
    //         return BadRequest();

    //     var res = new CadastrarCursoResponse(resultado.Value);

    //     return Created("/api/Cursos");
    // }
}
