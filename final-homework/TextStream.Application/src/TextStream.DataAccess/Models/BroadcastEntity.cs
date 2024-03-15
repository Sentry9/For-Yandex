using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TextStream.Api.Contracts.Types;

namespace TextStream.DataAccess.Models;

internal class BroadcastEntity
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("date_start")]
    public DateTime DateStart { get; set; }

    [Column("home_team_name")]
    public string HomeTeamName { get; set; }

    [Column("guest_team_name")]
    public string GuestTeamName { get; set; }
    
    [Column("status")]
    public StatusType StatusType { get; set; }
}