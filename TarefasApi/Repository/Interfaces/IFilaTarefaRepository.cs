using TarefasApi.Models.Dtos;

namespace TarefasApi.Repository.Interfaces
{
    public interface IFilaTarefaRepository
    {
        void SendMsgTarefa(string msg);
        Task<List<string>> RecieveMsgTarefa();
    }
}
