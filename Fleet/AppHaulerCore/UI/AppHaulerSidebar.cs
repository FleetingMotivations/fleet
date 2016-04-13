using Fleet.AppHaulerCore.UI.Interfaces;

using Gtk;

namespace Fleet.AppHaulerCore.UI
{
    public class AppHaulerSidebar : IComponent
    {

        private Window sidebarWindow { get; set; }

        public AppHaulerSidebar()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Construct()
        {
            sidebarWindow = new Window("AppHauler");
            sidebarWindow.Resize(200,200); // Read from configuration

            // Create a demo label
            var label = new Label("Test Label");
            sidebarWindow.Add(label);
        }

        /// <summary>
        /// Launches the SIdebacr component
        /// </summary>
        public void Launch()
        {
            sidebarWindow.ShowAll();
            Gtk.Application.Run();
        }

        /// <summary>
        /// Initializes Gtk and the Sidebar component
        /// </summary>
        public void Initialize()
        {
            Gtk.Application.Init();
        }

    }
}
