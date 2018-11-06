using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unittest
{
    [TestClass]
    public class LoggingSetup
    {
        [AssemblyInitialize]
        public static void Configure(TestContext tc)
        {
            //Diag output will go to the "output" logs if you add tehse two lines
            //TextWriterTraceListener writer = new TextWriterTraceListener(System.Console.Out);
            //Debug.Listeners.Add(writer);

            Debug.WriteLine("Diag Called inside Configure before log4net setup");
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            // create the first logger AFTER we run the configuration
            ILog LOG = LogManager.GetLogger(typeof(LoggingSetup));
            LOG.Debug("log4net initialized for tests");
            Debug.WriteLine("Diag Called inside Configure after log4net setup");
        }
    }
}
