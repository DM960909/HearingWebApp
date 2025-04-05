using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HearingApp.Data;

//Add profile data for users by adding properties to this class
public class ApplicationUser : IdentityUser
{

    [Required]
    [PersonalData]
    public string FirstName {get; set;}

    
    [PersonalData]
    [Required]
    public string LastName {get; set;}

}
