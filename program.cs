using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

public class HashGenerator
{
    public string GetHash(string input, int paddingLength, int runs)
    {
        List<Dictionary<string, object>> final = new List<Dictionary<string, object>>();
        string padding = new string('0', paddingLength);
        int currentRuns = 0;
        int postFix = 0;
        while (true)
        {
            string target = input + postFix.ToString();
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(target));
                string encodedHash = BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
                if (encodedHash.StartsWith(padding))
                {
                    final.Add(new Dictionary<string, object> { { "hash", encodedHash }, { "postfix", postFix } });
                    currentRuns++;
                }
                if (currentRuns == runs)
                {
                    string s = "{\"hash\":" + Newtonsoft.Json.JsonConvert.SerializeObject(final) + ",\"type\":\"HashChallenge\"}";
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
                }
                postFix++;
            }
        }
    }
}


class Program
{
    static void Main(string[] args)
    {
        var hashGenerator = new HashGenerator();
        var result = hashGenerator.GetHash("7a7cbff4-40e5-499f-ab6d-b7a8d69956af", 3, 25);
        Console.WriteLine(result);
        Thread.Sleep(-1);
    }
}
