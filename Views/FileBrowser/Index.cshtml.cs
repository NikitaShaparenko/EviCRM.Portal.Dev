using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.IO;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EviCRM.OfficeIntegrator.Models;
using System.Collections.Generic;

namespace EviCRM.OfficeIntegrator.Pages
{
    public class FileBrowserModel : PageModel
    {

        public List<FileModel> Files { get; set; }

        private IHostingEnvironment Environment;
        public FileBrowserModel(IHostingEnvironment _environment)
        {
            this.Environment = _environment;
        }

        public void OnGet()
        {
            //Fetch all files in the Folder (Directory).
            string[] filePaths = Directory.GetFiles(Path.Combine(this.Environment.WebRootPath, "store/"));

            //Copy File names to Model collection.
            this.Files = new List<FileModel>();
            foreach (string filePath in filePaths)
            {
                this.Files.Add(new FileModel { FileName = Path.GetFileName(filePath) });
            }
        }

        public FileResult OnGetDownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(this.Environment.WebRootPath, "store/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        public void OnGetSelectFile(string fileName)
        {
          // BackendConnector.Post(fileName);
         
        }
    }
}
