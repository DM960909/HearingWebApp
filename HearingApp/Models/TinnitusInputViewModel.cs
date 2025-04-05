using System;

namespace HearingApp.Models;
using System.ComponentModel.DataAnnotations;

public class TinnitusInputViewModel
{

    [Required]
    [Range(0,10)]
    [Display(Name = "How bad is your tinnitus overall today?")]
    public int SeverityScore{get;set;}

    [Required]
    [Range(0,10)]
    [Display(Name ="How are you coping today? ")]
    public int CopingScore {get;set;}
    

}
