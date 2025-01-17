var Account = Account || {};

var propAccount = {
    Formulario: {
        login: "#txtLogin",
        senha: "#txtSenha",
        botaoEntrar: "#btnEntrar"
    }
};

Account = {
    Iniciar: function () {
        this.SetEventos();
    },
    SetEventos: function () {
        $('input').keypress(function (e) {
            if (e.which == 13) {
                $(propAccount.Formulario.botaoEntrar).click();
            }
        });
        $(propAccount.Formulario.botaoEntrar).on("click", function () {
            var login = $(propAccount.Formulario.login).val();
            var senha = $(propAccount.Formulario.senha).val();
            var msg = '';
            if (login === '' || login === undefined || login === null)
                msg = "-Campo login é obrigatório! <br />";
            if (login.length > 20)
                msg += "-Campo login ultrapassou o limite de caracteres! <br />";
            if (senha === '' || senha === undefined || senha === null)
                msg += "-Campo senha é obrigatório! <br />";
            if (senha.length > 20)
                msg += "Campo senha ultrapassou o limite de caracteres! <br />";

            if (msg != '')
                Sistema.Mensagem.warning(msg, "Atenção!");
            else {
                $.ajax({
                    url: "/Account/Logon",
                    type: "POST",
                    data: JSON.stringify({
                        login: login,
                        senha: senha
                    }),
                    success: function (result) {
                        if (result.statusCode !== 200)
                            Sistema.Mensagem.Error(result.msg, "Atenção!");
                        else {
                            window.location = "/Home";
                        }
                    }
                });
            }
        });
    }
};