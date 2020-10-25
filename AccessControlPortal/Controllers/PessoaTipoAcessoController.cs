using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AccessControlPortal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccessControlPortal.Controllers
{
    public class PessoaTipoAcessoController : Controller
    {
        private string Baseurl = "https://localhost:44372/";

        // GET: PessoaTipoAcesso/Create
        public async Task<ActionResult> Create(int pessoa)
        {
            //Pegar os códigos de acesso que não estão em uso:
            List<CodigoAcesso> codigosLivres = new List<CodigoAcesso>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/CodigoAcesso/GetCodigosAcessoLivres");

                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    codigosLivres = JsonConvert.DeserializeObject<List<CodigoAcesso>>(EmpResponse);

                    var model = new CriarPessoaTipoAcesso
                    {
                        IdPessoa = pessoa,
                        IdCodigoAcesso = codigosLivres.Select(x => x.Id).ToList()
                    };

                    return View(model);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        // POST: PessoaTipoAcesso/Create
        [HttpPost]
        public async Task<JsonResult> Create(int idPessoa, int idTipoAcesso, Guid idCodigoAcesso)
        {
            var pessoa = new PessoaCreate();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync<PessoaCreate>($"api/PessoaTipoAcesso/AddPessoaTipoAcesso?idPessoa={idPessoa}&idTipoAcesso={idTipoAcesso}&idCodigoAcesso={idCodigoAcesso}", pessoa);

                if (Res.IsSuccessStatusCode)
                {
                    return Json(Url.Action("Index", "Home"));
                }
                else
                {
                    return Json(Url.Action("Error"));
                }
            }
        }

        public ActionResult BaixaNoAcesso()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetIdPessoaTipoAcesso(string cpf)
        {
            PessoaTipoAcesso EmpInfo = new PessoaTipoAcesso();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/PessoaTipoAcesso/GetPessoaTipoAcesso?cpf=" + cpf);

                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    EmpInfo = JsonConvert.DeserializeObject<PessoaTipoAcesso>(EmpResponse);

                    var aux = this.SairDoCondominio(EmpInfo);

                    if (await aux)
                    {
                        return Json(Url.Action("Index", "Home"));
                    }
                    else
                    {
                        return Json(Url.Action("Index", "Home"));
                    }
                }
                else
                {
                    return Json(Url.Action("Error"));
                }
            }
        }

        [HttpPut]
        public async Task<bool> SairDoCondominio(PessoaTipoAcesso empInfo)
        {
            var idPessoaTipoAcesso = empInfo.Id;
            var idCodigoAcesso = empInfo.IdCodigoAcesso;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PutAsJsonAsync<PessoaTipoAcesso>("api/PessoaTipoAcesso/UpdatePessoaTipoAcesso?idPessoaTipoAcesso=" + idPessoaTipoAcesso, empInfo);

                if (Res.IsSuccessStatusCode)
                {
                    var aux = this.UpdateStatusCartao(idCodigoAcesso);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [HttpPut]
        public async Task<bool> UpdateStatusCartao(Guid guid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PutAsJsonAsync<Guid>($"api/CodigoAcesso/UpdateCodigoAcesso?id={guid}&staus=false", guid);

                if (Res.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}