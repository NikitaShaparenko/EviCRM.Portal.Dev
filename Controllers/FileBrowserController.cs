using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace EviCRM.Portal.Controllers
{
    public class FileBrowserController : Controller
    {
        IWebHostEnvironment Environment;

        public FileBrowserController(IWebHostEnvironment enviroment)
        {
            Environment = enviroment;
        }


        public FileResult OnGetDownloadFile(string fileName)
        {
            string path = Path.Combine(Environment.WebRootPath, "store", fileName);

            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }

        // GET: FileBrowserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FileBrowserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FileBrowserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FileBrowserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FileBrowserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FileBrowserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FileBrowserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FileBrowserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
