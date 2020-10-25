using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessControlPortal.Models
{
    public class PessoaTipoAcesso
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public int IdTipoAcesso { get; set; }
        public Guid IdCodigoAcesso { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime? Saida { get; set; }
        public bool InCondominio { get; set; }
    }
}
