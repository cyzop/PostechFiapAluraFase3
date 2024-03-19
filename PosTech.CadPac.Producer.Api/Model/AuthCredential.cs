using PosTech.CadPac.Producer.Api.Authentication.DataAnnotation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PosTech.CadPac.Producer.Api.Model
{
    [Serializable]
    [DataContract]
    public class AuthCredential
    {
        [XmlAttribute]
        [DataMember]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [ClientIdValidation]
        public string clientId { get; set; }

        [XmlAttribute]
        [DataMember]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [ClientHashValidation]
        public string secretKey { get; set; }
    }
}
