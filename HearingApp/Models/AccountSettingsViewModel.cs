using System;

namespace HearingApp.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class AccountSettingsViewModel
{

    [Required]
    [EmailAddress]
    public string Email {get; set;}

    [Display(Name= "First Name")]
    public string FirstName{get; set;}

    [Display(Name="Last Name")]
    public string LastName {get; set;}

    [DataType(DataType.Password)]
    [Display(Name ="New Password")]
    public string NewPassword {get; set;}

    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword {get; set;}
    

}
