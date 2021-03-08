using System;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Models
{
    [ModelMetadataType(typeof(TaskMetadata))]
    public partial class Task
    {
    }

    [ModelMetadataType(typeof(UserMetadata))]
    public partial class User
    {
    }

}
