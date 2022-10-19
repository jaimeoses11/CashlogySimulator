namespace SimuladorCashlogy
{
    public class Config
    {
        public bool isConfig;
        public bool isRun;

        public Config(string[] args)
        {
            isConfig = false;
            isRun = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "/config") isConfig = true;
                if (args[i] == "/run") isRun = true;
            }

            if (isConfig && isRun) isConfig = false;
        }
    }
}
