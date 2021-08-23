using business;
using DataContextPerfume;
using Microsoft.AspNet.Identity;
using PerfumesLeo.Models.Repository;
using PerfumesLeo.Models.ViewModels;
using PerfumesLeo.ServiceReferenceCorreios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PerfumesLeo.Controllers
{
    public class HomeController : Controller
    {
        private BD banco = new BD();
        public IProdutoRepository ProdutoRepository { get; }
        public IPedidoRepository PedidoRepository { get; }
        public IClienteRepository ClienteRepository { get; }
        public AccountController AccountController { get; }

        public HomeController(IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository,
            IClienteRepository clienteRepository, AccountController accountController)
        {
            ProdutoRepository = produtoRepository;
            PedidoRepository = pedidoRepository;
            ClienteRepository = clienteRepository;
            AccountController = accountController;
        }

        public ActionResult Index()
        {
            return View(ProdutoRepository.GetAll());
        }

        public async Task<ActionResult> BuscaProdutos(string pesquisa)
        {
            return View(await ProdutoRepository.GetProdutosAsync(pesquisa));
        }

        [Authorize]
        public async Task<ActionResult> Carrinho(int? codigo)
        {
            int num = 0;
            Pedido pedido = null;
            var email = User.Identity.GetUserName();
            Cliente cli = banco.Cliente.First(c => c.UserName == email);

            if (Request.Cookies[User.Identity.GetUserName()] != null)
            {
                num = int.Parse(Request.Cookies[User.Identity.GetUserName()].Value);
                pedido = await PedidoRepository.Get(num);
            }
            else
            {
                pedido = PedidoRepository.Add(cli);
                HttpCookie cookie = new HttpCookie(User.Identity.GetUserName(), pedido.IdPedido.ToString());
                cookie.Expires.AddDays(15);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                num = pedido.IdPedido;
            }

            if (codigo != null)
            {
                await PedidoRepository.AddItemAsync((int)codigo, num);
            }

            List<ItemPedido> itens = pedido.Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);
            return View(carrinhoViewModel);
        }

        [Authorize]
        public async Task<ActionResult> Cadastro()
        {
            var cookie = Request.Cookies[User.Identity.GetUserName()];
            var id = int.Parse(cookie.Value);
            Pedido pedido = await PedidoRepository.Get(id);

            if (pedido == null)
            {
                return RedirectToAction("Carrossel");
            }
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Resumo(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                var urlPedido = "http://localhost:53443/Home/RetornaPedido/" + pedido.IdPedido;
                pedido = await PedidoRepository.UpdateCadastroAsync(pedido);
                pedido = await PedidoRepository.Get(pedido.IdPedido);
                string[] myCookies = Request.Cookies.AllKeys;
                foreach (string cookie in myCookies)
                {
                    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-16);
                }
                IdentityMessage msg = new IdentityMessage
                {
                    Body = "<p style='color:blue; background-color:yellow;'>" +
                    $" N° Pedido - {pedido.IdPedido.ToString()} <a href='{urlPedido}'> visualizar pedido <a/>." +
                    $" Agradecemos pela sua prefêrencia. <p/>" +
                    "<p style='color:red; background-color:yellow;'> Volte sempre. Assinado Perfumes Hinode sz sz sz sz <p/>" +
                    "<img src='https://cdn.ecvol.com/s/loja.anatuori.com/produtos/ligia-vestido-festa-longo-evase-ombro-a-ombro-com-alcas-saia-plissada-madrinha-casamento-formatura-cor-marsala/m/0.jpg?v=1' />",
                    Subject = "Pedido confirmado.",
                    Destination = User.Identity.GetUserName()
                };
                await AccountController.SendMarketingAsync(msg);
                return View(pedido);
            }
            return RedirectToAction("Cadastro");
        }

        public async Task<ActionResult> RetornaPedido(int id)
        {
            Pedido pedido = await PedidoRepository.Get(id);
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateQuantidade(int Id, int Quant)
        {
            banco.Configuration.ProxyCreationEnabled = false;
            var itemPedido = new ItemPedido { IdItem = Id, Quantidade = Quant };
            var cookie = Request.Cookies[User.Identity.GetUserName()];
            var id = int.Parse(cookie.Value);
            var ped = await PedidoRepository.UpdateQuantidadeAsync(itemPedido, id);

            string[] result = new string[5];

            result[0] = ped.ItemPedido.Subtotal.ToString();
            result[1] = ped.ItemPedido.IdItem.ToString();
            result[2] = ped.ItemPedido.Quantidade.ToString();
            result[3] = ped.CarrinhoViewModel.Itens.Count.ToString();
            result[4] = ped.CarrinhoViewModel.Total.ToString();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public JsonResult CorreiosCalc(long cep)
        {

            string nCdEmpresa = string.Empty;
            string sDsSenha = string.Empty;
            string nCdServico = "41106";
            string sCepOrigem = "36774016";
            string sCepDestino = cep.ToString();
            string nVlPeso = 2.ToString();
            int nCdFormato = 1;
            decimal nVlComprimento = 40;
            decimal nVlAltura = 40;
            decimal nVlLargura = 40;
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