using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Assemblies;

namespace PolovniAutomobiliDohvatanje
{
    [RunInstaller(true)]
    public class PolAutServiceInstaller : System.Configuration.Install.Installer
    {
        private string serviceName;

        public PolAutServiceInstaller()
        {
            ServiceProcessInstaller process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
             
            ServiceInstaller srvInst = new ServiceInstaller();
            srvInst.StartType = ServiceStartMode.Manual;
            srvInst.ServiceName = Common.Properties.Settings.Default.NazivServisa;
            srvInst.DisplayName = Common.Properties.Settings.Default.NazivServisaDuzi;
            
            Installers.Add(process);
            Installers.Add(srvInst);

            serviceName = srvInst.ServiceName;            
        }

        private void SetRegistryDescription(string serviceName)
        {
            string descriptionKeyName = "Description";
            string descriptionKeyValue = Properties.Resources.ServiceDescription;
            string regKeyPath = @"SYSTEM\CurrentControlSet\services\" + serviceName;

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKeyPath, true);
            if (key != null)
            {
                try
                {
                    key.SetValue(descriptionKeyName, descriptionKeyValue);
                    key.Close();                    
                }
                catch (Exception ex)
                {
                    Common.EventLogger.WriteEventError("Ne mogu da dodam opis za servis.", ex);
                }
                finally
                {
                    key.Close();
                }
            }
            else
                Common.EventLogger.WriteEventError("Ne mogu da otvorim registry key " + regKeyPath);
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            try
            {
                base.Install(stateSaver);
                SetRegistryDescription(serviceName);
                Common.EventLogger.WriteEventInfo("Servis je uspesno instaliran.");
            }
            catch (Exception ex)
            {
                Common.EventLogger.WriteEventError("Greska pri instalaciji servisa.", ex);
            }
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            try
            {
                base.Uninstall(savedState);
                RemoveRegistryDescription(serviceName);
                Common.EventLogger.WriteEventInfo("Servis je uspesno uklonjen.");
            }
            catch (Exception ex)
            {
                Common.EventLogger.WriteEventError("Greska pri deinstalaciji servisa.", ex);
            }
        }

        private void RemoveRegistryDescription(string serviceName)
        {
            string descriptionKeyName = "Description";
            string regKeyPath = @"SYSTEM\CurrentControlSet\services\" + serviceName;

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKeyPath, true);
            if (key != null)
            {
                try
                {
                    key.DeleteValue(descriptionKeyName);
                    key.Close();
                }
                catch (Exception ex)
                {
                    Common.EventLogger.WriteEventError("Ne mogu da uklonim opis za servis.", ex);
                }
                finally
                {
                    key.Close();
                }
            }
            else
                Common.EventLogger.WriteEventError("Ne mogu da otvorim registry key " + regKeyPath);
        }
    }
}
