/**
 * PSI Component for the Tobii EyeX eye tracker.
 */
namespace EyeXComponent
{
    using System;
    using System.Timers;
    using Microsoft.Psi;
    using Tobii.EyeX.Framework;
    using EyeXFramework;
    using Tobii.Research;

    class EyeXComponent
    {
        /***************************GLOBAL_VARIABLES**********************/
        public Emitter<GazePointDataStream> gazePoint;
        public Emitter<EyePositionDataStream> eyeData;
        public Emitter<FixationDataStream> fixationData;

        private static EyeXHost host;

        public GazePointDataStream gazeDataStream;
        public EyePositionDataStream eyePositionStream;

        private static double frameDataX = 0.0;
        private static double frameDataY = 0.0;

        private static double leftEyeX = 0.0;
        private static double leftEyeY = 0.0;
        private static double leftEyeZ = 0.0;
        private static double leftEyeNormalizedX = 0.0;
        private static double leftEyeNormalizedY = 0.0;
        private static double leftEyeNormalizedZ = 0.0;

        private static double rightEyeX = 0.0;
        private static double rightEyeY = 0.0;
        private static double rightEyeZ = 0.0;
        private static double rightEyeNormalizedX = 0.0;
        private static double rightEyeNormalizedY = 0.0;
        private static double rightEyeNormalizedZ = 0.0;

        private static double fixationX = 0.0;
        private static double fixationY = 0.0;
        /***************************GLOBAL_VARIABLES**********************/
        /**
         * Driver
         */
        public static void Main(string[] args)
        {
            switch (EyeXHost.EyeXAvailability)
            {
                case Tobii.EyeX.Client.EyeXAvailability.NotAvailable:
                    Console.WriteLine("EyeX Engine unavailable.");
                    Console.WriteLine("Please install the EyeX Engine then retry.");
                    Console.In.Read();
                    return;
                case Tobii.EyeX.Client.EyeXAvailability.NotRunning:
                    Console.WriteLine("EyeX Engine is not running.");
                    Console.WriteLine("Please start the EyeX Engine and retry.");
                    Console.In.Read();
                    return;
            }

            using (EyeXHost h = new EyeXHost())
            {
                h.Start();
                Console.WriteLine("EyeXHost started.");

                h.ScreenBoundsChanged += (s, e) =>
                {
                    Console.WriteLine("Screen bounds in pixels: " + e);
                };

                h.DisplaySizeChanged += (s, e) =>
                {
                    Console.WriteLine("Display size in millimeters: " + e);
                };

                h.EyeTrackingDeviceStatusChanged += (s, e) =>
                {
                    Console.WriteLine("Eye tracking device status: " + e);
                };

                h.UserPresenceChanged += (s, e) =>
                {
                    Console.WriteLine("User presence: " + e);
                };

                h.UserProfileNameChanged += (s, e) =>
                {
                    Console.WriteLine("Active profile name: " + e);
                };

                h.UserProfilesChanged += (s, e) =>
                {
                    Console.WriteLine("User profile names: " + e);
                };

                h.GazeTrackingChanged += (s, e) =>
                {
                    Console.WriteLine("Gaze tracking: " + e);
                };

                Timer t = new Timer();
                t.Interval = 2000;
                t.Elapsed += DisplayData;
                t.Enabled = true;

                using (Pipeline p = Pipeline.Create())
                {
                    EyeXComponent eyeXComponent = new EyeXComponent(p, h);
                    Envelope en = new Envelope();

                    eyeXComponent.ReceiveGazeData(en);

                    p.Run();
                }
            }
        }
        /******************************MAIN*******************************/
        /**
         * Initiates the eye tracker data stream.
         */
        public EyeXComponent(Pipeline pipeline, EyeXHost h)
        {
            host = h;

            gazePoint = pipeline.CreateEmitter<GazePointDataStream>
                (this, nameof(gazePoint));

            eyeData = pipeline.CreateEmitter<EyePositionDataStream>
                (this, nameof(eyeData));

            fixationData = pipeline.CreateEmitter<FixationDataStream>
                (this, nameof(fixationData));
        }
        /*************************EYE_X_COMPONENT.INIT********************/
        /*
         * Receive method for the EyeX gaze data stream.
         */
        private void ReceiveGazeData(Envelope e)
        {
            using (gazeDataStream = host.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered))
            {
                Console.WriteLine("EyeX data stream initiated.");

                gazePoint.Post(gazeDataStream, e.OriginatingTime);

                using (eyePositionStream = host.CreateEyePositionDataStream())
                {
                    Console.WriteLine("EyeX eye position stream initiated.");

                    eyeData.Post(eyePositionStream, e.OriginatingTime);

                    gazeDataStream.Next += (s, q) =>
                    {
                        frameDataX = q.X;
                        frameDataY = q.Y;
                    };

                    eyePositionStream.Next += (s, q) =>
                    {
                        leftEyeX = q.LeftEye.X;
                        leftEyeY = q.LeftEye.Y;
                        leftEyeZ = q.LeftEye.Z;

                        leftEyeNormalizedX = q.LeftEyeNormalized.X;
                        leftEyeNormalizedY = q.LeftEyeNormalized.Y;
                        leftEyeNormalizedZ = q.LeftEyeNormalized.Z;

                        rightEyeX = q.RightEye.X;
                        rightEyeY = q.RightEye.Y;
                        rightEyeZ = q.RightEye.Z;

                        rightEyeNormalizedX = q.RightEyeNormalized.X;
                        rightEyeNormalizedY = q.RightEyeNormalized.Y;
                        rightEyeNormalizedZ = q.RightEyeNormalized.Z;
                    };

                    Console.WriteLine("Listening for gaze data...");

                    using (FixationDataStream fixationStream = host.CreateFixationDataStream(FixationDataMode.Sensitive))
                    {
                        fixationData.Post(fixationStream, e.OriginatingTime);

                        fixationStream.Next += (s, q) =>
                        {
                            fixationX = q.X;
                            fixationY = q.Y;
                        };

                        Console.WriteLine("Recording fixation points...");
                        Console.In.Read();
                    }
                }
            }
        }
        /***************************RECEIVE_GAZE_DATA*********************/
        /**
         * Prints information from the eye tracker every 2 seconds.
         */
        private static void DisplayData(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Eye data recorded at: " + e.SignalTime);
            Console.WriteLine("Left X: " + leftEyeX);
            Console.WriteLine("Left Y: " + leftEyeY);
            Console.WriteLine("Left Z: " + leftEyeZ);
            Console.WriteLine("Right X: " + rightEyeX);
            Console.WriteLine("Right Y: " + rightEyeY);
            Console.WriteLine("Right Z: " + rightEyeZ);
        }
        /******************************DISPLAY_DATA***********************/
    }
    /******************************EYE_X_COMPONENT************************/
}
/******************************EYE_X_COMPONENT.CLASS**********************/

