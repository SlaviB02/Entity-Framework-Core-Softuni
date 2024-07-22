﻿using Footballers.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class Footballer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;
        [Required]
        public DateTime ContractStartDate { get; set; }

        [Required]
        public DateTime ContractEndDate { get; set; }

        [Required]
        public PositionType PositionType { get; set; }

        [Required]
        public BestSkillType BestSkillType { get; set; }

        [Required]
        public int CoachId {  get; set; }
        [ForeignKey(nameof(CoachId))]
        public Coach Coach { get; set; } = null!;

        public ICollection<TeamFootballer> TeamsFootballers { get; set; } = new HashSet<TeamFootballer>();
    }
}
