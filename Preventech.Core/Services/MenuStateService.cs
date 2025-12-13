namespace Preventech.Core.Services;

public class MenuStateService
{
    /// <summary>
    /// Se inscreve no evento de alternância do menu, assim, quando ele for alternado, o componente inscrito será notificado.
    /// </summary>
    public event Action? OnMenuToggled;

    /// <summary>
    /// Quando o menu está em modo mobile (na parte superior), notifica os inscritos que o menu foi alternado.
    /// </summary>
    public void NotifyMenuToggled()
    {
        OnMenuToggled?.Invoke();
    }
}
