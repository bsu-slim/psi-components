using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.Util;
using Microsoft.Psi;
using Microsoft.Psi.Components;
using JsonConfig;

namespace ActiveMQComponent
{
    /// <summary>
    /// A Psi Component to send messages through ActiveMQ 
    /// 
    /// An ActiveMQ process is started to handle ActiveMQ messages. A singleton is used
    /// to ensure only one process is started, and that process is killed when the calling 
    /// process is finished or killed.
    /// 
    /// Messages are sent as strings wrapped in xml through ActiveMQ.
    /// </summary>
    class ActiveMQ : ConsumerProducer<string, string>, IDisposable
    {
        private IConnection connection;
        private ISession session;
        private IDestination destinationSend;
        private IDestination destinationReceive;
        private IMessageConsumer consumer;
        private IMessageProducer producer;

        private string destinationSendName;
        private string destinationRecvName;
        private ActiveMQProcess activeMQProcess;

        /// <summary>
        /// A Psi Component to send messages through ActiveMQ 
        /// 
        /// messages are sent as string wrapped in xml.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="destinationName"></param>
        public ActiveMQ(Pipeline pipeline, string destinationRecvName, string destinationSendName) : base(pipeline)
        {
            checkConfig(); //Check that the config has all items we need, otherwise throw an exception
            //Start ActiveMQ server 
            this.activeMQProcess = ActiveMQProcess.Instance;
            this.activeMQProcess.Start();

            //Connect to ActiveMQ Server
            IConnectionFactory factory = new NMSConnectionFactory(
                $"{Config.Global.ActiveMQ.Protocol}://{Config.Global.ActiveMQ.Host}:{Config.Global.ActiveMQ.Port}"
            );

            bool notConnected = true;
            int attempts = 0;
            int maxAttempts = 5;
            while (notConnected && attempts < maxAttempts)
            {
                try
                {
                    this.connection = factory.CreateConnection();
                    notConnected = false;
                }
                catch (Apache.NMS.NMSConnectionException)
                {
                    attempts++;
                    System.Threading.Thread.Sleep(1000);
                }
            }
            Console.WriteLine($"{Config.Global.ActiveMQ.Protocol}://{Config.Global.ActiveMQ.Host}:{Config.Global.ActiveMQ.Port}");
            this.connection = factory.CreateConnection(); //Last Try
            this.session = this.connection.CreateSession();

            this.destinationSendName = destinationSendName;
            this.destinationRecvName = destinationRecvName;

            //Create destinations to send and receive messages
            this.destinationSend = SessionUtil.GetQueue(session, this.destinationSendName);
            this.destinationReceive = SessionUtil.GetQueue(session, this.destinationRecvName);
            this.consumer = session.CreateConsumer(this.destinationReceive);
            this.producer = session.CreateProducer(this.destinationSend);
            this.producer.DeliveryMode = MsgDeliveryMode.NonPersistent; //TODO should this be a parameter?
            this.connection.Start();

            //Setup message listener
            consumer.Listener += new MessageListener(OnMessage);
        }

        /// <summary>
        /// Receives a string from upstream in the pipeline. Relays that string to
        /// the ActiveMQ send destination.
        /// </summary>
        /// <param name="str">String to send to send destination</param>
        /// <param name="e">Pipeline Envelope</param>
        protected override void Receive(string str, Envelope e)
        {
            //Send a message using ActiveMQ server
            this.producer.Send(PsiMQJson.SerializeString(str, e.OriginatingTime));
        }

        /// <summary>
        /// Method to handle messages sent to the receiece destination through ActiveMQ
        /// </summary>
        /// <param name="receivedMsg"></param>
        private void OnMessage(IMessage receivedMsg)
        {
            IBytesMessage message = receivedMsg as IBytesMessage;
            string messageString = System.Text.Encoding.UTF8.GetString(message.Content);
            if (PsiMQJson.isValid(messageString))
            {
                PsiMQJson obj = PsiMQJson.FromJson(messageString);
                this.Out.Post(obj.Content, obj.DateTime);
            }
        }

        /// <summary>
        /// Disposes of this item and all its disposables
        /// </summary>
        public void Dispose()
        {
            this.connection.Dispose();
            this.session.Dispose();
            this.consumer.Dispose();
        }

        /// <summary>
        /// Singleton object to start an ActiveMQ process.
        /// 
        /// Given an executable from a config file (JsonConfig), this class will run the executable
        /// in the background. The executable will be stopped when the calling process is stopped.
        /// </summary>
        private class ActiveMQProcess
        {
            private static ActiveMQProcess instance = null;
            private bool notStarted = true;
            public static ActiveMQProcess Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new ActiveMQProcess();
                    }
                    return instance;
                }
            }

            private ActiveMQProcess()
            {
                notStarted = true;
            }

            public void Start()
            {
                if (this.notStarted)
                {
                    Console.WriteLine(Config.Global.ActiveMQ.Executable);
                    Process p = new Process
                    {
                        StartInfo = new ProcessStartInfo(Config.Global.ActiveMQ.Executable, "start")
                    };
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.Start();
                    ChildProcessTracker.AddProcess(p);
                }
            }
        }

        /// <summary>
        /// Verifies that the JsonConfig file has the proper elements.
        /// 
        /// 
        /// </summary>
        /// 
        /// <exception cref="ActiveMQ.ActiveMQConfigurationException">
        /// Thrown when a config section or parameter for activemq is missing from the 
        /// JsonConfig file
        /// </exception>
        private void checkConfig()
        {
            var activeMQ = Config.Global.ActiveMQ;
            if (activeMQ == null) throw new ActiveMQConfigurationException("Missing JsonConfig configuration parameter: ActiveMQ");
            var protocol = Config.Global.ActiveMQ.Protocol;
            var host = Config.Global.ActiveMQ.Host;
            var port = Config.Global.ActiveMQ.Port;
            var executable = Config.Global.ActiveMQ.Executable;
            if (protocol == null) throw new ActiveMQConfigurationException("Missing JsonConfig configuration parameter: ActiveMQ.Protocol");
            if (host == null) throw new ActiveMQConfigurationException("Missing JsonConfig configuration parameter: ActiveMQ.Host");
            if (port == null) throw new ActiveMQConfigurationException("Missing JsonConfig configuration parameter: ActiveMQ.Port");
            if (executable == null) throw new ActiveMQConfigurationException("Missing JsonConfig configuration parameter: ActiveMQ.Executable");

        }
    }
}
