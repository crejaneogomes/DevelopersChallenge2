using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using src.Model;
using src.Models;

namespace src.Services
{
    public class OFXTransactionService
    {
        const string transactionTypePattern = @"<TRNTYPE>[\s\S]*?\n";
        const string postedDatePattern = @"<DTPOSTED>[\s\S]*?\n";
        const string transactionAmountPattern = @"<TRNAMT>[\s\S]*?\n";
        const string descriptionPattern = @"<MEMO>[\s\S]*?\n";
        const string pattern = @"<STMTTRN>[\s\S]*?<\/STMTTRN>";

        private readonly ILogger<OFXTransactionService> _logger;
        private readonly ChallengeDBContext _context;

        public OFXTransactionService(ILogger<OFXTransactionService> logger, ChallengeDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<OFXTransactionModel> ReadOFXFileTransactions(string filePath)
        {
            string str = null;
            List<OFXTransactionModel> transactions = new List<OFXTransactionModel>();
            List<string> registers = new List<string>();
            if (File.Exists(filePath))
            {
                Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader streamReader = new StreamReader(stream);
                str = streamReader.ReadToEnd();
                streamReader.Close();
                stream.Close();
            }
        
            RegexOptions options = RegexOptions.Multiline;
            
            foreach (Match m in Regex.Matches(str, pattern, options))
            {
                registers.Add(m.Value);
            }

            foreach(var reg in registers)
            {
                OFXTransactionModel transaction = new OFXTransactionModel();
                transaction.TransactiontType = Regex.Matches(reg, transactionTypePattern, options)[0].Value.Split(">")[1].Replace(System.Environment.NewLine,"");
                var date = Regex.Matches(reg, postedDatePattern, options)[0].Value.Split(">")[1].Split("[")[0].Replace(System.Environment.NewLine,"");
                transaction.TransactionAmount = Regex.Matches(reg, transactionAmountPattern, options)[0].Value.Split(">")[1].Replace(System.Environment.NewLine,"");
                transaction.Description = Regex.Matches(reg, descriptionPattern, options)[0].Value.Split(">")[1].Replace("\'n","").Replace(System.Environment.NewLine,"");

                string textDate= date.Substring(6,2) + "/" + date.Substring(4,2) + "/" + date.Substring(0,4);

                transaction.PostedDate = DateTime.ParseExact(textDate, "dd/MM/yyyy", null);
                transactions.Add(transaction);
            }

            return transactions;
            
        }

        public void AddOFXTransactions(List<OFXTransactionModel> transactionsList){
            foreach(var t in transactionsList)
            {
                AddTransaction(t);       
            }
        }

        public async Task<List<OFXTransactionModel>> getTransactions()
        {
            return await _context.OFXTransactionModels.OrderBy(x => x.PostedDate).ToListAsync();
        }

        public void AddTransaction(OFXTransactionModel newData)
        {
            var transaction = _context.OFXTransactionModels.FirstOrDefault(x => x.TransactiontType == newData.TransactiontType 
                && x.PostedDate == newData.PostedDate
                && x.Description == newData.Description);

            if (transaction == null) 
            {
                _context.OFXTransactionModels.Add(newData);
                _context.SaveChanges();
            }
        }

    }
}
