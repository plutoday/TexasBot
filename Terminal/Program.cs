using log4net.Config;

namespace Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            var input = new Input();
            var recorder = new Recorder()
            {
                Input = input
            };
            recorder.Drive();
        }
    }
}
