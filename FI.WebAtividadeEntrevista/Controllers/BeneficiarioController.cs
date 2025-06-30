using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;
using WebAtividadeEntrevista.Models;

namespace FI.WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(BeneficiarioModal model)
        {
            //BoCliente bo = new BoCliente();

            //if (!this.ModelState.IsValid)
            //{
            //    List<string> erros = (from item in ModelState.Values
            //                          from error in item.Errors
            //                          select error.ErrorMessage).ToList();

            //    Response.StatusCode = 400;
            //    return Json(string.Join(Environment.NewLine, erros));
            //}
            //else
            //{

            //    model.Id = bo.Incluir(new Cliente()
            //    {
            //        CEP = model.CEP,
            //        Cidade = model.Cidade,
            //        Email = model.Email,
            //        Estado = model.Estado,
            //        Logradouro = model.Logradouro,
            //        Nacionalidade = model.Nacionalidade,
            //        Nome = model.Nome,
            //        Sobrenome = model.Sobrenome,
            //        Cpf = model.Cpf.Replace(".", "").Replace("-", ""),
            //        Telefone = model.Telefone
            //    });


                return Json("Cadastro efetuado com sucesso");
            
        }

        [HttpPost]
        public JsonResult Alterar(BeneficiarioModal model)
        {
            //BoCliente bo = new BoCliente();

            //if (!this.ModelState.IsValid)
            //{
            //    List<string> erros = (from item in ModelState.Values
            //                          from error in item.Errors
            //                          select error.ErrorMessage).ToList();

            //    Response.StatusCode = 400;
            //    return Json(string.Join(Environment.NewLine, erros));
            //}
            //else
            //{
            //    bo.Alterar(new Beneficiario()
            //    {
            //            Id = model.Id,
            //            Nome = model.Nome,
            //            Cpf = model.Cpf,
                    
            //    });

                return Json("Cadastro alterado com sucesso");
            
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.BeneficiarioModal model = null;

            if (cliente != null)
            {
                model = new BeneficiarioModal()
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Cpf = cliente.Cpf,
                };


            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
