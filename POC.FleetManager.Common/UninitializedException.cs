namespace POC.FleetManager.Common
{
    public class UninitializedException:Exception
    {
        public override string Message => $"Extension was not initialized. You must call Initialize before first use.";
    }
}
