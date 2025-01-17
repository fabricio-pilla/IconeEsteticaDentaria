var ConsultarOrdemServico = ConsultarOrdemServico || {};

var propConsultarOrdemServico = {
    table: "#tableOrdemServico",
    numeroOS: "#txtNumeroOS",
    nomePaciente: "#txtNomePaciente",
    botaoPesquisar: "#btnPesquisar"
};

ConsultarOrdemServico = {
    Iniciar: function () {
        this.initTable();
        this.setEventos();
    },
    initTable: function () {
        $(propConsultarOrdemServico.table).dataTable({
            autoWidth: false,
            destroy: true,
            columnDefs: [
              {
                  data: "NumeroOS",
                  render: function (data, type, row, meta) {
                      if (row.Edita == 'Sim')
                          return "<a href='editarordemservico/" + data + "'><i title='Clique para editar' class='cursor-pointer fa fa-pencil text-navy'></i></a> ";
                      else
                          return "";
                  },
                  width: '30px',
                  class: 'text-center',
                  targets: [0]
              },
              { data: "NumeroOS", targets: [1] },
              { data: "NomePaciente", targets: [2] },
              { data: "CpfPaciente", targets: [3] },
              { data: "DataEntrada", class: 'text-center', targets: [4] },
              { data: "Situacao", class: 'text-center', targets: [5] },
              { data: "Edita", class: 'text-center', targets: [6] }
            ]
        });
    },
    setEventos: function () {
        $(propConsultarOrdemServico.botaoPesquisar).on("click", function () {
            $.ajax({
                url: "PesquisarOrdemServico",
                type: "POST",
                data: JSON.stringify({
                    numeroOS: ($(propConsultarOrdemServico.numeroOS).val() ? $(propConsultarOrdemServico.numeroOS).val() : 0),
                    nomePaciente: $(propConsultarOrdemServico.nomePaciente).val()
                }),
                success: function (result) {
                    if (result.statusCode === 200) {
                        ConsultarOrdemServico.populateTable(result.data);
                    }
                    else
                        Sistema.Mensagem.Error(result.msg, "Atenção!");
                }
            });
        });
    },
    populateTable: function (data) {
        $(propConsultarOrdemServico.table).DataTable().clear().draw();
        if (data.length > 0) {
            $(propConsultarOrdemServico.table).dataTable().fnAddData(data);
        }
    }
};