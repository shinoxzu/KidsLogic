using System.ComponentModel.DataAnnotations;

namespace KidsLogic.Models.DataTransferObjects;

public class UserLoginDTO
{
    [Required(ErrorMessage = "Необходимо ввести адрес электронной почты.")]
    [EmailAddress(ErrorMessage = "Введен некорректный адрес.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Необходимо ввести пароль.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}