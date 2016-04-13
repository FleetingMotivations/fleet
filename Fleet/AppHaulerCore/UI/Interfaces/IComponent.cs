using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Fleet.AppHaulerCore.UI.Interfaces
{
    interface IComponent
    {
        /// <summary>
        /// Initialize a UI component, accesssing and setting any needed config
        /// </summary>
        void Initialize();

        /// <summary>
        /// Launch the UI component doing and required rendering operations
        /// </summary>
        void Launch();

        /// <summary>
        /// Construct the UI component
        /// </summary>
        void Construct();

    }
}
