using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ResourcesAPI.Models;
using ResourcesAPI.Wrappers;

namespace ResourcesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        /*private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };*/

        //private readonly ILogger<FilesController> _logger;
        private readonly ResourcesContext _context;

        private readonly IWebHostEnvironment _env;
        public FilesController(ResourcesContext context, IWebHostEnvironment env)
        {
            _context = context;

            _env = env;
        }
        /*public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
        }*/

        [HttpGet("GetAllFiles")]
        public async Task<ActionResult<IEnumerable<Files>>> GetAllFiles()
        {
            return await _context.Files.ToListAsync();
        }

        [HttpGet("GetFile/{rowid}")]
        public async Task<ActionResult<Files>> GetFile(int rowid)
        {
            var file = await _context.Files.FindAsync(rowid);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }

        [HttpGet("GetFilesByClass/{class_id}")]
        public async Task<dynamic> GetFilesByClass(int class_id)
        {
            IEnumerable<dynamic> files = await _context.Files
            .Where(x => x.class_id == class_id)
            //.Select(x => new { x.filename, x.filedata })
            .ToListAsync();

            foreach (Files file in files)
            {
                file.filedata = await GetFileUrlLocalAsync(file);
            }

            return files.Select(x => new { x.filename, x.filedata });
        }

        [HttpPut("PutFile/{rowid}")]
        public async Task<IActionResult> PutFile(int rowid, [FromBody] ResourcesRequest request)
        {
            dynamic params_ = JsonConvert.DeserializeObject(request.parameters);

            dynamic data = JsonConvert.DeserializeObject(request.data);

            //int rowid = params_["rowid"];

            if (!FileExists(rowid))
            {
                return BadRequest();
            }

            Files file = _context.Files.FirstOrDefault(x => x.rowid == rowid);

            string filename = data["filename"];
            //string filedata = data["filedata"];
            int class_id = data["class_id"];
            int class_order = data["class_order"];
            string userid = data["userid"];

            file.filename = filename;
            //file.filedata = filedata;
            file.class_id = class_id;
            file.class_order = class_order;

            //
            file.createdby = userid;
            file.createdon = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("PostFile")]
        public async Task<dynamic> PostFile([FromBody] ResourcesRequest request)
        {
            /*
            Request Example

            {
            "process": "Files",
            "action": "PostFile",
            "data": "{\"rowid\": 0,\"filename\": \"filename\",\"filedata\": \"base64/url",\"class_id\": 0,\"class_order\": 0,\"userid\": \"jco\"}",
            "parameters": "{}"
            }

            */

            dynamic data = JsonConvert.DeserializeObject(request.data);

            string filename = data["filename"];
            string filedata = data["filedata"];
            int class_id = data["class_id"];
            int class_order = data["class_order"];
            string createdby = data["userid"];

            //Reffers to a new version of the file with the same name of the same owner
            if (ExistFileByNameAndUser(filename, createdby))
                filename = DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_" + filename;

            Files file = new Files
            {
                filename = filename,
                filedata = filedata,
                class_id = class_id,
                class_order = class_order,
                createdby = createdby,
                createdon = DateTime.Now
            };

            _context.Files.Add(file);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFile", new { rowid = file.rowid }, file.filename);
        }

        // DELETE: api/YourModels/5
        [HttpDelete("DeleteFile/{id}")]
        public async Task<IActionResult> DeleteFile(int rowid)
        {
            var file = await _context.Files.FindAsync(rowid);
            if (file == null)
            {
                return NotFound();
            }

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FileExists(int rowid)
        {
            return _context.Files.Any(e => e.rowid == rowid);
        }

        private bool ExistFileByNameAndUser(string filename, string createdby)
        {
            return _context.Files.Any(e => e.filename == filename && e.createdby == createdby);
        }

        private async Task<string> GetFileUrlLocalAsync(Files file)
        {
            if (file.filedata.Contains("http"))
                return file.filedata;

            // Get the absolute path of the running app
            string appPath = AppDomain.CurrentDomain.BaseDirectory;

            string folderPath = Path.Combine(appPath,"public/Files");

            // Check if the folder exists, if not, create it
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (file.filedata.Contains(","))
                file.filedata = file.filedata.Split(",")[1];

            // Convert the base64 string to bytes
            byte[] bytes = Convert.FromBase64String(file.filedata);
            // Get the full path of the file
            string filePath = Path.Combine(folderPath, file.filename);
            // Save the bytes to a file
            await System.IO.File.WriteAllBytesAsync(filePath, bytes);

            var host = HttpContext.Request.Host.ToUriComponent();
            
            // Get the absolute path of the Data folder
            string dataPath = Path.Combine(folderPath, file.filename);

            // Get the relative path of the Data folder from the app path
            string relativePath = Path.GetRelativePath(appPath, dataPath);

            // Return the local URL of the file
            return $"{HttpContext.Request.Scheme}://{host}/{relativePath}";
        }
    }
}
