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

        // GET: PessoaTipoAcesso
        public ActionResult Index()
        {
            return View();
        }

        // GET: PessoaTipoAcesso/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PessoaTipoAcesso/Create
        public async Task<ActionResult> Create(Pessoa empInfo)
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
                }
                else
                {
                    return View("Error");
                }
            }

            var model = new CriarPessoaTipoAcesso
            {
                IdPessoa = empInfo.Id,
                IdCodigoAcesso = codigosLivres.Select(x => x.Id).ToList()
            };

            return View(model);
        }

        // POST: PessoaTipoAcesso/Create
        [HttpPost]
        public async Task<JsonResult> Create(int idPessoa = 1, int idTipoAcesso = 3, string idCodigoAcessoString = "E760A8CE-415D-4F2F-A269-28B32710ECDA")
        {
            Guid idCodigoAcesso = Guid.Parse(idCodigoAcessoString);

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

        // GET: PessoaTipoAcesso/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PessoaTipoAcesso/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PessoaTipoAcesso/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PessoaTipoAcesso/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}