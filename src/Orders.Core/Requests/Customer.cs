namespace Orders.Core.Requests;

public class Customer
{
    public string CPF { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    public string Celular { get; set; }
    public DateTime? DataDeNascimento { get; set; }
    public string Genero { get; set; }
    public string Rg { get; set; }
    public string NomeMae { get; set; }
    public bool PoliticamenteExposta { get; set; }
    public bool? EmailsPromocionais { get; set; }
    public bool? CompartilhaDados { get; set; }
    public bool? TermoAceitePP { get; set; }
    public bool? TermoAceiteItau { get; set; }
    public string Senha { get; set; }
    public string TipoDeEmail { get; set; }
}
