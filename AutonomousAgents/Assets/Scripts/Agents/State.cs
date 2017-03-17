abstract public class State <T> {

	abstract public void Enter (T agent);
    
    //takes an Agent Object parameter and switches state for that agent
    abstract public void Execute (T agent);
	abstract public void Exit (T agent);
}