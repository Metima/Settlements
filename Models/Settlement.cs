using System;
using System.ComponentModel.DataAnnotations;

namespace Settlements.Models {

    /// <summary>
    /// Summary description for Settlments
    /// </summary>
    public class SettlementModel
    {
        public int Id { get; set; }
        [Display(Name = "שם ישוב")]
        public required string Name { get; set; }
    }

}