using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace src.Models
{
    public class OFXTransaction
    {
        public string TransactiontType { get; set; }
        public DateTime PostedDate { get; set; }
        public string TransactionAmount { get; set; }
        public string Description { get; set; }
        

        public void ReadOFXFileTransactions(string filePath)
        {
            string str = null;
            List<string> registers = new List<string>();
            if (File.Exists(filePath))
            {
                Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader streamReader = new StreamReader(stream);
                str = streamReader.ReadToEnd();
                streamReader.Close();
                stream.Close();
            }
            string pattern = @"<STMTTRN>[\s\S]*?<\/STMTTRN>";
        
            RegexOptions options = RegexOptions.Multiline;
            
            foreach (Match m in Regex.Matches(str, pattern, options))
            {
                registers.Add(m.Value);
            }

            const string transactionTypePattern = @"<TRNTYPE>[\s\S]*?\n";
            const string postedDatePattern = @"<DTPOSTED>[\s\S]*?\n";
            const string transactionAmountPattern = @"<TRNAMT>[\s\S]*?\n";
            const string descriptionPattern = @"<MEMO>[\s\S]*?\n";

            foreach(var reg in registers)
            {
                this.TransactiontType = Regex.Matches(reg, transactionTypePattern, options)[0].Value.Split(">")[1].Replace(System.Environment.NewLine,"");
                var date = Regex.Matches(reg, postedDatePattern, options)[0].Value.Split(">")[1].Split("[")[0].Replace(System.Environment.NewLine,"");
                this.TransactionAmount = Regex.Matches(reg, transactionAmountPattern, options)[0].Value.Split(">")[1].Replace("-","").Replace(System.Environment.NewLine,"");
                this.Description = Regex.Matches(reg, descriptionPattern, options)[0].Value.Split(">")[1].Replace("\'n","").Replace(System.Environment.NewLine,"");

                string text= date.Substring(6,2) + "/" + date.Substring(4,2) + "/" + date.Substring(0,4);

                this.PostedDate = DateTime.ParseExact(text, "dd/MM/yyyy", null);
            }
            
        }

    }
}