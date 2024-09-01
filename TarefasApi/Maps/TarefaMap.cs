using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TarefasApi.Models.Entities;

namespace TarefasApi.Maps
{

    public class TarefaMap : BaseMap<Tarefa>
    {
        public TarefaMap() : base("tb_tarefa")
        { }

        public override void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Descricao).HasColumnName("descricao").HasColumnType("varchar(100)").IsRequired();
            builder.Property(x => x.Data).HasColumnName("data");
            builder.Property(x => x.Status).HasColumnName("status").HasColumnType("bit");


        }
    }

}
