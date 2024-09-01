using AutoMapper;
using TarefasApi.Models.Dtos;
using TarefasApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TarefasApi.Helpers
{
    public class TarefaProfile : Profile
    {
        public TarefaProfile()
        {
            
            CreateMap<Tarefa, TarefaDto>();

            CreateMap<TarefaAdicionarDto, Tarefa>();
            CreateMap<TarefaAtualizarDto, Tarefa>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
