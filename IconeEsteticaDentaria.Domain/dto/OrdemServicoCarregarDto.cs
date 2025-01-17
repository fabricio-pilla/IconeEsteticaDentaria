using System.Collections.Generic;

namespace IconeEsteticaDentaria.Domain.dto
{
    public class OrdemServicoCarregarDto
    {
        public int ClienteId { get; set; }

        public int NumeroOrdemServico { get; set; }

        public string ClienteNome { get; set; }

        public string DataEntrada { get; set; }

        public string PacienteCpf { get; set; }

        public string PacienteNome { get; set; }

        public string Observacao { get; set; }

        public bool Edita { get; set; }

        public IEnumerable<ItensOrdemSericoCarregarDto> ItensOrdemServico { get; set; }
    }
}
