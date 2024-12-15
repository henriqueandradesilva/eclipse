using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Domain.Entities;

public class User : BaseGenericEntity
{
    public User()
    {

    }

    public User(
        long id,
        long userRoleId,
        string name,
        string email,
        string password,
        bool isActive) : base(id)
    {
        UserRoleId = userRoleId;
        Name = name?.ToUpper();
        Email = email?.ToUpper();
        Password = BCrypt.Net.BCrypt.HashPassword(password);
        IsActive = isActive;
    }

    public long UserRoleId { get; private set; }

    public string Name { get; private set; }

    public string Email { get; private set; }

    public string Password { get; private set; }

    public bool IsActive { get; private set; }

    public virtual UserRole UserRole { get; private set; }

    public virtual ICollection<Project> ListProject { get; private set; } = new HashSet<Project>();

    public virtual ICollection<Task> ListTask { get; private set; } = new HashSet<Task>();

    public virtual ICollection<AuditLog> ListAuditLog { get; private set; } = new HashSet<AuditLog>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<User> _validator
        = new Validators.ValidatorUser();

    #endregion

    #region Extensions

    public void SetActive(
        bool isActive)
    {
        IsActive = isActive;
    }

    #endregion
}