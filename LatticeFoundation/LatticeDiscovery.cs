using System;
using Mono.Zeroconf;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fleet.Lattice.Discovery
{
    //	==	==	==	==	==	==
    //	Service Discovery	==
    //	==	==	==	==	==	==

    public class LatticeDiscovery
    {

        //	==	==	==
        //	Members	==
        //	==	==	==

        private IDictionary<IService, ServiceRecord> records;
        private ServiceBrowser browser;
        private Object @lock = new Object();

        public IReadOnlyDictionary<IService, ServiceRecord> CurrentRecords
        {
            get
            {
                lock (@lock)
                {
                    return new ReadOnlyDictionary<IService, ServiceRecord>(this.records);
                }
            }
        }

        //	==	==	==	==
        //	Constructor	==
        //	==	==	==	==

        public LatticeDiscovery()
        {
            this.records = new Dictionary<IService, ServiceRecord>();
        }

        ~LatticeDiscovery()
        {
            this.browser?.Dispose();
        }

        //	==	==	==	==	==	==	==
        //	Discovery Management	==
        //	==	==	==	==	==	==	==

        public Boolean StartBrowsing(String regtype = "_lattice._tcp", String domain = "local")
        {

            if (this.browser == null)
            {
                var browser = new ServiceBrowser();

                browser.ServiceAdded += OnServiceAdded;
                browser.ServiceRemoved += OnServiceRemoved;

                browser.Browse(regtype, domain);
                this.browser = browser;

                return true;
            }

            return false;
        }

        public Boolean StopBrowsing()
        {

            if (this.browser != null)
            {
                this.browser.Dispose();
                this.browser = null;

                return true;
            }

            return false;
        }

        //	==	==	==	==	==	==	==	==
        //	Service Delegate Methods	==
        //	==	==	==	==	==	==	==	==

        private void OnServiceAdded(Object o, ServiceBrowseEventArgs args)
        {
            var service = args.Service;
            service.Resolved += OnServiceResolve;
            service.Resolve();
        }

        private void OnServiceResolve(Object o, ServiceResolvedEventArgs args)
        {
            var service = args.Service;
            var record = new ServiceRecord();

            record.Hostname = service.HostEntry.AddressList[0].ToString();
            record.Port = service.Port;
            record.ServiceName = service.Name;

            //var key = record.Hostname + ":" + record.Port;

            lock (@lock)
            {
                this.records[service] = record;
            }
        }

        private void OnServiceRemoved(Object o, ServiceBrowseEventArgs args)
        {

            lock (@lock)
            {
                //var record = this.records [arg];
                //Logger.Debug ("Removed Service: " + record);

                this.records.Remove(args.Service);
            }
        }
    }

    //	==	==	==	==	==
    //	Service Record	==
    //	==	==	==	==	==

    public struct ServiceRecord
    {
        public String Hostname { get; set; }
        public Int16 Port { get; set; }
        public String ServiceName { get; set; }

        override public String ToString()
        {
            return this.ServiceName + " - " + this.Hostname + ":" + this.Port;
        }
    }
}
