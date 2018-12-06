clr.AddReference("Ans.UI.Toolkit") 
clr.AddReference("Ans.UI.Toolkit.Base") 
from Ansys.UI.Toolkit import *

def init(context):    
	ExtAPI.Log.WriteMessage("Init ExtSample1...")
def HighFiveOut(analysis_obj):    
	MessageBox.Show("High five! ExtSample1 is a success!")