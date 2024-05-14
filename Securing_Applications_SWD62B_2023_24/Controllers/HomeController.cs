using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Securing_Applications_SWD62B_2023_24.Helpers;
using Securing_Applications_SWD62B_2023_24.Models;
using System.Diagnostics;

namespace Securing_Applications_SWD62B_2023_24.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            _webHostEnvironment = hostingEnvironment;
            _logger = logger;
        }

        [CustomActionFilter]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles ="Admin, Moderator")]
        [Authorize()]
        [AuthorizeFileAccess()]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // https://mysite.com/Home/UploadImage/
        // [HttpGet]
        public IActionResult UploadImage()
        {
            return View();
        }

        // probably these whitelists should be setup and kept outside of the controller...
        private List<string> fileExtensionWhiteList = new List<string>()
        {
            "jpg", "jpeg", "png"
        };

        private Dictionary<string, byte[]> signaturePreFixWhiteList = new Dictionary<
            string,     // file extension
            byte[]>();  // accepted byte

        [HttpPost]
        public IActionResult UploadImage(IFormFile fileUpload)
        {
            // setup should be in a helper not here...
            signaturePreFixWhiteList.Add("jpg", new byte[] { 255, 216, 255 });
            signaturePreFixWhiteList.Add("jpeg", new byte[] { 255, 216, 255 });
            // signaturePreFixWhiteList.Add("png", new byte[] { 255, 216, 255 });

            // end setup

            int signatureFileLength = 5;

            if (fileUpload == null)
            {
                return View();
            }

            string fileName = fileUpload.FileName;
            string fileType = fileUpload.ContentType;

            int lastIndexOfFullStop = fileUpload.FileName.LastIndexOf('.');

            if (lastIndexOfFullStop == -1)
            {
                return Content("The file has no extension! Please use a valid extension");
            }

            string fileExtension = fileUpload.FileName.Substring(lastIndexOfFullStop + 1);

            if (!fileExtensionWhiteList.Contains(fileExtension)) {
                return Content("Invalid file extension!");
            }

            // fileType can be used similarly

            long length = fileUpload.Length;

            if (length <= signatureFileLength)
            {
                return Content("File is too small. Has an error occured in upload?");
            }

            using (var stream = fileUpload.OpenReadStream())
            {
                byte[] fileSignature = new byte[signatureFileLength];
                stream.Read(fileSignature, 0, signatureFileLength);

                if (!signaturePreFixWhiteList.ContainsKey(fileExtension))
                {
                    // the file signature for this extension is NOT setup!
                    return Content("You need to setup this signature!!");
                }

                // For the method I'm describing make sure that the length of acceptedFileSignature is NOT zero...
                byte[] acceptedFileSignature = signaturePreFixWhiteList[fileExtension];

                // compare the fileSignature against the acceptedFileSignature
                for (int i = 0; i < acceptedFileSignature.Length; i++)
                {
                    if (fileSignature[i] != acceptedFileSignature[i])
                    {
                        // we have a mismatch!
                        return Content("Incorrect file signature!");
                    }
                }

                // all the bytes of the acceptedFileSignature have matched! So file signature is OK

                // reset the stream to the beginning!
                stream.Seek(0, SeekOrigin.Begin);
                // stream.Position = 0;


                string contentPath = _webHostEnvironment.ContentRootPath;
                string fullPath = contentPath + "/Files/" + Guid.NewGuid().ToString() + "_" + fileName;

                using (var writer = System.IO.File.Create(fullPath))
                {
                    fileUpload.CopyToAsync(writer).Wait();
                }

                // we can handle the file normally (e.g. save it to disk)
                return Content("The file is accepted and uploaded successfully!");

            }

            // return Content("I have received your HTTP Post");
        }
    }
}