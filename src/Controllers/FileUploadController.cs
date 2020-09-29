using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using src.Models;
using src.Services;

namespace src.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly OFXTransactionService _oFXTransactionService;


        public FileUploadController(ILogger<FileUploadController> logger, OFXTransactionService oFXTransactionService)
        {
            _logger = logger;
            _oFXTransactionService = oFXTransactionService;
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                List<OFXTransactionModel> transactionsList = new List<OFXTransactionModel>();
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName();
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        stream.Close();
                    }
                    transactionsList = _oFXTransactionService.ReadOFXFileTransactions(filePath);
                    _oFXTransactionService.AddOFXTransactions(transactionsList);
                }
            }

            return View("~/Views/Home/Index.cshtml",await _oFXTransactionService.getTransactions());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
