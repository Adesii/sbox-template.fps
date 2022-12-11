using Sandbox;

namespace Facepunch.Gunfight.WeaponSystem;

public partial class Recoil : WeaponComponent, ISingletonComponent
{
	[Net, Predicted] public Vector2 CurrentRecoil { get; set; }
	[Net, Predicted] public TimeUntil TimeUntilRemove { get; set; }

	public void AddRecoil( float x, float y )
	{
		CurrentRecoil += new Vector2( x, y );
		TimeUntilRemove = 0.2f;
	}

	public void RemoveRecoil( float x, float y )
	{
		CurrentRecoil -= new Vector2( x, y );
	}

	public override void Simulate( IClient cl, Player player )
	{
		base.Simulate( cl, player );

		var pitchOffset = Input.AnalogLook.pitch;

		if ( TimeUntilRemove )
			CurrentRecoil -= 50f * Time.Delta;

		if ( pitchOffset > 0f )
		{
			var newPitch = (CurrentRecoil.y - pitchOffset).Clamp( 0f, float.MaxValue );
			CurrentRecoil = CurrentRecoil.WithY( newPitch );
		}

		CurrentRecoil = CurrentRecoil.Clamp( 0, 100 );
		DebugOverlay.ScreenText( $"{CurrentRecoil}", 50, 0 );
	}
}