using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    [MetadataType(typeof(TaskMetadata))]
    public partial class Task
    {
    }

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
    }
}
