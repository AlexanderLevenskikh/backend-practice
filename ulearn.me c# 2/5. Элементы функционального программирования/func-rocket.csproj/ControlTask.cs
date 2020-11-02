namespace func_rocket
{
    public class ControlTask
    {
        public static Turn ControlRocket(Rocket rocket, Vector target)
        {
            return (target - rocket.Location).Angle -
                (rocket.Location + rocket.Velocity + ForcesTask.GetThrustForce(10)(rocket) -
                 rocket.Location).Angle > 0
                    ? Turn.Right
                    : Turn.Left;
        }
    }
}