using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Common.Security
{
    public static class Security
    {
        private static bool _optimalAsymmetricEncryptionPadding = false;
        private static readonly string PublicKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPnhacC9HemNUUTdWV1c2TE56SHh0bWRNU0F3QmgxNGM3MWlkUGRhRUR4dUVxend2YS82bzRaLysrL2VjWHVSTEcyQW0xSk9uZkFLSm5OS211MnRENm05MDhFckRHM3pOekRYWDN4S2xFbThwSGFoYlJMSlhUcmYralNnYjFpTmtRZmZiT0hFT2lVTm5ldDBEOTBPVFhhY1Z0Z3UxVnJlYVErdnVFUkxsMUpYVSs1TTlvS0F1R1J2eVZ5anN6N0NHR3RRTTBJaHpJRFdrMGFDdWZINGJ4SFpWbWZhYitUQUlHOEt1aHdSQll3aHpzM3VmOWFhMmVJVWZjbVR0MitNVkxrZnJNZkhmRS9QbWoycmhkMEtHV1VQUVR4NUErM0xUY3EydWR6LzJyTmRGaDBXNk4yaTZMenJJV2Rpb1B3Mk9NVU1ld0FPNmFGSkNKT3JOT3NaV1hob0pLRXpMR2VsYTJadi95NWZGKzdTNURVRmIyWFJRWUxmWFFDOCs2c1pmcTd5SmVRaHBSSWVNNE1lTFRBazRoNlZ2Z3Zpamx3YmEyY0lhZ3RWYnJLczg5OFdCQnNWeUdFUDlnbWhKR0F6dDRWSE80Y3FTOVNtN1Z2bmxncUNHaHJmSk5mRmpkZ3JRZ1VZMGJ6QmtGd2FUNzRWNHRvbFY3RnJ5UmlWdFU0RzJoaTBwZVhtZW1qRjE1UElMM0JzdnZtOExZK3AyN1ptaVhLTTB3VDZncjdaME4zQkVUbnY3dERXWk9hN3FFdEN3bTRieGlQem81WVdUYWlOWU5FUld0a2RDdkFaaThLRzFJN2RBTFNaWGVMTFZkV0s2UEpXa2ZKaDA1WVE3VWNyQUovcVp4TjdTSElIeXhsSHU4R0h1ck1DeE8vcVJDSTI2TW9RaDFmdGdLbDhrPTwvTW9kdWx1cz48RXhwb25lbnQ+QVFBQjwvRXhwb25lbnQ+PC9SU0FLZXlWYWx1ZT4=";
        private static readonly string PrivateKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPnhacC9HemNUUTdWV1c2TE56SHh0bWRNU0F3QmgxNGM3MWlkUGRhRUR4dUVxend2YS82bzRaLysrL2VjWHVSTEcyQW0xSk9uZkFLSm5OS211MnRENm05MDhFckRHM3pOekRYWDN4S2xFbThwSGFoYlJMSlhUcmYralNnYjFpTmtRZmZiT0hFT2lVTm5ldDBEOTBPVFhhY1Z0Z3UxVnJlYVErdnVFUkxsMUpYVSs1TTlvS0F1R1J2eVZ5anN6N0NHR3RRTTBJaHpJRFdrMGFDdWZINGJ4SFpWbWZhYitUQUlHOEt1aHdSQll3aHpzM3VmOWFhMmVJVWZjbVR0MitNVkxrZnJNZkhmRS9QbWoycmhkMEtHV1VQUVR4NUErM0xUY3EydWR6LzJyTmRGaDBXNk4yaTZMenJJV2Rpb1B3Mk9NVU1ld0FPNmFGSkNKT3JOT3NaV1hob0pLRXpMR2VsYTJadi95NWZGKzdTNURVRmIyWFJRWUxmWFFDOCs2c1pmcTd5SmVRaHBSSWVNNE1lTFRBazRoNlZ2Z3Zpamx3YmEyY0lhZ3RWYnJLczg5OFdCQnNWeUdFUDlnbWhKR0F6dDRWSE80Y3FTOVNtN1Z2bmxncUNHaHJmSk5mRmpkZ3JRZ1VZMGJ6QmtGd2FUNzRWNHRvbFY3RnJ5UmlWdFU0RzJoaTBwZVhtZW1qRjE1UElMM0JzdnZtOExZK3AyN1ptaVhLTTB3VDZncjdaME4zQkVUbnY3dERXWk9hN3FFdEN3bTRieGlQem81WVdUYWlOWU5FUld0a2RDdkFaaThLRzFJN2RBTFNaWGVMTFZkV0s2UEpXa2ZKaDA1WVE3VWNyQUovcVp4TjdTSElIeXhsSHU4R0h1ck1DeE8vcVJDSTI2TW9RaDFmdGdLbDhrPTwvTW9kdWx1cz48RXhwb25lbnQ+QVFBQjwvRXhwb25lbnQ+PFA+MDZXUXNRNVRnWFFyQ2RnWUFweXFEd3J6dnZMKzExMCs2ckhNejZBTEQrV2pIM2lCdnc3cGJENktTVXkxK0dEVkZ2bGNTdmt4SkRpUDZxTE91alRqWGZqYXhYU3kzclI5UU1FTGE3ZjBDN3FjT1NHeFd6MjhMUElTQmpyb1pXYXphVmVndHc3SW9SeFp3dUY1NW5sTlpGOEFGbUNlUG11RnI0KzFJWUdmQkFVSDAwTEk3dkFOSXA4YTEyVER1UlNzMlBOdnBoa2FaR25xVGNXNlZWR0tnVDNqME8wV0lxelFvZ29GQW5xSEJhazZjKzl2U29kWDBMbjNnOXNjU3ltektEOEFFYkxZSlorWE9yOWFpNUVnTUtZdjVJazl1Zjc5V0tMTmRmRWp3NGphYk9nTnJ2dmpXRmlxeVdFVXdJTjlYRk5qOTI2VHJWM09saEZxM0wvVnR3PT08L1A+PFE+N3dPSi9ZdkpZWkpwTVVTMjZ2QWVRaXN2SWN6ZzZkMFhobU9LbXRsQzJnMlh5MXRUYnFJRUdvY2xucTMzZ0hEdmw3THI0U2owZTdPWG5zckNYTThMQ2twOEcwbGowLzFJZEEwTjRpR0FZYUNCSElyZTVsRjFvZW92TFNWY25hbGhhQWRhdERoMWpxM0tjZVVyM0xIUkN2UEExUHhqT2RiK252R1ZxVThPNVRXYWgvVHVDSG5ITG1wQTZWeVNUejBOUE1KSlIybWp2TW1ia3gzTGdnR291aGxCcW03Y0FidW1vRXVEdzVyaUFQR3VXZElrVG5ZL3E5VWtRdkJkc0lYSkdxUld6MlZNRkZYWkgwcElCNHNtaTFKV0wyNkhLeVV5YlNkL0E1WnRtK3lnOVd5Z2l1VGdaVDQvWE56eHFLRjh4NzhZQXhCaS9jRlFqOU8yMTh6K2Z3PT08L1E+PERQPkdKb01RYWlLcUdlYWdTQnZXZzVmN2pPMXRhS0ZZUDlqeXVBMEVJWm9Bc2NNdVNIRzR6Q1dqWkNQZm9tK05VcjhPaGx1VXdDNFo1OXppekMxbXd0K3luT2M2b3J3SjljUElESFFaQXNVQVRFRjY5WXd5WXhaalZmdUpHOUFpRmp4emZMenFaTzhXbzN6R1NZZ2gxdmVWU1o5Mmh3TkNQQnlyNytpckd4bTBZN01KQkNGVHREMGxldXNzUkR6ZDFZaDZXNFNzbG9FcVZ0dWN1L2pBdnZoZjdoei9Oc3lVWUdKRnV6WFBEeUY2WENNYk9HT0xEbWpTWTMwYVkxVXNwLzhSVmRkYUxGRFVzeEVSZjNzUm9EdUh2bVNZNXhZWW9CemJwUXVFZUdmNktERjB4YitVRExUY0hlT0VxNklQUUNMMSttUm16ZHRUeCt5WGR0VWNxaVI3UT09PC9EUD48RFE+RTFGMDZNSlFuNHRHNTAySGYwdXpGbVFKSmpFNkNJenZKdFNLMG1NM0RRckdOeENGbU02TmlGdGVCZ3BIaytFTTdIVHdyYjB5cllEaGcxc2VCSVJUZnh4d0ZZQ0VDaEZSdHRlTFJMaXllTEdSWGU0M2YrUWJudCtmdW9WbmkvS3h4Y2U2WkJDZ0o4MFNMTXg2RkJkbkx2eHVuSWhkdU1JUXlHWDhVN2ZwRzB1TzF1aE1DUEZXMFFDVGlJa0w1azJuOFQ0K3ovL1ExU1J5WjV5ajR6a2hHeEdKaGkzNXFFWXpmSWM5K2lSZG1mZnV3S0hFZjNiOVJIVXd2aHhSWmtBWjlnWk1ZZXp0RGtPMkRkcGlJZ0gwTnBTTUZMOVpPM3FCTGVOZ1Y3amoyUk55R3dRUm9jaEgvc2JlTmpKQmNnRERaaDJpNGpTRDI4Q3hNejdJYTM3Tkh3PT08L0RRPjxJbnZlcnNlUT5rZ0NYT3lxV1IxUHNVZXhVRFZDWDhJaE1hQml6SDFLYk9GbzBSNFlLS0tNN29xa1J4dzAwc0hpajhGTXV5OWVuTmVXQUhaYkh6TVhRWGtKVVIxK0JIRmIveDVnNUhYWjR5dlJaZjE4Sks5RlZiRVhubG1mTTRQMVpMZUJOalhoMTFaZkdSYm1EQ2Z1NVZBZDFSMFNaK3F5UnpqY2dCZ25lWEFheXQySlNHTWRUQ3o4a1ZReVdKem1mTWNmak5vNGVEV24vckZVWDE0SWhvYjdoc0J2ZlkyNVhVTVExU1VQUmYwY25GTEc3bkQ4OHJvUnZ0b3hNVmtReDVpbERpaGRtaFYvTkNNT3ZZRCtBcDNCSFNpd2prMktnc1FGaHhLOWxHUUxPWFlpYjJvdDJ5VlpPOTVOcDdEY3IvajFiUW96dHNLU0FjOXJtY0xubVFWaHdyRCtqSEE9PTwvSW52ZXJzZVE+PEQ+TndPcWU2TTd3OUFLdVB4OG0zL1E2OUN0NVJRSUtYOUhFM3BpVHZlREc2MjR3Mmdqa3FhTGlialVMZEJXOUhuYnptMzFkczk5L0trNlhwa1hTMmgvUFVHODVkODlud3F0NzROMmRkUkNlN1UzYUx2dGhLSTZDdWx2UnI3bFFUSmR1eFFwa2dqcWVlUU9jNnF1ZjZnV3FTWUVOVDFxYXo0VUF5eGZTTnJ4V0hwcUJqNFFXNFhKM3AyWC9uTzVJdmE2a0U3U0J1VklkU05qS2RWck9kWFI3MUNXOWtpMTUrZHdrelRWbGZDOUpWMzBQS01PRkl2M3lJQS9WSklQcjNjT2NSZ0lNbWNOK3ZodHZBazBaVTkza0JaRUkzMXZZdmNmc01JSGI4N0tDZk8vYmhWL2ZNMFVzM2sxYzdTR0t2bmVLT2RQZ3hMcTV1ck9xZjhkMjZjd1JnOHIrOFNncld2MStaSS9vS3pDajZ6VGJHTExETjB6VWp0cjE2bVJLUFZZVDE1Zk5iQXpjaUNDNk5renZYcWVFM2Y5Y1JQc1NkcTJldGV6azFZUFJPdk92WkRlelFLWFRsYjJyczhCQWtrMExSSTZjTTc0R0hjQmVQVXlTbVpLVnYycUtVNzhWUnNhK3dqcXUvK1BIWDFsRnRtZ2FBanZKU1ZyZEYrN3pteTZVZ096R1U3UU9OV05tRis1bnZyM1M4dHVVMjJDZUtkWGdCYVYwbUpJMUtzN3VWMjVnM0JiRllueVV3MkdwcjlQVndEb3pwL1hDNEdXN2tnZkZMaHgyd0pkUDZ4ZEI2WmFpa1YrZEViNlFOTE1WUzRoRXdmcnFRWHlzU0pZQXlyeURoRzZlRFFqZVJMams2UzFzc1poZzAyTHV6MFVUN1FoR3RZL0p3OG9obms9PC9EPjwvUlNBS2V5VmFsdWU+";
        /// <summary>
        /// Mã hóa password
        /// </summary>       
        public static string EncryptPassword(string Password)
        {
            System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
            byte[] hashBytes = encoding.GetBytes(Password);

            //Compute the SHA-1 hash
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] cryptPassword = sha1.ComputeHash(hashBytes);

            return BitConverter.ToString(cryptPassword);
        }

        public static string Encrypt(string plainText)
        {
            int keySize = 0;
            byte[] keyBytes = Convert.FromBase64String(PublicKey);
            var stringKey = Encoding.UTF8.GetString(keyBytes);
            var encrypted = Encrypt(Encoding.UTF8.GetBytes(plainText), keySize, stringKey);

            return Convert.ToBase64String(encrypted);
        }

        private static byte[] Encrypt(byte[] data, int keySize, string publicKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            if (String.IsNullOrEmpty(publicKeyXml)) throw new ArgumentException("Key is null or empty", "publicKeyXml");

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(publicKeyXml);
                return provider.Encrypt(data, _optimalAsymmetricEncryptionPadding);
            }
        }

        public static string Decrypt(string encryptedText)
        {
            int keySize = 0;
            byte[] keyBytes = Convert.FromBase64String(PrivateKey);
            var stringKey = Encoding.UTF8.GetString(keyBytes);
            var decrypted = Decrypt(Convert.FromBase64String(encryptedText), keySize, stringKey);

            return Encoding.UTF8.GetString(decrypted);
        }

        private static byte[] Decrypt(byte[] data, int keySize, string publicAndPrivateKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            if (String.IsNullOrEmpty(publicAndPrivateKeyXml)) throw new ArgumentException("Key is null or empty", "publicAndPrivateKeyXml");

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(publicAndPrivateKeyXml);
                return provider.Decrypt(data, _optimalAsymmetricEncryptionPadding);
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
