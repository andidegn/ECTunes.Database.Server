using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Runtime.Remoting;
using ECTunes.Database.Util;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Runtime.Remoting.Channels.Http;
using ECTunes.Database.ConfigLibrary;

namespace ECTunes.Database_Server {
    public class Server {

        private readonly int WIDTH = 40;

        public Server() {
            IDbConnectorRemote db = new DbConnector();
            // TCP

            // Creating a custom formatter for a TcpChannel sink chain.
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            // Creating the IDictionary to set the port on the channel instance.
            IDictionary props = new Hashtable();
            props["port"] = DbConnector.PORT;
            // Pass the properties for the port setting and the server provider in the server chain argument. (Client remains null here.)
            TcpServerChannel channel = new TcpServerChannel(props, provider);

            ChannelServices.RegisterChannel(channel, true);
            RemotingConfiguration.RegisterWellKnownServiceType(
               typeof(DbConnector), DbConnector.SERVER_PATH,
               WellKnownObjectMode.SingleCall);
        }

        public String GetServerInfo() {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetLine());
            sb.Append("\n");
            String line2 = String.Format("| IP: {0}", GetIp());
            sb.Append(line2 + GetSpaces(line2) + "|");
            sb.Append("\n");
            String line3 = String.Format("| Port: {0}", DbConnector.PORT);
            sb.Append(line3 + GetSpaces(line3) + "|");
            sb.Append("\n");
            String line4 = String.Format("| Server Path: {0}", DbConnector.SERVER_PATH);
            sb.Append(line4 + GetSpaces(line4) + "|");
            sb.Append("\n");
            sb.Append(GetLine());
            return sb.ToString();
        }

        private String GetIp() {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        private String GetSpaces(String start) {
            String spaces = "";
            for (int i = start.Length; i < WIDTH - 1; i++)
                spaces += " ";
            return spaces;
        }

        private String GetLine() {
            String line = "+";
            for (int i = 0; i < WIDTH - 2; i++)
                line += "-";
            return line + "+";
        }
    }
}
