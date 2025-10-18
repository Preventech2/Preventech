using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;
using Preventech.Core.Services;

namespace Preventech.Core.Controllers
{
    [Route("api/localizacao")]
    [ApiController]
    public class LocalizacaoController(ApplicationDbContext context) : ControllerBase
    {

        /// <summary>
        /// Handler GET para localizações com filtro, valores inválidos 
        /// são wildcards
        /// </summary>
        /// <param name="filtro">Filtro para localizações</param>
        /// <returns>Lista de localizações que condizem ao filtro</returns>
        [HttpGet]
        public async Task<ApiResponse<List<Localizacao>>> GetLocalizacoes([FromQuery] Localizacao filtro)
        {
            try
            {
                var query = context.Localizacoes
                    .Include(loc => loc.Responsavel)
                    .AsQueryable()
                    .Where(query =>
                        (filtro.Campus <= 0 || query.Campus == filtro.Campus)
                     && (filtro.Predio <= 0 || query.Predio == filtro.Predio)
                     && (filtro.Andar <= 0 || query.Andar == filtro.Andar)
                     && (filtro.Numero <= 0 || query.Numero == filtro.Numero)
                     && (string.IsNullOrWhiteSpace(filtro.Apelido) || query.Apelido == filtro.Apelido)
                    );


                var res = await query.ToListAsync();

                if (!query.Any())
                    return new ApiResponse<List<Localizacao>>
                    {
                        Success = false,
                        Message = $"Localização não encontrada",
                        Data = null
                    };

                return new ApiResponse<List<Localizacao>>
                {
                    Success = true,
                    Message = $"Salas recuperadas",
                    Data = res
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Localizacao>>
                {
                    Success = false,
                    Message = $"Erro ao buscar localização: {ex.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Handler POST para criação de novas localizações
        /// </summary>
        /// <param name="localizacao">Localização a ser adicionada</param>
        /// <returns>Localização que foi adicionada</returns>
        [HttpPost]
        public async Task<ApiResponse<Localizacao>> CadastrarLocalizacao([FromBody] Localizacao localizacao)
        {
            try
            {
                localizacao.Responsavel = (await context.Usuarios.FindAsync(localizacao.Responsavel.Id))!;
                await context.Localizacoes.AddAsync(localizacao);
                await context.SaveChangesAsync();

                return new ApiResponse<Localizacao>
                {
                    Success = true,
                    Message = "Localização cadastrada com sucesso",
                    Data = localizacao
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Localizacao>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar localização: {ex.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Handler PATCH para edição de uma localização
        /// </summary>
        /// <param name="localizacao">localização para ser editada</param>
        /// <returns>Localização editada</returns>
        [HttpPatch]
        public async Task<ApiResponse<Localizacao>> EditarLocalizacao([FromBody] Localizacao localizacao)
        {
            try
            {
                await context.Localizacoes
                    .Where(x => x.Id == localizacao.Id)
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(loc => loc.Apelido, localizacao.Apelido)
                        .SetProperty(loc => loc.Campus, localizacao.Campus)
                        .SetProperty(loc => loc.Predio, localizacao.Predio)
                        .SetProperty(loc => loc.Andar, localizacao.Andar)
                        .SetProperty(loc => loc.Numero, localizacao.Numero)
                        .SetProperty(loc => loc.Responsavel, localizacao.Responsavel)
                    );

                await context.SaveChangesAsync();

                return new ApiResponse<Localizacao>
                {
                    Success = true,
                    Message = "Localização editada com sucesso",
                    Data = localizacao
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Localizacao>
                {
                    Success = false,
                    Message = $"Erro ao editar localização: {ex.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Handler DELETE para localizações
        /// </summary>
        /// <param name="localizacao">Localização para deletar</param>
        /// <returns>Localização deletada</returns>
        [HttpDelete]
        public async Task<ApiResponse<Localizacao>> RemoverLocalização([FromBody] Localizacao localizacao)
        {
            try
            {
                await context.Localizacoes
                    .Where(loc => loc == localizacao)
                    .ExecuteDeleteAsync();

                // Salva as mudanças no banco de dados

                return new ApiResponse<Localizacao>
                {
                    Success = true,
                    Message = "Localização deletada com sucesso",
                    Data = localizacao
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Localizacao>
                {
                    Success = false,
                    Message = $"Erro ao deletar localização: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
