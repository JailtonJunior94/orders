namespace Orders.Core.Requests;

public class Payment
{
    public string IdentificadorDaAutenticacao { get; set; }
    public string FormaDePagamento { get; set; }
    public string TipoDaOperacao { get; set; }
    public bool EhTitularCartao { get; set; }
    public string NomeNoCartao { get; set; }
    public string IdCartao { get; set; }
    public string DadosDoCartao { get; set; }
    public string DadosDoDispositivo { get; set; }
}
