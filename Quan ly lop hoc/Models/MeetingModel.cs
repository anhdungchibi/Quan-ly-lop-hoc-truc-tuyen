using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LMS_SASS.Models
{

    [Table("Meetings")]
    [PrimaryKey(nameof(Id))]
    public class MeetingModel
    {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public required int Id { get; set; } 

      [ForeignKey(nameof(Course))]
      public required int CourseId { get; set; } 
      public required CourseModel Course { get; set; }

      public required string Name { get; set; }

      [DataType(DataType.DateTime)]
      public required DateTime StartDate { get; set; }

      [DataType(DataType.DateTime)]
      public required DateTime EndDate { get; set; }

      [DataType(DataType.DateTime)]
      public required DateTime Created { get; set; }

      [DataType(DataType.Text)]
      public required string url { get; set; }
    }
}