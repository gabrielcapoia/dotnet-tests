using NerdStore.BDD.Tests.Config;

namespace NerdStore.BDD.Tests.User
{
    public abstract class BaseUserScreen : PageObjectModel
    {
        protected BaseUserScreen(SeleniumHelper helper) : base(helper) { }

        public void AcessarSiteLoja()
        {
            Helper.IrParaUrl(Helper.Configuration.DomainUrl);
        }

        public bool ValidarSaudacaoUsuarioLogado(Usuario usuario)
        {
            return Helper.ObterElementoPorXPath("/html/body/header/nav/div/div/ul/li[1]/a").Text.Contains(usuario.Email);
        }

        public bool ValidarMensagemDeErroFormulario(string mensagem)
        {
            return Helper.ObterTextoElementoPorClasseCss("text-danger")
                .Contains(mensagem);
        }
    }
}
