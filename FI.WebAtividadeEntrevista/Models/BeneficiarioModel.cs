using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        /// <summary>
        /// Identificador do Beneficiário
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///   Identificador do Cliente ao qual o Beneficiário está vinculado.
        /// </summary>
        public long? ClienteId { get; set; }

        /// <summary>
        /// CPF do Beneficiário
        /// </summary>
        [Required]
        [Cpf(ErrorMessage = "O cpf não é valido.")]
        public string Cpf { get; set; }

        /// <summary>
        /// Nome do Beneficiário.
        /// </summary>
        public string Nome { get; set; }
        public ClienteModel Cliente { get; set; }
    }
}