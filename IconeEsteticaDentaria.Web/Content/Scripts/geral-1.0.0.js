var Inicializador = Inicializador || {};

Inicializador = {
    Iniciar: function () {
        this.AplicarMascaras();
        this.SetConfiguracoes();
    },
    AplicarMascaras: function () {
        $(".maskCpf").mask("999.999.999-99");
        $(".maskData").mask("99/99/9999");
    },
    SetConfiguracoes: function () {
        // CONFIGURAÇÕES DO TOASTR NOTIFICATION
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-bottom-center",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        $.ajaxSetup({
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                $("#loader").removeClass("display-none");
            },
            complete: function () {
                $("#loader").addClass("display-none");
            },
            error: function (error) {
                var msg = JSON.parse(error.responseText);
                if (msg.Message === undefined) {
                    msg.Message = "";
                }
                switch (error.status) {
                    case 204:
                        if (msg.Message === "") {
                            msg.Message = "O conteúdo não foi encontrado!";
                        }
                        Sistema.Mensagem.Error(msg.Message, "Atenção!");
                        break;
                    case 400:
                        if (msg.Message === "") {
                            msg.Message = "Ocorreu um problema no processo que você executou!";
                        }
                        Sistema.Mensagem.Error(msg.Message, "Atenção!");
                        break;
                    case 401:
                        location.href = Ambiente.UrlMvc + "/Home/Index";
                        break;
                    case 404:
                        if (msg.Message === "") {
                            msg.Message = "Conteúdo não encontrado!";
                        }
                        Sistema.Mensagem.Error(msg.Message, "Atenção!");
                        break;
                    case 405:
                        if (msg.Message === "") {
                            msg.Message = "Você não tem permissão para acessar essa funcionalidade!";
                        }
                        Sistema.Mensagem.Error(msg.Message, "Não Permitido!");
                        break;
                    case 415:
                        if (msg.Message === "") {
                            msg.Message = "O formato da requisição é inválido!";
                        }
                        Sistema.Mensagem.Error(msg.Message, "Formato Inválido!");
                        break;
                    case 500:
                        if (msg.Message === "") {
                            msg.Message = "Ocorreu um erro no processo que estava sendo executado!";
                        }
                        Sistema.Mensagem.Error(msg.Message, "Erro!");
                        break;
                    default:
                        if (msg.Message === "") {
                            msg.Message = "Ocorreu um erro!";
                        }
                        Sistema.Mensagem.Error(msg.Message, "Erro!");
                        break;
                }
            }
        });

        if ($.fn.dataTable !== undefined) {
            $.extend(true, $.fn.dataTable.defaults, {
                paging: false,
                searching: false,
                info: false
            });

            //Paginate
            $.fn.dataTable.defaults.oLanguage.oPaginate.sFirst = "Primeiro";
            $.fn.dataTable.defaults.oLanguage.oPaginate.sLast = "Último";
            $.fn.dataTable.defaults.oLanguage.oPaginate.sNext = "Próximo";
            $.fn.dataTable.defaults.oLanguage.oPaginate.sPrevious = "Anterior";
            //Outros
            $.fn.dataTable.defaults.oLanguage.sEmptyTable = "Nenhum registro encontrado";
            $.fn.dataTable.defaults.oLanguage.sInfo = "Mostrando _START_ de _END_ de _TOTAL_ registros";
            $.fn.dataTable.defaults.oLanguage.sInfoEmpty = "Mostrando 0 de 0 de 0 registros";
            $.fn.dataTable.defaults.oLanguage.sInfoFiltered = "(filtrado de _MAX_ registros)";
            $.fn.dataTable.defaults.oLanguage.sLengthMenu = "Mostrar _MENU_ registros";
            $.fn.dataTable.defaults.oLanguage.sLoadingRecords = "Carregando...";
            $.fn.dataTable.defaults.oLanguage.sProcessing = "Processando...";
            $.fn.dataTable.defaults.oLanguage.sSearch = "Procurar:";
            $.fn.dataTable.defaults.oLanguage.sZeroRecords = "Nenhum registro correspondente encontrado";
        };
    }
}

var Sistema = Sistema || {};

Sistema = {
    Mensagem: {
        Comum: function (mensagem, titulo, classes) {
            toastr[classes](mensagem, titulo);
        },
        Error: function (msg, titulo) {
            Sistema.Mensagem.Comum(msg, titulo, 'error');
        },
        Info: function (msg, titulo) {
            Sistema.Mensagem.Comum(msg, titulo, 'info');
        },
        Success: function (msg, titulo) {
            Sistema.Mensagem.Comum(msg, titulo, 'success');
        },
        warning: function (msg, titulo) {
            Sistema.Mensagem.Comum(msg, titulo, 'warning');
        },
        confirm: function (titulo, subtitulo, callbackSim, callbackNao) {
            window.swal(
              {
                  title: titulo,
                  text: subtitulo,
                  type: "warning",
                  showCancelButton: true,
                  confirmButtonText: "Sim",
                  cancelButtonColor: "#DD6B55",
                  cancelButtonText: "Não",
                  closeOnConfirm: true,
                  closeOnCancel: true
              },
              function (isConfirm) {
                  if (isConfirm) {
                      if (typeof (callbackSim) == "function") {
                          callbackSim();
                      }
                  } else {
                      if (typeof (callbackNao) == "function") {
                          callbackNao();
                      }
                  }
              }
            );
        }
    },
    PopulateGenericSelect: function (elementId, data) {
        var options = $(elementId);
        $.each(data, function () {
            options.append($("<option />").val(this.Id).text(this.Descricao));
        });
    },
    InicializarChosen: function () {
        $(".chosen-select").chosen({
            no_results_text: "Nenhum registro encontrado para: "
        });
    },
    ContadorDeCaracteres: function (idTextArea, output, maxlenght, dontErase) {
        if (dontErase == undefined) dontErase = false;

        var qtde = $(idTextArea).val().length; //  + qtde;

        if (qtde <= maxlenght && $(output) !== null) {
            $(output).html(maxlenght - qtde);
        }
        else {
            var diff = maxlenght - qtde;
            if (diff < 0) {
                if (dontErase && diff === -1) {
                    $(idTextArea).val($(idTextArea).val().substring(0, maxlenght));
                } else if (!dontErase) {
                    $(idTextArea).val($(idTextArea).val().substring(0, maxlenght));
                }
            }
            if ($(output) !== null)
                $(output).html(maxlenght - qtde + 1);
        }
    }
}