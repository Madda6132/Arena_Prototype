namespace RPG.Creatures {
    /// <summary>
    /// Such as player input or AI
    /// </summary>
    public interface ICreatureControler {

        public bool IsMoving { get; }
        public ResultObserver<bool> MovingObserver { get; }

        public void DisabledControler();
        public void EnableControler();

    }

}
