using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface IUtils
    {
        string Md5(string sInput);
        string Sha256(string data);
        string GetIpAddress();
    }
    public interface IVnPayLibrary
    {
        void AddRequestData(string key, string value);
        void AddResponseData(string key, string value);
        string GetResponseData(string key);
        string CreateRequestUrl(string baseUrl, string vnp_HashSecret);
        bool ValidateSignature(string inputHash, string secretKey);
    }
}
