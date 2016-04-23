using System;
using Mono.Zeroconf;

namespace Fleet.Lattice.Broadcast
{
    public class LatticeBroadcast
    {
        // Naming Details
        private String servicename;
        private Int16 port;

        // Zeroconf Service
        private IRegisterService zeroconfService;

        //  ==  ==  ==  ==
        //  Constructor ==
        //  ==  ==  ==  ==

        public LatticeBroadcast(String serviceName = "Fleet Workstation", Int16 port = 80)
        {
            this.servicename = serviceName;
            this.port = port;
        }

        ~LatticeBroadcast()
        {
            if (this.zeroconfService != null)
                this.zeroconfService.Dispose();
        }

        //	==	==	==	==	==	==
        //	Zeroconf Management	==
        //	==	==	==	==	==	==

        public Boolean RegisterZeroconfService(String servicename = "Lattice", String regtype = "_lattice._tcp", String replydomain = "local")
        {

            if (this.zeroconfService == null)
            {
                var service = new RegisterService();

                service.Name = this.servicename;
                service.RegType = regtype;
                service.ReplyDomain = replydomain;
                service.Port = this.port;

                var record = new TxtRecordItem("service_name", servicename);
                var records = new TxtRecord();
                records.Add(record);

                service.TxtRecord = records;

                service.Register();

                this.zeroconfService = service;
                return true;
            }

            return false;
        }

        public Boolean DeregisterZeroconfService()
        {
            if (this.zeroconfService != null)
            {
                this.zeroconfService.Dispose();
                this.zeroconfService = null;
            }

            return false;
        }
    }
}
