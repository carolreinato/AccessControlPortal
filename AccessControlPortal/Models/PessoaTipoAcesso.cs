using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

    public class CriarPessoaTipoAcesso
    {
        public int IdPessoa { get; set; }
        public List<int> TipoAcesso { get; set; }
        public List<Guid> IdCodigoAcesso { get; set; }
    }
}
