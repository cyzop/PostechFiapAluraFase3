namespace PosTech.CadPac.Producer.Api.Authentication
{
    public class AuthenticationCredentials
    {
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public string BaseHash { get;private set; }

        public AuthenticationCredentials(string clientId, string clientSecret, string baseHash)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            BaseHash = baseHash;
        }
    }
}
