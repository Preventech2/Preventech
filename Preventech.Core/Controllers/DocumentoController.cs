using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;

namespace Preventech.Core.Controllers
{
    [Route("api/documentos")]
    [ApiController]
    public class DocumentoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DocumentoController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost("upload/{ordemId:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocumento(Guid ordemId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }

            try
            {
                // 1. Defina o diretório de destino: wwwroot/uploads/{OrdemId}
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", ordemId.ToString());

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // 2. Gere um nome de arquivo único para evitar colisões
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePathOnDisk = Path.Combine(uploadsFolder, uniqueFileName);

                // 3. Salve o arquivo no disco
                using (var stream = new FileStream(filePathOnDisk, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 4. Crie o registro no banco de dados
                var caminhoRelativo = Path.Combine("uploads", ordemId.ToString(), uniqueFileName);

                var documento = new DocumentoAnexado(
                    NomeArquivo: file.FileName,
                    TipoConteudo: file.ContentType,
                    Url: caminhoRelativo // Salva o caminho relativo no DB
                );
                documento.OrdemServicoId = ordemId;

                _context.DocumentosAnexados.Add(documento);
                await _context.SaveChangesAsync();

                // 5. Retorna sucesso
                return Ok(new { Message = "Documento anexado com sucesso!", DocumentoId = documento.Id });
            }
            catch (Exception ex)
            {
                // Registre o erro no log
                return StatusCode(500, $"Erro interno ao fazer upload: {ex.Message}");
            }
        }


        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadDocument(Guid id)
        {
            try
            {
                var documento = await _context.DocumentosAnexados
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (documento == null || string.IsNullOrEmpty(documento.Url))
                {
                    return NotFound("Documento não encontrado.");
                }

                // 1. Obtenha o caminho completo no disco
                var fullPath = Path.Combine(_environment.WebRootPath, documento.Url);

                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound("Arquivo no disco não encontrado.");
                }

                // 2. Use FileStreamResult para ler e servir o arquivo do disco eficientemente
                var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);

                string contentType = string.IsNullOrEmpty(documento.TipoConteudo)
                    ? "application/octet-stream"
                    : documento.TipoConteudo;

                return File(stream, contentType, documento.NomeArquivo);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao processar o download: {ex.Message}");
            }
        }
    }
}
