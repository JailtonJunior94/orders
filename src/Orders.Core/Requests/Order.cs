namespace Orders.Core.Requests;

public class Order
{
    public string CanalAquisicao { get; set; }
    public string OrigemCliente { get; set; }
    public int? Origem { get; set; }
    public string CustomerID { get; set; }
    public short Quantidade { get; set; }
    public string CodigoProduto { get; set; }
    public string CodigoCampanha { get; set; }
    public string PlacaVeiculo { get; set; }
    public int? ValorEmCentavos { get; set; }
    public bool Substituicao { get; set; }
    public string ValorTaxaSubstituicao { get; set; }
    public int TipoReversaoId { get; set; }
    public int? AdesaoSubstituidaId { get; set; }
    public bool AtivacaoAutomatica { get; set; }
    public bool? ClienteEstaPresente { get; set; }
    public Payment Pagamento { get; set; }
    public Customer Cliente { get; set; }
    public Address Endereco { get; set; }
}
