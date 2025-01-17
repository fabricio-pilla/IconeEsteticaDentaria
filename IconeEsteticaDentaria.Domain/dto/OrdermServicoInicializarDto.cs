using System.Collections.Generic;

namespace IconeEsteticaDentaria.Domain.dto
{
    public class OrdermServicoInicializarDto
    {
        public Usuario Usuario { get; set; }

        public IEnumerable<SelectGenericDto> Servicos { get; set; }

        public string DataEntrada { get; set; }
    }
}
