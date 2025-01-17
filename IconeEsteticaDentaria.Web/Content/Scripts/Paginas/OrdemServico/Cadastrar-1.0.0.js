var CadastrarOrdemServico = CadastrarOrdemServico || {};

var propCadastrarOrdemServico = {
    table: "#tableItensOS",
    numeroOS: "#txtNumeroOS",
    idCliente: "#txtIdCliente",
    nomeCliente: "#txtNomeCliente",
    dataEntrada: "#txtDataEntrada",
    cpfPaciente: "#txtCpfPaciente",
    nomePaciente: "#txtNomePaciente",
    observacaoOS: "#txtObservacaoOS",
    contadorObservacaoOS: "#contadorObservacaoOS",
    botaoSalvar: "#btnSalvar",
    botaoCancelar: "#btnCancelar",
    itensOS: {
        botaoAdicionarItensOS: "#btnAdicionarItensOS",
        selectServico: "#ddlServicos",
        quantidade: "#txtQuantidade",
        index: "#hdnIndex",
        itemOSId: "#hdnItemOSId"
    }
};

CadastrarOrdemServico = {
    Iniciar: function () {
        this.setEventos();
        this.itensOrdemServico.initTable();
        this.itensOrdemServico.setEventos();
        this.inicializarTela();
    },
    inicializarTela: function () {
        $.ajax({
            url: "/Inicializar",
            type: "GET",
            success: function (result) {
                if (result.statusCode === 200) {
                    $(propCadastrarOrdemServico.idCliente).val(result.data.Usuario.Id);
                    $(propCadastrarOrdemServico.nomeCliente).val(result.data.Usuario.Nome);
                    $(propCadastrarOrdemServico.dataEntrada).val(result.data.DataEntrada);

                    Sistema.PopulateGenericSelect(propCadastrarOrdemServico.itensOS.selectServico, result.data.Servicos);

                    var numeroOS = $(propCadastrarOrdemServico.numeroOS).val();
                    if (numeroOS > 0)
                        CadastrarOrdemServico.carregarEdicaoOrdemServico(numeroOS);
                }
                else
                    Sistema.Mensagem.Error(result.msg, "Atenção!");

                Sistema.InicializarChosen();
            }
        });
    },
    setEventos: function () {
        $(propCadastrarOrdemServico.botaoSalvar).on("click", function () {

            var lista = $(propCadastrarOrdemServico.table).DataTable().data();
            var arrayItensOS = [];
            for (var i = 0; i < lista.length; i++) {
                arrayItensOS.push(lista[i]);
            }

            var ordemServicoSalvarDto = {
                NumeroOrdemServico: $(propCadastrarOrdemServico.numeroOS).val(),
                CpfPaciente: $(propCadastrarOrdemServico.cpfPaciente).val(),
                NomePaciente: $(propCadastrarOrdemServico.nomePaciente).val(),
                Observacao: $(propCadastrarOrdemServico.observacaoOS).val(),
                ItensOrdemServico: arrayItensOS
            };


            $.ajax({
                url: "/SalvarOrdemServico",
                type: "POST",
                data: JSON.stringify({
                    ordemServicoSalvarDto: ordemServicoSalvarDto
                }),
                success: function (result) {
                    if (result.statusCode === 200) {
                        Sistema.Mensagem.Success(result.msg, "Ordem de Serviço!");                        
                        CadastrarOrdemServico.limparCampos();
                    }
                    else
                        Sistema.Mensagem.Error(result.msg, "Atenção!");
                }
            });
        });

        $(propCadastrarOrdemServico.botaoCancelar).on("click", function () {            
            CadastrarOrdemServico.limparCampos();
        });

        $(propCadastrarOrdemServico.observacaoOS).bind("keypress keyup focus", function () {
            Sistema.ContadorDeCaracteres(propCadastrarOrdemServico.observacaoOS, propCadastrarOrdemServico.contadorObservacaoOS, $(propCadastrarOrdemServico.observacaoOS).attr("maxlength"), false);
        });
    },
    limparCampos: function () {
        if ($(propCadastrarOrdemServico.numeroOS).val() > 0)
            window.location.href = "/ConsultarOrdemServico";

        $(propCadastrarOrdemServico.cpfPaciente).val('');
        $(propCadastrarOrdemServico.nomePaciente).val('');
        $(propCadastrarOrdemServico.observacaoOS).val('');
        $(propCadastrarOrdemServico.table).DataTable().clear().draw();
        $(propCadastrarOrdemServico.contadorObservacaoOS).html($(propCadastrarOrdemServico.observacaoOS).attr("maxlength"));
        CadastrarOrdemServico.itensOrdemServico.limparCampos();
    },
    carregarEdicaoOrdemServico: function (numeroOrdemServico) {
        $.ajax({
            url: "/carregardadoseditarordemservico/" + numeroOrdemServico,
            type: "GET",
            success: function (result) {
                if (result.statusCode === 200) {
                    if (!result.data.Edita) {
                        $(propCadastrarOrdemServico.botaoSalvar).remove();
                        $(propCadastrarOrdemServico.itensOS.botaoAdicionarItensOS).remove();
                        $(propCadastrarOrdemServico.cpfPaciente).prop('disabled', true);
                        $(propCadastrarOrdemServico.nomePaciente).prop('disabled', true);
                        $(propCadastrarOrdemServico.observacaoOS).prop('disabled', true);
                        $(propCadastrarOrdemServico.itensOS.quantidade).prop('disabled', true);
                        $(propCadastrarOrdemServico.itensOS.selectServico).prop('disabled', true);
                        $(propCadastrarOrdemServico.itensOS.selectServico).trigger('chosen:updated');
                    }

                    $(propCadastrarOrdemServico.cpfPaciente).val(result.data.PacienteCpf);
                    $(propCadastrarOrdemServico.nomePaciente).val(result.data.PacienteNome);
                    $(propCadastrarOrdemServico.observacaoOS).val(result.data.Observacao);

                    CadastrarOrdemServico.itensOrdemServico.populateTable(result.data.ItensOrdemServico);
                }
                else
                    Sistema.Mensagem.Error(result.msg, "Atenção!");

                Sistema.InicializarChosen();
            }
        });
    },
    itensOrdemServico: {
        initTable: function () {
            $(propCadastrarOrdemServico.table).dataTable({
                autoWidth: false,
                destroy: true,
                columnDefs: [
                  {
                      data: "Id",
                      render: function (data, type, row, meta) {
                          return "<i onclick='CadastrarOrdemServico.itensOrdemServico.carregarEdicaoItensOS(" + row.Id + "," + row.ServicoId + "," + row.Quantidade + "," + meta.row + ");' title='Clique para editar' class='cursor-pointer fa fa-pencil text-navy'></i> " +
                                 "<i onclick='CadastrarOrdemServico.itensOrdemServico.confirmaExcluir(" + meta.row + ");" + "' class='padding-left-10 cursor-pointer fa fa-trash-o' title='Clique para excluir do banco de dados!'></i>";
                      },
                      width: '50px',
                      class: 'text-center',
                      targets: [0]
                  },
                  { data: "ServicoDescricao", targets: [1] },
                  { data: "Quantidade", width: '50px', targets: [2] },
                ]
            });
        },
        setEventos: function () {
            $(propCadastrarOrdemServico.itensOS.botaoAdicionarItensOS).on("click", function () {
                if (IsValid()) {
                    var itemOS = {
                        Id: $(propCadastrarOrdemServico.itensOS.itemOSId).val() > 0 ? $(propCadastrarOrdemServico.itensOS.itemOSId).val() : 0,
                        ServicoId: $(propCadastrarOrdemServico.itensOS.selectServico).val(),
                        ServicoDescricao: $(propCadastrarOrdemServico.itensOS.selectServico + ' :selected').text(),
                        Quantidade: $(propCadastrarOrdemServico.itensOS.quantidade).val()
                    }

                    if ($(propCadastrarOrdemServico.itensOS.botaoAdicionarItensOS).html() === "Adicionar") {
                        $(propCadastrarOrdemServico.table).dataTable().fnAddData(itemOS);
                    } else {
                        $(propCadastrarOrdemServico.table).dataTable().fnUpdate(itemOS, $(propCadastrarOrdemServico.itensOS.index).val());
                    }

                    CadastrarOrdemServico.itensOrdemServico.limparCampos();
                }

                function IsValid() {
                    var msg = "";

                    if (!$(propCadastrarOrdemServico.itensOS.selectServico).val())
                        msg += "- Campo serviços é obrigátorio! <br />";

                    if (!$(propCadastrarOrdemServico.itensOS.quantidade).val())
                        msg += "- Campo quantidade é obrigatório! <br />";
                    else if ($(propCadastrarOrdemServico.itensOS.quantidade).val() <= 0)
                        msg += "- Campo quantidade tem que ser maior que 0! <br />";

                    if (msg === "" && $(propCadastrarOrdemServico.itensOS.botaoAdicionarItensOS).html() === "Adicionar") {
                        var data = $(propCadastrarOrdemServico.table).DataTable().data();
                        var retorno = $.grep(data, function (e) {
                            return e.ServicoId == $(propCadastrarOrdemServico.itensOS.selectServico).val();
                        });

                        if (retorno.length > 0)
                            msg += "- Este serviço já foi adicionado! <br />";
                    }

                    if (msg) {
                        Sistema.Mensagem.warning(msg, "Atenção!");
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            });
        },
        limparCampos: function () {
            $(propCadastrarOrdemServico.itensOS.selectServico).val('');
            $(propCadastrarOrdemServico.itensOS.selectServico).trigger('chosen:updated');
            $(propCadastrarOrdemServico.itensOS.quantidade).val('');
            $(propCadastrarOrdemServico.itensOS.botaoAdicionarItensOS).html("Adicionar").removeProp("disabled");
            $(propCadastrarOrdemServico.itensOS.index).val(0);
            $(propCadastrarOrdemServico.itensOS.itemOSId).val(0);
        },
        populateTable: function (data) {
            $(propCadastrarOrdemServico.table).DataTable().clear().draw();
            if (data.length > 0) {
                $(propCadastrarOrdemServico.table).dataTable().fnAddData(data);
            }
        },
        confirmaExcluir: function (index) {
            Sistema.Mensagem.confirm('Atenção!', 'Deseja realmente excluir este item?',
                         function () { CadastrarOrdemServico.itensOrdemServico.excluir(index) });
        },
        excluir: function (index) {
            $(propCadastrarOrdemServico.table).dataTable().fnDeleteRow(index);
        },
        carregarEdicaoItensOS: function (itemOSId, servicoId, quantidade, index) {
            $(propCadastrarOrdemServico.itensOS.selectServico).val(servicoId).prop('disabled', true);
            $(propCadastrarOrdemServico.itensOS.selectServico).trigger('chosen:updated');
            $(propCadastrarOrdemServico.itensOS.quantidade).val(quantidade);
            $(propCadastrarOrdemServico.itensOS.botaoAdicionarItensOS).html("Atualizar");
            $(propCadastrarOrdemServico.itensOS.index).val(index);
            $(propCadastrarOrdemServico.itensOS.itemOSId).val(itemOSId);
        }
    }
};