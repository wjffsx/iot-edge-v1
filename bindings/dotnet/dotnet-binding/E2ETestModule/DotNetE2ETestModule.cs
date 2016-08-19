﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.IoT.Gateway;
using System.Threading;

namespace E2ETestModule
{
    public class DotNetE2ETestModule : IGatewayModule
    {
        private Broker broker;
        private string configuration;

        public void Create(Broker broker, string configuration)
        {
            this.broker = broker;
            this.configuration = configuration;

            Thread oThread = new Thread(new ThreadStart(this.threadBody));
            // Start the thread
            oThread.Start();
        }

        public void Destroy()
        {
            Console.WriteLine("This is C# Sensor Module Destroy!");
        }

        public void Receive(Message received_message)
        {
            Dictionary<string, string> thisIsMyProperty = new Dictionary<string, string>();
            thisIsMyProperty.Add("source", "DotNetE2ETestmodule");
            thisIsMyProperty.Add("MsgType", "Reply");
            thisIsMyProperty.Add("ModuleId", configuration);

            Message messageToPublish = new Message("Data Msg Received: " + received_message.ToString(), thisIsMyProperty);

            this.broker.Publish(messageToPublish);
        }

        public void threadBody()
        {
            int msgCounter = 0;
            while (true)
            {
                Dictionary<string, string> thisIsMyProperty = new Dictionary<string, string>();
                thisIsMyProperty.Add("source", "DotNetE2ETestmodule");
                thisIsMyProperty.Add("MsgType", "Request");
                thisIsMyProperty.Add("ModuleId", configuration);

                Message messageToPublish = new Message("SensorData: " + msgCounter, thisIsMyProperty);

                this.broker.Publish(messageToPublish);

                //Publish a message every 1 seconds. 
                Thread.Sleep(1000);
                msgCounter++;
            }
        }
    }
}