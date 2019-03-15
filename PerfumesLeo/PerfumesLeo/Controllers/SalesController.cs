using PerfumesLeo.ServiceReferenceCorreios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PerfumesLeo.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CorreiosCalc(string cep)
        {

            string nCdEmpresa = string.Empty;
            string sDsSenha = string.Empty;
            string nCdServico = "41106";
            string sCepOrigem = "36774016";
            string sCepDestino = cep;
            string nVlPeso = 2.ToString();
            int nCdFormato = 1;
            decimal nVlComprimento = 20;
            decimal nVlAltura = 20;
            decimal nVlLargura = 20;
            decimal nVlDiametro = 0;
            string sCdMaoPropria = "N";
            decimal nVlValorDeclarado = 0;
            string sCdAvisoRecebimento = "N";
            string data = DateTime.Now.ToString("dd/MM/yyyy");

            CalcPrecoPrazoWSSoapClient wsCorreios = new CalcPrecoPrazoWSSoapClient();

            cResultado retornoCorreios = wsCorreios.CalcPrecoPrazoData(nCdEmpresa, sDsSenha,
                nCdServico, sCepOrigem, sCepDestino, nVlPeso, nCdFormato, nVlComprimento, nVlAltura,
                nVlLargura, nVlDiametro, sCdMaoPropria, nVlValorDeclarado, sCdAvisoRecebimento,
              data);


            string[] result = new string[3];

            result[0] = retornoCorreios.Servicos[0].Valor;
            result[1] = retornoCorreios.Servicos[0].PrazoEntrega;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}