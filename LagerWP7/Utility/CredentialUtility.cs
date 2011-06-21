using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Phone.Info;

namespace LagerWP7.Utility {
    public class CredentialUtility {
        private const string _fileName = "usercred.dat";
        private const string _defaultKey = "70D47CAF65CB4EEC87DE461EA1FA7FBC";

        internal static void DeleteCredentials() {
            try {
                IsolatedStorageSettings.ApplicationSettings.Remove("UntappdUsername");
                IsolatedStorageSettings.ApplicationSettings.Remove("UntappdPassword");
                IsolatedStorageSettings.ApplicationSettings.Save();
            } catch (Exception) {
            }
        }

        internal static void StoreCredentials() {
            try {
                string anid = UserExtendedProperties.GetValue("ANID") as string;
                string key;
                if (anid != null) {
                    key = anid.Substring(2, 32);
                } else {
                    key = _defaultKey;
                }

                IsolatedStorageSettings.ApplicationSettings["UntappdUsername"] = App.ViewModel.UntappdUsername;
                IsolatedStorageSettings.ApplicationSettings["UntappdPassword"] = Encrypt(App.ViewModel.UntappdPassword, key, App.ApiKey);
                IsolatedStorageSettings.ApplicationSettings.Save();
            } catch (Exception) {
            }
        }

        internal static bool LoadCredentials() {
            try {
                string un = null;
                string pw = null;

                IsolatedStorageSettings.ApplicationSettings.TryGetValue("UntappdUsername", out un);
                IsolatedStorageSettings.ApplicationSettings.TryGetValue("UntappdPassword", out pw);

                if (String.IsNullOrEmpty(un) || String.IsNullOrEmpty(pw)) {
                    return false;
                }

                string anid = UserExtendedProperties.GetValue("ANID") as string;
                string key;
                if (anid != null) {
                    key = anid.Substring(2, 32);
                } else {
                    key = _defaultKey;
                }

                App.ViewModel.UntappdPassword = Decrypt(pw, key, App.ApiKey);
                App.ViewModel.UntappdUsername = un;
                
                return true;
            } catch (Exception) {
            }

            return false;
        }

        internal static string Encrypt(string dataToEncrypt, string password, string salt) {
            AesManaged aes = null;
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;

            try {
                //Generate a Key based on a Password and HMACSHA1 pseudo-random number generator
                //Salt must be at least 8 bytes long
                //Use an iteration count of at least 1000
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000);

                //Create AES algorithm
                aes = new AesManaged();
                //Key derived from byte array with 32 pseudo-random key bytes
                aes.Key = rfc2898.GetBytes(32);
                //IV derived from byte array with 16 pseudo-random key bytes
                aes.IV = rfc2898.GetBytes(16);

                //Create Memory and Crypto Streams
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

                //Encrypt Data
                byte[] data = Encoding.UTF8.GetBytes(dataToEncrypt);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                //Return Base 64 String
                return Convert.ToBase64String(memoryStream.ToArray());
            } finally {
                if (cryptoStream != null)
                    cryptoStream.Close();

                if (memoryStream != null)
                    memoryStream.Close();

                if (aes != null)
                    aes.Clear();
            }
        }

        internal static string Decrypt(string dataToDecrypt, string password, string salt) {
            AesManaged aes = null;
            MemoryStream memoryStream = null;

            try {
                //Generate a Key based on a Password and HMACSHA1 pseudo-random number generator
                //Salt must be at least 8 bytes long
                //Use an iteration count of at least 1000
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000);

                //Create AES algorithm
                aes = new AesManaged();
                //Key derived from byte array with 32 pseudo-random key bytes
                aes.Key = rfc2898.GetBytes(32);
                //IV derived from byte array with 16 pseudo-random key bytes
                aes.IV = rfc2898.GetBytes(16);

                //Create Memory and Crypto Streams
                memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

                //Decrypt Data
                byte[] data = Convert.FromBase64String(dataToDecrypt);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                //Return Decrypted String
                byte[] decryptBytes = memoryStream.ToArray();

                //Dispose
                if (cryptoStream != null)
                    cryptoStream.Dispose();

                //Retval
                return Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
            } finally {
                if (memoryStream != null)
                    memoryStream.Dispose();

                if (aes != null)
                    aes.Clear();
            }
        }

    }
}
