using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Media;
//using System.Reflection;

namespace Ding
{
    class Ding
    {
        SoundPlayer player = new SoundPlayer();

        static void Main(string[] args)
        {

            if (args.Length == 0)
                Console.WriteLine("Ding needs a host or IP Address.");
            else
            {
                Ding p = new Ding();
                p.DoDing(args[0]);
            }
        }


        public void DoDing(string who)
        {
            AutoResetEvent waiter = new AutoResetEvent(false);


            // Create an instance of the SoundPlayer class.


            // Listen for the LoadCompleted event.
            player.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);
            player.Stream = this.GetType().Assembly.GetManifestResourceStream("Ding.Ding.wav");
            player.LoadAsync();

            //DateTime t0 = DateTime.Now;

            Ping pingSender = new Ping();

            // When the PingCompleted event is raised,
            // the PingCompletedCallback method is called.
            //pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            // Wait 3 seconds for a reply.
            int timeout = 3000;

            // Set options for transmission:
            // The data can go through 64 gateways or routers
            // before it is destroyed, and the data packet
            // cannot be fragmented.
            PingOptions options = new PingOptions(64, true);

            Console.WriteLine("\nDinging: {0} with 32 bytes of data:\n", who);
            //Console.WriteLine("Time to live={0}", options.Ttl);
            //Console.WriteLine("Don't fragment={0}\n", options.DontFragment);

            for (int i = 0; i < 5; i++)
            {
                // Send the ping asynchronously.
                // Use the waiter as the user token.
                // When the callback completes, it can wake up this thread.
                // pingSender.SendAsync(who, timeout, buffer, options, waiter);

                DisplayReply(pingSender.Send(who, timeout, buffer, options));
                Thread.Sleep(1500);
                // Prevent this example application from ending.
                // A real application should do something useful
                // when possible.
                //waiter.WaitOne();
            }

            /*
            TimeSpan t = new TimeSpan(0, 0, 0, 7);
            if (DateTime.Now - t0 < t)
                Thread.Sleep(t - (DateTime.Now - t0));
            */
            Console.WriteLine("\nDing completed!");
        }

/*

        public void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            // If the operation was canceled, display a message to the user.
            if (e.Cancelled)
            {
                Console.WriteLine("Ping canceled.");
            }

            // If an error occurred, display the exception to the user.
            else if (e.Error != null)
            {
                Console.WriteLine("Ping failed:");
                Console.WriteLine(e.Error.ToString());
            }
            else
            {
                PingReply reply = e.Reply;

                DisplayReply(reply);
            }
            // Let the main thread resume. 
            // UserToken is the AutoResetEvent object that the main thread 
            // is waiting for.
            ((AutoResetEvent)e.UserState).Set();

        }
 
 */


        public void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Reply from {0}: bytes={1} time={2}ms TTL={3}", reply.Address.ToString(), reply.Buffer.Length, reply.RoundtripTime, reply.Options.Ttl);
                /*
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                 */
            }
            else
                Console.WriteLine(reply.Status);
        }


 


        private void player_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            player.Play();
        }

    }
}
