using IconeEsteticaDentaria.Comum.Validator;
using IconeEsteticaDentaria.Data.Repositorios;
using IconeEsteticaDentaria.Domain.dto;
using IconeEsteticaDentaria.Web.Attributes;
using IconeEsteticaDentaria.Web.Util;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace IconeEsteticaDentaria.Web.Controllers
{
    [CustomAuthorize]
    public class OrdemServicoController : BaseController
    {
        #region Actions
        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            return View(id);
        }

        [HttpGet]
        public ActionResult Consultar()
        {
            return View();
        }
        #endregion


        #region Json
        [HttpGet]
        public JsonResult Inicializar()
        {
            try
            {
                var retorno = new OrdemServicoRepositorio().Inicializar();
                retorno.Usuario = _UsuarioLogado;

                return new HttpResultJson(HttpStatusCode.OK, "", retorno);
            }
            catch (Exception)
            {
                return new HttpResultJson(HttpStatusCode.BadRequest, "Ocorreu um problema ao inicializar à tela cadastrar ordem de serviço!");
            }
        }

        [HttpGet]
        public JsonResult CarregarDadosEditar(int numeroOS)
        {
            try
            {
                if (_UsuarioLogado.Id <= 0)
                    return new HttpResultJson(HttpStatusCode.BadRequest, "Sua sessão expirou!");

                if (numeroOS <= 0)
                    return new HttpResultJson(HttpStatusCode.BadRequest, "Ordem de serviço inválida!");

                var retorno = new OrdemServicoRepositorio().CarregarOrdemServico(numeroOS, _UsuarioLogado.Id);

                if (retorno != null)
                    return new HttpResultJson(HttpStatusCode.OK, "", retorno);
                else
                    return new HttpResultJson(HttpStatusCode.BadRequest, "Ordem de serviço não encontrada!");
            }
            catch (Exception)
            {
                return new HttpResultJson(HttpStatusCode.BadRequest, "Ocorreu um problema ao carregar os dados para editar à ordem de serviço!");
            }
        }

        [HttpPost]
        public JsonResult Pesquisar(int numeroOS, string nomePaciente)
        {
            try
            {
                if (_UsuarioLogado.Id <= 0)
                    return new HttpResultJson(HttpStatusCode.BadRequest, "Sua sessão expirou!");

                var retorno = new OrdemServicoRepositorio().PesquisarOrdemServico(numeroOS, nomePaciente, _UsuarioLogado.Id);

                return new HttpResultJson(HttpStatusCode.OK, "", retorno);
            }
            catch (Exception)
            {
                return new HttpResultJson(HttpStatusCode.BadRequest, "Ocorreu um problema ao pesquisar ordem de serviço!");
            }
        }


        [HttpPost]
        public JsonResult Salvar(OrdemServicoSalvarDto ordemServicoSalvarDto)
        {
            try
            {
                if (_UsuarioLogado.Id <= 0)
                    return new HttpResultJson(HttpStatusCode.BadRequest, "Sua sessão expirou!");

                if (ordemServicoSalvarDto.NumeroOrdemServico <= 0)
                    ordemServicoSalvarDto.DataEntrada = DateTime.Now;

                ordemServicoSalvarDto.IdCliente = _UsuarioLogado.Id;

                var msg = validarSalvar(ordemServicoSalvarDto);

                if (msg != "")
                    return new HttpResultJson(HttpStatusCode.BadRequest, msg);

                if (new OrdemServicoRepositorio().AdicionarEditar(ordemServicoSalvarDto))
                    return new HttpResultJson(HttpStatusCode.OK, "Salvo com sucesso!");
                else
                    return new HttpResultJson(HttpStatusCode.BadRequest, "Não foi possível salvar à ordem de serviço!");
            }
            catch (Exception)
            {
                return new HttpResultJson(HttpStatusCode.BadRequest, "Ocorreu um problema ao salvar à ordem de serviço!");
            }
        }
        #endregion

        #region Validações
        private string validarSalvar(OrdemServicoSalvarDto ordemServicoSalvarDto)
        {
            var msg = "";

            if (ordemServicoSalvarDto.NumeroOrdemServico <= 0 && DateTime.Now.ToShortDateString() != ordemServicoSalvarDto.DataEntrada.ToShortDateString())
                msg += "Campo data entrada não pode ser menor que há data atual! <br />";

            if (ordemServicoSalvarDto.IdCliente <= 0)
                msg += "- Campo cliente é obrigatório! <br />";

            if (!Validator.StringValida(ordemServicoSalvarDto.CpfPaciente))
                msg += "- Campo cpf do paciente é obrigatório! <br />";
            else if (ordemServicoSalvarDto.CpfPaciente.Length > 14)
                msg += "- Campo cpf do paciente ultrapassou o limite permitido! <br />";
            else if (!Validator.ValidaCPF(ordemServicoSalvarDto.CpfPaciente))
                msg += "Campo cpf do paciente inválido! <br />";

            if (!Validator.StringValida(ordemServicoSalvarDto.NomePaciente))
                msg += "- Campo nome do paciente é obrigatório! <br />";
            else if (ordemServicoSalvarDto.NomePaciente.Length > 200)
                msg += "- Campo nome do paciente ultrapassou o limite permitido! <br />";

            if (!Validator.StringValida(ordemServicoSalvarDto.Observacao))
                msg += "- Campo observação da O.S. é obrigatório! <br />";
            else if (ordemServicoSalvarDto.Observacao.Length > 2000)
                msg += "- Campo observação da O.S. ultrapassou o limite permitido! <br />";

            if (ordemServicoSalvarDto.ItensOrdemServico == null || !ordemServicoSalvarDto.ItensOrdemServico.Any())
                msg += "- É obrigatório inserir itens na ordem de serviço! <br />";
            else if (ordemServicoSalvarDto.ItensOrdemServico.Any(x => x.ServicoId <= 0))
                msg += "- Não é permitido inserir serviços com id 0 na ordem de serviço! <br />";
            else if (ordemServicoSalvarDto.ItensOrdemServico.Any(x => x.Quantidade <= 0))
                msg += "- Não é permitido inserir quantidade menor/igual 0 na ordem de serviço! <br />";


            return msg;
        }
        #endregion
    }
}