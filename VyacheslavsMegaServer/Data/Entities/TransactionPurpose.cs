using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Text.Json.Serialization;

namespace VyacheslavsMegaServer.Data.Entities
{
    public class TransactionPurpose : Base.EntityBase
    {
        public enum TransactionType
        {
            Income = 0,
            Expense = 1
        }

        [MaxLength(50)]
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [MaxLength(200)]
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Тип транзакции")]
        public TransactionType Type { get; set; }

        #region Navigations

        [ValidateNever]
        [JsonIgnore]
        public virtual ICollection<FundTransaction> Transactions { get; } = new List<FundTransaction>();

        #endregion

        public string GetTypeText()
        {
            return Type == 0 ? "Доход" : "Расход";
        }

        public static string GetTypeText(TransactionType type)
        {
            return type == TransactionType.Income ? "Доход" : "Расход";
        }
    }
}
