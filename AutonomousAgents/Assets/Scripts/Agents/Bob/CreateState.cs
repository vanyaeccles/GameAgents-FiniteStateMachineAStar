using UnityEngine;

public sealed class CreateState : State<Bob> {
	
	static readonly CreateState instance = new CreateState();
	
	public static CreateState Instance {
		get {
			return instance;
		}
	}
	
	static CreateState () {}
	private CreateState () {}
	
	public override void Enter (Bob agent) {
		Debug.Log("Gathering creative energies...");
	}
	
	public override void Execute (Bob agent) {
		agent.CreateTime();
		Debug.Log("...creating more time, for a total of " + agent.createdTime + " unit" + (agent.createdTime > 1 ? "s" : "") + "...");
		agent.ChangeState(WaitState.Instance);
	}
	
	public override void Exit (Bob agent) {
		Debug.Log("...creativity spent!");
	}
}