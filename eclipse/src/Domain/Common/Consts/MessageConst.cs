namespace Domain.Common.Consts;

public class MessageConst
{
    #region Default

    public const string TitleRequired = "O campo Titulo é obrigatório.";

    public const string TitleMaxPermitted = "O máximo permitido para o campo Titulo é de 100 caracteres.";

    public const string DescriptionRequired = "O campo Descrição é obrigatório.";

    public const string DescriptionMaxPermitted = "O máximo permitido para o campo Descrição é de 150 caracteres.";

    public const string ActionNotPermitted = "Ação não permitida.";

    public const string ProjectIdRequired = "O campo Projeto é obrigatório.";

    public const string UserIdRequired = "O campo Usuário é obrigatório.";

    public const string ExpectedStartDateRequired = "O campo Data de Início é obrigatório.";

    public const string ExpectedEndDateRequired = "O campo Data de Vencimento é obrigatório.";

    public const string StatusRequired = "O campo Status é obrigatório.";

    public const string PriorityRequired = "O campo Prioridade é obrigatório.";

    public const string MessageEmpty = "Nenhuma registro encontrado.";

    #endregion

    #region Project

    public const string ProjectCreated = "Projeto cadastrado com sucesso.";

    public const string ProjectUpdated = "Projeto modificado com sucesso.";

    public const string ProjectNotExist = "Nenhum Projeto foi encontrado.";

    public const string ProjectExist = "Este Projeto já existe.";

    public const string ProjectNotChangedPriority = "Não é permitido alterar a prioridade de um Projeto.";

    public const string ProjectTaskPending = "Há tarefas pendentes associadas a este projeto. Para excluí-lo, será necessário concluí-las ou removê-las primeiro.";

    #endregion

    #region Task

    public const string TaskCreated = "Tarefa cadastrada com sucesso.";

    public const string TaskUpdated = "Tarefa modificada com sucesso.";

    public const string TaskNotExist = "Nenhuma Tarefa foi encontrada.";

    public const string TaskExist = "Esta Tarefa já existe.";

    public const string TaskNotChangedPriority = "Não é permitido alterar a prioridade de uma Tarefa.";

    public const string TaskMaxPermitted = $"O número máximo de tarefas permitido por projeto é de vinte";

    #endregion

    #region User

    public const string UserUserRoleIdRequired = "O campo Perfil é obrigatório.";

    public const string UserNameRequired = "O campo Nome é obrigatório.";

    public const string UserNameMaxPermitted = "O máximo permitido para o campo Nome é de 100 caracteres.";

    public const string UserEmailRequired = "O campo E-mail é obrigatório.";

    public const string UserEmailMaxPermitted = "O máximo permitido para o campo E-mail é de 255 caracteres.";

    public const string UserPasswordRequired = "O campo Senha é obrigatório.";

    public const string UserPasswordMaxPermitted = "O máximo permitido para o campo Senha é de 255 caracteres.";

    #endregion
}