﻿using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Domain.Entities;

public class TaskComment : BaseGenericEntity
{
    public TaskComment()
    {

    }

    public TaskComment(
        long id,
        long taskId,
        long userId,
        string description) : base(id)
    {
        TaskId = taskId;
        UserId = userId;
        Description = description;
    }

    public long TaskId { get; private set; }

    public long UserId { get; private set; }

    public string Description { get; private set; }

    public virtual Task Task { get; private set; }

    public virtual User User { get; private set; }

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<TaskComment> _validator
        = new Validators.ValidatorTaskComment();

    #endregion

    #region Extensions

    public void AddUser(
        User user)
    {
        if (user != null)
            User = user;
    }

    public override string ToString()
    {
        var message = string.Empty;

        if (DateUpdated != null)
            return $"Comentário Modificado: {Description}, Usuário: {User.Name}, Data Criação: {DateCreated}, Data Alteração: {DateUpdated} ";

        return $"Comentário Criado: {Description}, Usuário: {User.Name}, Data Criação: {DateCreated}";
    }

    #endregion
}