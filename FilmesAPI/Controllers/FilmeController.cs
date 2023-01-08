using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
// O  [Route("[controller]")] serve para pegar o nome do controlador e colocar na rota. O controller escrito dessa maneira no route diz para ele.
//copiar o nome da classe de controller para trás, então com a classe com nome FilmeController, ele copiou Filme e colocou.
// dentro do route, ficando, "por debaixo dos panos", dessa maneira: [Route("[Filme]")].
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    // O FromBody quer dizer que a informação vem através do corpo da requisição.
    // Estudar mais sobre CreatedAtAction - https://www.macoratti.net/19/06/aspnc_3dwebapi1.htm
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    { 
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new {id = filme.Id}, filme);

    }
    /// <summary>
    /// Recupera os dados inseridos pelo POST
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso os dados sejam recuperados com sucesso</response>
    [HttpGet]
    // Recupera os dados inseridos pelo AdicionaFilme, que no caso é um HttpPost.
    // Para Explicitar que o usuário vai passar os valores de skip e take através de uma consulta, utilizei o FromQuery
    // O skip está = 0 para caso o usuário não passar nenhum valor ele ser 0 e não pular nada
    // O take está =50, pois foi um valor que julguei que não seria muito exagerado e para não sobrecarregar a memória sistema de filmes
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery]int skip = 0, [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId (int id)
    {
      var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }
    /// <summary>
    /// Executa uma requisição HTTP usando o método PUT
    /// </summary>
    /// /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a  requisição seja feita com sucesso - No Content</response>
    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if(filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    // o HttpPatch serve para atualizações parciais
    /// <summary>
    /// Corrige os dados inseridos de forma errada
    /// </summary>
    /// /// <param name="patch">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso os dados sejam atualizados com sucesso - No Content</response>
    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }
    /// <summary>
    /// Deleta dados inseridos de forma errada
    /// </summary>
    /// /// <param name="id">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso os dados sejam deletados com sucesso - No Content</response>
    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }



}

