namespace Game;

public partial class SnowController : GpuParticles2D
{
	public override void _Ready()
	{
		GameWorld.Instance.OnPlayerDeath += HandlePlayerDeath;
	}

	private void HandlePlayerDeath()
	{
		var processMat = ProcessMaterial as ParticleProcessMaterial;
		processMat.Gravity = new Vector3(0, processMat.Gravity.Y, processMat.Gravity.Z);
	}
}
