using Domain.Common.Enums;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Project : BaseGenericEntity
{
    public Project()
    {

    }


    public Project(
        long id,
        long userId,
        string title,
        string description,
        DateTime expectedStartDate,
        DateTime expectedEndDate,
        StatusEnum status,
        PriorityEnum priority) : base(id)
    {
        UserId = userId;
        Title = title;
        Description = description;
        ExpectedStartDate = expectedStartDate;
        ExpectedEndDate = expectedEndDate;
        Status = status;
        Priority = priority;
    }

    public long UserId { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public DateTime ExpectedStartDate { get; private set; }

    public DateTime ExpectedEndDate { get; private set; }

    public StatusEnum Status { get; private set; }

    public PriorityEnum Priority { get; private set; }

    public virtual User User { get; private set; }

    public virtual ICollection<Task> ListTask { get; private set; } = new HashSet<Task>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<Project> _validator
        = new Validators.ValidatorProject();

    #endregion
}