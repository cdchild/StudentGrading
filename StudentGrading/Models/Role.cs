
namespace StudentGrading.Models
{
    //an enumeration of roles used in this app, changes to these role should automatically reflect in the db initializer seed method
    //however you'd have to go through the entire application to remove, add, change references
    public enum Role
    {
        Professor = 1,
        Registrar = 2,
        Grader = 3,
        Student = 4
    }
}