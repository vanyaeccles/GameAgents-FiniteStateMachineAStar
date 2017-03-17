public class StateMachine <T> {
	
	private T agent;
	private State<T> state;
    private State<T> previousState;
    private State<T> globalState;

    public void Awake () {
		this.state = null;
        this.previousState = null;
        this.globalState = null;
    }

	public void Init (T agent, State<T> startState, State<T> globalState) {
		this.agent = agent;
		this.state = startState;
        this.globalState = globalState; 
	}



	public void Update () {
        if (this.globalState != null) this.globalState.Execute(this.agent);
        if (this.state != null) this.state.Execute(this.agent);
 
    }
	
	public void ChangeState (State<T> nextState) {
        previousState = state;
		if (this.state != null) this.state.Exit(this.agent);
		this.state = nextState;
		if (this.state != null) this.state.Enter(this.agent);
	}

    public void RevertToPreviousState()
    {
        this.globalState.Exit(this.agent);
        if (previousState != null) ChangeState(previousState);
    }
}