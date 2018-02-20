using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzazelBot.Core.Data.Models
{
    public class FavoriteCharacter
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public   string  uId { get; set; }
        [Required]
        public string CharacterName { get; set; }

        public DateTime AddedAt { get; set; }
        [Required]
        public string ServerId { get; set; }
    }
}
