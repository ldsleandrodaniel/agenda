using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda.Models
{
    public class ModeloAgenda
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("data", TypeName = "timestamp without time zone")]
        public DateTime Data { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("linha")]
        public int Linha { get; set; }

        [Column("situacao")]
        public int Situacao { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; }
    }
}