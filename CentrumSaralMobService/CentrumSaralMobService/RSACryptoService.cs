using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CentrumSaralMobService
{
    public class RSACryptoService
    {
        static public X509Certificate2 getCertificate_Private(string certificateName)
        {
            //Opens the personal certificates store.
            X509Store store = null;
            store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            IEnumerable certs = store.Certificates.Find(X509FindType.FindByIssuerName, "Sectigo RSA Domain Validation Secure Server CA", true);
            // X509Certificate2 certificate1 = null;
            //foreach (X509Certificate2 tmpcert in certs)
            //{
            //    certificate1 = tmpcert;
            //    break;
            //}
            X509Certificate2 certificate1 = new X509Certificate2(certificateName, "ftt@123", X509KeyStorageFlags.Exportable);

            X509Certificate2Collection collection = new X509Certificate2Collection();
            collection.Add(certificate1);

            if (collection.Count == 1)
            {
                return collection[0];
            }
            else if (collection.Count > 1)
            {
                throw new Exception(string.Format("More than one certificate with name '{0}' found in store LocalMachine/My.", certificateName));
            }
            else
            {
                throw new Exception(string.Format("Certificate '{0}' not found in store LocalMachine/My.", certificateName));
            }
        }

        static public X509Certificate2 getCertificate_Public(string certificateName)
        {

            //Opens the personal certificates store.
            X509Store store = null;
            store = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            IEnumerable certs = store.Certificates.Find(X509FindType.FindByIssuerName, "ABC Vendor", true);
            X509Certificate2 certificate1 = null;

            foreach (X509Certificate2 tmpcert in certs)
            {
                certificate1 = tmpcert;
                break;
            }
            //Create a collection and add two of the certificates.
            X509Certificate2Collection collection = new X509Certificate2Collection();
            collection.Add(certificate1);

            if (collection.Count == 1)
            {
                return collection[0];
            }
            else if (collection.Count > 1)
            {
                throw new Exception(string.Format("More than one certificate with name '{0}' found in store LocalMachine/My.", certificateName));
            }
            else
            {
                throw new Exception(string.Format("Certificate '{0}' not found in store LocalMachine/My.", certificateName));
            }
        }

        static public string EncryptRsa(byte[] input, byte[] certificate)
        {

            string output = string.Empty;
            X509Certificate2 cert = new X509Certificate2(certificate);
            //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(4096))
            {
                if (cert == null || input.Length <= 0)
                    throw new Exception("A x509 certificate and string for encryption must be provided");

                RSA.FromXmlStringNew(cert.PublicKey.Key.ToXmlString(false));
                byte[] exponent = null;
                byte[] modulus = null;
                if (RSA != null)
                {
                    RSAParameters parameters = RSA.ExportParameters(false);
                    exponent = parameters.Exponent;
                    modulus = parameters.Modulus;
                }

                //Set RSAKeyInfo to the public key values. 
                RSAParameters RSAKeyInfo = new RSAParameters();
                RSAKeyInfo.Modulus = modulus;
                RSAKeyInfo.Exponent = exponent;

                //Import key parameters into RSA.
                RSA.ImportParameters(RSAKeyInfo);
                //byte[] data=Encoding.UTF8.GetBytes(input);          
                byte[] bytesEncrypte = RSA.Encrypt(input, false);
                output = Convert.ToBase64String(bytesEncrypte);
            }
            return output;


        }

        static public byte[] EncryptRsaByte(byte[] input, string certificateName)
        {

            byte[] bytesEncrypte;
            X509Certificate2 cert = getCertificate_Public(certificateName);
            //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                if (cert == null || input.Length <= 0)
                    throw new Exception("A x509 certificate and string for encryption must be provided");
                RSA.FromXmlStringNew(cert.PublicKey.Key.ToXmlString(false));
                byte[] exponent = null;
                byte[] modulus = null;
                if (RSA != null)
                {
                    RSAParameters parameters = RSA.ExportParameters(false);
                    exponent = parameters.Exponent;
                    modulus = parameters.Modulus;
                }

                //Set RSAKeyInfo to the public key values. 
                RSAParameters RSAKeyInfo = new RSAParameters();
                RSAKeyInfo.Modulus = modulus;
                RSAKeyInfo.Exponent = exponent;

                //Import key parameters into RSA.
                RSA.ImportParameters(RSAKeyInfo);
                //byte[] data=Encoding.UTF8.GetBytes(input);          
                bytesEncrypte = RSA.Encrypt(input, false);
                //bytesEncrypte = Convert.ToBase64String(bytesEncrypte);
            }
            return bytesEncrypte;


        }

        static public string decryptRsa(byte[] encrypted, string certificateName)
        {
            string text = string.Empty;
            X509Certificate2 cert = getCertificate_Private(certificateName);
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                if (cert == null || encrypted.Length <= 0)
                    throw new Exception("A x509 certificate and string for encryption must be provided");

                if (!cert.HasPrivateKey)
                    throw new Exception("x509 certicate does not contain a private key for decryption");


                RSA.FromXmlStringNew(cert.PrivateKey.ToXmlString(true));
                string vPublicKey = RSA.ToXmlString(false);
                string vPrivateKey = RSA.ToXmlString(true);
                //byte[] bytesEncrypted = Convert.FromBase64String(encrypted);
                byte[] bytesDecrypted = RSA.Decrypt(encrypted, false);
                text = Encoding.UTF8.GetString(bytesDecrypted);
            }
            return text;
        }

        static public byte[] decryptRsaByte(byte[] encrypted, string certificateName)
        {
            byte[] bytesDecrypted;
            X509Certificate2 cert = getCertificate_Private(certificateName);
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                if (cert == null || encrypted.Length <= 0)
                    throw new Exception("A x509 certificate and string for encryption must be provided");

                if (!cert.HasPrivateKey)
                    throw new Exception("x509 certicate does not contain a private key for decryption");

                RSA.FromXmlStringNew(cert.PrivateKey.ToXmlString(true));

                //byte[] bytesEncrypted = Convert.FromBase64String(encrypted);
                bytesDecrypted = RSA.Decrypt(encrypted, false);
                //text = Encoding.UTF8.GetString(bytesDecrypted);
            }
            return bytesDecrypted;
        }
    }
}