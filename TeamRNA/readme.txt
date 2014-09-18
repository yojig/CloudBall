Field
public static readonly Rectangle Borders = new Rectangle(0.0f, 0.0f, 1920f, 1080f);
public static readonly Rectangle EnemyGoal = new Rectangle(Field.Borders.Right.X, 383f, 0.0f, 312f);

public struct Constants
{
/// <summary>
/// speedFactor is a internal parameter that regulates the speed of the simulation. Increasing this makes the simulation faster, but reduces numerical precision.
/// 
/// </summary>
private static float speedFactor = 2f;
/// <summary>
/// Maximum distance (in units) a player can be from the ball to pick it up.
/// </summary>
public static readonly float BallMaxPickUpDistance = 40f;
/// <summary>
/// Number of rounds from when the ball lost due to a tackle before it can be picked up by any player.
/// </summary>
public static readonly int BallTackleTimer = (int) (0.0 / (double) Constants.speedFactor);
/// <summary>
/// Number of rounds from when the ball is shot until it can be picked up by any player.
/// </summary>
public static readonly int BallShootTimer = (int) (15.0 / (double) Constants.speedFactor);
/// <summary>
/// Maximum velocity of the ball (in units/round).
/// </summary>
public static readonly float BallMaxVelocity = 6f * Constants.speedFactor;
/// <summary>
/// Time taken for ball to halve its velocity (in rounds).
/// </summary>
public static readonly float BallHalfTime = 200f / Constants.speedFactor;
/// <summary>
/// Standard deviation (in radians) of a shot at max strength.
/// </summary>
public static readonly float BallMaxStrStd = 0.2f;
/// <summary>
/// Number of rounds after picking up the ball that a player is immune to tackling.
/// </summary>
public static readonly int BallPickUpImmunityTimer = (int) (20.0 / (double) Constants.speedFactor);
/// <summary>
/// Maximum velocity of a player (in units/round).
/// </summary>
public static readonly float PlayerMaxVelocity = 1.5f * Constants.speedFactor;
/// <summary>
/// Time taken for a player to halve its velocity (in rounds) if it does not accelerate.
/// </summary>
public static readonly float PlayerVelocityHalfTime = 20f / Constants.speedFactor;
/// <summary>
/// Maximum distance (in units) a player can tackle at.
/// </summary>
public static readonly float PlayerMaxTackleDistance = 50f;
/// <summary>
/// Time (in rounds) a player is unable to preform any action after he has been tackled.
/// </summary>
public static readonly int PlayerFallenTime = (int) (100.0 / (double) Constants.speedFactor);
/// <summary>
/// Time after a player has preformed a tackle before he can do it again.
/// </summary>
public static readonly int PlayerTackleCooldown = (int) (400.0 / (double) Constants.speedFactor);
/// <summary>
/// Chance that a tackle will succeed.
/// </summary>
public static readonly float PlayerTackleChance = 0.8f;
/// <summary>
/// The maximum strength the ball can be shot using. A ball shot at max strength will have max velocity.
/// </summary>
public static readonly float PlayerMaxShootStr = 10f;
/// <summary>
/// Advanced. Each round; Ball.Velocity *= BallSlowDownFactor.
/// </summary>
public static readonly float BallSlowDownFactor = (float) Math.Pow(0.5, 1.0 / (double) Constants.BallHalfTime);
/// <summary>
/// Advanced. Each round; Player.Velocity *= PlayerSlowDownFactor.
/// </summary>
public static readonly float PlayerSlowDownFactor = (float) Math.Pow(0.5, 1.0 / (double) Constants.PlayerVelocityHalfTime);
/// <summary>
/// Advanced. Each round; Player.Velocity += Player.direction * movePower * Constants.PlayerAccelerationFactor.
/// </summary>
public static readonly float PlayerAccelerationFactor = (1f - Constants.PlayerSlowDownFactor) / Constants.PlayerSlowDownFactor;
/// <summary>
/// Advanced. The max thread time (milliseconds) your team is allowed each round.
/// </summary>
public static readonly int GameEngineMaxThreadTime = 40;
/// <summary>
/// Advanced. The maximum allowed ammount of timeouts.
/// </summary>
public static readonly int GameEngineMaxAllowedTimeouts = 10;
/// <summary>
/// Number of rounds in a match.
/// </summary>
public static readonly int GameEngineMatchLength = (int) (7200.0 / (double) Constants.speedFactor);

}