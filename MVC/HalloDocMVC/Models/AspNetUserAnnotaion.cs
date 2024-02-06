using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDocMVC.Data
{
    // Define the partial class with the same name as the scaffolded model class
    public partial class Aspnetuser
    {
        // This class can contain any methods or properties that you want to add to the scaffolded model class
    }

    // Define a partial class in a separate file to add data annotations
    // Make sure the partial class has the same name as the scaffolded model class
    // and is in the same namespace
    [MetadataType(typeof(AspnetuserMetadata))]
    public partial class Aspnetuser
    {
        // This class is used to add data annotations to properties in the scaffolded model class

        // For example, you can add data annotations to the Email property like this:
        public class AspnetuserMetadata
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            [StringLength(256, ErrorMessage = "Email must be less than {1} characters")]
            public string Email { get; set; }
        }

        // You can add data annotations to other properties in the same way
    }
}
