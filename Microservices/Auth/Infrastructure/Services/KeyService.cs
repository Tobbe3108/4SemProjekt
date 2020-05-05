using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Api.Infrastructure.Services
{
    public class KeyService
    {
        public static RSAParameters ParseXmlString(string xml)
        {
            RSAParameters parameters = new RSAParameters();

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(xml);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (System.Xml.XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }
            return parameters;
        }


        public static RsaSecurityKey BuildRsaSigningKey(string xml)
        {
            var parameters = ParseXmlString(xml);
            var rsaProvider = new RSACryptoServiceProvider(2048);
            rsaProvider.ImportParameters(parameters);
            var key = new RsaSecurityKey(rsaProvider);
            return key;
        }
    }
}

