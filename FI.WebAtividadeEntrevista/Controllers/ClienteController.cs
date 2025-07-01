using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Text.Json;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            var model = new ClienteModel();
            return View("Forms", model);
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            var beneficiarios = new List<BeneficiarioModel>();

            if (model.BeneficiariosJson != null)
                beneficiarios = JsonSerializer.Deserialize<List<BeneficiarioModel>>(model.BeneficiariosJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            BoCliente bo = new BoCliente();
            
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                
                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Cpf = model.Cpf.Replace(".","").Replace("-",""),
                    Telefone = model.Telefone
                });

                BoBeneficiario boBeneficiario = new BoBeneficiario();

                if (beneficiarios != null && beneficiarios.Count > 0)
                {
                    foreach (var item in beneficiarios)
                    {
                        item.ClienteId = model.Id;
                        boBeneficiario.Incluir(new Beneficiario()
                        {
                            Nome = item.Nome,
                            Cpf = item.Cpf,
                            ClienteId = item.ClienteId.Value
                        });
                    }
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            var beneficiarios = new List<BeneficiarioModel>();

            if (model.BeneficiariosJson != null)
             beneficiarios = JsonSerializer.Deserialize<List<BeneficiarioModel>>(model.BeneficiariosJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Cpf = model.Cpf.Replace(".","").Replace("-",""),
                    Telefone = model.Telefone
                });

                UpdateBeneficiariosPorCliente(beneficiarios, model.Id);
                               
                return Json("Cadastro alterado com sucesso");
            }
        }

        private void UpdateBeneficiariosPorCliente(List<BeneficiarioModel> beneficiarios, long idCliente)
        {
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            var todosBeneficiariosDb = boBeneficiario.ListarPorCliente(idCliente);

            var listaIds = beneficiarios.Where(x => x.Id > 0).Select(ben => ben.Id).ToList();

            var beneficiariosExcluir = todosBeneficiariosDb.Where(x => !beneficiarios.Any(b => b.Id == x.Id)).ToList();

            var beneficiariosEditar = todosBeneficiariosDb.Where(x => beneficiarios.Any(b => b.Id == x.Id)).ToList();

            var beneficiariosCriar = beneficiarios.Where(x => x.Id == 0).Select(vm =>
            {
                vm.ClienteId = idCliente;
                return vm;
            }).ToList();

            if(beneficiariosExcluir != null && beneficiariosExcluir.Count > 0)
            {
                foreach (var item in beneficiariosExcluir)
                {
                    boBeneficiario.Excluir(item.Id);
                }
            }
            if (beneficiariosEditar != null && beneficiariosEditar.Count > 0)
            {
                foreach (var item in beneficiariosEditar)
                {
                    var beneficiario = beneficiarios.FirstOrDefault(b => b.Id == item.Id);
                    if (beneficiario != null)
                    {
                        boBeneficiario.Alterar(new Beneficiario()
                        {
                            Id = item.Id,
                            Nome = beneficiario.Nome,
                            Cpf = beneficiario.Cpf,
                            ClienteId = idCliente
                        });
                    }
                }
            }

            if (beneficiariosCriar != null && beneficiariosCriar.Count > 0)
            {
                foreach (var item in beneficiariosCriar)
                {
                    boBeneficiario.Incluir(new Beneficiario()
                    {
                        Nome = item.Nome,
                        Cpf = item.Cpf,
                        ClienteId = idCliente
                    });
                }
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            BoBeneficiario boBeneficiario = new BoBeneficiario();

            List<Beneficiario> benficiarios = boBeneficiario.ListarPorCliente(id);

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Cpf = cliente.Cpf,
                    Telefone = cliente.Telefone,
                    Beneficiarios = benficiarios.Select(benficiario => new BeneficiarioModel()
                    {
                        Id = benficiario.Id,
                        Nome = benficiario.Nome,
                        Cpf = benficiario.Cpf,
                        ClienteId = benficiario.ClienteId
                    }).ToList(),
                };

            
            }

            return View("Forms", model);
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