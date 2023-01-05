using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
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

    [HttpPost]
    // O FromBody quer dizer que a informação vem através do corpo da requisição.
    // Estudar mais sobre CreatedAtAction - https://www.macoratti.net/19/06/aspnc_3dwebapi1.htm
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    { 
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new {id = filme.Id}, filme);

    }

    [HttpGet]
    // Recupera os dados inseridos pelo AdicionaFilme, que no caso é um HttpPost.
    // Para Explicitar que o usuário vai passar os valores de skip e take através de uma consulta, utilizei o FromQuery
    // O skip está = 0 para caso o usuário não passar nenhum valor ele ser 0 e não pular nada
    // O take está =50, pois foi um valor que julguei que não seria muito exagerado e para não sobrecarregar a memória sistema de filmes
    public IEnumerable<Filme> RecuperaFilmes([FromQuery]int skip = 0, [FromQuery] int take = 50)
    {
        return _context.Filmes.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId (int id)
    {
      var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        return Ok(filme);
    }
}

