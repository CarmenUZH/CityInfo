using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider) //Basically says "figure out what file type you need"
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ??
                throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        private readonly String Thoughts = "Hey, glaubst du, dass Chad Chad nur so beliebt ist, weil sie 'twinge of cringe' so funny sagt?";
        private readonly String ComicThoughts = "We see Detective silver in front of a big pinboard with red string. You can basically just copy and paste detective pinboards. " +
            "in the same pannel we get the speech bubble: 'Well well well, if it isn't my favourite overworked detective'. Then second panel:" +
            "Its silver from the front, with a blurry Cara in the background, she's standing in the door and is backlit. Silver is also backlit. Silver says 'I don't have time for this, Cara' " +
            "Third panel, zoom in on Cara, she has the most 'im secretly the murderer smile' she says 'Silver, my friend, i dont need you TIME,'" +
            "we switch to the next panel, its just caras hand hodling the envelope of information about whateverthefucksilver was studying and she finishes her sentence with 'i need your ATTENTION'";

        [HttpGet]
        public ActionResult GetFile()
        {
            var pathToFile = "testpdf.pdf";
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(
                pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            Console.WriteLine(Thoughts);
            return File(bytes,contentType, Path.GetFileName(pathToFile)); //Content type for pdf is not "text/plain", gets determined by the file extension content blödsinn
        }
    }
}
