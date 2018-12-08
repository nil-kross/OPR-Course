using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Ansys_Helper {
    public class Program {
        static void Main(string[] args) {
            var xmlTemplateString =
@"
<extension version='@[Version]' name='@[Name]'>
  <author>Lomtseu</author>
  <guid>@[Guid]</guid>
  <script src='main.py'/>
  <interface context='Project'>
    <images>images</images>
    <callbacks>
      <oninit>init</oninit>
    </callbacks>
    <toolbar name='@[Name]' caption='@[Name]'>
      <entry name='Start' icon='@[Icon]'>
        <callbacks>
          <onclick>Start</onclick>
        </callbacks>
      </entry>
    </toolbar>
  </interface>
</extension>
";
            var pyTemplateString =
@"
clr.AddReference('Ans.UI.Toolkit')
clr.AddReference('Ans.UI.Toolkit.Base')
clr.AddReference('Ansys')
from Ansys.UI.Toolkit import *
from Ansys import *

def init(context):
    ExtAPI.Log.WriteMessage('Init ExtSample1...')
def HighFiveOut(analysis_obj):
    MessageBox.Show('High five! ExtSample1 is a success!')
    MessageBox.Show(analysis_obj.ToString())
    MessageBox.Show(ExtAPI.DataModel.ToString())
";
            var ansysFolderString = @"C:\Program Files\ANSYS\";
            var myExtensionsString = @"My Extensions\";
            var name = "GenAl";
            var version = (new Random()).Next(0, Int32.MaxValue - 1);
            var guid = new Guid();
            var values = new Dictionary<String, String>();
            var icon = "start";

            values["@[Version]"] = version.ToString();
            values["@[Guid]"] = guid.ToString();
            values["@[Name]"] = name;
            values["@[Icon]"] = icon;

            {
                String[] strings = {
                    xmlTemplateString,
                    pyTemplateString
                };

                for (var i = 0; i < strings.Length; i++) {
                    foreach (var key in values.Keys) {
                        strings[i] = strings[i].Replace(key, values[key]);
                    }
                }
            }

            Program.RecreateMyExtensionsFolder(ansysFolderString + myExtensionsString);

            File.Create(ansysFolderString + myExtensionsString + name + ".xml").Close();
            StreamWriter streamWriter = new StreamWriter(ansysFolderString + myExtensionsString + name + ".xml", true);
            streamWriter.Write(xmlTemplateString);
            streamWriter.Flush();
        }

        public static void RecreateMyExtensionsFolder(String myExtensionsPathway) {
            if (Directory.Exists(myExtensionsPathway)) {
                try {
                    Directory.Delete(myExtensionsPathway, true);
                } catch {
                    Program.RecreateMyExtensionsFolder(myExtensionsPathway);
                }
            }

            if (!Directory.Exists(myExtensionsPathway)) {
                Directory.CreateDirectory(myExtensionsPathway);
            }
        }
    }
}