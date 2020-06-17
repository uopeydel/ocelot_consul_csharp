using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp1
{
    public class Encrypt
    {
        /// <summary>
        /// This method is equals comment below
        /// </summary>
        /// <param name="_privateKey"></param>
        /// <param name="_tokenKey"></param>
        /// <returns></returns>
        private static string EncryptConvertJavaToCSharp(string _privateKey, string _tokenKey)
        {
            string datetimeWithKey = DateTime.Now.ToString("YYYY-MM-ddTHH:mm:ss.fff");
            datetimeWithKey = datetimeWithKey + "|" + _tokenKey;
            datetimeWithKey = "2020-06-09T14:51:39.845|94sJdS9gDM5bJAdnjgzwHlFH";//test
            string result = "";
            TripleDES tripDES = new TripleDESCryptoServiceProvider();
            tripDES.Mode = CipherMode.ECB;
            tripDES.Padding = PaddingMode.PKCS7;
            byte[] keyBytes = Encoding.UTF8.GetBytes(_privateKey);
            byte[] ivBytes = new byte[8];
            tripDES.Key = keyBytes;
            tripDES.IV = ivBytes;
            byte[] writeBytes = Encoding.UTF8.GetBytes(datetimeWithKey);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, tripDES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(writeBytes, 0, writeBytes.Length);
                    cs.Close();
                }
                result = Convert.ToBase64String(ms.ToArray());
            }
            return result;
        }
    }
     
    /*

        private static String privateKey = "nOtnyYDW2GNPt2020DhipaYa";
        private static String tokenKey = "94sJdS9gDM5bJAdnjgzwHlFH";
        private static String genToken(){
        String token = "";
        SimpleDateFormat fmtDate = new SimpleDateFormat("YYYY-MM-dd",Locale.ENGLISH); 
        SimpleDateFormat fmtTime= new SimpleDateFormat("HH:mm:ss.S",Locale.ENGLISH); 
        
        try{
            Date d = new Date();
            String d1 = fmtDate.format(d)+"T"+fmtTime.format(d);//format >> YYYY-MM-ddTHH:mm:ss.S
            System.out.println("dd format >> YYYY-MM-ddTHH:mm:ss.S : \n"+d1);
            
            
            String plaintext = d1 + "|" + tokenKey;
            System.out.println("plaintext = \n"+plaintext);

            //Encrypt
            byte[] encryptPassword = privateKey.getBytes("utf-8");
            Cipher cipher = Cipher.getInstance("DESEDE/ECB/PKCS5Padding");
            SecretKeySpec myKey = new SecretKeySpec(encryptPassword,"DESede");
            cipher.init(Cipher.ENCRYPT_MODE, myKey);
            byte[] encryptedPlainText = cipher.doFinal(plaintext.getBytes("UTF-8"));
            
            //Encode to Base64 Format
            String encoded = DatatypeConverter.printBase64Binary(encryptedPlainText);//version 1.6-1.7
            
            
            token = encoded;
            
        }catch(Exception e){
            e.printStackTrace();
        }
        
        System.out.println("token: "+token);
        return token;
    }


    */
}
