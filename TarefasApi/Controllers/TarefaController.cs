using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TarefasApi.Models.Dtos;
using TarefasApi.Models.Entities;
using TarefasApi.Repository.Interfaces;


namespace TarefasApi.Controllers
{
    [ApiController]
    [EnableCors("AllowCors")]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepository _repository;
        private readonly IFilaTarefaRepository _repositoryMsg;
        private readonly IMapper _mapper;
        private readonly ILogger<TarefaController> _logger;

        public TarefaController(ITarefaRepository repository,
                                IFilaTarefaRepository repositoryMsg,
                                IMapper mapper,
                                ILogger<TarefaController> logger)
        {
            _repository = repository;
            _repositoryMsg = repositoryMsg;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tarefas = await _repository.GetTarefas();

            _logger.LogInformation("Consulta à lista de tarefas");

            return tarefas.Any()
                ? Ok(tarefas.ToList().OrderByDescending(x => x.Id))
                : NotFound("Tarefas não encontradas");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest("Tarefa inválida");

            var tarefa = await _repository.GetTarefaById(id);

            var tarefaRetorno = _mapper.Map<TarefaDto>(tarefa);

            _logger.LogInformation("Consulta à tarefa com Id = ${id}", id);

            return tarefaRetorno != null
                ? Ok(tarefaRetorno)
                : NotFound("Tarefa não existe na base de dados");
        }

        [HttpPost]
        public async Task<IActionResult> Post(TarefaAdicionarDto tarefa)
        {
            if (string.IsNullOrEmpty(tarefa.Descricao)) return BadRequest("Dados inválidos");

            var tarefaAdicionar = _mapper.Map<Tarefa>(tarefa);

            _repository.Add(tarefaAdicionar);

            _logger.LogInformation($"Adição da tarefa {tarefaAdicionar.Descricao}", tarefaAdicionar.Id);

            var msg = $"Tarefa \"{tarefaAdicionar.Descricao}\" criada com sucesso em " + DateTime.Now.ToString("dd/MM/yyyy") + ".";
            _repositoryMsg.SendMsgTarefa(msg);

            return await _repository.SaveChangesAsync()
                ? Ok()
                : BadRequest("Erro ao adicionar a Tarefa");

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TarefaAtualizarDto tarefa)
        {
            if (id <= 0) return BadRequest("Tarefa inválida");

            var tarefaBanco = await _repository.GetTarefaById(id);

            if (tarefaBanco == null)
                return NotFound("Tarefa não encontrada na base de dados");

            var tarefaAtualizar = _mapper.Map(tarefa, tarefaBanco);

            _repository.Update(tarefaAtualizar);

            _logger.LogInformation($"Atualização da tarefa com Id = {tarefaAtualizar.Id}", tarefaAtualizar.Id);

            var msg = $"Tarefa alterada com sucesso (Id = {tarefaAtualizar.Id}) em " + DateTime.Now.ToString("dd/MM/yyyy") + ".";
            _repositoryMsg.SendMsgTarefa(msg);

            return await _repository.SaveChangesAsync()
                ? Ok()
                : BadRequest("Erro ao atualizar a Tarefa");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Tarefa inválida");

            var tarefaBanco = await _repository.GetTarefaById(id);

            if (tarefaBanco == null)
                return NotFound("Tarefa não encontrada na base de dados");

            _repository.Delete(tarefaBanco);

            _logger.LogInformation($"Exclução da tarefa com Id = {id}", id);

            var msg = $"Tarefa excluída com sucesso (Id = {id}) em " + DateTime.Now.ToString("dd/MM/yyyy") + ".";
            _repositoryMsg.SendMsgTarefa(msg);

            return await _repository.SaveChangesAsync()
                ? Ok()
                : BadRequest("Erro ao deletar a Tarefa");
        }

        [HttpGet("getMessages")]
        public async Task<IActionResult> GetMessages()
        {
            var listaMsgs = await _repositoryMsg.RecieveMsgTarefa();

            _logger.LogInformation("Consulta à lista de tarefas");

            return listaMsgs.Any()
                ? Ok(listaMsgs)
                : NotFound("Não há mensagens disponíveis no momento.");
        }

    }
}

