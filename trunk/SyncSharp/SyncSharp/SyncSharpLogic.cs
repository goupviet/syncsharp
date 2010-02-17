﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using SyncSharp.Storage;

namespace SyncSharp.Business
{
	public class SyncSharpLogic
	{
		private SyncProfile currentProfile;

		public SyncProfile Profile
		{
			get { return currentProfile; }
		}

		public void loadProfile()
		{
			String ID = getMachineID();
			if (checkProfileExists(ID))
			{
				Stream str = File.OpenRead(@".\Profiles\" + ID);
				BinaryFormatter formatter = new BinaryFormatter();
				currentProfile = (SyncProfile)formatter.Deserialize(str);
				str.Close();
			}
			else
			{
				currentProfile = new SyncProfile(ID);
			}
		}

		public void saveProfile()
		{
			String ID = getMachineID();
			if (!Directory.Exists(@".\Profiles\"))
				Directory.CreateDirectory(@".\Profiles\");
			Stream str = File.OpenWrite(@".\Profiles\" + ID);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(str, currentProfile);
			str.Close();
		}

		private bool checkProfileExists(String id)
		{
			return File.Exists(@".\Profiles\" + id);
		}

		private String getMachineID()
		{
			string cpuInfo = string.Empty;
			ManagementClass mc = new ManagementClass("win32_processor");
			ManagementObjectCollection moc = mc.GetInstances();

			foreach (ManagementObject mo in moc)
			{
				if (cpuInfo == "")
				{
					//Get only the first CPU's ID
					cpuInfo = mo.Properties["processorID"].Value.ToString();
					break;
				}
			}
			return cpuInfo;
		}

		public void addNewTask()
		{
			TaskWizardForm form = new TaskWizardForm(this);
			form.GetSelectTypePanel.Hide();
			form.GetFolderPairPanel.Show();
			form.ShowDialog();
		}
	}
}
