using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AccessControlPortal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace AccessControlPortal.Controllers
{
    public class PessoaController : Controller
    {
        private string Baseurl = "https://localhost:44372/";

        // GET: Pessoa
        public ActionResult Index()
        {
            return View();
        }

        public async Task<Pessoa> GetPessoaByCpf(string cpf)
        {
            Pessoa pessoa = new Pessoa();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Pessoa/GetPessoa?cpf=" + cpf);

                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    pessoa = JsonConvert.DeserializeObject<Pessoa>(EmpResponse);

                    return pessoa;
                }
                else
                {
                    //Lógica de fallha
                    return pessoa;
                }

            }
        }

        // GET: Pessoa/Details/5
        [HttpPost]
        public async Task<JsonResult> Details(string cpf)
        {
            Pessoa EmpInfo = new Pessoa();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Pessoa/GetPessoa?cpf=" + cpf);

                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    EmpInfo = JsonConvert.DeserializeObject<Pessoa>(EmpResponse);
                    return Json(Url.Action("Create", "PessoaTipoAcesso", new { pessoa = EmpInfo.Id}));
                }
                else
                {
                    //return View("Create");
                    return Json(Url.Action("Create", "Pessoa"));
                }
                
            }
        }

        // GET: Pessoa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pessoa/Create
        [HttpPost]
        public async Task<ActionResult> Create(PessoaCreate pessoa)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync<PessoaCreate>($"api/Pessoa/AddPessoa?nome={pessoa.Nome}&cpf={pessoa.Cpf}&telefone={pessoa.Telefone}", pessoa);

                if (Res.IsSuccessStatusCode)
                {
                    var pessoaAdd = GetPessoaByCpf(pessoa.Cpf);

                    if(pessoaAdd != null)
                    {
                        return RedirectToAction("Create", "PessoaTipoAcesso", pessoaAdd.Id);
                    }
                    else
                    {
                        return RedirectToAction("Error");
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
        }
    }
}