
namespace IconeEsteticaDentaria.Domain
{
    public class ItensOrdemServico
    {
        public Servico Servico { get; set; }

        public int Item { get; set; }

        public int OrdemServicoId { get; set; }

        public int PessoaId { get; set; }

        public int Quantidade { get; set; }

        public decimal ValorTotal { get; set; }
    }
}
