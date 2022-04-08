using System.ComponentModel.DataAnnotations;

namespace KidsLogic.Models.DataTransferObjects;

public class UserRegisterDTO
{
    [Required(ErrorMessage = "Необходимо ввести адрес электронной почты.")]
    [EmailAddress(ErrorMessage = "Введен некорректный адрес.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Необходимо ввести Ваше имя.")]
    [DataType(DataType.Text)]
    [MaxLength(14, ErrorMessage = "Имя должно быть короче 15 символов.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Необходимо ввести Вашу фамилию.")]
    [DataType(DataType.Text)]
    [MaxLength(14, ErrorMessage = "Фамилия должна быть короче 15 символов.")]
    public string LastName { get; set; } = null!;
}