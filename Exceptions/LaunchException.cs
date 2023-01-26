using System;

namespace WpfEssentials.Exceptions
{
    public static class LaunchException
    {
        public static void Launch(Exception e)
        {
            var prompt = new ExceptionPrompt(e);
            prompt.Show();
        }
    }
}
