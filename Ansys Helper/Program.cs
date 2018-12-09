using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ansys_Helper {
    public class Program {
        static void Main(string[] args) {
            var xmlTemplateString =
@"
<extension version='@[Version]' name='@[Name]' debug='true'>
  <author>Lomtseu</author>
  <guid>@[Guid]</guid>
  <description>Course work.</description>
  <appStoreId>1488</appStoreId>
  <script src='main.py'/>

  <simdata context='DesignXplorer | Project'>
    <optimizer
      name='@[Name]'
      caption='@[Name]'
      version='@[Version]'>
      <callbacks>
        <OnCreate>OnCreate</OnCreate>
        <CanRun>CanRun</CanRun>
        <Description>Description</Description>
        <Configuration>Configuration</Configuration>
        <Status>Status</Status>
        <QuickHelp>QuickHelp</QuickHelp>
        <InputParametersEdited>InputParametersEdited</InputParametersEdited>
        <MethodPropertiesEdited>MethodPropertiesEdited</MethodPropertiesEdited>
        <OnMigrate>OnMigrate</OnMigrate>
        <OnRelease>OnRelease</OnRelease>
      </callbacks>
      <property name='GenerationsAmount' caption='Generations Amount' control='integer' default='10' />
    </optimizer>
  </simdata>

</extension>
";
            var pyTemplateString =
@"
clr.AddReference('Ans.UI.Toolkit')
clr.AddReference('Ans.UI.Toolkit.Base')
clr.AddReference('Ansys')
from Ansys.UI.Toolkit import *
from Ansys import *

def OnCreate(entity):
    generations = entity.Properties['GenerationsAmount'].Value

    return GenAlOptimizer(generations)
def CanRun(entity):
    generations = entity.Properties['GenerationsAmount'].Value

    if generations > 0:
        return True
    else:
        return False
def QuickHelp(entity):
    generations = entity.Properties['GenerationsAmount'].Value

    if generations > 0:
        return 'OK'
    else:
        return 'Generations Amount field value should be greater then 0!'
def Description(entity):
    return 'Daniil Lomtseu course project.'
def Configuration(entity):
    generations = entity.Properties['GenerationsAmount'].Value
    configuration = 'Generates %d points.' %(generations)

    return configuration
def Status(entity):
    return 'IDK'
def InputParametersEdited(entity):
    ExtAPI.Log.WriteMessage('InputParametersEdited is called!')
    ExtAPI.Log.WriteMessage(entity.ToString())
    ExtAPI.Log.WriteMessage(entity.GetType().Assembly.FullName)
def MethodPropertiesEdited(entity):
    ExtAPI.Log.WriteMessage('MethodPropertiesEdited is called!')
    ExtAPI.Log.WriteMessage(entity.ToString())
    ExtAPI.Log.WriteMessage(entity.GetType().Assembly.FullName)
def OnMigrate(old, new):
    ExtAPI.Log.WriteMessage('OnMigrate is called!')
def OnRelease(entity):
    ExtAPI.Log.WriteMessage('Shutting down..')
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

                xmlTemplateString = strings[0];
                pyTemplateString = strings[1];
            }

            Program.RecreateMyExtensionsFolder(ansysFolderString + myExtensionsString);
            Program.CreateFile(ansysFolderString + myExtensionsString, name + ".xml", xmlTemplateString);
            Program.CreateFile(ansysFolderString + myExtensionsString + name, '\\' + "main" + ".py", pyTemplateString);

            {
                var dllPathwayString = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Ansys\bin\Debug\Ansys.dll");

                if (File.Exists(dllPathwayString)) {
                    var ansysVersionString = "v170";
                    var directoriesArray = Directory.GetDirectories(ansysFolderString + @"ANSYS Inc\");
                    var regex = new Regex("[v].*");

                    foreach (var directory in directoriesArray) {
                        var folder = directory.Split('\\').Reverse().First();
                        var match = regex.Match(folder);

                        if (match != null) {
                            ansysVersionString = directory;
                        }
                    }

                    {
                        var destinationFolderString = ansysVersionString + @"\Addins\ACT\bin\Win64\";

                        if (File.Exists(destinationFolderString + "Ansys.dll")) {
                            File.Delete(destinationFolderString + "Ansys.dll");
                        }
                        File.Move(dllPathwayString, destinationFolderString + "Ansys.dll");
                    }
                }
            }
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

        public static void CreateFile(String folderPathway, String fileName, String content) {
            if (!Directory.Exists(folderPathway)) {
                Directory.CreateDirectory(folderPathway);
            }
            File.Create(folderPathway + '\\' + fileName).Close();
            if (File.Exists(folderPathway + '\\' + fileName)) {
                StreamWriter streamWriter = new StreamWriter(folderPathway + '\\' + fileName, true);

                streamWriter.Write(content);
                streamWriter.Flush();
            }
        }
    }
}