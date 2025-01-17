using System;
using System.Collections.Generic;

namespace IconeEsteticaDentaria.Domain.dto
{
    public class OrdemServicoSalvarDto
    {
        public int NumeroOrdemServico { get; set; }

        public int IdCliente { get; set; }

        public DateTime DataEntrada { get; set; }

        public string CpfPaciente { get; set; }

        public string NomePaciente { get; set; }

        public string Observacao { get; set; }

        public IEnumerable<ItensOrdemServicoSalvarDto> ItensOrdemServico { get; set; }
    }
}
