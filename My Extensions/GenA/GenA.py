clr.AddReference("Ans.UI.Toolkit") 
clr.AddReference("Ans.UI.Toolkit.Base") 
from Ansys.UI.Toolkit import *

def init(context):    
	ExtAPI.Log.WriteMessage("Init ExtSample1...")
	ExtAPI.Log.WriteMessage(context) 
def HighFiveOut(analysis_obj):    
	MessageBox.Show(analysis_obj)
	MessageBox.Show("GenA!")