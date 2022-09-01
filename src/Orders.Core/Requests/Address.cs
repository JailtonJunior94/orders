namespace Orders.Core.Requests;

public class Address
{
    public string TipoDeEndereco { get; set; }
    public string CEP { get; set; }
    public string Endereco { get; set; }
    public string NumeroEndereco { get; set; }
    public string Complemento { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string IBGECode { get; set; }
    public string Pais { get; set; }
}
