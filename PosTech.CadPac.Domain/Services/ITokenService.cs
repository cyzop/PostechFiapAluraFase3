namespace PosTech.CadPac.Domain.Services
{
    public interface ITokenService
    {
        public string GenerateDynamicSecret();

        public object CreateAuthenticationToken(string clientId);
    }
}
