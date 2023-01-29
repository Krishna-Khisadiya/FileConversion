using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace FileConversion.Controllers
{
    /// <summary>
    /// File Conversion
    /// </summary>
    [ApiController]
    [Route("file")]
    public class FileController : ControllerBase
    {
        /// <summary>
        ///   Convert File into ZIP 
        /// </summary>
        /// <param name = "file">Upload File</param>
        /// <returns>Download Converted Zip File</returns>
        /// <response code="400">If the file is null</response>
        /// <response code="200">If the file is suceessfully converted into zip </response>
        [HttpPost]
        [Route("convert/zip")]
        [Produces("application/zip")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ConvertFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not Found");

            var zipFileMemoryStream = new MemoryStream();

            try
            {
                using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Create, true))
                {
                    var entryFile = archive.CreateEntry(file.FileName);
                    using (var entryStream = entryFile.Open())
                    {
                        using (var fileStream = file.OpenReadStream())
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }
                    }
                }
                zipFileMemoryStream.Seek(0, SeekOrigin.Begin);
                return File(zipFileMemoryStream, "application/zip", file.Name + ".zip");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

