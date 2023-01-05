using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

// O required é quando o preenchimento do campo é obrigatório, MaxLength é para limitar o número de caracteres, Range é para definir o valor mínimo e máximo.
// No StringLenth da para colocar o mínimo e o máximo de caracteres.
// Diferença entre StringLenth e MaxLenth - https://social.msdn.microsoft.com/Forums/en-US/95d484d5-9592-4678-8a57-4483689f27ec/what-is-difference-between-stringlength-and-maxlength-attribute?forum=aspadoentitylinq
// Para sermos mais expressivos com quem for consumir a API, podemos deixar mensagens mais claras de erro através do parâmetro ErrorMessage.
public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O título do filme é obrigatório")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "O gênero do filme é obrigatório")]
    //[StringLength(50, ErrorMessage = "O tamanho do gênero tem de estar entre 1 e 50 caracteres.", MinimumLength = 1)]
    [MaxLength(50, ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres")]
    public string Genero { get; set; }
    [Required]
    [Range(70, 600, ErrorMessage = "A duração deve ter entre 70 e 600 minutos")]
    public int Duracao { get; set; }
}
