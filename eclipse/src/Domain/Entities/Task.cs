using Domain.Common.Enums;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Task : BaseGenericEntity
{
    public Task()
    {

    }

    public Task(
        long id,
        long projectId,
        long userId,
        string title,
        string description,
        DateTime expectedStartDate,
        DateTime expectedEndDate,
        StatusEnum status,
        PriorityEnum priority) : base(id)
    {
        ProjectId = projectId;
        UserId = userId;
        Title = title;
        Description = description;
        ExpectedStartDate = expectedStartDate;
        ExpectedEndDate = expectedEndDate;
        Status = status;
        Priority = priority;
    }

    public long ProjectId { get; private set; }

    public long UserId { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public DateTime ExpectedStartDate { get; private set; }

    public DateTime ExpectedEndDate { get; private set; }

    public StatusEnum Status { get; private set; }

    public PriorityEnum Priority { get; private set; }

    public virtual User User { get; private set; }

    public virtual Project Project { get; private set; }

    public virtual ICollection<TaskComment> ListTaskComment { get; private set; } = new HashSet<TaskComment>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<Task> _validator
        = new Validators.ValidatorTask();

    #endregion
}