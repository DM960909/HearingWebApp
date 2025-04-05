using System;

namespace HearingApp.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HearingApp.Data;

public class TinnitusScore
{

    public int Id {get; set;}

    [Required]
    [Range(0,10)]
    [Display(Name="Current Tinnitus Severity")]
    public int SeverityScore{get; set;}

    [Required]
    [Range(0,10)]
    [Display(Name = "Current Coping Score")]

    public int CopingScore{get;set;}

    public DateTime CreatedAt {get; set;}

    //VVV Foreign Key to ApplicationUser VVVV
    public string UserId {get; set;}
    public ApplicationUser User {get; set;}
    
}
