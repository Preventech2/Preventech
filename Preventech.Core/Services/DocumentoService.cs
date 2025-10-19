using Microsoft.AspNetCore.Components.Forms;
using Preventech.Core.DTOs;
using System.Net.Http.Headers;

namespace Preventech.Core.Services;

public class DocumentoService
{
    private readonly HttpClient _httpClient;

    public DocumentoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<object>> UploadDocumentoAsync(Guid ordemId, IBrowserFile file)
    {
        try
        {
            // Cria o objeto de conteúdo multipart
            using var content = new MultipartFormDataContent();

            // Define o tamanho máximo de buffer (deve ser menor que o limite do servidor)
            const int MAX_STREAM_SIZE = 1024 * 1024 * 50; // 5MB

            // Cria o StreamContent a partir do IBrowserFile
            var fileContent = new StreamContent(file.OpenReadStream(MAX_STREAM_SIZE));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            // Adiciona o arquivo ao conteúdo
            content.Add(
                content: fileContent,
                name: "file", // Deve corresponder ao parâmetro [FromForm] IFormFile file no Controller
                fileName: file.Name
            );

            // Envia a requisição
            var response = await _httpClient.PostAsync($"api/documentos/upload/{ordemId}", content);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<object> { Success = true, Message = "Upload concluído." };
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                return new ApiResponse<object> { Success = false, Message = $"Falha no servidor ({response.StatusCode}): {error}" };
            }
        }
        catch (Exception ex)
        {
            return new ApiResponse<object> { Success = false, Message = $"Erro de rede/cliente: {ex.Message}" };
        }
    }
}