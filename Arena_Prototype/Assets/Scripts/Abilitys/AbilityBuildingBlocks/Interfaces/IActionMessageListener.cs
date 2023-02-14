using RPG.Abilitys.Form;

public interface IActionMessageListener
{

    //Used to listen to a message if it matches
    public void Perform(AbstractFormBehavior behavior, string message);
}
